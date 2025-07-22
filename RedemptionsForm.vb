Imports System.Data.SqlClient

Public Class RedemptionsForm
    
    Private Sub RedemptionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configurar formulario
        Me.Text = "Canje de Puntos - Sistema de Fidelidad"
        Me.Size = New Size(800, 700)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(245, 245, 245)
        
        SetupControls()
        LoadCustomers()
        LoadRedemptions()
    End Sub
    
    Private Sub SetupControls()
        Me.Controls.Clear()
        
        ' Panel principal
        Dim mainPanel As New Panel()
        mainPanel.Dock = DockStyle.Fill
        mainPanel.Padding = New Padding(20)
        
        ' Título
        Dim titleLabel As New Label()
        titleLabel.Text = "CANJE DE PUNTOS"
        titleLabel.Font = New Font("Segoe UI", 18, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(51, 51, 51)
        titleLabel.Height = 40
        titleLabel.Dock = DockStyle.Top
        
        ' Panel de formulario
        Dim formPanel As New Panel()
        formPanel.Height = 280
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
        
        ' Puntos disponibles
        Dim lblAvailablePoints As New Label()
        lblAvailablePoints.Text = "Puntos Disponibles:"
        lblAvailablePoints.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblAvailablePoints.Location = New Point(450, 20)
        lblAvailablePoints.Size = New Size(130, 25)
        
        Dim lblAvailableValue As New Label()
        lblAvailableValue.Name = "lblAvailableValue"
        lblAvailableValue.Text = "0"
        lblAvailableValue.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        lblAvailableValue.ForeColor = Color.FromArgb(40, 167, 69)
        lblAvailableValue.Location = New Point(590, 20)
        lblAvailableValue.Size = New Size(100, 25)
        
        ' Puntos a canjear
        Dim lblRedeemPoints As New Label()
        lblRedeemPoints.Text = "Puntos a Canjear:"
        lblRedeemPoints.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblRedeemPoints.Location = New Point(20, 70)
        lblRedeemPoints.Size = New Size(120, 25)
        
        Dim nudRedeemPoints As New NumericUpDown()
        nudRedeemPoints.Name = "nudRedeemPoints"
        nudRedeemPoints.Font = New Font("Segoe UI", 12)
        nudRedeemPoints.Location = New Point(150, 70)
        nudRedeemPoints.Size = New Size(120, 30)
        nudRedeemPoints.Minimum = 0
        nudRedeemPoints.Maximum = 999999
        nudRedeemPoints.Value = 0
        AddHandler nudRedeemPoints.ValueChanged, AddressOf NudRedeemPoints_ValueChanged
        
        ' Valor equivalente
        Dim lblEquivalentValue As New Label()
        lblEquivalentValue.Text = "Valor Equivalente:"
        lblEquivalentValue.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblEquivalentValue.Location = New Point(290, 70)
        lblEquivalentValue.Size = New Size(120, 25)
        
        Dim lblValueAmount As New Label()
        lblValueAmount.Name = "lblValueAmount"
        lblValueAmount.Text = "$0.00"
        lblValueAmount.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblValueAmount.ForeColor = Color.FromArgb(70, 130, 180)
        lblValueAmount.Location = New Point(420, 70)
        lblValueAmount.Size = New Size(100, 25)
        
        ' Comentarios
        Dim lblComments As New Label()
        lblComments.Text = "Comentarios:"
        lblComments.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblComments.Location = New Point(20, 120)
        lblComments.Size = New Size(100, 25)
        
        Dim txtComments As New TextBox()
        txtComments.Name = "txtComments"
        txtComments.Font = New Font("Segoe UI", 10)
        txtComments.Location = New Point(130, 120)
        txtComments.Size = New Size(400, 60)
        txtComments.Multiline = True
        txtComments.ScrollBars = ScrollBars.Vertical
        
        ' Información de canje
        Dim lblInfo As New Label()
        lblInfo.Text = "* 1 punto = $1.00 de descuento" & vbCrLf & "* Los puntos canjeados se descontarán del saldo del cliente"
        lblInfo.Font = New Font("Segoe UI", 9, FontStyle.Italic)
        lblInfo.ForeColor = Color.FromArgb(108, 117, 125)
        lblInfo.Location = New Point(20, 190)
        lblInfo.Size = New Size(400, 40)
        
        ' Panel de botones
        Dim buttonPanel As New Panel()
        buttonPanel.Height = 50
        buttonPanel.Dock = DockStyle.Top
        buttonPanel.Padding = New Padding(20, 10, 20, 10)
        
        Dim btnRedeem As Button = CreateButton("Canjear Puntos", Color.FromArgb(220, 53, 69))
        btnRedeem.Name = "btnRedeem"
        btnRedeem.Location = New Point(20, 10)
        btnRedeem.Size = New Size(150, 30)
        AddHandler btnRedeem.Click, AddressOf BtnRedeem_Click
        
        Dim btnClear As Button = CreateButton("Limpiar", Color.FromArgb(108, 117, 125))
        btnClear.Name = "btnClear"
        btnClear.Location = New Point(180, 10)
        AddHandler btnClear.Click, AddressOf BtnClear_Click
        
        Dim btnBack As Button = CreateButton("Volver", Color.FromArgb(70, 130, 180))
        btnBack.Name = "btnBack"
        btnBack.Location = New Point(320, 10)
        AddHandler btnBack.Click, AddressOf BtnBack_Click
        
        ' DataGridView para mostrar canjes recientes
        Dim lblRecentRedemptions As New Label()
        lblRecentRedemptions.Text = "CANJES RECIENTES"
        lblRecentRedemptions.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblRecentRedemptions.ForeColor = Color.FromArgb(51, 51, 51)
        lblRecentRedemptions.Height = 30
        lblRecentRedemptions.Dock = DockStyle.Top
        lblRecentRedemptions.Padding = New Padding(0, 10, 0, 0)
        
        Dim dgvRedemptions As New DataGridView()
        dgvRedemptions.Name = "dgvRedemptions"
        dgvRedemptions.Dock = DockStyle.Fill
        dgvRedemptions.BackgroundColor = Color.White
        dgvRedemptions.BorderStyle = BorderStyle.None
        dgvRedemptions.AllowUserToAddRows = False
        dgvRedemptions.AllowUserToDeleteRows = False
        dgvRedemptions.ReadOnly = True
        dgvRedemptions.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRedemptions.MultiSelect = False
        dgvRedemptions.Font = New Font("Segoe UI", 9)
        dgvRedemptions.RowHeadersVisible = False
        dgvRedemptions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        
        ' Agregar controles a los paneles
        formPanel.Controls.AddRange({lblCustomer, cmbCustomer, lblAvailablePoints, lblAvailableValue,
                                   lblRedeemPoints, nudRedeemPoints, lblEquivalentValue, lblValueAmount,
                                   lblComments, txtComments, lblInfo})
        buttonPanel.Controls.AddRange({btnRedeem, btnClear, btnBack})
        
        ' Agregar paneles al panel principal
        mainPanel.Controls.Add(dgvRedemptions)
        mainPanel.Controls.Add(lblRecentRedemptions)
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
            
            Dim query As String = "SELECT CustomerID, Name, CurrentPoints FROM Customers WHERE CurrentPoints > 0 ORDER BY Name"
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
    
    Private Sub LoadRedemptions()
        Try
            Dim query As String = "SELECT TOP 20 r.RedemptionID, c.Name, r.RedemptionDate, r.RedeemPoints, r.Comments " &
                                "FROM Redemptions r INNER JOIN Customers c ON r.CustomerID = c.CustomerID " &
                                "ORDER BY r.RedemptionDate DESC"
            
            Dim dataTable As DataTable = DatabaseHelper.GetDataTable(query)
            
            If dataTable IsNot Nothing Then
                Dim dgv As DataGridView = CType(Me.Controls.Find("dgvRedemptions", True)(0), DataGridView)
                dgv.DataSource = dataTable
                
                ' Configurar columnas
                dgv.Columns("RedemptionID").HeaderText = "ID Canje"
                dgv.Columns("RedemptionID").Width = 80
                dgv.Columns("Name").HeaderText = "Cliente"
                dgv.Columns("RedemptionDate").HeaderText = "Fecha"
                dgv.Columns("RedemptionDate").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
                dgv.Columns("RedeemPoints").HeaderText = "Puntos Canjeados"
                dgv.Columns("Comments").HeaderText = "Comentarios"
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al cargar canjes: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub CmbCustomer_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim cmbCustomer As ComboBox = CType(sender, ComboBox)
            Dim lblAvailableValue As Label = CType(Me.Controls.Find("lblAvailableValue", True)(0), Label)
            Dim nudRedeemPoints As NumericUpDown = CType(Me.Controls.Find("nudRedeemPoints", True)(0), NumericUpDown)
            
            If cmbCustomer.SelectedItem IsNot Nothing Then
                Dim selectedCustomer As CustomerItem = CType(cmbCustomer.SelectedItem, CustomerItem)
                lblAvailableValue.Text = selectedCustomer.CurrentPoints.ToString()
                nudRedeemPoints.Maximum = selectedCustomer.CurrentPoints
                nudRedeemPoints.Value = 0
            Else
                lblAvailableValue.Text = "0"
                nudRedeemPoints.Maximum = 0
                nudRedeemPoints.Value = 0
            End If
            
            UpdateEquivalentValue()
            
        Catch ex As Exception
            ' Ignorar errores de selección
        End Try
    End Sub
    
    Private Sub NudRedeemPoints_ValueChanged(sender As Object, e As EventArgs)
        UpdateEquivalentValue()
    End Sub
    
    Private Sub UpdateEquivalentValue()
        Try
            Dim nudRedeemPoints As NumericUpDown = CType(Me.Controls.Find("nudRedeemPoints", True)(0), NumericUpDown)
            Dim lblValueAmount As Label = CType(Me.Controls.Find("lblValueAmount", True)(0), Label)
            
            Dim equivalentValue As Decimal = nudRedeemPoints.Value
            lblValueAmount.Text = equivalentValue.ToString("C2")
            
        Catch ex As Exception
            ' Ignorar errores de conversión
        End Try
    End Sub
    
    Private Sub BtnRedeem_Click(sender As Object, e As EventArgs)
        Try
            Dim cmbCustomer As ComboBox = CType(Me.Controls.Find("cmbCustomer", True)(0), ComboBox)
            Dim nudRedeemPoints As NumericUpDown = CType(Me.Controls.Find("nudRedeemPoints", True)(0), NumericUpDown)
            Dim txtComments As TextBox = CType(Me.Controls.Find("txtComments", True)(0), TextBox)
            
            ' Validaciones
            If cmbCustomer.SelectedItem Is Nothing Then
                MessageBox.Show("Seleccione un cliente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbCustomer.Focus()
                Return
            End If
            
            If nudRedeemPoints.Value <= 0 Then
                MessageBox.Show("Ingrese una cantidad de puntos válida mayor a 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                nudRedeemPoints.Focus()
                Return
            End If
            
            Dim selectedCustomer As CustomerItem = CType(cmbCustomer.SelectedItem, CustomerItem)
            
            If nudRedeemPoints.Value > selectedCustomer.CurrentPoints Then
                MessageBox.Show("La cantidad de puntos a canjear no puede ser mayor a los puntos disponibles.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                nudRedeemPoints.Focus()
                Return
            End If
            
            ' Confirmar canje
            Dim equivalentValue As Decimal = nudRedeemPoints.Value
            Dim confirmMessage As String = $"¿Confirma el canje de {nudRedeemPoints.Value} puntos por ${equivalentValue:F2}?{vbCrLf}{vbCrLf}" &
                                        $"Cliente: {selectedCustomer.Name}{vbCrLf}" &
                                        $"Puntos actuales: {selectedCustomer.CurrentPoints}{vbCrLf}" &
                                        $"Puntos después del canje: {selectedCustomer.CurrentPoints - nudRedeemPoints.Value}"
            
            Dim result As DialogResult = MessageBox.Show(confirmMessage, "Confirmar Canje", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            
            If result = DialogResult.Yes Then
                ' Usar procedimiento almacenado para canjear puntos
                Dim parameters As New List(Of SqlParameter) From {
                    New SqlParameter("@CustomerID", selectedCustomer.CustomerID),
                    New SqlParameter("@RedeemPoints", CInt(nudRedeemPoints.Value)),
                    New SqlParameter("@Comments", If(String.IsNullOrWhiteSpace(txtComments.Text), DBNull.Value, txtComments.Text.Trim()))
                }
                
                Dim spResult = DatabaseHelper.ExecuteStoredProcedure("sp_RedeemPoints", parameters)
                
                If spResult IsNot Nothing AndAlso spResult.ToString() = "SUCCESS" Then
                    MessageBox.Show($"Canje realizado exitosamente.{vbCrLf}Cliente: {selectedCustomer.Name}{vbCrLf}Puntos canjeados: {nudRedeemPoints.Value}{vbCrLf}Valor: ${equivalentValue:F2}", 
                                  "Canje Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    
                    ClearForm()
                    LoadCustomers() ' Recargar para actualizar puntos
                    LoadRedemptions()
                ElseIf spResult IsNot Nothing AndAlso spResult.ToString() = "INSUFFICIENT_POINTS" Then
                    MessageBox.Show("Puntos insuficientes para realizar el canje.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    MessageBox.Show("Error al procesar el canje.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al procesar canje: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        CType(Me.Controls.Find("nudRedeemPoints", True)(0), NumericUpDown).Value = 0
        CType(Me.Controls.Find("txtComments", True)(0), TextBox).Clear()
        CType(Me.Controls.Find("lblAvailableValue", True)(0), Label).Text = "0"
        CType(Me.Controls.Find("lblValueAmount", True)(0), Label).Text = "$0.00"
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
