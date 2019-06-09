using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace FirstVar
{
    class Program
    {
        /// <summary>
        /// загрузка файла с исходными данными
        /// </summary>
        /// <param name="FileName">имя файла с входными данными</param>
        /// <returns></returns>
        private static int[] LoadData(string FileName)
        {
            string sLine = "";
            ArrayList arrText = new ArrayList();
            try
            {
                StreamReader objReader = new StreamReader(FileName);
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                objReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка чтения файла!!! " + ex.Message);
                throw new Exception("Ошибка чтения файла!!");
            }
            String[] words = arrText[0].ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] Ret = new int[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                try
                {
                    Ret[i] = Convert.ToInt32(words[i]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка формата файла!!! " + ex.Message);
                    throw new Exception("Ошибка формата файла!!");
                }
            }
            return Ret;
        }

        /// <summary>
        /// Сохранение выходного файла
        /// </summary>
        /// <param name="Output">массив с числами</param>
        /// <param name="FileName">имя выходного файла</param>
        private static void SaveData(int[] Output, string FileName)
        {
            string text = "";
            for (int i = 0; i < Output.Length; i++)
            {
                text += Output[i].ToString();
            }
            SaveTextToFile(FileName, text);
        }

        /// <summary>
        /// Сохраняет текст в заданный файл
        /// </summary>
        /// <param name="FileName">Имя файла</param>
        /// <param name="text">Текст для записи</param>
        private static void SaveTextToFile(string FileName, string text)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FileName, false, System.Text.Encoding.Default))
                {
                    sw.Write(text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка записи файла!!! " + ex.Message);
            }
        }

        /// <summary>
        /// Возвращает следующее значение последовательности
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        static int[] Next(int[] Source)
        {
            int[] Element = Source;
            for (int i = Element.Length - 1; i > 0; i--)
            {
                if (Element[i] < 9)
                {
                    Element[i]++;
                    break;
                }
                else
                {
                    Element[i] = 0;
                }
            }
            return Element;
        }

        /// <summary>
        /// Определяем дошли ли мы до конечного значения (все 9 в массиве)
        /// </summary>
        /// <param name="Current"></param>
        /// <returns></returns>
        static bool CheckEnd(int[] Current)
        {
            for (int i = 0; i < Current.Length; i++)
            {
                if (Current[i] != 9)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Вычисляем значение хэш функции
        /// </summary>
        /// <param name="s">Проверяемая последовательность</param>
        /// <param name="x">Значение х</param>
        /// <param name="m">Значение m</param>
        /// <returns></returns>
        static int CalcP(int[] s, int x, int m)
        {
            int r = 1;
            int h = 0;
            for (int i = 0; i < s.Length; i++)
            {
                h = (h + s[i] * r) % m;
                r = (r * x) % m;
            }
            return h;
        }
        static void Main(string[] args)
        {
            // Загружаем данные
            int[] Input = LoadData("INPUT.TXT");
            if (Input.Length < 4)
            {
                Console.WriteLine("Ошибка! В файле слишком мало данных, должно быть 4 строки с целыми числами!!!");
                Console.ReadLine();
                return;
            }
            // присваиваем значения переменным
            int x = Input[0];
            int m = Input[1];
            int L = Input[2];
            int v = Input[3];
            // инициализируем первое значение 000...0
            int[] Source = new int[L];
            // запускаем перебор
            while (true)
            {
                if (CalcP(Source, x, m) == v)
                {
                    // если ответ совпадает выводим на экран и в файл, останавливаем перебор
                    Console.WriteLine("Исходная последовательность, подходящая к заданной хэш функции найдена");
                    SaveData(Source, "OUTPUT.TXT");
                    for (int i = 0; i < Source.Length; i++)
                        Console.Write(Source[i]);
                    Console.WriteLine();
                    break;
                }
                else
                {
                    // если ответ не совпадает
                    // проверяем достигли ли мы конца всех вариантов, если нет, то продолжаем перебор
                    if (CheckEnd(Source))
                        Source = Next(Source);
                    else
                    {
                        // если достигли конца, то выводим инфу что не нашли решения, останавливаем цикл
                        Console.WriteLine("NO SOLUTION");
                        SaveTextToFile("OUTPUT.TXT", "NO SOLUTION");
                        break;
                    }
                }
            }
            Console.WriteLine("Для продолжения нажмите любую клавишу");
            Console.ReadLine();
        }
    }
}
