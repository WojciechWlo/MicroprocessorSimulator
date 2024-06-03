using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
namespace MicroprocessorSim
{


    public partial class Form1 : Form
    {
        public struct commandLine
        {
            public string command;
            public string first;
            public string second;

        }

        List<commandLine> commandLines = new List<commandLine>();
        public string currentConsoleText = "";
        public bool inputSign = false;
        public bool registeredMode = false;
        public commandLine cl;
        public int stepModeIndex;
        public int wholeModeIndex;
        public int SP = 99;
        public ushort[] stack = new ushort[100];
        public bool execute = true;
        public Form1()
        {

            InitializeComponent();
            consoleBox.ReadOnly = true;
            stackBox.SelectionMode = SelectionMode.None;
            variableUpDown.Maximum = 65535;
            if (!registerMode.Checked)
            {
                Register2.Enabled = false;
            }
            if (!wholeMode.Checked)
            {
                runB.Enabled = false;
                nextB.Enabled = false;
                finishB.Enabled = false;
            }
            CommandList.SelectedItem = "Podstawowe funkcje";
        }
        private void variableUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void immediateMode_CheckedChanged(object sender, EventArgs e)
        {
            mode_of_implementation();
        }

        private void registerMode_CheckedChanged(object sender, EventArgs e)
        {
            mode_of_implementation();
        }

        private void stepMode_CheckedChanged(object sender, EventArgs e)
        {
            mode_of_realization();
        }

        private void wholeMode_CheckedChanged(object sender, EventArgs e)
        {
            mode_of_realization();
        }

        private void mode_of_implementation()
        {
            if (!registerMode.Checked)
            {
                variableUpDown.Enabled = true;
                Register2.Enabled = false;
                registeredMode = false;
            }
            else
            {
                variableUpDown.Enabled = false;
                Register2.Enabled = true;
                registeredMode = true;
            }

        }
        private void mode_push_pop()
        {

        }
        private void mode_of_realization()
        {
            if (stepMode.Checked)
            {
                runB.Enabled = false;
                startB.Enabled = true;
            }
            else
            {
                runB.Enabled = true;
                startB.Enabled = false;
            }

        }

