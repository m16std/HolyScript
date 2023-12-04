using System;
using System.Collections.Generic;
using System.Windows;
using AngouriMath.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Microsoft.Win32;


namespace custom_pl
    
{
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
    public class code_param
    {
        public string code;
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string[] keywords = { "молитва", "аще", "инако", "допрежь", "быть", "цифирь", "целым", "словом", "боуковой", "многи", "молви" };
        string[] operators = { "худо", "вяще", "есть", "або", "й", "въ" };
        List<vars> variables = new List<vars>();

        public string remove_some_shit(string text)
        {
            text = text.Trim(new Char[] { ' ', '\n', '\r', '"' });
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
        public string find_next_space_left(ref int i)
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
        public void find_amin(ref int i, int end)
        {
            int x = i;
            bool flag = false;
            while (x < end)
            {
                string inako = find_next_space(ref x);
                if (inako == "Аминь")
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
        }

        public void read(int start, int end)
        {
            int i = start;

            while (i < end)
            {
                string word = find_next_space(ref i);

                if (word == "молвить")
                {
                    add_output(find_next_quotes(ref i));
                }
                if (word == "придать")
                {
                    bool success = false;
                    string name = find_next_space_left(i - 2);
                    string chislo = find_next_space(ref i);
                    for (int z = 0; z < variables.Count; z++)
                    {
                        if (variables[z].type != 0)
                            continue;

                        if (variables[z].name != name)
                            continue;

                        variables[z].value += (int)chislo.EvalNumerical();
                        success = true;
                        break;
                    }


                    if (!success)
                    {
                        string pos = name;
                        name = find_next_space_left(ref i);
                        name = find_next_space_left(ref i);
                        name = find_next_space_left(ref i);
                        string vi = find_next_space_left(ref i);
                        name = find_next_space_left(ref i);

                        for (int z = 0; z < variables.Count; z++)
                        {


                            if (variables[z].type != 1)
                                continue;

                            if (variables[z].name != name)
                                continue;

                            int numericValue, pos_val = 0;

                            bool posisNumber = int.TryParse(pos, out numericValue);

                            if (!posisNumber)
                                for (int c = 0; c < variables.Count; c++)
                                {
                                    if (variables[c].type != 0)
                                        continue;

                                    if (variables[c].name != pos)
                                        continue;

                                    pos_val = variables[z].value;
                                }
                            else
                                pos_val = (int)pos.EvalNumerical();

                            variables[z].arr[pos_val] += (int)chislo.EvalNumerical();
                            chislo = find_next_space(ref i);
                            chislo = find_next_space(ref i);
                            chislo = find_next_space(ref i);
                            chislo = find_next_space(ref i);
                            break;
                        }
                    }

                }
                if (word == "быть")
                {
                    string name = find_next_space_left(i - 2);
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
                    if (condition == "въ")
                    {
                        left_arr = true;
                        vi = condition;
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
                        if (var1_val != var2_val)
                        {
                            find_inako(ref i, end);
                        }
                    }
                    if (condition == "худо")
                    {
                        if (var1_val > var2_val)
                        {
                            find_inako(ref i, end);
                        }
                    }
                    if (condition == "вяще")
                    {
                        if (var1_val > var2_val)
                        {
                            find_inako(ref i, end);
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
                if (word == "допрежь")
                {
                    int i_start = i;
                    find_next_space_left(ref i_start);
                    bool left_arr = false, right_arr = false;
                    int var1_val = 0, var2_val = 0;
                    int pos_val = 0;
                    int numericValue;
                    string pos = "", vi = "";
                    string var1 = find_next_space(ref i);
                    string condition = find_next_space(ref i);
                    if (condition == "въ")
                    {
                        left_arr = true;
                        vi = condition;
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
                            int j = i;
                            find_amin(ref j, end);
                            read(i, j - 6);
                            i = i_start;
                        }
                        else
                        {
                            find_amin(ref i, end);
                        }
                    }
                    if (condition == "худо")
                    {
                        if (var1_val < var2_val)
                        {
                            int j = i;
                            find_amin(ref j, end);
                            read(i, j - 6);
                            i = i_start;
                        }
                        else
                        {
                            find_amin(ref i, end);
                        }
                    }
                    if (condition == "вяще")
                    {
                        if (var1_val > var2_val)
                        {
                            int j = i;
                            find_amin(ref j, end);
                            read(i, j - 6);
                            i = i_start;
                        }
                        else
                        {
                            find_amin(ref i, end);
                        }
                    }

                }
            
            }
        }
        private void parsing(object sender, RoutedEventArgs e)
        {
            output_textbox.Text = "";
            variables.Clear();
            int i, j, start=-1, end=-1;

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

            read(start, end);
            add_output("Аминь");
        }
        private void open(object sender, RoutedEventArgs e)
        {
            var yalm = @"";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                yalm = System.IO.File.ReadAllText(openFileDialog.FileName);

            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
            var parameter = deserializer.Deserialize<code_param>(yalm);
            input_textbox.Text = parameter.code;
        }
        /*
        private void save(object sender, RoutedEventArgs e)
        {
            var yaml = input_textbox.Text.Replace("\n", "\n\n");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                System.IO.File.WriteAllText(saveFileDialog.FileName, yaml);
        */
        private void save(object sender, RoutedEventArgs e)
        {
            code_param parameter = new code_param();
            parameter.code = input_textbox.Text;

            var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var yaml = serializer.Serialize(parameter);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                System.IO.File.WriteAllText(saveFileDialog.FileName, yaml);
        }
        private void set_font_size(object sender, RoutedEventArgs e)
        {
            if (input_textbox == null)
                return;
            input_textbox.FontSize = font_size_slider.Value;
            output_textbox.FontSize = font_size_slider.Value;
        }
    }
}
