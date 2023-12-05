
namespace ChatTCPFormExample
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonLoadLE_eql = new System.Windows.Forms.Button();
            this.buttonSetValueArrayV2 = new System.Windows.Forms.Button();
            this.buttonSetValueArray = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(273, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(292, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(14, 83);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(588, 346);
            this.textBox2.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(406, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "LE";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "|*.le; *.txt";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(507, 11);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "TXT";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonLoadLE_eql
            // 
            this.buttonLoadLE_eql.Location = new System.Drawing.Point(406, 39);
            this.buttonLoadLE_eql.Name = "buttonLoadLE_eql";
            this.buttonLoadLE_eql.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadLE_eql.TabIndex = 6;
            this.buttonLoadLE_eql.Text = "LoadLE=";
            this.buttonLoadLE_eql.UseVisualStyleBackColor = true;
            this.buttonLoadLE_eql.Click += new System.EventHandler(this.buttonLoadLE_eql_Click);
            // 
            // buttonSetValueArrayV2
            // 
            this.buttonSetValueArrayV2.Location = new System.Drawing.Point(174, 57);
            this.buttonSetValueArrayV2.Name = "buttonSetValueArrayV2";
            this.buttonSetValueArrayV2.Size = new System.Drawing.Size(112, 20);
            this.buttonSetValueArrayV2.TabIndex = 24;
            this.buttonSetValueArrayV2.Text = "SetValueArrayV2";
            this.buttonSetValueArrayV2.UseVisualStyleBackColor = true;
            this.buttonSetValueArrayV2.Click += new System.EventHandler(this.buttonSetValueArrayV2_Click);
            // 
            // buttonSetValueArray
            // 
            this.buttonSetValueArray.Location = new System.Drawing.Point(56, 57);
            this.buttonSetValueArray.Name = "buttonSetValueArray";
            this.buttonSetValueArray.Size = new System.Drawing.Size(112, 20);
            this.buttonSetValueArray.TabIndex = 23;
            this.buttonSetValueArray.Text = "SetValueArray";
            this.buttonSetValueArray.UseVisualStyleBackColor = true;
            this.buttonSetValueArray.Click += new System.EventHandler(this.buttonSetValueArray_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 441);
            this.Controls.Add(this.buttonSetValueArrayV2);
            this.Controls.Add(this.buttonSetValueArray);
            this.Controls.Add(this.buttonLoadLE_eql);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(630, 480);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(630, 480);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonLoadLE_eql;
        private System.Windows.Forms.Button buttonSetValueArrayV2;
        private System.Windows.Forms.Button buttonSetValueArray;
    }
}