        private void pushOnEndB_Click(object sender, EventArgs e)
        {
            cl.command = null;
            cl.first = null;
            cl.second = null;
            if (!CommandList.SelectedItem.Equals("Podstawowe funkcje"))
            {
                if (CommandList.SelectedItem.Equals("POP") || CommandList.SelectedItem.Equals("PUSH"))
                {
                    if (CommandList.SelectedItem.Equals("POP"))
                    {
                        cl.command = "POP";

                    }
                    else if (CommandList.SelectedItem.Equals("PUSH"))
                    {
                        cl.command = "PUSH";
                    }
                    foreach (RadioButton c in Register1.Controls.OfType<RadioButton>())
                    {
                        if (c.Checked == true)
                        {
                            cl.first = c.Text;
                            break;
                        }
                    }
                    commandLines.Add(cl);
                    commandBox.Items.Add((commandBox.Items.Count + 1).ToString() + ".    " + cl.command + "    " + cl.first);
                }
                else
                {

                    cl.first = "AH";
                    cl.command = "MOV";
                    if (CommandList.SelectedItem.Equals("2AH, INT 21H"))
                    {

                        cl.second = "42";
                    }
                    else if (CommandList.SelectedItem.Equals("2CH, INT 21H"))
                    {
                        cl.second = "44";
                    }
                    else if (CommandList.SelectedItem.Equals("0H, INT 21H"))
                    {
                        cl.second = "0";
                    }
                    else if (CommandList.SelectedItem.Equals("19H, INT 21H"))
                    {
                        cl.second = "25";
                    }
                    else if (CommandList.SelectedItem.Equals("4H, INT 33H"))
                    {
                        cl.second = "4";
                    }
                    else if (CommandList.SelectedItem.Equals("2H, INT 21H"))
                    {
                        cl.second = "2";
                    }
                    else if (CommandList.SelectedItem.Equals("1H, INT 21H"))
                    {
                        cl.second = "1";
                    }
                    commandLines.Add(cl);
                    commandBox.Items.Add((commandBox.Items.Count + 1).ToString() + ".    " + cl.command + "    " + cl.first + ",    " + cl.second);
                    cl.command = "INT";
                    cl.first = "21H";
                    cl.second = null;
                    commandLines.Add(cl);
                    commandBox.Items.Add((commandBox.Items.Count + 1).ToString() + ".    " + cl.command + "    " + cl.first);

                }

            }
            else
            {
                if (registeredMode)
                {
                    if (ADD.Checked)
                    {
                        cl.command = "ADD";
                    }
                    else if (SUB.Checked)
                    {
                        cl.command = "SUB";
                    }
                    else
                    {
                        cl.command = "MOV";
                    }

                    foreach (RadioButton c in Register1.Controls.OfType<RadioButton>())
                    {
                        if (c.Checked == true)
                        {
                            cl.first = c.Text;
                            break;
                        }
                    }
                    foreach (RadioButton c in Register2.Controls.OfType<RadioButton>())
                    {
                        if (c.Checked == true)
                        {
                            cl.second = c.Text;
                            break;
                        }
                    }
                    commandLines.Add(cl);
                    commandBox.Items.Add((commandBox.Items.Count + 1).ToString() + ".    " + cl.command + "    " + cl.first + ",    " + cl.second);

                }
                else
                {
                    if (ADD.Checked)
                    {
                        cl.command = "ADD";
                    }
                    else if (SUB.Checked)
                    {
                        cl.command = "SUB";
                    }
                    else
                    {
                        cl.command = "MOV";
                    }

                    foreach (RadioButton c in Register1.Controls.OfType<RadioButton>())
                    {
                        if (c.Checked == true)
                        {
                            cl.first = c.Text;
                            break;
                        }
                    }
                    cl.second = variableUpDown.Value.ToString();
                    commandLines.Add(cl);
                    commandBox.Items.Add((commandBox.Items.Count + 1).ToString() + ".    " + cl.command + "    " + cl.first + ",    " + cl.second);
                }
            }
        }
        private void update_list()
        {

            while (commandBox.Items.Count > 0)
            {
                commandBox.Items.RemoveAt(0);
            }
            for (int i = 0; i < commandLines.Count; i++)
            {
                cl = commandLines[i];
                if (cl.second == null)
                {
                    commandBox.Items.Add((commandBox.Items.Count + 1).ToString() + ".    " + cl.command + "    " + cl.first);
                }
                else
                {
                    commandBox.Items.Add((commandBox.Items.Count + 1).ToString() + ".    " + cl.command + "    " + cl.first + ",    " + cl.second);
                }

            }
        }

