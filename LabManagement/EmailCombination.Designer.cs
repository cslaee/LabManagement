namespace LabManagement
{
    partial class EmailCombinations
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.send = new System.Windows.Forms.Button();
            this.combination = new System.Windows.Forms.TextBox();
            this.lockerNumber = new System.Windows.Forms.TextBox();
            this.emailAddress = new System.Windows.Forms.TextBox();
            this.lockNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.add = new System.Windows.Forms.Button();
            this.outGoingMessage = new System.Windows.Forms.TextBox();
            this.Clear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(529, 25);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(58, 23);
            this.send.TabIndex = 0;
            this.send.Text = "Send";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.Button_Send);
            // 
            // combination
            // 
            this.combination.Location = new System.Drawing.Point(175, 25);
            this.combination.Name = "combination";
            this.combination.Size = new System.Drawing.Size(65, 20);
            this.combination.TabIndex = 2;
            // 
            // lockerNumber
            // 
            this.lockerNumber.Location = new System.Drawing.Point(89, 25);
            this.lockerNumber.Name = "lockerNumber";
            this.lockerNumber.Size = new System.Drawing.Size(80, 20);
            this.lockerNumber.TabIndex = 3;
            // 
            // emailAddress
            // 
            this.emailAddress.Location = new System.Drawing.Point(246, 25);
            this.emailAddress.Name = "emailAddress";
            this.emailAddress.Size = new System.Drawing.Size(154, 20);
            this.emailAddress.TabIndex = 4;
            // 
            // lockNumber
            // 
            this.lockNumber.Location = new System.Drawing.Point(15, 25);
            this.lockNumber.Name = "lockNumber";
            this.lockNumber.Size = new System.Drawing.Size(68, 20);
            this.lockNumber.TabIndex = 5;
            this.lockNumber.TextChanged += new System.EventHandler(this.lockNumber_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Lock Number";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Email Content";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(246, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Email Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(175, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Combination";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(89, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Locker Number";
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(406, 23);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(53, 23);
            this.add.TabIndex = 11;
            this.add.Text = "Add";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // outGoingMessage
            // 
            this.outGoingMessage.Location = new System.Drawing.Point(15, 64);
            this.outGoingMessage.Multiline = true;
            this.outGoingMessage.Name = "outGoingMessage";
            this.outGoingMessage.Size = new System.Drawing.Size(572, 374);
            this.outGoingMessage.TabIndex = 12;
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(465, 23);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(58, 23);
            this.Clear.TabIndex = 13;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // EmailCombinations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 450);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.outGoingMessage);
            this.Controls.Add(this.add);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lockNumber);
            this.Controls.Add(this.emailAddress);
            this.Controls.Add(this.lockerNumber);
            this.Controls.Add(this.combination);
            this.Controls.Add(this.send);
            this.Name = "EmailCombinations";
            this.Load += new System.EventHandler(this.EmailCombinations_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button send;
        private System.Windows.Forms.TextBox combination;
        private System.Windows.Forms.TextBox lockerNumber;
        private System.Windows.Forms.TextBox emailAddress;
        private System.Windows.Forms.TextBox lockNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.TextBox outGoingMessage;
        private System.Windows.Forms.Button Clear;
    }
}

