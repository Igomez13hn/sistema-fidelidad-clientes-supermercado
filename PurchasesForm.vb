Imports System.Data.SqlClient

Public Class PurchasesForm
    
    Private Sub PurchasesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configurar formulario
        Me.Text = "Registro de Compras - Sistema de Fidelidad"
        Me.Size = New Size(800, 700)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(245, 245, 245)
        
        SetupControls()
        LoadCustomers()
        LoadPurchases()
    End Sub
    
    Private Sub SetupControls()
        Me.Controls.Clear()
        
        ' Panel principal
        Dim mainPanel As New Panel()
        mainPanel.Dock = DockStyle.Fill
        mainPanel.Padding = New Padding(20)
        
        ' Título
        Dim titleLabel As New Label()
        titleLabel.Text = "REGISTRO DE COMPRAS"
        titleLabel.Font = New Font("Segoe UI", 18, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(51, 51, 51)
        titleLabel.Height = 40
        titleLabel.Dock = DockStyle.Top
        
        ' Panel de formulario
        Dim formPanel As New Panel()
        formPanel.Height = 250
        formPanel.Dock = DockStyle.Top
        formPanel.BackColor = Color.White
        formPanel.Padding = New Padding(20)
        
        ' Selección de cliente
        Dim lblCustomer As New Label()
        lblCustomer.Text = "Cliente:"
        lblCustomer.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblCustomer.Location = New Point(20, 20)
        lblCustomer.Size = New Size(100, 25)
        
        Dim cmbCustomer As New ComboBox()
        cmbCustomer.Name = "cmbCustomer"
        cmbCustomer.Font = New Font("Segoe UI", 10)
        cmbCustomer.Location = New Point(130, 20)
        cmbCustomer.Size = New Size(300, 25)
        cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList
        AddHandler cmbCustomer.SelectedIndexChanged, AddressOf CmbCustomer_SelectedIndexChanged
        
        ' Puntos actuales del cliente
        Dim lblCurrentPoints As New Label()
        lblCurrentPoints.Text = "Puntos Actuales:"
        lblCurrentPoints.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblCurrentPoints.Location = New Point(450, 20)
        lblCurrentPoints.Size = New Size(120, 25)
        
        Dim lblPointsValue As New Label()
        lblPointsValue.Name = "lblPointsValue"
        lblPointsValue.Text = "0"
        lblPointsValue.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblPointsValue.ForeColor = Color.FromArgb(40, 167, 69)
        lblPointsValue.Location = New Point(580, 20)
        lblPointsValue.Size = New Size(100, 25)
        
        ' Monto de compra
        Dim lblAmount As New Label()
        lblAmount.Text = "Monto de Compra:"
        lblAmount.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblAmount.Location = New Point(20, 70)
        lblAmount.Size = New Size(120, 25)
        
        Dim txtAmount As New TextBox()
        txtAmount.Name = "txtAmount"
        txtAmount.Font = New Font("Segoe UI", 12)
        txtAmount.Location = New Point(150, 70)
        txtAmount.Size = New Size(150, 30)
        txtAmount.TextAlign = HorizontalAlignment.Right
        AddHandler txtAmount.TextChanged, AddressOf TxtAmount_TextChanged
        AddHandler txtAmount.KeyPress, AddressOf TxtAmount_KeyPress
        
        Dim lblCurrency As New Label()
        lblCurrency.Text = "$"
        lblCurrency.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblCurrency.Location = New Point(130, 70)
        lblCurrency.Size = New Size(20, 25)
        
        ' Puntos a ganar
        Dim lblEarnedPoints As New Label()
        lblEarnedPoints.Text = "Puntos a Ganar:"
        lblEarnedPoints.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblEarnedPoints.Location = New Point(320, 70)
        lblEarnedPoints.Size = New Size(120, 25)
        
        Dim lblEarnedValue As New Label()
        lblEarnedValue.Name = "lblEarnedValue"
        lblEarnedValue.Text = "0"
        lblEarnedValue.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        lblEarnedValue.ForeColor = Color.FromArgb(70, 130, 180)
        lblEarnedValue.Location = New Point(450, 70)
        lblEarnedValue.Size = New Size(100, 25)
        
        ' Información de cálculo
        Dim lblInfo As New Label()
        lblInfo.Text = "* Se otorga 1 punto por cada $1 de compra"
        lblInfo.Font = New Font("Segoe UI", 9, FontStyle.Italic)
        lblInfo.ForeColor = Color.FromArgb(108, 117, 125)
        lblInfo.Location = New Point(20, 110)
        lblInfo.Size = New Size(300, 20)
        
        ' Panel de botones
        Dim buttonPanel As New Panel()
        buttonPanel.Height = 50
        buttonPanel.Dock = DockStyle.Top
        buttonPanel.Padding = New Padding(20, 10, 20, 10)
        
        Dim btnRegister As Button = CreateButton("Registrar Compra", Color.FromArgb(40, 167, 69))
        btnRegister.Name = "btnRegister"
        btnRegister.Location = New Point(20, 10)
        btnRegister.Size = New Size(150, 30)
        AddHandler btnRegister.Click, AddressOf BtnRegister_Click
        
        Dim btnClear As Button = CreateButton("Limpiar", Color.FromArgb(108, 117, 125))
        btnClear.Name = "btnClear"
        btnClear.Location = New Point(180, 10)
        AddHandler btnClear.Click, AddressOf BtnClear_Click
        
        Dim btnBack As Button = CreateButton("Volver", Color.FromArgb(70, 130, 180))
        btnBack.Name = "btnBack"
        btnBack.Location = New Point(320, 10)
        AddHandler btnBack.Click, AddressOf BtnBack_Click
        
        ' DataGridView para mostrar compras recientes
        Dim lblRecentPurchases As New Label()
        lblRecentPurchases.Text = "COMPRAS RECIENTES"
        lblRecentPurchases.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblRecentPurchases.ForeColor = Color.FromArgb(51, 51, 51)
        lblRecentPurchases.Height = 30
        lblRecentPurchases.Dock = DockStyle.Top
        lblRecentPurchases.Padding = New Padding(0, 10, 0, 0)
        
        Dim dgvPurchases As New DataGridView()
        dgvPurchases.Name = "dgvPurchases"
        dgvPurchases.Dock = DockStyle.Fill
        dgvPurchases.BackgroundColor = Color.White
        dgvPurchases.BorderStyle = BorderStyle.None
        dgvPurchases.AllowUserToAddRows = False
        dgvPurchases.AllowUserToDeleteRows = False
        dgvPurchases.ReadOnly = True
        dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvPurchases.MultiSelect = False
        dgvPurchases.Font = New Font("Segoe UI", 9)
        dgvPurchases.RowHeadersVisible = False
        dgvPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        
        ' Agregar controles a los paneles
        formPanel.Controls.AddRange({lblCustomer, cmbCustomer, lblCurrentPoints, lblPointsValue, 
                                   lblAmount, lblCurrency, txtAmount, lblEarnedPoints, lblEarnedValue, lblInfo})
        buttonPanel.Controls.AddRange({btnRegister, btnClear, btnBack})
        
        ' Agregar paneles al panel principal
        mainPanel.Controls.Add(dgvPurchases)
        mainPanel.Controls.Add(lblRecentPurchases)
        mainPanel.Controls.Add(buttonPanel)
        mainPanel.Controls.Add(formPanel)
        mainPanel.Controls.Add(titleLabel)
        
        Me.Controls.Add(mainPanel)
    End Sub
    
    Private Function CreateButton(text As String, backColor As Color) As Button
        Dim btn As New Button()
        btn.Text = text
        btn.Size = New Size(130, 30)
        btn.BackColor = backColor
        btn.ForeColor = Color.White
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        btn.Cursor = Cursors.Hand
        Return btn
    End Function
    
    Private Sub LoadCustomers()
        Try
            Dim cmbCustomer As ComboBox = CType(Me.Controls.Find("cmbCustomer", True)(0), ComboBox)
            cmbCustomer.Items.Clear()
            
            Dim query As String = "SELECT CustomerID, Name, CurrentPoints FROM Customers ORDER BY Name"
            Dim reader As SqlDataReader = DatabaseHelper.ExecuteReader(query)
            
            If reader IsNot Nothing Then
                While reader.Read()
                    Dim item As New CustomerItem()
                    item.CustomerID = Convert.ToInt32(reader("CustomerID"))
                    item.Name = reader("Name").ToString()
                    item.CurrentPoints = Convert.ToInt32(reader("CurrentPoints"))
                    cmbCustomer.Items.Add(item)
                End While
                reader.Close()
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al cargar clientes: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub LoadPurchases()
        Try
            Dim query As String = "SELECT TOP 20 p.PurchaseID, c.Name, p.PurchaseDate, p.Amount, p.EarnedPoints " &
                                "FROM Purchases p INNER JOIN Customers c ON p.CustomerID = c.CustomerID " &
                                "ORDER BY p.PurchaseDate DESC"
            
            Dim dataTable As DataTable = DatabaseHelper.GetDataTable(query)
            
            If dataTable IsNot Nothing Then
                Dim dgv As DataGridView = CType(Me.Controls.Find("dgvPurchases", True)(0), DataGridView)
                dgv.DataSource = dataTable
                
                ' Configurar columnas
                dgv.Columns("PurchaseID").HeaderText = "ID Compra"
                dgv.Columns("PurchaseID").Width = 80
                dgv.Columns("Name").HeaderText = "Cliente"
                dgv.Columns("PurchaseDate").HeaderText = "Fecha"
                dgv.Columns("PurchaseDate").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
                dgv.Columns("Amount").HeaderText = "Monto"
                dgv.Columns("Amount").DefaultCellStyle.Format = "C2"
                dgv.Columns("EarnedPoints").HeaderText = "Puntos Ganados"
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al cargar compras: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub CmbCustomer_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim cmbCustomer As ComboBox = CType(sender, ComboBox)
            Dim lblPointsValue As Label = CType(Me.Controls.Find("lblPointsValue", True)(0), Label)
            
            If cmbCustomer.SelectedItem IsNot Nothing Then
                Dim selectedCustomer As CustomerItem = CType(cmbCustomer.SelectedItem, CustomerItem)
                lblPointsValue.Text = selectedCustomer.CurrentPoints.ToString()
            Else
                lblPointsValue.Text = "0"
            End If
            
        Catch ex As Exception
            ' Ignorar errores de selección
        End Try
    End Sub
    
    Private Sub TxtAmount_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim txtAmount As TextBox = CType(sender, TextBox)
            Dim lblEarnedValue As Label = CType(Me.Controls.Find("lblEarnedValue", True)(0), Label)
            
            Dim amount As Decimal
            If Decimal.TryParse(txtAmount.Text, amount) AndAlso amount >= 0 Then
                Dim earnedPoints As Integer = CInt(Math.Floor(amount))
                lblEarnedValue.Text = earnedPoints.ToString()
            Else
                lblEarnedValue.Text = "0"
            End If
            
        Catch ex As Exception
            ' Ignorar errores de conversión
        End Try
    End Sub
    
    Private Sub TxtAmount_KeyPress(sender As Object, e As KeyPressEventArgs)
        ' Solo permitir números, punto decimal y teclas de control
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c Then
            e.Handled = True
        End If
        
        ' Solo permitir un punto decimal
        If e.KeyChar = "."c AndAlso CType(sender, TextBox).Text.IndexOf("."c) > -1 Then
            e.Handled = True
        End If
    End Sub
    
    Private Sub BtnRegister_Click(sender As Object, e As EventArgs)
        Try
            Dim cmbCustomer As ComboBox = CType(Me.Controls.Find("cmbCustomer", True)(0), ComboBox)
            Dim txtAmount As TextBox = CType(Me.Controls.Find("txtAmount", True)(0), TextBox)
            
            ' Validaciones
            If cmbCustomer.SelectedItem Is Nothing Then
                MessageBox.Show("Seleccione un cliente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbCustomer.Focus()
                Return
            End If
            
            Dim amount As Decimal
            If Not Decimal.TryParse(txtAmount.Text, amount) OrElse amount <= 0 Then
                MessageBox.Show("Ingrese un monto válido mayor a 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtAmount.Focus()
                Return
            End If
            
            Dim selectedCustomer As CustomerItem = CType(cmbCustomer.SelectedItem, CustomerItem)
            Dim earnedPoints As Integer = CInt(Math.Floor(amount))
            
            ' Usar procedimiento almacenado para registrar la compra
            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@CustomerID", selectedCustomer.CustomerID),
                New SqlParameter("@Amount", amount)
            }
            
            Dim result = DatabaseHelper.ExecuteStoredProcedure("sp_RegisterPurchase", parameters)
            
            If result IsNot Nothing Then
                MessageBox.Show($"Compra registrada exitosamente.{vbCrLf}Cliente: {selectedCustomer.Name}{vbCrLf}Monto: ${amount:F2}{vbCrLf}Puntos ganados: {earnedPoints}", 
                              "Compra Registrada", MessageBoxButtons.OK, MessageBoxIcon.Information)
                
                ClearForm()
                LoadCustomers() ' Recargar para actualizar puntos
                LoadPurchases()
            Else
                MessageBox.Show("Error al registrar la compra.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al registrar compra: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub BtnClear_Click(sender As Object, e As EventArgs)
        ClearForm()
    End Sub
    
    Private Sub BtnBack_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    
    Private Sub ClearForm()
        CType(Me.Controls.Find("cmbCustomer", True)(0), ComboBox).SelectedIndex = -1
        CType(Me.Controls.Find("txtAmount", True)(0), TextBox).Clear()
        CType(Me.Controls.Find("lblPointsValue", True)(0), Label).Text = "0"
        CType(Me.Controls.Find("lblEarnedValue", True)(0), Label).Text = "0"
    End Sub
    
    ' Clase auxiliar para items del ComboBox
    Public Class CustomerItem
        Public Property CustomerID As Integer
        Public Property Name As String
        Public Property CurrentPoints As Integer
        
        Public Overrides Function ToString() As String
            Return $"{Name} (Puntos: {CurrentPoints})"
        End Function
    End Class
End Class