        private void pushCommandB_Click(object sender, EventArgs e)
        {
            cl.command = null;
            cl.first = null;
            cl.second = null;
            int si = commandBox.SelectedIndex;
            if (si != -1)
            {
                if (!CommandList.SelectedItem.Equals("Podstawowe funkcje"))
                {
                    if (CommandList.SelectedItem.Equals("POP") || CommandList.SelectedItem.Equals("PUSH"))
                    {
                        if (CommandList.SelectedItem.Equals("POP"))
                        {
                            cl.command = "POP";

                        }
                        else if (CommandList.SelectedItem.Equals("PUSH"))
                        {
                            cl.command = "PUSH";
                        }
                        foreach (RadioButton c in Register1.Controls.OfType<RadioButton>())
                        {
                            if (c.Checked == true)
                            {
                                cl.first = c.Text;
                                break;
                            }
                        }
                    }
                    else
                    {



                        cl.first = "AH";
                        cl.command = "MOV";
                        if (CommandList.SelectedItem.Equals("2AH, INT 21H"))
                        {

                            cl.second = "42";
                        }
                        else if (CommandList.SelectedItem.Equals("2CH, INT 21H"))
                        {
                            cl.second = "44";
                        }
                        else if (CommandList.SelectedItem.Equals("0H, INT 21H"))
                        {
                            cl.second = "0";
                        }
                        else if (CommandList.SelectedItem.Equals("19H, INT 21H"))
                        {
                            cl.second = "25";
                        }
                        else if (CommandList.SelectedItem.Equals("4H, INT 33H"))
                        {
                            cl.second = "4";
                        }
                        else if (CommandList.SelectedItem.Equals("2H, INT 21H"))
                        {
                            cl.second = "2";
                        }
                        else if (CommandList.SelectedItem.Equals("1H, INT 21H"))
                        {
                            cl.second = "1";
                        }
                    }
                    commandLines.Insert(commandBox.SelectedIndex + 1, cl);
                    update_list();
                    cl.command = "INT";
                    cl.first = "21H";
                    cl.second = null;
                    update_list();
                }
                else
                {
                    if (registeredMode)
                    {
                        if (ADD.Checked)
                        {
                            cl.command = "ADD";
                        }
                        else if (SUB.Checked)
                        {
                            cl.command = "SUB";
                        }
                        else if (MOV.Checked)
                        {
                            cl.command = "MOV";
                        }
                        foreach (RadioButton c in Register1.Controls.OfType<RadioButton>())
                        {
                            if (c.Checked == true)
                            {
                                cl.first = c.Text;
                                break;
                            }
                        }
                        foreach (RadioButton c in Register2.Controls.OfType<RadioButton>())
                        {
                            if (c.Checked == true)
                            {
                                cl.second = c.Text;
                                break;
                            }
                        }
                        commandLines.Insert(commandBox.SelectedIndex + 1, cl);

                    }
                    else
                    {
                        if (ADD.Checked)
                        {
                            cl.command = "ADD";
                        }
                        else if (SUB.Checked)
                        {
                            cl.command = "SUB";
                        }
                        else if (MOV.Checked)
                        {
                            cl.command = "MOV";
                        }
                        foreach (RadioButton c in Register1.Controls.OfType<RadioButton>())
                        {
                            if (c.Checked == true)
                            {
                                cl.first = c.Text;
                                break;
                            }
                        }
                        cl.second = variableUpDown.Value.ToString();
                        commandLines.Insert(commandBox.SelectedIndex + 1, cl);
                    }
                    update_list();

                }
                commandBox.SelectedIndex = si;

            }
        }

        private void eraseAllB_Click(object sender, EventArgs e)
        {
            while (commandBox.Items.Count > 0)
            {
                commandBox.Items.RemoveAt(0);
            }
            while (commandLines.Count > 0)
            {
                commandLines.RemoveAt(0);
            }
        }

        private void add_command(string register, string value)
        {
            Control RX = this.Controls.Find(register.Substring(0, 1) + "XL", true)[0];
            Control RL = this.Controls.Find(register.Substring(0, 1) + "LL", true)[0];
            Control RH = this.Controls.Find(register.Substring(0, 1) + "HL", true)[0];
            Control RXBin = this.Controls.Find(register.Substring(0, 1) + "XBinL", true)[0];
            int val = 0;
            try
            {
                val = Int32.Parse(value.Substring(0, value.Length));
            }
            catch
            {
                val = Int32.Parse(this.Controls.Find(value + "L", true)[0].Text);
            }
            if (register.Substring(1, 1).Equals("X"))
            {
                int tmp = Int32.Parse(RX.Text) + val;
                if (tmp > 65535)
                {
                    tmp = tmp - 65536;
                }
                RX.Text = tmp.ToString();
                string tmpBin = Convert.ToString(tmp, 2);
                while (tmpBin.Length < 16)
                {
                    tmpBin = "0" + tmpBin;
                }
                RXBin.Text = tmpBin;
                RH.Text = Convert.ToInt32(tmpBin.Substring(0, 8), 2).ToString();
                RL.Text = Convert.ToInt32(tmpBin.Substring(8, 8), 2).ToString();
            }
            else if (register.Substring(1, 1).Equals("L"))
            {
                int tmp = Int32.Parse(RL.Text) + val;
                if (tmp > 255)
                {
                    tmp = tmp - 256;
                }
                RL.Text = tmp.ToString();
                string tmpRL = Convert.ToString(tmp, 2);
                while (tmpRL.Length < 8)
                {
                    tmpRL = "0" + tmpRL;
                }
                RXBin.Text = RXBin.Text.Substring(0, 8) + tmpRL;
                RX.Text = Convert.ToInt32(RXBin.Text, 2).ToString();
            }
            else if (register.Substring(1, 1).Equals("H"))
            {
                int tmp = Int32.Parse(RH.Text) + val;
                if (tmp > 255)
                {
                    tmp = tmp - 256;
                }
                RH.Text = tmp.ToString();
                string tmpRH = Convert.ToString(tmp, 2);
                while (tmpRH.Length < 8)
                {
                    tmpRH = "0" + tmpRH;
                }
                RXBin.Text = tmpRH + RXBin.Text.Substring(8, 8);
                RX.Text = Convert.ToInt32(RXBin.Text, 2).ToString();
            }

        }

