using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using AngouriMath.Extensions;


namespace custom_pl
    
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class vars
    {
        public string name;
        public int type; //0 - int 1 - arr 2 - char 3 - string
        public int value;
        public int size;
        public List<int> arr = new List<int>();
        public char symb;
        public string text;
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string[] keywords = { "молитва", "аще", "инако", "допрежь", "быть", "цифирь", "целым", "словом", "боуковой", "многи", "молви" };
        string[] operators = { "худо", "вяще", "есть", "або", "й", "въ" };
        //string[] operators = { "{", "}", ";", "->", "[", "]", "(", ")", "++", "--", "~", "!", "-", "+", "&", "*", "/", ">>", "<<", ">", "<", "=>", "<=", "==", "!=", "^", "|", "||", "&&", "?:", "=", "*=", "/=", "+=", "-=", "%=", ">>=", "<<=", "&=", "|=", "^=", ",", ".", ".*", "->*", "?", "" };
        //Regex.IsMatch(symb_before.ToString(), @"[\ \+\-\/\*\=\(\)\[\]\{\}\<\>\;]") == true


        public string remove_some_shit(string text)
        {
            text = text.Trim(new Char[] { ' ', '\n', '\r' });
            return text;
        }

        private void add_output(string word)
        {
            output_textbox.Text += word + "\n";
        }
        public string find_next_space(ref int i)
        {
            int start = i;

            while (input_textbox.Text[start] != ' ' && start != input_textbox.Text.Length - 2 && input_textbox.Text[start] != '\n' && input_textbox.Text[start] != '\r' && start != 0)
                start++;

            i = start + 1;
            while (input_textbox.Text[i] != ' ' && i != input_textbox.Text.Length - 1 && input_textbox.Text[i] != '\n' && input_textbox.Text[i] != '\r')
                i++;

            return remove_some_shit(input_textbox.Text.Substring(start, i - start));
        }
        public string find_next_trans(ref int i)
        {
            int start = i;

            while (input_textbox.Text[start] != ' ' && start != input_textbox.Text.Length - 2 && input_textbox.Text[start] != '\n' && input_textbox.Text[start] != '\r' && start != 0)
                start++;

            i = start + 1;
            while (input_textbox.Text[i] != '\n' && input_textbox.Text[i] != '\r')
                i++;

            return remove_some_shit(input_textbox.Text.Substring(start, i - start));
        }
        public string find_next_space_left(int i)
        {
            int end = i;

            while (input_textbox.Text[end] != ' ' && end != 1 && input_textbox.Text[end] != '\n' && input_textbox.Text[end] != '\r' && end != input_textbox.Text.Length - 1)
                end--;

            i = end - 1;
            while (input_textbox.Text[i] != ' ' && i != 0 && input_textbox.Text[i] != '\n' && input_textbox.Text[i] != '\r')
                i--;

            return remove_some_shit(input_textbox.Text.Substring(i + 1, end - i));
        }
        public string find_next_quotes(ref int i)
        {
            int start = i;

            while (input_textbox.Text[start] != '"' && start != input_textbox.Text.Length - 2 && input_textbox.Text[start] != '\n' && input_textbox.Text[start] != '\r')
                start++;

            start++;
            i = start;
            while (input_textbox.Text[i] != '"' && i != input_textbox.Text.Length - 1 && input_textbox.Text[i] != '\n' && input_textbox.Text[i] != '\r')
                i++;

            return remove_some_shit(input_textbox.Text.Substring(start, i - start));
        }
        public string find_next_comma(ref int i)
        {
            int start = i;

            while (input_textbox.Text[start] != ' ' && start != input_textbox.Text.Length - 2 && input_textbox.Text[start] != '\n' && input_textbox.Text[start] != '\r')
                start++;

            i = start;
            while (input_textbox.Text[i] != ',' && i != input_textbox.Text.Length - 1 && input_textbox.Text[i] != '\n' && input_textbox.Text[i] != '\r')
                i++;

            return remove_some_shit(input_textbox.Text.Substring(start, i - start));
        }
        public void find_inako(ref int i, int end)
        {
            int x = i;
            bool flag = false;
            while (x < end)
            {
                string inako = find_next_space(ref x);
                if (inako == "инако")
                {
                    flag = true;
                    break;
                }
            }
            if (flag) //найдено инако
            {
                i = x;
                return;
            }
            if (!flag) //не найдено инако, пропускаем следующую строчку
            {
                while (i < end)
                {
                    if (input_textbox.Text[i] == '\n')
                        return;
                    i++;
                }
                while (i < end)
                {
                    if (input_textbox.Text[i] == '\n')
                        return;
                    i++;
                }
                return;
            }
        }
        private void parsing(object sender, RoutedEventArgs e)
        {
            output_textbox.Text = "";
            int i, j, start=-1, end=-1;

            List<vars> variables = new List<vars>();

            for (i = 0; i < input_textbox.Text.Length; i++)
            {
                if (find_next_space(ref i) == "Молитва")
                {
                    start = i;
                    break;
                }
            }
            for (i = input_textbox.Text.Length-1; i > 0 ; i--)
            {
                if (find_next_space_left(i) == "Аминь")
                {
                    end = i;
                    break;
                }
            }

            if (start == -1 || end == -1)
                return;

            for (i = start; i < end; i++)
            {
                for (j = 1; j <= end - i && j < 20; j++)
                {
                    string word = input_textbox.Text.Substring(i, j);

                    if (word == "молви")
                    {
                        add_output(find_next_quotes(ref i));
                        break;
                    }
                    if (word == "придать")
                    {
                        string name = find_next_space_left(i);
                        string chislo = find_next_space(ref i);
                        for (int z = 0; z < variables.Count; z++)
                        {
                            if (variables[z].type != 0)
                                continue;

                            if (variables[z].name != name)
                                continue;

                            variables[z].value += (int)chislo.EvalNumerical();
                            break;
                        }
                    }
                    if (word == "быть")
                    {
                        string name = find_next_space_left(i);
                        string type = find_next_space(ref i);

                        if (type == "цифирь")
                        {
                            vars newvar = new vars();
                            newvar.name = name;
                            newvar.type = 0;
                            newvar.value = (int)find_next_space(ref i).EvalNumerical();
                            variables.Add(newvar);
                        }

                        if (type == "многи")
                        {
                            vars newvar = new vars();
                            newvar.name = name;
                            newvar.type = 1;
                            int size = 0;

                            while (true)
                            {
                                newvar.arr.Add((int)find_next_comma(ref i).EvalNumerical());
                                size++;

                                if (input_textbox.Text[i] == '\n' || input_textbox.Text[i] == '\r')
                                    break;
                            }
                            newvar.size = size;
                            variables.Add(newvar);
                        }

                        if (type == "словом")
                        {

                        }

                        if (type == "боуковой")
                        {

                        }

                        break;
                    }
                    if (word == "аще")
                    {
                        bool left_arr = false, right_arr = false;
                        int var1_val = 0, var2_val = 0;
                        int pos_val = 0;
                        int numericValue;
                        string pos = "", vi;
                        string var1 = find_next_space(ref i);
                        string condition = find_next_space(ref i);
                        if(condition == "въ")
                        {
                            left_arr = true;
                            vi = condition;
                            pos = find_next_space(ref i);
                            bool posisNumber = int.TryParse(pos, out numericValue);
                            if(!posisNumber)
                                for (int z = 0; z < variables.Count; z++) 
                                {
                                    if (variables[z].type != 0)
                                        continue;

                                    if (variables[z].name != pos)
                                        continue;

                                    pos_val = variables[z].value;
                                }
                            else
                                pos_val = (int)pos.EvalNumerical();

                            condition = find_next_space(ref i);

                            for (int z = 0; z < variables.Count; z++) //если массив
                            {
                                if (variables[z].type != 1)
                                    continue;

                                if (variables[z].name != var1)
                                    continue;

                                var1_val = variables[z].arr[pos_val];
                            }
                        }
                        string var2 = find_next_space(ref i);
                        vi = find_next_space(ref i);
                        if (vi == "въ")
                        {
                            right_arr = true;
                            pos = find_next_space(ref i);
                            bool posisNumber = int.TryParse(pos, out numericValue);
                            if (!posisNumber)
                                for (int z = 0; z < variables.Count; z++)
                                {
                                    if (variables[z].type != 0)
                                        continue;

                                    if (variables[z].name != pos)
                                        continue;

                                    pos_val = variables[z].value;
                                }
                            else
                                pos_val = (int)pos.EvalNumerical();

                            condition = find_next_space(ref i);

                            for (int z = 0; z < variables.Count; z++) //если массив
                            {
                                if (variables[z].type != 1)
                                    continue;

                                if (variables[z].name != var2)
                                    continue;

                                var2_val = variables[z].arr[pos_val];
                            }
                        }

                        bool var1isNumber = int.TryParse(var1, out numericValue);
                        bool var2isNumber = int.TryParse(var2, out numericValue);


                        if (var1isNumber)
                            var1_val = (int)var1.EvalNumerical();
                        else if (!left_arr)
                        {
                            for (int z = 0; z < variables.Count; z++) //если целое
                            {
                                if (variables[z].type != 0)
                                    continue;

                                if (variables[z].name != var1)
                                    continue;

                                var1_val = variables[z].value;
                            }

                        }
                        if (var2isNumber)
                            var2_val = (int)var2.EvalNumerical();
                        else if (!right_arr)
                        {
                            for (int z = 0; z < variables.Count; z++) //если целое
                            {
                                if (variables[z].type != 0)
                                    continue;

                                if (variables[z].name != var2)
                                    continue;

                                var2_val = variables[z].value;
                            }
                        }

                        if (condition == "есть")
                        {
                            if (var1_val == var2_val)
                            {
                                break; //условие выполнилось, выходим, читаем дальше
                            }
                            else
                            {
                                find_inako(ref i, end);
                                break;
                            }
                        }
                        if (condition == "худо")
                        {
                            if (var1_val < var2_val)
                            {
                                break; //условие выполнилось, выходим, читаем дальше
                            }
                            else
                            {
                                find_inako(ref i, end);
                                break;
                            }
                        }
                        if (condition == "вяще")
                        {
                            if (var1_val > var2_val)
                            {
                                break; //условие выполнилось, выходим, читаем дальше
                            }
                            else
                            {
                                find_inako(ref i, end);
                                break;
                            }
                        }
                    }
                    if (word == "инако")
                    {
                        while (i < end)
                        {
                            if (input_textbox.Text[i] == '\n')
                                break;
                            i++;
                        }
                        i++;
                        while (i < end)
                        {
                            if (input_textbox.Text[i] == '\n')
                                break;
                            i++;
                        }
                    }
                }
            }
        }
    }
}
