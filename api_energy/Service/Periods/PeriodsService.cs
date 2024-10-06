using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text;
using api_energy.Assets;
using api_energy.Context;
using api_energy.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;

namespace api_energy.Service
{
    public class PeriodsService : IPeriodsService
    {
        private readonly AppDbContext _context;
        public PeriodsService(AppDbContext context)
        {
            _context = context;
        }


        public List<Periods> AllPeriods()
        {
            try
            {
                return _context.Periods.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AllPeriods", ex);
            }
        }        
        
        public Periods GetPeriodById(int periodId)
        {
            try
            {
                return _context.Periods.Include(x => x.files).FirstOrDefault(x => x.id == periodId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AllPeriods", ex);
            }
        }

        public (int TotalRows, List<Measurements> Results) getMeasurements(int id_period, int skip)
        {
            try
            {

                int totalRows = _context.Measurements
                        .Count(x => x.id_file == id_period);

                var results = _context.Measurements
                               .Where(x => x.id_file == id_period)
                               .Skip(skip)  
                               .Take(10)
                .ToList();

                return (totalRows, results);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AllPeriods", ex);
            }
        }

        public void GenerateReport(string base64String)
        {
            try
            {
                byte[] excelBytes = Convert.FromBase64String(base64String);

                using (MemoryStream stream = new MemoryStream(excelBytes))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);
                    Console.WriteLine($"Nombre de la hoja: {sheet.SheetName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al convertir Base64 a XLSX: {ex.Message}");
            }
        }