        private void sub_command(string register, string value)
        {
            Control RX = this.Controls.Find(register.Substring(0, 1) + "XL", true)[0];
            Control RL = this.Controls.Find(register.Substring(0, 1) + "LL", true)[0];
            Control RH = this.Controls.Find(register.Substring(0, 1) + "HL", true)[0];
            Control RXBin = this.Controls.Find(register.Substring(0, 1) + "XBinL", true)[0];
            int val = 0;
            try
            {
                val = Int32.Parse(value.Substring(0, value.Length));
            }
            catch
            {
                val = Int32.Parse(this.Controls.Find(value + "L", true)[0].Text);
            }
            if (register.Substring(1, 1).Equals("X"))
            {
                int tmp = Int32.Parse(RX.Text) - val;
                if (tmp < 0)
                {
                    tmp = 65536 + tmp;
                }
                RX.Text = tmp.ToString();
                string tmpBin = Convert.ToString(tmp, 2);
                while (tmpBin.Length < 16)
                {
                    tmpBin = "0" + tmpBin;
                }
                RXBin.Text = tmpBin;
                RH.Text = Convert.ToInt32(tmpBin.Substring(0, 8), 2).ToString();
                RL.Text = Convert.ToInt32(tmpBin.Substring(8, 8), 2).ToString();
            }
            else if (register.Substring(1, 1).Equals("L"))
            {
                int tmp = Int32.Parse(RL.Text) - val;
                if (tmp < 0)
                {
                    tmp = 256 + tmp;
                }
                RL.Text = tmp.ToString();
                string tmpRL = Convert.ToString(tmp, 2);
                while (tmpRL.Length < 8)
                {
                    tmpRL = "0" + tmpRL;
                }
                RXBin.Text = RXBin.Text.Substring(0, 8) + tmpRL;
                RX.Text = Convert.ToInt32(RXBin.Text, 2).ToString();
            }
            else if (register.Substring(1, 1).Equals("H"))
            {
                int tmp = Int32.Parse(RL.Text) - val;
                if (tmp < 0)
                {
                    tmp = 256 + tmp;
                }
                RH.Text = tmp.ToString();
                string tmpRH = Convert.ToString(tmp, 2);
                while (tmpRH.Length < 8)
                {
                    tmpRH = "0" + tmpRH;
                }
                RXBin.Text = tmpRH + RXBin.Text.Substring(8, 8);
                RX.Text = Convert.ToInt32(RXBin.Text, 2).ToString();
            }

        }

