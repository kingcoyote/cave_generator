namespace CellularAutomata
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picOutput = new System.Windows.Forms.PictureBox();
            this.txtAccept = new System.Windows.Forms.Label();
            this.dgvRules = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.numTunnelDistance = new System.Windows.Forms.NumericUpDown();
            this.numTunnelWidth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numTunnelClearance = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numLocalThreshold = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numFinalSize = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numFinalIterations = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.minimumOpen = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.maximumOpen = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvRulesGridSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRules_Iterations = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRules_NeighborhoodSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRules_NeighborhoodThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRules_Initialization = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTunnelDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTunnelWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTunnelClearance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLocalThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFinalSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFinalIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimumOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumOpen)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(877, 439);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.StartAutomata);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgvRules);
            this.groupBox1.Location = new System.Drawing.Point(466, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(490, 276);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rules";
            // 
            // picOutput
            // 
            this.picOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.picOutput.Location = new System.Drawing.Point(12, 12);
            this.picOutput.Name = "picOutput";
            this.picOutput.Size = new System.Drawing.Size(445, 445);
            this.picOutput.TabIndex = 10;
            this.picOutput.TabStop = false;
            this.picOutput.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawBox);
            // 
            // txtAccept
            // 
            this.txtAccept.AutoSize = true;
            this.txtAccept.Location = new System.Drawing.Point(463, 439);
            this.txtAccept.Name = "txtAccept";
            this.txtAccept.Size = new System.Drawing.Size(16, 13);
            this.txtAccept.TabIndex = 11;
            this.txtAccept.Text = "---";
            this.txtAccept.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvRules
            // 
            this.dgvRules.AllowUserToResizeRows = false;
            this.dgvRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvRulesGridSize,
            this.dgvRules_Iterations,
            this.dgvRules_NeighborhoodSize,
            this.dgvRules_NeighborhoodThreshold,
            this.dgvRules_Initialization});
            this.dgvRules.Location = new System.Drawing.Point(6, 19);
            this.dgvRules.Name = "dgvRules";
            this.dgvRules.Size = new System.Drawing.Size(484, 251);
            this.dgvRules.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(577, 296);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Tunnel Distance";
            // 
            // numTunnelDistance
            // 
            this.numTunnelDistance.Location = new System.Drawing.Point(696, 294);
            this.numTunnelDistance.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numTunnelDistance.Name = "numTunnelDistance";
            this.numTunnelDistance.Size = new System.Drawing.Size(60, 20);
            this.numTunnelDistance.TabIndex = 13;
            this.numTunnelDistance.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // numTunnelWidth
            // 
            this.numTunnelWidth.Location = new System.Drawing.Point(696, 320);
            this.numTunnelWidth.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numTunnelWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTunnelWidth.Name = "numTunnelWidth";
            this.numTunnelWidth.Size = new System.Drawing.Size(60, 20);
            this.numTunnelWidth.TabIndex = 15;
            this.numTunnelWidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(577, 322);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Tunnel Width";
            // 
            // numTunnelClearance
            // 
            this.numTunnelClearance.Location = new System.Drawing.Point(696, 346);
            this.numTunnelClearance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTunnelClearance.Name = "numTunnelClearance";
            this.numTunnelClearance.Size = new System.Drawing.Size(60, 20);
            this.numTunnelClearance.TabIndex = 17;
            this.numTunnelClearance.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(577, 348);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Tunnel Initialization";
            // 
            // numLocalThreshold
            // 
            this.numLocalThreshold.Location = new System.Drawing.Point(896, 346);
            this.numLocalThreshold.Name = "numLocalThreshold";
            this.numLocalThreshold.Size = new System.Drawing.Size(60, 20);
            this.numLocalThreshold.TabIndex = 23;
            this.numLocalThreshold.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(782, 348);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Final Local Threshold";
            // 
            // numFinalSize
            // 
            this.numFinalSize.Location = new System.Drawing.Point(896, 320);
            this.numFinalSize.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numFinalSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFinalSize.Name = "numFinalSize";
            this.numFinalSize.Size = new System.Drawing.Size(60, 20);
            this.numFinalSize.TabIndex = 21;
            this.numFinalSize.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(782, 322);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Final Local Size";
            // 
            // numFinalIterations
            // 
            this.numFinalIterations.Location = new System.Drawing.Point(896, 294);
            this.numFinalIterations.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numFinalIterations.Name = "numFinalIterations";
            this.numFinalIterations.Size = new System.Drawing.Size(60, 20);
            this.numFinalIterations.TabIndex = 19;
            this.numFinalIterations.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(782, 296);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Final Iterations";
            // 
            // minimumOpen
            // 
            this.minimumOpen.Location = new System.Drawing.Point(896, 372);
            this.minimumOpen.Name = "minimumOpen";
            this.minimumOpen.Size = new System.Drawing.Size(60, 20);
            this.minimumOpen.TabIndex = 25;
            this.minimumOpen.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(782, 374);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Minimum Open";
            // 
            // maximumOpen
            // 
            this.maximumOpen.Location = new System.Drawing.Point(896, 398);
            this.maximumOpen.Name = "maximumOpen";
            this.maximumOpen.Size = new System.Drawing.Size(60, 20);
            this.maximumOpen.TabIndex = 27;
            this.maximumOpen.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(782, 400);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Maximum Open";
            // 
            // dgvRulesGridSize
            // 
            this.dgvRulesGridSize.HeaderText = "Grid Size";
            this.dgvRulesGridSize.Name = "dgvRulesGridSize";
            this.dgvRulesGridSize.Width = 80;
            // 
            // dgvRules_Iterations
            // 
            this.dgvRules_Iterations.HeaderText = "Iterations";
            this.dgvRules_Iterations.Name = "dgvRules_Iterations";
            this.dgvRules_Iterations.Width = 60;
            // 
            // dgvRules_NeighborhoodSize
            // 
            this.dgvRules_NeighborhoodSize.HeaderText = "Local Size";
            this.dgvRules_NeighborhoodSize.Name = "dgvRules_NeighborhoodSize";
            // 
            // dgvRules_NeighborhoodThreshold
            // 
            this.dgvRules_NeighborhoodThreshold.HeaderText = "Local Threshold";
            this.dgvRules_NeighborhoodThreshold.Name = "dgvRules_NeighborhoodThreshold";
            this.dgvRules_NeighborhoodThreshold.Width = 110;
            // 
            // dgvRules_Initialization
            // 
            this.dgvRules_Initialization.HeaderText = "Initialization";
            this.dgvRules_Initialization.Name = "dgvRules_Initialization";
            this.dgvRules_Initialization.Width = 70;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 469);
            this.Controls.Add(this.maximumOpen);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.minimumOpen);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numLocalThreshold);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numFinalSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numFinalIterations);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numTunnelClearance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numTunnelWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numTunnelDistance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtAccept);
            this.Controls.Add(this.picOutput);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "CA-PCG Cave Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTunnelDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTunnelWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTunnelClearance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLocalThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFinalSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFinalIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimumOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumOpen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picOutput;
        private System.Windows.Forms.Label txtAccept;
        private System.Windows.Forms.DataGridView dgvRules;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numTunnelDistance;
        private System.Windows.Forms.NumericUpDown numTunnelWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numTunnelClearance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numLocalThreshold;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numFinalSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numFinalIterations;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown minimumOpen;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown maximumOpen;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvRulesGridSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvRules_Iterations;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvRules_NeighborhoodSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvRules_NeighborhoodThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvRules_Initialization;
    }
}