        public async Task AddPeriodFilesAsync(string name, List<IFormFile> files)
        {
            try
            {
                string uploadPath = @"C:\STD";

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                await UploadFilesAsync(files, uploadPath);

                var stdFiles = string.Join(",", files.Select(file => file.FileName));

                await ExecutePythonScript(stdFiles);

                var txtFiles = Directory.GetFiles(uploadPath, "*.txt");
                var stdFilesRoutin = Directory.GetFiles(uploadPath, "*.std");

                var newPeriod = new Periods
                {
                    name = name,
                    creator_username = "jhond",
                    date_create = DateTime.Now,
                    active = true,
                };


                _context.Add(newPeriod);
                _context.SaveChanges();


                var listNewFiles = new List<Files>();
                foreach (var file in files)
                {
                    var newFile = new Files
                    {
                        id_period = newPeriod.id,
                        name_file = file.FileName,
                    };
                    listNewFiles.Add(newFile);
                }

                _context.AddRange(listNewFiles);
                _context.SaveChanges();


                var listMeasurement = await ProcessTextFilesAsync(txtFiles, listNewFiles);
                _context.AddRange(listMeasurement);
                _context.SaveChanges();


                await deleteFiles(txtFiles);
                await deleteFiles(stdFilesRoutin);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task AditFilesPeriod(int id, List<IFormFile> files)
        {
            try
            {
                string uploadPath = @"C:\STD";

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                await UploadFilesAsync(files, uploadPath);

                var stdFiles = string.Join(",", files.Select(file => file.FileName));

                await ExecutePythonScript(stdFiles);

                var txtFiles = Directory.GetFiles(uploadPath, "*.txt");
                var stdFilesRoutin = Directory.GetFiles(uploadPath, "*.std");

                var listNewFiles = new List<Files>();
                foreach (var file in files)
                {
                    var newFile = new Files
                    {
                        id_period = id,
                        name_file = file.FileName,
                    };
                    listNewFiles.Add(newFile);
                }

                _context.AddRange(listNewFiles);
                _context.SaveChanges();


                var listMeasurement = await ProcessTextFilesAsync(txtFiles, listNewFiles);
                _context.AddRange(listMeasurement);
                _context.SaveChanges();

                await deleteFiles(txtFiles);
                await deleteFiles(stdFilesRoutin);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task deleteFiles(string[] files) 
        {
            foreach (var stdFile in files)
            {
                try
                {
                    if (File.Exists(stdFile))
                    {
                        File.Delete(stdFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error eliminando el archivo {stdFile}: {ex.Message}");
                }
            }
        }

        private async Task UploadFilesAsync(List<IFormFile> files, string uploadPath)
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    Console.WriteLine($"Subiendo archivo: {fileName} - Tamaño: {file.Length} bytes");

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var copiedFileSize = new FileInfo(filePath).Length;
                    Console.WriteLine($"Archivo copiado: {fileName} - Tamaño después de copiar: {copiedFileSize} bytes");
                }
                else
                {
                    Console.WriteLine($"El archivo {file.FileName} tiene longitud cero.");
                }
            }
        }

        private async Task<List<Measurements>> ProcessTextFilesAsync(string[] txtFiles, List<Files> files)
        {
            var listMeasurement = new List<Measurements>();

            foreach (var txtFile in txtFiles)
            {
                if (txtFile != "C:\\STD\\JNY.TXT")
                {
                    using (var reader = new StreamReader(txtFile))
                    {
                        await reader.ReadLineAsync(); 
                        string line;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            var parts = line.Split(',');
                            try
                            {
                                var measurement = new Measurements();
                                var properties = typeof(Measurements).GetProperties();

                                var realName = txtFile.Split("\\");
                                var nameStd = realName[2].Split(".");
                                measurement.id_file = files.FirstOrDefault(x => x.name_file == nameStd[0] + ".std")?.id ?? 0;

                                for (int i = 2; i < properties.Length && i - 2 < parts.Length; i++)
                                {
                                    var value = parts[i - 2];

                                    switch (Type.GetTypeCode(properties[i].PropertyType))
                                    {
                                        case TypeCode.DateTime:
                                            if (DateTime.TryParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
                                            {
                                                properties[i].SetValue(measurement, dateValue);
                                            }
                                            break;

                                        case TypeCode.Decimal:
                                            if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var decimalValue))
                                            {
                                                properties[i].SetValue(measurement, decimalValue);
                                            }
                                            break;

                                        case TypeCode.String:
                                            properties[i].SetValue(measurement, value);
                                            break;

                                        case TypeCode.Int32:
                                            if (int.TryParse(value.Replace(".", ""), out var intValue))
                                            {
                                                properties[i].SetValue(measurement, intValue);
                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                }

                                listMeasurement.Add(measurement);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error procesando línea: {line}. Detalles: {ex.Message}");
                            }
                        }
                    }
                }
            }

            return listMeasurement;
        }

        private async Task ExecutePythonScript(string stdFiles)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $@"C:\STD\scriptv2.py ""{stdFiles}""",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();

                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"Error al ejecutar el script: {error}");
                }

                Console.WriteLine(output);
            }
        }

