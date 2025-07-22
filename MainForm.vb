Public Class MainForm
    
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configurar la apariencia del formulario principal
        Me.Text = "Sistema de Fidelidad de Clientes - Supermercado"
        Me.WindowState = FormWindowState.Maximized
        Me.BackColor = Color.FromArgb(240, 240, 240)
        
        ' Probar conexión a la base de datos al iniciar
        If Not DatabaseHelper.TestConnection() Then
            MessageBox.Show("Advertencia: No se pudo conectar a la base de datos. Verifique la configuración.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        
        ' Configurar controles
        SetupControls()
    End Sub
    
    Private Sub SetupControls()
        ' Limpiar controles existentes
        Me.Controls.Clear()
        
        ' Panel principal
        Dim mainPanel As New Panel()
        mainPanel.Dock = DockStyle.Fill
        mainPanel.BackColor = Color.White
        mainPanel.Padding = New Padding(50)
        
        ' Título principal
        Dim titleLabel As New Label()
        titleLabel.Text = "SISTEMA DE FIDELIDAD DE CLIENTES"
        titleLabel.Font = New Font("Segoe UI", 24, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(51, 51, 51)
        titleLabel.TextAlign = ContentAlignment.MiddleCenter
        titleLabel.Dock = DockStyle.Top
        titleLabel.Height = 80
        
        ' Subtítulo
        Dim subtitleLabel As New Label()
        subtitleLabel.Text = "Gestión completa de puntos y recompensas"
        subtitleLabel.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        subtitleLabel.ForeColor = Color.FromArgb(102, 102, 102)
        subtitleLabel.TextAlign = ContentAlignment.MiddleCenter
        subtitleLabel.Dock = DockStyle.Top
        subtitleLabel.Height = 40
        
        ' Panel para botones
        Dim buttonPanel As New TableLayoutPanel()
        buttonPanel.RowCount = 2
        buttonPanel.ColumnCount = 2
        buttonPanel.Dock = DockStyle.Fill
        buttonPanel.Padding = New Padding(100, 50, 100, 100)
        buttonPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
        buttonPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
        buttonPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
        buttonPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
        
        ' Crear botones de navegación
        Dim btnCustomers As Button = CreateNavigationButton("GESTIÓN DE CLIENTES", "Agregar, editar y eliminar clientes")
        Dim btnPurchases As Button = CreateNavigationButton("REGISTRO DE COMPRAS", "Registrar compras y asignar puntos")
        Dim btnRedemptions As Button = CreateNavigationButton("CANJE DE PUNTOS", "Canjear puntos por recompensas")
        Dim btnReports As Button = CreateNavigationButton("REPORTES", "Ver reportes de puntos por cliente")
        
        ' Agregar eventos a los botones
        AddHandler btnCustomers.Click, AddressOf BtnCustomers_Click
        AddHandler btnPurchases.Click, AddressOf BtnPurchases_Click
        AddHandler btnRedemptions.Click, AddressOf BtnRedemptions_Click
        AddHandler btnReports.Click, AddressOf BtnReports_Click
        
        ' Agregar botones al panel
        buttonPanel.Controls.Add(btnCustomers, 0, 0)
        buttonPanel.Controls.Add(btnPurchases, 1, 0)
        buttonPanel.Controls.Add(btnRedemptions, 0, 1)
        buttonPanel.Controls.Add(btnReports, 1, 1)
        
        ' Agregar controles al panel principal
        mainPanel.Controls.Add(buttonPanel)
        mainPanel.Controls.Add(subtitleLabel)
        mainPanel.Controls.Add(titleLabel)
        
        ' Agregar panel principal al formulario
        Me.Controls.Add(mainPanel)
        
        ' Panel de estado en la parte inferior
        Dim statusPanel As New Panel()
        statusPanel.Height = 30
        statusPanel.Dock = DockStyle.Bottom
        statusPanel.BackColor = Color.FromArgb(51, 51, 51)
        
        Dim statusLabel As New Label()
        statusLabel.Text = "Sistema listo - Conexión a base de datos: " & If(DatabaseHelper.TestConnection(), "Conectado", "Desconectado")
        statusLabel.ForeColor = Color.White
        statusLabel.Font = New Font("Segoe UI", 9)
        statusLabel.Dock = DockStyle.Fill
        statusLabel.TextAlign = ContentAlignment.MiddleLeft
        statusLabel.Padding = New Padding(10, 5, 10, 5)
        
        statusPanel.Controls.Add(statusLabel)
        Me.Controls.Add(statusPanel)
    End Sub
    
    Private Function CreateNavigationButton(title As String, description As String) As Button
        Dim btn As New Button()
        btn.Text = title & vbCrLf & vbCrLf & description
        btn.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        btn.BackColor = Color.FromArgb(70, 130, 180)
        btn.ForeColor = Color.White
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.Dock = DockStyle.Fill
        btn.Margin = New Padding(10)
        btn.Cursor = Cursors.Hand
        btn.TextAlign = ContentAlignment.MiddleCenter
        
        ' Efectos hover
        AddHandler btn.MouseEnter, Sub(sender, e)
                                       btn.BackColor = Color.FromArgb(100, 149, 237)
                                   End Sub
        
        AddHandler btn.MouseLeave, Sub(sender, e)
                                       btn.BackColor = Color.FromArgb(70, 130, 180)
                                   End Sub
        
        Return btn
    End Function
    
    Private Sub BtnCustomers_Click(sender As Object, e As EventArgs)
        Try
            Dim customersForm As New CustomersForm()
            customersForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error al abrir formulario de clientes: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub BtnPurchases_Click(sender As Object, e As EventArgs)
        Try
            Dim purchasesForm As New PurchasesForm()
            purchasesForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error al abrir formulario de compras: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub BtnRedemptions_Click(sender As Object, e As EventArgs)
        Try
            Dim redemptionsForm As New RedemptionsForm()
            redemptionsForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error al abrir formulario de canjes: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub BtnReports_Click(sender As Object, e As EventArgs)
        Try
            Dim reportsForm As New ReportsForm()
            reportsForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error al abrir formulario de reportes: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim result As DialogResult = MessageBox.Show("¿Está seguro que desea salir del sistema?", "Confirmar Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        
        If result = DialogResult.No Then
            e.Cancel = True
        End If
    End Sub
End Class
