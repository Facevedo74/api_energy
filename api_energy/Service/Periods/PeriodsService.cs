using System;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text;
using api_energy.Context;
using api_energy.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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


        public (int TotalRows, List<Measurements> Results) getMeasurements(int id_period, int skip)
        {
            try
            {

                int totalRows = _context.Measurements
                        .Count(x => x.id_period == id_period);

                var results = _context.Measurements
                               .Where(x => x.id_period == id_period)
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


        public async Task AddPeriodFilesAsync(string name, List<IFormFile> files)
        {
            string uploadPath = @"C:\STD";

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploadPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            // Simulo que se ejecutto el bot Correctamente

            //Recorro ahora los txt en la misma ruta  @"C:\STD"
            var txtFiles = Directory.GetFiles(uploadPath, "*.txt");


            foreach (var txtFile in txtFiles)
            {
                using (var reader = new StreamReader(txtFile))
                {
                    await reader.ReadLineAsync();
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        var parts = line.Split('\t');
                        try
                        {
                            var measurement = new Measurements
                            {
                                Fecha = DateTime.ParseExact(parts[1], "dd/MM/yyyy", CultureInfo.InvariantCulture)

                            };
                            _context.Measurements.Add(measurement);

                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
        


                        


                        Console.WriteLine($"Procesando línea: {line}");
                    }
                }
            }

            var newPeriod = new Periods
            {
                name = name,
                creator_username = "jhond",
                date_create = DateTime.Now,
                active = true,
            };

            //_context.Add(newPeriod);
            //_context.SaveChanges();

        }

        public string GenerateTxT(int periodId)
        {
            var measurements = _context.Measurements
                .Where(m => m.id_period == periodId)
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

            // Crear una cadena para las filas de datos
            var sb = new StringBuilder();

            // Añadir los encabezados
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
               .Where(m => m.id_period == periodId)
               .ToList();

            if (measurements == null)
            {
                throw new Exception("No measurements found for the given period ID.");
            }

            using (var workbook = new XLWorkbook())
            {
                var datos = workbook.Worksheets.Add("datos");
                var calculos = workbook.Worksheets.Add("Calculos");
                var curva_tipica = workbook.Worksheets.Add("Curva tipica");
                var valores_para_graficos = workbook.Worksheets.Add("Valores para graficos");
                var resultados = workbook.Worksheets.Add("Resultados");
                var resultados_3 = workbook.Worksheets.Add("Resultados (3)");
                var hoja_1 = workbook.Worksheets.Add("Hoja1");
                var graficos_2 = workbook.Worksheets.Add("Gráfico2 (V-wh|t)");

                WorksheetsDatos(datos, measurements);
                WorksheetsCalculos(calculos, measurements.Count);
                WorksheetsResultados(resultados);


                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    var base64String = Convert.ToBase64String(content);

                    return base64String;
                }
            }
        }

        public void WorksheetsDatos(IXLWorksheet worksheet, List<Measurements> measurements)
        {
            var headers = new[]
                   { "Id" , "Fecha" ,"Tiempo" , "Tensión/L1 - L2"  , "Energías/Energía III +" , "Distorsión armónica/VL1", "Distorsión armónica/VL2", "Distorsión armónica/VL3",
                      "Flicker (Pst)/L1", "Flicker (Pst)/L2" , "Flicker (Pst)/L3"};


            for (int i = 1; i <= headers.Length; i++)
            {
                worksheet.Cell(1, i).Value = headers[i - 1];
            }


            worksheet.Cell(1, 14).Value = "Distorsión armónica Promedio"; // Celda N
            worksheet.Cell(1, 15).Value = "Flicker (Pst) y Promedio"; // Celda O
            worksheet.Cell(1, 16).Value = "Flicker (Pst) y Promedio"; // Celda P 


            for (int i = 0; i < measurements.Count; i++)
            {
                var measurement = measurements[i];
                int row = i + 2;

                worksheet.Cell(row, 1).Value = measurement.id;
                worksheet.Cell(row, 2).Value = measurement.Fecha.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 3).Value = measurement.Tiempo.ToString(@"hh\:mm\:ss");
                worksheet.Cell(row, 4).Value = measurement.Tension_L1_L2;
                worksheet.Cell(row, 4).Style.NumberFormat.Format = "0.0";

                worksheet.Cell(row, 5).Value = 0;
                worksheet.Cell(row, 6).Value = measurement.Distorsion_Armonica_VL1;
                worksheet.Cell(row, 7).Value = measurement.Distorsion_Armonica_VL2;
                worksheet.Cell(row, 8).Value = measurement.Distorsion_Armonica_VL3;
                worksheet.Cell(row, 9).Value = measurement.Flicker_Pst_L1;
                worksheet.Cell(row, 10).Value = measurement.Flicker_Pst_L2;
                worksheet.Cell(row, 11).Value = measurement.Flicker_Pst_L3;
            }

            //Formulas
            worksheet.Cell(2, 14).FormulaA1 = "SUMIF(F:H, \">0\")";
            worksheet.Cell(3, 14).FormulaA1 = "COUNTIF(datos!A:A, \">0\")";
            worksheet.Cell(4, 14).FormulaA1 = "N3*3";
            worksheet.Cell(5, 14).FormulaA1 = "ROUND((N2/N4), 2)";

            worksheet.Cell(2, 15).FormulaA1 = "SUMIF(datos!I:K, \">0\")";
            worksheet.Cell(3, 15).FormulaA1 = "COUNTIF(Calculos!A:A, \">0\")";
            worksheet.Cell(4, 15).FormulaA1 = "O3*3";
            worksheet.Cell(5, 15).FormulaA1 = "ROUND((O2/O4), 2)";

            worksheet.Cell(3, 18).FormulaA1 = "Resultados!G8"; // R3
            worksheet.Cell(4, 18).FormulaA1 = "Resultados!G9"; // R4
            worksheet.Cell(5, 18).FormulaA1 = "IF(R4=7, \"RURAL\", IF(R4=5, \"URBANO\", IF(R4=7, \"RURAL\", IF(R4=5, \"URBANO\", IF(R4=7, \"RURAL\")))))"; // R5
            worksheet.Columns().AdjustToContents();

        }

        public void WorksheetsCalculos(IXLWorksheet worksheet, int numberOfRows)
        {

            var headers = new[]
            {
                "Fecha","Tiempo","Tensión / L1 - L2","Energías / Energía III +","% U ","DUsup","VALORACION CE","CE","CE*ENE(KWh)",
                "ENE (Wh)","ENE Deficiente","Fecha-hora","Fecha-hora Nueva","Calculo de 15 minutos","Calculo 15min y 70 %\r\n","%U\r\n",
                "-22", "-21,5",  "-21", "-20,5" , "-20", "-19,5","-19", "-18,5","-18", "-17,5","-17", "-16,5", "-16", "-15,5","-15", "-14,5",
                "-14", "-13,5","-13", "-12,5","-12", "-11,5","-11", "-10,5","-10", "-9,5", "-9", "-8,5","-8", "-8,0","-7,5", "-7,0", "-6,5", "-6",
                "-5,5", "-5","-4,5", "-4","-3,5", "-3","-2,5", "-2","-1,5", "-1","-0,5", "0","0,5", "1","1,5", "2","2,5", "3","3,5", "4",
                "4,5", "5","5,5", "6","6,5", "7,0","7,5", "8,0","8,5", "9","9,5", "10","10,5", "11","11,5", "12","12,5", "13","13,5", "14",
                "14,5", "15","15,5", "16","16,5", "17","17,5", "18","18,5", "19","19,5", "20","20,5", "21","21,5", "22","22,5", "23", "" ,"","",
                "-22", "-21,5", "-21", "-20,5", "-20", "-19,5", "-19", "-18,5", "-18", "-17,5", "-17", "-16,5", "-16", "-15,5", "-15", "-14,5", "-14", "-13,5",
                "-13", "-12,5", "-12", "-11,5", "-11", "-10,5", "-10", "-9,5", "-9", "-8,5", "-8,0", "-7,5", "-7,0", "-6,5", "-6", "-5,5", "-5",
                "-4,5", "-4", "-3,5", "-3", "-2,5", "-2", "-1,5", "-1", "-0,5", "0", "0,5", "1", "1,5", "2", "2,5", "3", "3,5",
                "4", "4,5", "5", "5,5", "6", "6,5", "7,0", "7,5", "8,0", "8,5", "9", "9,5", "10", "10,5", "11", "11,5",
                "12", "12,5", "13", "13,5", "14", "14,5", "15", "15,5", "16", "16,5", "17", "17,5", "18", "18,5", "19", "19,5",
                "20", "20,5", "21", "21,5", "22", "22,5", "23"

            };

            for (int i = 1; i <= headers.Length; i++)
            {
                worksheet.Cell(1, i).Value = headers[i - 1];
            }

            worksheet.Range(1, 7, 1, 8).Merge();
            worksheet.Range(1, 10, 2, 10).Merge();
            worksheet.Range(1, 11, 2, 11).Merge();
            worksheet.Range(1, 12, 2, 12).Merge();



            for (int i = 2; i <= numberOfRows + 2; i++)
            {
                worksheet.Cell(i, 1).FormulaA1 = $"datos!B{i}";
                worksheet.Cell(i, 2).FormulaA1 = $"datos!C{i}";

                worksheet.Cell(i, 3).FormulaA1 = $"datos!D{i}";
                worksheet.Cell(i, 3).Style.NumberFormat.Format = "0.0";

                worksheet.Cell(i, 4).FormulaA1 = $"IF(Resultados!A$1=0;datos!E{i};Resultados!D12+'Curva tipica'!C{i})"; // Validar
                worksheet.Cell(i, 5).FormulaA1 = $"IF(AND(A{i}<>0;P{i}=0);ABS((C{i}-datos!R$3)/datos!R$3)*100;0)";
                worksheet.Cell(i, 6).FormulaA1 = $"IF(E{i}>datos!R$4;E{i}-datos!R$4;0)";
                worksheet.Cell(i, 7).FormulaA1 = $"IF(F{i}>11;1,85;IF(F{i}>10;1,447;IF(F{i}>9;1,033;IF(F{i}>8;0,62;IF(F{i}>7;0,421;IF(F{i}>6;0,223;IF(F{i}>5;0,196;0)))))))";
                worksheet.Cell(i, 8).FormulaA1 = $"IF(F{i}>5;0;IF(F{i}>4;0,169;IF(F{i}>3;0,142;IF(F{i}>2;0,115;IF(F{i}>1;0,089;IF(F{i}>0;0,062;0))))))";
                worksheet.Cell(i, 9).FormulaA1 = $"=SUM(G{i}:H{i})";

                if (i > 2)
                {
                    worksheet.Cell(i, 10).FormulaA1 = $"I{i}*(D{i}-D{i - 1})/1000";
                    worksheet.Cell(i, 11).FormulaA1 = $"IF(D{i}<>0;(D{i}-D{i - 1});0)";
                    worksheet.Cell(i, 11).FormulaA1 = $"IF(G{i}+H{i}>0;K{i};0)";
                }

                worksheet.Cell(i, 12).FormulaA1 = $"A{i}+B{i}";
                worksheet.Cell(i, 13).FormulaA1 = i > 2 ? $"M{i}-M{i - 1}" : $"{0}";
                worksheet.Cell(i, 14).FormulaA1 = $"IF(OR(N{i}=0,0104166666715173;N{i}=0,0104166666642413);0;1)";
                worksheet.Cell(i, 15).FormulaA1 = i > 2 ? $"IF(AND((O{i})=0;ABS((C{i}-datos!R$3)/datos!R$3)*100<30);0;1)" : $"O2";
                worksheet.Cell(i, 16).FormulaA1 = $"IF(A{i}<>0;((C{i}-datos!R$3)/datos!R$3)*100;0)";

            }

            worksheet.Columns().AdjustToContents();
        }

        public void WorksheetsResultados(IXLWorksheet worksheet){
            worksheet.Cell(12, 4).Value = 0;
            worksheet.Cell(12, 4).Value = 0;
            worksheet.Cell(31, 4).Value = 68778435;
            worksheet.Cell(33, 4).Value = 1;


            worksheet.Cell(16, 6).Value = 0;


            worksheet.Cell(8, 7).Value = 240;
            worksheet.Cell(9, 7).Value = 5;
            worksheet.Cell(10, 7).FormulaA1 = $"datos!R5";
            worksheet.Cell(18, 7).FormulaA1 = $"IF(A1=0;SUM(Calculos!K:K);(D14-D12)/1000)";
            worksheet.Cell(19, 7).FormulaA1 = $"SUM(Calculos!L:L)";
            worksheet.Cell(20, 7).FormulaA1 = $"G18-G19";
            worksheet.Cell(21, 7).FormulaA1 = $"COUNTIF(Calculos!A:A;0)";
            worksheet.Cell(22, 7).FormulaA1 = $"G21-G24";

            worksheet.Cell(23, 7).Style.NumberFormat.Format = "General";
            worksheet.Cell(23, 7).FormulaA1 = $"COUNTIF(Calculos!I:I; \">0\")"; // $"COUNTIF(Calculos!I:I; \">0\")"

            worksheet.Cell(24, 7).FormulaA1 = $"SUM(Calculos!P:P)";
            worksheet.Cell(25, 7).FormulaA1 = $"100-((G21-G23)/G21)*100";
            worksheet.Cell(27, 7).FormulaA1 = $"IF(G25>5;SUM(Calculos!J:J);0)";
            worksheet.Cell(31, 7).FormulaA1 = $"G8";


            worksheet.Cell(19, 11).FormulaA1 = $"datos!N5";
            worksheet.Cell(20, 11).FormulaA1 = $"datos!O5";
            worksheet.Cell(24, 11).FormulaA1 = $"IF(G25>5;\"TIEMPO FUERA DE RANGO > 5%\";\"TIEMPO FUERA DE RANGO < 5%\")";
            worksheet.Cell(31, 11).Value = 3;
            worksheet.Cell(33, 11).Value = "CHITRE (CAB)";

            worksheet.Cell(11, 12).FormulaA1 = $"G21 / 96";
            worksheet.Cell(13, 12).Value = 75;
            worksheet.Cell(16, 12).FormulaA1 = $"(L11+L13)*(G27/L11)";


        }
    }
}