        private void mov_command(string register, string value)
        {
            Control RX = this.Controls.Find(register.Substring(0, 1) + "XL", true)[0];
            Control RL = this.Controls.Find(register.Substring(0, 1) + "LL", true)[0];
            Control RH = this.Controls.Find(register.Substring(0, 1) + "HL", true)[0];
            Control RXBin = this.Controls.Find(register.Substring(0, 1) + "XBinL", true)[0];
            int val = 0;
            try
            {
                val = Int32.Parse(value.Substring(0, value.Length));
            }
            catch
            {
                val = Int32.Parse(this.Controls.Find(value + "L", true)[0].Text);
            }
            if (register.Substring(1, 1).Equals("X"))
            {
                if (val > 65535)
                {
                    val = 65535;
                }

                RX.Text = val.ToString();
                string tmpBin = Convert.ToString(val, 2);
                while (tmpBin.Length < 16)
                {
                    tmpBin = "0" + tmpBin;
                }
                RXBin.Text = tmpBin;
                RH.Text = Convert.ToInt32(tmpBin.Substring(0, 8), 2).ToString();
                RL.Text = Convert.ToInt32(tmpBin.Substring(8, 8), 2).ToString();
            }
            else if (register.Substring(1, 1).Equals("L"))
            {
                if (val > 255)
                {
                    val = 255;
                }

                RL.Text = val.ToString();
                string tmpRL = Convert.ToString(val, 2);
                while (tmpRL.Length < 8)
                {
                    tmpRL = "0" + tmpRL;
                }
                RXBin.Text = RXBin.Text.Substring(0, 8) + tmpRL;
                RX.Text = Convert.ToInt32(RXBin.Text, 2).ToString();
            }
            else if (register.Substring(1, 1).Equals("H"))
            {
                if (val > 255)
                {
                    val = 255;
                }

                RH.Text = val.ToString();
                string tmpRH = Convert.ToString(val, 2);
                while (tmpRH.Length < 8)
                {
                    tmpRH = "0" + tmpRH;
                }
                RXBin.Text = tmpRH + RXBin.Text.Substring(8, 8);
                RX.Text = Convert.ToInt32(RXBin.Text, 2).ToString();
            }

        }

        private void clear_registers()
        {
            foreach (Label l in registersData.Controls.OfType<Label>())
            {

                if (l.Name.Substring(l.Name.Length - 1, 1).Equals("L"))
                {
                    if (l.Name.Length == 3)
                    {
                        l.Text = "0";
                    }
                    else
                    {
                        l.Text = "0000000000000000";
                    }

                }

            }


        }
        private void push_command(string register)
        {
            if (SP >= 0)
            {
                this.stack[SP] = Convert.ToUInt16(this.Controls.Find(register + "L", true)[0].Text);
                SP--;
                stackBox.Items.Add(this.Controls.Find(register + "L", true)[0].Text);
            }
            else
            {
                this.execute = false;
                MessageBox.Show("Stos jest pełny!");
            }
        }
        private void pop_command(string register)
        {

            if (SP <= 98)
            {
                SP++;

                mov_command(register, this.stack[SP].ToString() + " ");
                stackBox.Items.RemoveAt(stackBox.Items.Count - 1);
            }
            else
            {
                this.execute = false;
                MessageBox.Show("Stos jest pusty!");
            }
        }
        private void int_command()
        {

            if (AHL.Text.Equals("42"))
            {
                DateTime now = DateTime.Now;
                mov_command("DH", now.Month.ToString());
                mov_command("DL", now.Day.ToString());
                mov_command("CX", now.Year.ToString());
            }
            else if (AHL.Text.Equals("44"))
            {
                DateTime now = DateTime.Now;
                mov_command("CH", now.Hour.ToString());
                mov_command("CL", now.Minute.ToString());
                mov_command("DH", now.Second.ToString());
            }
            else if (AHL.Text.Equals("0"))
            {
                Application.Exit();
            }
            else if (AHL.Text.Equals("25"))
            {
                mov_command("AL", Encoding.ASCII.GetBytes(Path.GetPathRoot(Environment.SystemDirectory).Substring(0, 1))[0].ToString());
            }
            else if (AHL.Text.Equals("4"))
            {
                Cursor.Position = new Point(Convert.ToUInt16(CXL.Text), Convert.ToUInt16(DXL.Text));
            }
            else if (AHL.Text.Equals("2"))
            {
                consoleBox.Text = consoleBox.Text + Convert.ToChar(Convert.ToUInt16(DLL.Text));
            }
            else if (AHL.Text.Equals("1"))
            {
                if (stepMode.Checked)
                {
                    commandBox.SelectedIndex = --stepModeIndex;
                }
                inputSign = true;
                currentConsoleText = consoleBox.Text;
                consoleBox.ReadOnly = false;
                inputSignTimer.Start();
            }
        }
        private void read_line(int i)
        {
            string line = commandBox.Items[i].ToString();
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            line = regex.Replace(line, " ");
            string[] subs = line.Split(" ");
            if (subs.Length == 4)
            {
                if (subs[1].Equals("ADD"))
                {
                    add_command(subs[2], subs[3]);

                }
                else if (subs[1].Equals("SUB"))
                {
                    sub_command(subs[2], subs[3]);

                }
                else if (subs[1].Equals("MOV"))
                {
                    mov_command(subs[2], subs[3]);

                }

            }
            else if (subs.Length == 3)
            {
                if (subs[1].Equals("INT"))
                {
                    int_command();

                }
                else if (subs[1].Equals("PUSH"))
                {
                    push_command(subs[2]);
                }
                else if (subs[1].Equals("POP"))
                {
                    pop_command(subs[2]);
                }
            }

        }
        private void main_loop()
        {
            while (wholeModeIndex < commandBox.Items.Count)
            {
                read_line(wholeModeIndex);
                if (!this.execute)
                {
                    break;
                }
                wholeModeIndex++;
                if (inputSign)
                {
                    break;
                }
            }
            MessageBox.Show("Koniec programu");
        }
        private void runB_Click(object sender, EventArgs e)
        {
            stackBox.Items.Clear();
            commandBox.ClearSelected();
            consoleBox.Text = "";
            this.execute = true;
            SP = 99;
            inputSign = false;
            wholeModeIndex = 0;
            clear_registers();
            main_loop();
        }

