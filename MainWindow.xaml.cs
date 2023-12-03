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
        string[] operators = { "помене", "або", "й" };
        //string[] operators = { "{", "}", ";", "->", "[", "]", "(", ")", "++", "--", "~", "!", "-", "+", "&", "*", "/", ">>", "<<", ">", "<", "=>", "<=", "==", "!=", "^", "|", "||", "&&", "?:", "=", "*=", "/=", "+=", "-=", "%=", ">>=", "<<=", "&=", "|=", "^=", ",", ".", ".*", "->*", "?", "" };
        //Regex.IsMatch(symb_before.ToString(), @"[\ \+\-\/\*\=\(\)\[\]\{\}\<\>\;]") == true
        /*
                    int index = Array.IndexOf(keywords, word);

                    if (index < 0)
                        continue;
         */
        private void add_output(string word)
        {
            output_textbox.Text += word;
        }

        public string find_next_space(ref int i)
        {
            int start = i;

            while (input_textbox.Text[start] != ' ' && start != input_textbox.Text.Length - 2 && input_textbox.Text[start] != '\n' && input_textbox.Text[start] != '\r')
                start++;

            start++;
            i = start;
            while (input_textbox.Text[i] != ' ' && i != input_textbox.Text.Length - 1 && input_textbox.Text[i] != '\n' && input_textbox.Text[i] != '\r')
                i++;

            return input_textbox.Text.Substring(start, i - start);
        }

        public string find_next_space_left(int i)
        {
            int end = i;

            while (input_textbox.Text[end] != ' ' && end != 1 && input_textbox.Text[end] != '\n' && input_textbox.Text[end] != '\r')
                end--;

            i = end;
            while (input_textbox.Text[i] != ' ' && i != 0 && input_textbox.Text[i] != '\n' && input_textbox.Text[i] != '\r')
                i--;

            return input_textbox.Text.Substring(i + 1, end - i);
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

            return input_textbox.Text.Substring(start, i - start);
        }

        public string find_next_comma(ref int i)
        {
            int start = i;

            while (input_textbox.Text[start] != ' ' && start != input_textbox.Text.Length - 2 && input_textbox.Text[start] != '\n' && input_textbox.Text[start] != '\r')
                start++;

            start++;
            i = start;
            while (input_textbox.Text[i] != ',' && i != input_textbox.Text.Length - 1 && input_textbox.Text[i] != '\n' && input_textbox.Text[i] != '\r')
                i++;

            return input_textbox.Text.Substring(start, i - start);
        }

        private void parsing(object sender, RoutedEventArgs e)
        {
            output_textbox.Text = "";
            int i, j;
            string[] new_var = { };
            string[] new_var_type = { };
            string[] new_var_value = { };
            List < vars > variables = new List<vars>();

            for (i = 0; i < input_textbox.Text.Length; i++)
            {


            }





            for (i = 0; i < input_textbox.Text.Length; i++)
            {
                for (j = 1; j <= input_textbox.Text.Length - i && j < 20; j++)
                {
                    string word = input_textbox.Text.Substring(i, j);

                    if (word == "молви")
                    {
                        add_output(find_next_quotes(ref i));
                        break;
                    }
                    if (word == "быть")
                    {
                        string name = find_next_space_left(i);
                        string type = find_next_space(ref i);

                        if (type == "целым")
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
                    if (word == "еси")
                    {

                    }
                }
            }

        }
    }
}
