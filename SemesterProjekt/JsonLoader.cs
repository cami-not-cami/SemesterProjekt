using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace SemesterProjekt
{
    internal  static class JsonLoader
    {
        public static T ReadFromJsonFile<T>(string path) where T : new()
        {

            try
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(path));

            }
            catch(Exception ex) 
            {
                MessageBox.Show($"Erorr loading file:{ex.Message}");
                return new T();
            }
           

        }



    }
}