        private void eraseLastB_Click(object sender, EventArgs e)
        {
            if (commandLines.Count > 0)
            {
                commandBox.Items.RemoveAt(commandBox.Items.Count - 1);
                commandLines.RemoveAt(commandLines.Count - 1);
            }

        }

        private void eraseSelectedB_Click(object sender, EventArgs e)
        {
            int si = commandBox.SelectedIndex;
            if (si != -1)
            {
                commandBox.Items.RemoveAt(si);
                commandLines.RemoveAt(si);
                update_list();
                if (si > commandBox.Items.Count - 1)
                {
                    commandBox.SelectedIndex = si - 1;
                }
                else
                {
                    commandBox.SelectedIndex = si;
                }
            }
        }

        private void startB_Click(object sender, EventArgs e)
        {
            stackBox.Items.Clear();
            consoleBox.Text = "";
            this.execute = true;
            this.SP = 99;
            clear_registers();
            pushCommandB.Enabled = false;
            eraseSelectedB.Enabled = false;
            pushOnEndB.Enabled = false;
            eraseLastB.Enabled = false;
            eraseAllB.Enabled = false;
            runB.Enabled = false;
            startB.Enabled = false;
            nextB.Enabled = true;
            finishB.Enabled = true;
            if (commandBox.Items.Count <= 0)
            {
                MessageBox.Show("Koniec programu");
                finishB_Click(sender, e);
            }
            else
            {
                commandBox.SelectedIndex = 0;
                stepModeIndex = commandBox.SelectedIndex;
            }
        }

        private void nextB_Click(object sender, EventArgs e)
        {
            if (!inputSign)
            {
                read_line(stepModeIndex);
                stepModeIndex++;
                if (stepModeIndex < commandBox.Items.Count)
                {
                    commandBox.SelectedIndex = stepModeIndex;
                }
                else
                {
                    if (!this.execute)
                    {
                        finishB_Click(sender, e);
                    }
                    else
                    {
                        if (!(AHL.Text == "1" && inputSign))
                        {
                            MessageBox.Show("Koniec programu");
                            finishB_Click(sender, e);
                        }

                    }

                }
            }


        }