        public string GenerateTxT(int periodId)
        {
            var measurements = _context.Measurements
                .Where(m => m.id_file == periodId)
                .ToList();

            if (measurements == null || !measurements.Any())
            {
                throw new Exception("No measurements found for the given period ID.");
            }

            var headers = new[]
                   {
                "Fecha", "Tiempo", "Período", "Unidad V", "Unidad A", "Unidad W", "Unidad An",
                "Tensión/L1 - L2", "Tensión/L2 - L3", "Tensión/L3 - L1", "Tensión/Trifásica (f-f)",
                "Tensión/L1", "Tensión/L2", "Tensión/L3", "Tensión/Trifásica",
                "Corriente/L1", "Corriente/L2", "Corriente/L3", "Corriente/Trifásica",
                "P. Activa/L1 +", "P. Activa/L2 +", "P. Activa/L3 +", "P. Activa/Trifásica +",
                "P. Inductiva/L1 +", "P. Inductiva/L2 +", "P. Inductiva/L3 +", "P. Inductiva/Trifásica +",
                "P. Capacitiva/L1 +", "P. Capacitiva/L2 +", "P. Capacitiva/L3 +", "P. Capacitiva/Trifásica +",
                "Factor de potencia/L1 +", "Factor de potencia/L2 +", "Factor de potencia/L3 +", "Factor de potencia/Trifásica +",
                "P. Aparente/L1 ~", "P. Aparente/L2 ~", "P. Aparente/L3 ~", "P. Aparente/Trifásica",
                "Energías/Energía III +", "Energías/Energía L III +", "Energías/Energía C III +",
                "Distorsión armónica/VL1", "Distorsión armónica/VL2", "Distorsión armónica/VL3",
                "Distorsión armónica/IL1", "Distorsión armónica/IL2", "Distorsión armónica/IL3",
                "Flicker (Pst)/L1", "Flicker (Pst)/L2", "Flicker (Pst)/L3",
                "Armónicos VL1/Armónico 2", "Armónicos VL1/Armónico 3", "Armónicos VL1/Armónico 4",
                "Armónicos VL1/Armónico 5", "Armónicos VL1/Armónico 6", "Armónicos VL1/Armónico 7",
                "Armónicos VL1/Armónico 8", "Armónicos VL1/Armónico 9", "Armónicos VL1/Armónico 10",
                "Armónicos VL1/Armónico 11", "Armónicos VL1/Armónico 12", "Armónicos VL1/Armónico 13",
                "Armónicos VL1/Armónico 14", "Armónicos VL1/Armónico 15",
                "Armónicos VL2/Armónico 2", "Armónicos VL2/Armónico 3", "Armónicos VL2/Armónico 4",
                "Armónicos VL2/Armónico 5", "Armónicos VL2/Armónico 6", "Armónicos VL2/Armónico 7",
                "Armónicos VL2/Armónico 8", "Armónicos VL2/Armónico 9", "Armónicos VL2/Armónico 10",
                "Armónicos VL2/Armónico 11", "Armónicos VL2/Armónico 12", "Armónicos VL2/Armónico 13",
                "Armónicos VL2/Armónico 14", "Armónicos VL2/Armónico 15",
                "Armónicos VL3/Armónico 2", "Armónicos VL3/Armónico 3", "Armónicos VL3/Armónico 4",
                "Armónicos VL3/Armónico 5", "Armónicos VL3/Armónico 6", "Armónicos VL3/Armónico 7",
                "Armónicos VL3/Armónico 8", "Armónicos VL3/Armónico 9", "Armónicos VL3/Armónico 10",
                "Armónicos VL3/Armónico 11", "Armónicos VL3/Armónico 12", "Armónicos VL3/Armónico 13",
                "Armónicos VL3/Armónico 14", "Armónicos VL3/Armónico 15",
                "Fundamentales/V L1", "Fundamentales/V L2", "Fundamentales/V L3",
                "Flicker (Plt)/L1 ~", "Flicker (Plt)/L2 ~", "Flicker (Plt)/L3 ~"
            };

            var sb = new StringBuilder();

            sb.AppendLine(string.Join("\t", headers));

            // Añadir cada fila de datos
            foreach (var measurement in measurements)
            {
                sb.AppendLine(
                    $"{measurement.Fecha:dd/MM/yyyy}\t" +
                    $"{measurement.Tiempo:hh\\:mm\\:ss}\t" +
                    $"{measurement.Periodo}\t" +
                    $"{measurement.Unidad_V}\t" +
                    $"{measurement.Unidad_A}\t" +
                    $"{measurement.Unidad_W}\t" +
                    $"{measurement.Unidad_An}\t" +
                    $"{measurement.Tension_L1_L2}\t" +
                    $"{measurement.Tension_L2_L3}\t" +
                    $"{measurement.Tension_L3_L1}\t" +
                    $"{measurement.Tension_Trifasica_ff}\t" +
                    $"{measurement.Tension_L1}\t" +
                    $"{measurement.Tension_L2}\t" +
                    $"{measurement.Tension_L3}\t" +
                    $"{measurement.Tension_Trifasica}\t" +
                    $"{measurement.Corriente_L1}\t" +
                    $"{measurement.Corriente_L2}\t" +
                    $"{measurement.Corriente_L3}\t" +
                    $"{measurement.Corriente_Trifasica}\t" +
                    $"{measurement.Distorsion_Armonica_VL1}\t" +
                    $"{measurement.Distorsion_Armonica_VL2}\t" +
                    $"{measurement.Distorsion_Armonica_VL3}\t" +
                    $"{measurement.Distorsion_Armonica_IL1}\t" +
                    $"{measurement.Distorsion_Armonica_IL2}\t" +
                    $"{measurement.Distorsion_Armonica_IL3}\t" +
                    $"{measurement.Flicker_Pst_L1}\t" +
                    $"{measurement.Flicker_Pst_L2}\t" +
                    $"{measurement.Flicker_Pst_L3}"
                );
            }

            var base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(sb.ToString()));

