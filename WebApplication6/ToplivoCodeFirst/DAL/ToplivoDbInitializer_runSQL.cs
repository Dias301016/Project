//Класс для инициализации базы данных путем заполнения ее таблиц тестовым набором записей
using System;
using System.Data.Entity;
using System.IO;
using System.Web;

namespace ToplivoCodeFirst.Models
{
    public class ToplivoDbInitializer_runSQL : DropCreateDatabaseAlways<ToplivoContext>
    {
        protected override void Seed(ToplivoContext db)
        {
            //задание пути к файлу с текстом T-SQL инструкции
            string readPath = HttpContext.Current.Server.MapPath("~") + "/Scripts/FuelBase/FillDB.sql";

            //считывание текста SQL инструкции из внешнего текстового файла
            string SQLstring = "";
            try
            {
                using (StreamReader sr = new StreamReader(readPath, System.Text.Encoding.Default))
                {
                    SQLstring = sr.ReadToEnd();
                }
                //Выполнение T-SQL инструкции
                db.Database.ExecuteSqlCommand (SQLstring);
                base.Seed(db);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}