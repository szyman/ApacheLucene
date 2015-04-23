namespace EDwI_lab1.View
{
    partial class InternetBotForm
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
            this.SearchListView = new System.Windows.Forms.ListView();
            this.WebPageColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.HitsColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FindButton = new System.Windows.Forms.Button();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SearchListView
            // 
            this.SearchListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.WebPageColumn,
            this.HitsColumn});
            this.SearchListView.Location = new System.Drawing.Point(34, 70);
            this.SearchListView.Name = "SearchListView";
            this.SearchListView.Size = new System.Drawing.Size(216, 169);
            this.SearchListView.TabIndex = 5;
            this.SearchListView.UseCompatibleStateImageBehavior = false;
            this.SearchListView.View = System.Windows.Forms.View.Details;
            // 
            // WebPageColumn
            // 
            this.WebPageColumn.Text = "Webpage";
            this.WebPageColumn.Width = 140;
            // 
            // HitsColumn
            // 
            this.HitsColumn.Text = "Hits";
            // 
            // FindButton
            // 
            this.FindButton.Location = new System.Drawing.Point(175, 21);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(75, 23);
            this.FindButton.TabIndex = 4;
            this.FindButton.Text = "Find";
            this.FindButton.UseVisualStyleBackColor = true;
            this.FindButton.Click += new System.EventHandler(this.FindButton_Click);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(55, 21);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(100, 20);
            this.SearchTextBox.TabIndex = 3;
            // 
            // InternetBotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.SearchListView);
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.SearchTextBox);
            this.Name = "InternetBotForm";
            this.Text = "InternetBotForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView SearchListView;
        private System.Windows.Forms.ColumnHeader WebPageColumn;
        private System.Windows.Forms.ColumnHeader HitsColumn;
        private System.Windows.Forms.Button FindButton;
        private System.Windows.Forms.TextBox SearchTextBox;
    }
}