        private void finishB_Click(object sender, EventArgs e)
        {
            inputSignTimer.Stop();
            inputSign = false;
            pushCommandB.Enabled = true;
            eraseSelectedB.Enabled = true;
            pushOnEndB.Enabled = true;
            eraseLastB.Enabled = true;
            eraseAllB.Enabled = true;
            startB.Enabled = true;
            nextB.Enabled = false;
            finishB.Enabled = false;
        }

        private void saveB_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text files|*.txt";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                using var sw = new StreamWriter(saveFileDialog1.FileName);
                for (int i = 0; i < commandLines.Count; i++)
                {
                    if (commandLines[i].second == null)
                    {
                        sw.WriteLine(commandLines[i].command + "    " + commandLines[i].first);
                    }
                    else
                    {
                        sw.WriteLine(commandLines[i].command + "    " + commandLines[i].first + ",    " + commandLines[i].second);
                    }


                }
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private bool loadLines(string line, object sender, EventArgs e)
        {
            cl.command = null;
            cl.first = null;
            cl.second = null;
            bool valid = true;
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            line = regex.Replace(line, " ");
            string[] subs = line.Split(" ");
            if (subs.Length == 3)
            {
                if (subs[0].Equals("ADD") || subs[0].Equals("SUB") || subs[0].Equals("MOV"))
                {

                    cl.command = subs[0];



                }
                else valid = false;
            }
            else if (subs.Length == 2)
            {
                if (subs[0].Equals("PUSH") || subs[0].Equals("POP") || subs[0].Equals("INT"))
                {

                    cl.command = subs[0];
                    if (subs[0].Equals("INT"))
                    {
                        if (subs[1].Equals("21H"))
                        {
                            cl.first = subs[1];
                            commandLines.Add(cl);
                            commandBox.Items.Add((commandBox.Items.Count + 1).ToString() + ".    " + cl.command + "    " + cl.first);
                            return true;
                        }
                        else
                        {
                            valid = false;
                        }
                    }
                }
                else valid = false;

            }
            else valid = false;

            if (valid && subs[1].Length == 3 && subs[1].Substring(2, 1).Equals(",") && (subs[1].Substring(0, 1).Equals("A") || subs[1].Substring(0, 1).Equals("B") || subs[1].Substring(0, 1).Equals("C") || subs[1].Substring(0, 1).Equals("D")))
            {
                if (subs[1].Substring(1, 1).Equals("X") || subs[1].Substring(1, 1).Equals("H") || subs[1].Substring(1, 1).Equals("L"))
                {
                    cl.first = subs[1].Substring(0, subs[1].Length - 1);

                }
                else valid = false;

            }
            else valid = false;
            if (valid && subs.Length == 3)
            {
                try
                {
                    int tmp = Int32.Parse(subs[2].Substring(0, subs[2].Length));
                    cl.second = subs[2].Substring(0, subs[2].Length);
                }
                catch
                {
                    if (subs[2].Length == 2)
                    {
                        if (subs[2].Substring(0, 1).Equals("A") || subs[2].Substring(0, 1).Equals("B") || subs[2].Substring(0, 1).Equals("C") || subs[2].Substring(0, 1).Equals("D"))
                        {
                            if (subs[2].Substring(1, 1).Equals("X") || subs[2].Substring(1, 1).Equals("H") || subs[2].Substring(1, 1).Equals("L"))
                            {
                                cl.second = subs[2];
                            }
                            else valid = false;
                        }
                        else valid = false;
                    }
                    else valid = false;
                }
            }



            if (!valid)
            {
                MessageBox.Show("Błąd w składni w linijce " + (commandBox.Items.Count + 1) + "!");
                return false;
            }
            else
            {

                commandLines.Add(cl);
                if (cl.second == null)
                {
                    commandBox.Items.Add((commandBox.Items.Count + 1).ToString() + ".    " + cl.command + "    " + cl.first);
                }
                else
                {
                    commandBox.Items.Add((commandBox.Items.Count + 1).ToString() + ".    " + cl.command + "    " + cl.first + ",    " + cl.second);
                }
            }
            return true;
        }