            return base64Encoded;
        }

        public string ExportExcel(int periodId)
        {
            var measurements = _context.Measurements
               .Where(m => m.id_file == periodId)
               .ToList();

            if (measurements == null || measurements.Count == 0)
            {
                throw new Exception("No measurements found for the given period ID.");
            }

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Plantilla_Analisis_de_datos.xls"); // Cambia "tu_archivo.xlsx" por el nombre de tu archivo

            byte[] excelBytes = File.ReadAllBytes(filePath);

            //byte[] excelBytes = Convert.FromBase64String(fileBytes);//ReportData.Data_Analysis_Template);

            using (MemoryStream stream = new MemoryStream(excelBytes))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                ISheet datos = workbook.GetSheetAt(0);
                WorksheetsDatos(datos, measurements);

                ISheet sheet = workbook.GetSheetAt(1);
                HSSFFormulaEvaluator evaluator = new HSSFFormulaEvaluator((HSSFWorkbook)workbook);
                for (int rowIndex = 1; rowIndex <= measurements.Count; rowIndex++)
                {
                    IRow row = sheet.GetRow(rowIndex);
                    if (row != null) 
                    {
                        ICell cellA = row.GetCell(0);
                        ICell cellB = row.GetCell(1);
                        ICell cellM = row.GetCell(12);
                        evaluator.EvaluateFormulaCell(cellA);                
                        evaluator.EvaluateFormulaCell(cellB);                
                        evaluator.EvaluateFormulaCell(cellM);
                        
                    }
                }

                HSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                using (MemoryStream newStream = new MemoryStream())
                {
                    workbook.Write(newStream);
                    return Convert.ToBase64String(newStream.ToArray());  
                }

            }
        }

        public void WorksheetsDatos(ISheet sheet, List<Measurements> measurements)
        {

            for (int i = 0; i < measurements.Count; i++)
            {
                var measurement = measurements[i];
               


                int rowNumber = i + 1; 
                IRow row = sheet.GetRow(rowNumber) ?? sheet.CreateRow(rowNumber);

                row.CreateCell(0).SetCellValue(measurement.id);

                //DateTime dateValue = DateTime.ParseExact(measurement.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string outputDateString = dateValue.ToString("dd/MM/yyyy");


                row.GetCell(1).SetCellValue(measurement.Fecha);

                row.CreateCell(2).SetCellValue(measurement.Tiempo);
                row.CreateCell(3).SetCellValue((double)measurement.Tension_L1_L2);
                row.CreateCell(4).SetCellValue((double)measurement.Energias_Energia_III);
                row.CreateCell(5).SetCellValue((double)measurement.Distorsion_Armonica_VL1);
                row.CreateCell(6).SetCellValue((double)measurement.Distorsion_Armonica_VL2);
                row.CreateCell(7).SetCellValue((double)measurement.Distorsion_Armonica_VL3);
                row.CreateCell(8).SetCellValue((double)measurement.Flicker_Pst_L1);
                row.CreateCell(9).SetCellValue((double)measurement.Flicker_Pst_L2);
                row.CreateCell(10).SetCellValue((double)measurement.Flicker_Pst_L3);
            }


        }

      
    }
}

