Imports System.Data.SqlClient

Public Class CustomersForm
    Private currentCustomerId As Integer = 0
    
    Private Sub CustomersForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configurar formulario
        Me.Text = "Gestión de Clientes - Sistema de Fidelidad"
        Me.Size = New Size(900, 600)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(245, 245, 245)
        
        SetupControls()
        LoadCustomers()
    End Sub
    
    Private Sub SetupControls()
        Me.Controls.Clear()
        
        ' Panel principal
        Dim mainPanel As New Panel()
        mainPanel.Dock = DockStyle.Fill
        mainPanel.Padding = New Padding(20)
        
        ' Título
        Dim titleLabel As New Label()
        titleLabel.Text = "GESTIÓN DE CLIENTES"
        titleLabel.Font = New Font("Segoe UI", 18, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(51, 51, 51)
        titleLabel.Height = 40
        titleLabel.Dock = DockStyle.Top
        
        ' Panel de formulario
        Dim formPanel As New Panel()
        formPanel.Height = 200
        formPanel.Dock = DockStyle.Top
        formPanel.BackColor = Color.White
        formPanel.Padding = New Padding(20)
        
        ' Crear controles de entrada
        Dim lblName As New Label()
        lblName.Text = "Nombre:"
        lblName.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblName.Location = New Point(20, 20)
        lblName.Size = New Size(100, 25)
        
        Dim txtName As New TextBox()
        txtName.Name = "txtName"
        txtName.Font = New Font("Segoe UI", 10)
        txtName.Location = New Point(130, 20)
        txtName.Size = New Size(200, 25)
        
        Dim lblEmail As New Label()
        lblEmail.Text = "Email:"
        lblEmail.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblEmail.Location = New Point(350, 20)
        lblEmail.Size = New Size(100, 25)
        
        Dim txtEmail As New TextBox()
        txtEmail.Name = "txtEmail"
        txtEmail.Font = New Font("Segoe UI", 10)
        txtEmail.Location = New Point(460, 20)
        txtEmail.Size = New Size(200, 25)
        
        Dim lblPhone As New Label()
        lblPhone.Text = "Teléfono:"
        lblPhone.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblPhone.Location = New Point(20, 60)
        lblPhone.Size = New Size(100, 25)
        
        Dim txtPhone As New TextBox()
        txtPhone.Name = "txtPhone"
        txtPhone.Font = New Font("Segoe UI", 10)
        txtPhone.Location = New Point(130, 60)
        txtPhone.Size = New Size(200, 25)
        
        Dim lblAddress As New Label()
        lblAddress.Text = "Dirección:"
        lblAddress.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblAddress.Location = New Point(350, 60)
        lblAddress.Size = New Size(100, 25)
        
        Dim txtAddress As New TextBox()
        txtAddress.Name = "txtAddress"
        txtAddress.Font = New Font("Segoe UI", 10)
        txtAddress.Location = New Point(460, 60)
        txtAddress.Size = New Size(200, 25)
        txtAddress.Multiline = True
        txtAddress.Height = 40
        
        ' Panel de botones
        Dim buttonPanel As New Panel()
        buttonPanel.Height = 50
        buttonPanel.Dock = DockStyle.Top
        buttonPanel.Padding = New Padding(20, 10, 20, 10)
        
        Dim btnAdd As Button = CreateButton("Agregar Cliente", Color.FromArgb(40, 167, 69))
        btnAdd.Name = "btnAdd"
        btnAdd.Location = New Point(20, 10)
        AddHandler btnAdd.Click, AddressOf BtnAdd_Click
        
        Dim btnUpdate As Button = CreateButton("Actualizar Cliente", Color.FromArgb(255, 193, 7))
        btnUpdate.Name = "btnUpdate"
        btnUpdate.Location = New Point(160, 10)
        AddHandler btnUpdate.Click, AddressOf BtnUpdate_Click
        
        Dim btnDelete As Button = CreateButton("Eliminar Cliente", Color.FromArgb(220, 53, 69))
        btnDelete.Name = "btnDelete"
        btnDelete.Location = New Point(300, 10)
        AddHandler btnDelete.Click, AddressOf BtnDelete_Click
        
        Dim btnClear As Button = CreateButton("Limpiar", Color.FromArgb(108, 117, 125))
        btnClear.Name = "btnClear"
        btnClear.Location = New Point(440, 10)
        AddHandler btnClear.Click, AddressOf BtnClear_Click
        
        Dim btnBack As Button = CreateButton("Volver", Color.FromArgb(70, 130, 180))
        btnBack.Name = "btnBack"
        btnBack.Location = New Point(580, 10)
        AddHandler btnBack.Click, AddressOf BtnBack_Click
        
        ' DataGridView para mostrar clientes
        Dim dgvCustomers As New DataGridView()
        dgvCustomers.Name = "dgvCustomers"
        dgvCustomers.Dock = DockStyle.Fill
        dgvCustomers.BackgroundColor = Color.White
        dgvCustomers.BorderStyle = BorderStyle.None
        dgvCustomers.AllowUserToAddRows = False
        dgvCustomers.AllowUserToDeleteRows = False
        dgvCustomers.ReadOnly = True
        dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCustomers.MultiSelect = False
        dgvCustomers.Font = New Font("Segoe UI", 9)
        dgvCustomers.RowHeadersVisible = False
        dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        AddHandler dgvCustomers.SelectionChanged, AddressOf DgvCustomers_SelectionChanged
        
        ' Agregar controles a los paneles
        formPanel.Controls.AddRange({lblName, txtName, lblEmail, txtEmail, lblPhone, txtPhone, lblAddress, txtAddress})
        buttonPanel.Controls.AddRange({btnAdd, btnUpdate, btnDelete, btnClear, btnBack})
        
        ' Agregar paneles al panel principal
        mainPanel.Controls.Add(dgvCustomers)
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
            Dim query As String = "SELECT CustomerID, Name, Email, Phone, Address, CurrentPoints, CreatedDate FROM Customers ORDER BY Name"
            Dim dataTable As DataTable = DatabaseHelper.GetDataTable(query)
            
            If dataTable IsNot Nothing Then
                Dim dgv As DataGridView = CType(Me.Controls.Find("dgvCustomers", True)(0), DataGridView)
                dgv.DataSource = dataTable
                
                ' Configurar columnas
                dgv.Columns("CustomerID").HeaderText = "ID"
                dgv.Columns("CustomerID").Width = 50
                dgv.Columns("Name").HeaderText = "Nombre"
                dgv.Columns("Email").HeaderText = "Email"
                dgv.Columns("Phone").HeaderText = "Teléfono"
                dgv.Columns("Address").HeaderText = "Dirección"
                dgv.Columns("CurrentPoints").HeaderText = "Puntos Actuales"
                dgv.Columns("CreatedDate").HeaderText = "Fecha Registro"
                dgv.Columns("CreatedDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            End If
        Catch ex As Exception
            MessageBox.Show("Error al cargar clientes: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub BtnAdd_Click(sender As Object, e As EventArgs)
        Try
            Dim txtName As TextBox = CType(Me.Controls.Find("txtName", True)(0), TextBox)
            Dim txtEmail As TextBox = CType(Me.Controls.Find("txtEmail", True)(0), TextBox)
            Dim txtPhone As TextBox = CType(Me.Controls.Find("txtPhone", True)(0), TextBox)
            Dim txtAddress As TextBox = CType(Me.Controls.Find("txtAddress", True)(0), TextBox)
            
            ' Validaciones
            If String.IsNullOrWhiteSpace(txtName.Text) Then
                MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtName.Focus()
                Return
            End If
            
            ' Insertar cliente
            Dim query As String = "INSERT INTO Customers (Name, Email, Phone, Address) VALUES (@Name, @Email, @Phone, @Address)"
            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@Name", txtName.Text.Trim()),
                New SqlParameter("@Email", If(String.IsNullOrWhiteSpace(txtEmail.Text), DBNull.Value, txtEmail.Text.Trim())),
                New SqlParameter("@Phone", If(String.IsNullOrWhiteSpace(txtPhone.Text), DBNull.Value, txtPhone.Text.Trim())),
                New SqlParameter("@Address", If(String.IsNullOrWhiteSpace(txtAddress.Text), DBNull.Value, txtAddress.Text.Trim()))
            }
            
            Dim result As Integer = DatabaseHelper.ExecuteNonQuery(query, parameters)
            
            If result > 0 Then
                MessageBox.Show("Cliente agregado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearForm()
                LoadCustomers()
            Else
                MessageBox.Show("Error al agregar cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al agregar cliente: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs)
        Try
            If currentCustomerId = 0 Then
                MessageBox.Show("Seleccione un cliente para actualizar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            
            Dim txtName As TextBox = CType(Me.Controls.Find("txtName", True)(0), TextBox)
            Dim txtEmail As TextBox = CType(Me.Controls.Find("txtEmail", True)(0), TextBox)
            Dim txtPhone As TextBox = CType(Me.Controls.Find("txtPhone", True)(0), TextBox)
            Dim txtAddress As TextBox = CType(Me.Controls.Find("txtAddress", True)(0), TextBox)
            
            If String.IsNullOrWhiteSpace(txtName.Text) Then
                MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtName.Focus()
                Return
            End If
            
            Dim query As String = "UPDATE Customers SET Name = @Name, Email = @Email, Phone = @Phone, Address = @Address WHERE CustomerID = @CustomerID"
            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@CustomerID", currentCustomerId),
                New SqlParameter("@Name", txtName.Text.Trim()),
                New SqlParameter("@Email", If(String.IsNullOrWhiteSpace(txtEmail.Text), DBNull.Value, txtEmail.Text.Trim())),
                New SqlParameter("@Phone", If(String.IsNullOrWhiteSpace(txtPhone.Text), DBNull.Value, txtPhone.Text.Trim())),
                New SqlParameter("@Address", If(String.IsNullOrWhiteSpace(txtAddress.Text), DBNull.Value, txtAddress.Text.Trim()))
            }
            
            Dim result As Integer = DatabaseHelper.ExecuteNonQuery(query, parameters)
            
            If result > 0 Then
                MessageBox.Show("Cliente actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearForm()
                LoadCustomers()
            Else
                MessageBox.Show("Error al actualizar cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al actualizar cliente: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs)
        Try
            If currentCustomerId = 0 Then
                MessageBox.Show("Seleccione un cliente para eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            
            Dim result As DialogResult = MessageBox.Show("¿Está seguro que desea eliminar este cliente? Esta acción no se puede deshacer.", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            
            If result = DialogResult.Yes Then
                Dim query As String = "DELETE FROM Customers WHERE CustomerID = @CustomerID"
                Dim parameters As New List(Of SqlParameter) From {
                    New SqlParameter("@CustomerID", currentCustomerId)
                }
                
                Dim deleteResult As Integer = DatabaseHelper.ExecuteNonQuery(query, parameters)
                
                If deleteResult > 0 Then
                    MessageBox.Show("Cliente eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ClearForm()
                    LoadCustomers()
                Else
                    MessageBox.Show("Error al eliminar cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al eliminar cliente: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub BtnClear_Click(sender As Object, e As EventArgs)
        ClearForm()
    End Sub
    
    Private Sub BtnBack_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    
    Private Sub ClearForm()
        currentCustomerId = 0
        CType(Me.Controls.Find("txtName", True)(0), TextBox).Clear()
        CType(Me.Controls.Find("txtEmail", True)(0), TextBox).Clear()
        CType(Me.Controls.Find("txtPhone", True)(0), TextBox).Clear()
        CType(Me.Controls.Find("txtAddress", True)(0), TextBox).Clear()
    End Sub
    
    Private Sub DgvCustomers_SelectionChanged(sender As Object, e As EventArgs)
        Try
            Dim dgv As DataGridView = CType(sender, DataGridView)
            
            If dgv.SelectedRows.Count > 0 Then
                Dim row As DataGridViewRow = dgv.SelectedRows(0)
                
                currentCustomerId = Convert.ToInt32(row.Cells("CustomerID").Value)
                CType(Me.Controls.Find("txtName", True)(0), TextBox).Text = row.Cells("Name").Value?.ToString() ?? ""
                CType(Me.Controls.Find("txtEmail", True)(0), TextBox).Text = row.Cells("Email").Value?.ToString() ?? ""
                CType(Me.Controls.Find("txtPhone", True)(0), TextBox).Text = row.Cells("Phone").Value?.ToString() ?? ""
                CType(Me.Controls.Find("txtAddress", True)(0), TextBox).Text = row.Cells("Address").Value?.ToString() ?? ""
            End If
        Catch ex As Exception
            ' Ignorar errores de selección
        End Try
    End Sub
End Class
