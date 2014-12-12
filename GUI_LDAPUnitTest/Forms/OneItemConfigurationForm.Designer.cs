namespace GUI_LDAPUnitTest.Forms
{
	partial class OneItemConfigurationForm
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
			this.oneItemConfigurationLabel = new System.Windows.Forms.Label();
			this.oneItemConfigurationTextBox = new System.Windows.Forms.TextBox();
			this.oneItemConfigurationButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// oneItemConfigurationLabel
			// 
			this.oneItemConfigurationLabel.AutoSize = true;
			this.oneItemConfigurationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.oneItemConfigurationLabel.Location = new System.Drawing.Point(12, 9);
			this.oneItemConfigurationLabel.Name = "oneItemConfigurationLabel";
			this.oneItemConfigurationLabel.Size = new System.Drawing.Size(48, 16);
			this.oneItemConfigurationLabel.TabIndex = 0;
			this.oneItemConfigurationLabel.Text = "default";
			// 
			// oneItemConfigurationTextBox
			// 
			this.oneItemConfigurationTextBox.Location = new System.Drawing.Point(15, 28);
			this.oneItemConfigurationTextBox.Name = "oneItemConfigurationTextBox";
			this.oneItemConfigurationTextBox.Size = new System.Drawing.Size(262, 20);
			this.oneItemConfigurationTextBox.TabIndex = 1;
			// 
			// oneItemConfigurationButton
			// 
			this.oneItemConfigurationButton.Location = new System.Drawing.Point(99, 100);
			this.oneItemConfigurationButton.Name = "oneItemConfigurationButton";
			this.oneItemConfigurationButton.Size = new System.Drawing.Size(108, 29);
			this.oneItemConfigurationButton.TabIndex = 2;
			this.oneItemConfigurationButton.Text = "OK";
			this.oneItemConfigurationButton.UseVisualStyleBackColor = true;
			this.oneItemConfigurationButton.Click += new System.EventHandler(this.oneItemConfigurationButton_Click);
			// 
			// oneItemConfigurationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(312, 141);
			this.Controls.Add(this.oneItemConfigurationButton);
			this.Controls.Add(this.oneItemConfigurationTextBox);
			this.Controls.Add(this.oneItemConfigurationLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "oneItemConfigurationForm";
			this.Text = "Setup";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label oneItemConfigurationLabel;
		private System.Windows.Forms.TextBox oneItemConfigurationTextBox;
		private System.Windows.Forms.Button oneItemConfigurationButton;
	}
}