        private void loadB_Click(object sender, EventArgs e)
        {


            OpenFileDialog loadDialog1 = new OpenFileDialog();
            loadDialog1.Filter = "Text files|*.txt";
            loadDialog1.Title = "Load an Image File";
            loadDialog1.ShowDialog();
            if (loadDialog1.FileName != "")
            {
                eraseAllB_Click(sender, e);
                String line;
                using var r = new StreamReader(loadDialog1.FileName);
                bool v;
                while (true)
                {
                    line = r.ReadLine();
                    if (line == null)
                        break;
                    v = loadLines(line, sender, e);
                    if (!v)
                    {

                        eraseAllB_Click(sender, e);
                        break;
                    }

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program służący do symulacji mikroprocesora. Umożliwia on operacje przenoszenia, dodawania lub odejmowania bitów pomiędzy " +
                "rejestrami. Budowa programu odbywa się przez wciśnięcie odpowiednich przycisków, umożliwiających zarówno dodawanie kolejnych linijek kodu " +
                "jak i ich usuwanie. Program można zapisać oraz wczytywać (plik powinien mieć rozszerzenie .txt). W przypadku błędu w składni, wczytywanego pliku, program " +
                "wyświetli odpowiedni komunikat oraz zakończy wczytywanie.", "Pomoc");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CommandList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CommandList.SelectedItem.Equals("Podstawowe funkcje"))
            {
                panel1.Enabled = true;
                panel2.Enabled = true;
                Register1.Enabled = true;
                if (registerMode.Checked == true)
                {
                    Register2.Enabled = true;

                }
                foreach (RadioButton c in Register1.Controls.OfType<RadioButton>())
                {
                    if (!c.Text.Substring(1, 1).Equals("X"))
                    {
                        c.Enabled = true;
                    }
                }
            }
            else
            {
                panel1.Enabled = false;
                panel2.Enabled = false;
                if (registerMode.Checked == true)
                {
                    Register2.Enabled = false;

                }
                if (CommandList.SelectedItem.Equals("POP") || CommandList.SelectedItem.Equals("PUSH"))
                {
                    Register1.Enabled = true;
                    bool tmp = false;
                    foreach (RadioButton c in Register1.Controls.OfType<RadioButton>())
                    {
                        if (!c.Text.Substring(1, 1).Equals("X"))
                        {

                            c.Enabled = false;
                        }
                        if (c.Text.Substring(1, 1).Equals("X") && c.Checked == true && !tmp)
                        {
                            tmp = true;
                        }
                    }
                    if (!tmp)
                    {
                        AX1.Checked = true;

                    }
                }
                else
                {
                    Register1.Enabled = false;
                }

            }
        }

        private void inputSignTimer_Tick(object sender, EventArgs e)
        {
            if (!currentConsoleText.Equals(consoleBox.Text))
            {
                consoleBox.ReadOnly = true;
                inputSign = false;
                inputSignTimer.Stop();
                if (currentConsoleText.Length < consoleBox.Text.Length)
                {

                    if (consoleBox.Text.Length - currentConsoleText.Length == 2)
                    {
                        mov_command("AL", "13");
                    }
                    else
                    {
                        string tmp = consoleBox.Text;
                        mov_command("AL", Encoding.ASCII.GetBytes(tmp.Substring(currentConsoleText.Length, 1))[0].ToString());
                    }

                }
                else
                {
                    if (currentConsoleText.Length - consoleBox.Text.Length == 1)
                    {
                        mov_command("AL", "8");
                    }
                    consoleBox.Text = currentConsoleText;
                }
                stepModeIndex++;
                currentConsoleText = "";
                if (wholeMode.Checked)
                {
                    main_loop();
                }
                else
                {
                    if (stepModeIndex == commandBox.Items.Count)
                    {
                        MessageBox.Show("Koniec programu");
                        finishB_Click(sender, e);
                    }
                    else
                    {
                        commandBox.SelectedIndex = stepModeIndex;
                    }
                }

            }
        }

        private void commandBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
