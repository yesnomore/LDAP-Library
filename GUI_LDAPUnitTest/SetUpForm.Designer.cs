namespace GUI_LDAPUnitTest
{
	partial class SetUpForm
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
			this.finishSetUpButton = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.appConfigPathTextBox = new System.Windows.Forms.TextBox();
			this.appConfigPathLabel = new System.Windows.Forms.Label();
			this.appConfigPathButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// finishSetUpButton
			// 
			this.finishSetUpButton.Location = new System.Drawing.Point(166, 67);
			this.finishSetUpButton.Name = "finishSetUpButton";
			this.finishSetUpButton.Size = new System.Drawing.Size(144, 33);
			this.finishSetUpButton.TabIndex = 0;
			this.finishSetUpButton.Text = "OK";
			this.finishSetUpButton.UseVisualStyleBackColor = true;
			this.finishSetUpButton.Click += new System.EventHandler(this.finishSetUpButton_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// appConfigPathTextBox
			// 
			this.appConfigPathTextBox.Location = new System.Drawing.Point(97, 26);
			this.appConfigPathTextBox.Name = "appConfigPathTextBox";
			this.appConfigPathTextBox.Size = new System.Drawing.Size(282, 20);
			this.appConfigPathTextBox.TabIndex = 1;
			// 
			// appConfigPathLabel
			// 
			this.appConfigPathLabel.AutoSize = true;
			this.appConfigPathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.appConfigPathLabel.Location = new System.Drawing.Point(12, 27);
			this.appConfigPathLabel.Name = "appConfigPathLabel";
			this.appConfigPathLabel.Size = new System.Drawing.Size(79, 16);
			this.appConfigPathLabel.TabIndex = 2;
			this.appConfigPathLabel.Text = "Config Path:";
			// 
			// appConfigPathButton
			// 
			this.appConfigPathButton.Location = new System.Drawing.Point(373, 26);
			this.appConfigPathButton.Name = "appConfigPathButton";
			this.appConfigPathButton.Size = new System.Drawing.Size(26, 20);
			this.appConfigPathButton.TabIndex = 3;
			this.appConfigPathButton.Text = "...";
			this.appConfigPathButton.UseVisualStyleBackColor = true;
			this.appConfigPathButton.Click += new System.EventHandler(this.appConfigPathButton_Click);
			// 
			// SetUpForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(439, 112);
			this.Controls.Add(this.appConfigPathButton);
			this.Controls.Add(this.appConfigPathLabel);
			this.Controls.Add(this.appConfigPathTextBox);
			this.Controls.Add(this.finishSetUpButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "SetUpForm";
			this.Text = "Insert Path of LDAP Library Config File ";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button finishSetUpButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TextBox appConfigPathTextBox;
		private System.Windows.Forms.Label appConfigPathLabel;
		private System.Windows.Forms.Button appConfigPathButton;
	}
}