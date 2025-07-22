Imports System.Data.SqlClient

Public Class ReportsForm
    
    Private Sub ReportsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configurar formulario
        Me.Text = "Reportes de Fidelidad - Sistema de Fidelidad"
        Me.Size = New Size(1000, 700)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(245, 245, 245)
        
        SetupControls()
        LoadCustomerReport()
    End Sub
    
    Private Sub SetupControls()
        Me.Controls.Clear()
        
        ' Panel principal
        Dim mainPanel As New Panel()
        mainPanel.Dock = DockStyle.Fill
        mainPanel.Padding = New Padding(20)
        
        ' Título
        Dim titleLabel As New Label()
        titleLabel.Text = "REPORTES DE FIDELIDAD"
        titleLabel.Font = New Font("Segoe UI", 18, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(51, 51, 51)
        titleLabel.Height = 40
        titleLabel.Dock = DockStyle.Top
        
        ' Panel de filtros
        Dim filterPanel As New Panel()
        filterPanel.Height = 80
        filterPanel.Dock = DockStyle.Top
        filterPanel.BackColor = Color.White
        filterPanel.Padding = New Padding(20)
        
        ' Filtro por nombre
        Dim lblSearch As New Label()
        lblSearch.Text = "Buscar Cliente:"
        lblSearch.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblSearch.Location = New Point(20, 20)
        lblSearch.Size = New Size(100, 25)
        
        Dim txtSearch As New TextBox()
        txtSearch.Name = "txtSearch"
        txtSearch.Font = New Font("Segoe UI", 10)
        txtSearch.Location = New Point(130, 20)
        txtSearch.Size = New Size(200, 25)
        AddHandler txtSearch.TextChanged, AddressOf TxtSearch_TextChanged
        
        ' Botones de acción
        Dim btnRefresh As Button = CreateButton("Actualizar", Color.FromArgb(40, 167, 69))
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Location = New Point(350, 20)
        AddHandler btnRefresh.Click, AddressOf BtnRefresh_Click
        
        Dim btnExport As Button = CreateButton("Exportar", Color.FromArgb(70, 130, 180))
        btnExport.Name = "btnExport"
        btnExport.Location = New Point(490, 20)
        AddHandler btnExport.Click, AddressOf BtnExport_Click
        
        Dim btnBack As Button = CreateButton("Volver", Color.FromArgb(108, 117, 125))
        btnBack.Name = "btnBack"
        btnBack.Location = New Point(630, 20)
        AddHandler btnBack.Click, AddressOf BtnBack_Click
        
        ' Panel de estadísticas
        Dim statsPanel As New Panel()
        statsPanel.Height = 100
        statsPanel.Dock = DockStyle.Top
        statsPanel.BackColor = Color.FromArgb(248, 249, 250)
        statsPanel.Padding = New Padding(20)
        
        ' Estadísticas generales
        Dim lblStatsTitle As New Label()
        lblStatsTitle.Text = "ESTADÍSTICAS GENERALES"
        lblStatsTitle.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblStatsTitle.ForeColor = Color.FromArgb(51, 51, 51)
        lblStatsTitle.Location = New Point(20, 10)
        lblStatsTitle.Size = New Size(200, 25)
        
        Dim lblTotalCustomers As New Label()
        lblTotalCustomers.Name = "lblTotalCustomers"
        lblTotalCustomers.Text = "Total Clientes: 0"
        lblTotalCustomers.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblTotalCustomers.ForeColor = Color.FromArgb(70, 130, 180)
        lblTotalCustomers.Location = New Point(20, 40)
        lblTotalCustomers.Size = New Size(150, 25)
        
        Dim lblTotalPoints As New Label()
        lblTotalPoints.Name = "lblTotalPoints"
        lblTotalPoints.Text = "Total Puntos: 0"
        lblTotalPoints.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblTotalPoints.ForeColor = Color.FromArgb(40, 167, 69)
        lblTotalPoints.Location = New Point(180, 40)
        lblTotalPoints.Size = New Size(150, 25)
        
        Dim lblAvgPoints As New Label()
        lblAvgPoints.Name = "lblAvgPoints"
        lblAvgPoints.Text = "Promedio Puntos: 0"
        lblAvgPoints.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblAvgPoints.ForeColor = Color.FromArgb(255, 193, 7)
        lblAvgPoints.Location = New Point(340, 40)
        lblAvgPoints.Size = New Size(150, 25)
        
        Dim lblActiveCustomers As New Label()
        lblActiveCustomers.Name = "lblActiveCustomers"
        lblActiveCustomers.Text = "Clientes Activos: 0"
        lblActiveCustomers.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblActiveCustomers.ForeColor = Color.FromArgb(220, 53, 69)
        lblActiveCustomers.Location = New Point(500, 40)
        lblActiveCustomers.Size = New Size(150, 25)
        
        ' DataGridView para el reporte
        Dim dgvReport As New DataGridView()
        dgvReport.Name = "dgvReport"
        dgvReport.Dock = DockStyle.Fill
        dgvReport.BackgroundColor = Color.White
        dgvReport.BorderStyle = BorderStyle.None
        dgvReport.AllowUserToAddRows = False
        dgvReport.AllowUserToDeleteRows = False
        dgvReport.ReadOnly = True
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvReport.MultiSelect = False
        dgvReport.Font = New Font("Segoe UI", 9)
        dgvReport.RowHeadersVisible = False
        dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvReport.AllowUserToOrderColumns = True
        
        ' Configurar alternating row colors
        dgvReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250)
        
        ' Agregar controles a los paneles
        filterPanel.Controls.AddRange({lblSearch, txtSearch, btnRefresh, btnExport, btnBack})
        statsPanel.Controls.AddRange({lblStatsTitle, lblTotalCustomers, lblTotalPoints, lblAvgPoints, lblActiveCustomers})
        
        ' Agregar paneles al panel principal
        mainPanel.Controls.Add(dgvReport)
        mainPanel.Controls.Add(statsPanel)
        mainPanel.Controls.Add(filterPanel)
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
    
    Private Sub LoadCustomerReport(Optional searchFilter As String = "")
        Try
            Dim query As String = "SELECT * FROM vw_CustomerReport"
            Dim parameters As List(Of SqlParameter) = Nothing
            
            If Not String.IsNullOrWhiteSpace(searchFilter) Then
                query &= " WHERE Name LIKE @SearchFilter"
                parameters = New List(Of SqlParameter) From {
                    New SqlParameter("@SearchFilter", "%" & searchFilter.Trim() & "%")
                }
            End If
            
            query &= " ORDER BY CurrentPoints DESC, Name"
            
            Dim dataTable As DataTable = DatabaseHelper.GetDataTable(query, parameters)
            
            If dataTable IsNot Nothing Then
                Dim dgv As DataGridView = CType(Me.Controls.Find("dgvReport", True)(0), DataGridView)
                dgv.DataSource = dataTable
                
                ' Configurar columnas
                ConfigureReportColumns(dgv)
                
                ' Actualizar estadísticas
                UpdateStatistics(dataTable)
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al cargar reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub ConfigureReportColumns(dgv As DataGridView)
        Try
            If dgv.Columns.Count > 0 Then
                dgv.Columns("CustomerID").HeaderText = "ID"
                dgv.Columns("CustomerID").Width = 50
                dgv.Columns("Name").HeaderText = "Nombre del Cliente"
                dgv.Columns("Email").HeaderText = "Email"
                dgv.Columns("Phone").HeaderText = "Teléfono"
                dgv.Columns("CurrentPoints").HeaderText = "Puntos Actuales"
                dgv.Columns("CurrentPoints").DefaultCellStyle.Format = "N0"
                dgv.Columns("CurrentPoints").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgv.Columns("TotalPurchases").HeaderText = "Total Compras"
                dgv.Columns("TotalPurchases").DefaultCellStyle.Format = "C2"
                dgv.Columns("TotalPurchases").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgv.Columns("TotalEarnedPoints").HeaderText = "Puntos Ganados"
                dgv.Columns("TotalEarnedPoints").DefaultCellStyle.Format = "N0"
                dgv.Columns("TotalEarnedPoints").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgv.Columns("TotalRedeemedPoints").HeaderText = "Puntos Canjeados"
                dgv.Columns("TotalRedeemedPoints").DefaultCellStyle.Format = "N0"
                dgv.Columns("TotalRedeemedPoints").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgv.Columns("TotalTransactions").HeaderText = "Total Transacciones"
                dgv.Columns("TotalTransactions").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                
                ' Colorear filas según puntos
                For Each row As DataGridViewRow In dgv.Rows
                    If row.Cells("CurrentPoints").Value IsNot Nothing Then
                        Dim points As Integer = Convert.ToInt32(row.Cells("CurrentPoints").Value)
                        If points >= 1000 Then
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220) ' Gold
                        ElseIf points >= 500 Then
                            row.DefaultCellStyle.BackColor = Color.FromArgb(230, 247, 255) ' Light Blue
                        ElseIf points = 0 Then
                            row.DefaultCellStyle.ForeColor = Color.Gray
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            ' Ignorar errores de configuración de columnas
        End Try
    End Sub
    
    Private Sub UpdateStatistics(dataTable As DataTable)
        Try
            If dataTable IsNot Nothing AndAlso dataTable.Rows.Count > 0 Then
                Dim totalCustomers As Integer = dataTable.Rows.Count
                Dim totalPoints As Integer = 0
                Dim activeCustomers As Integer = 0
                
                For Each row As DataRow In dataTable.Rows
                    Dim points As Integer = Convert.ToInt32(row("CurrentPoints"))
                    totalPoints += points
                    If points > 0 Then
                        activeCustomers += 1
                    End If
                Next
                
                Dim avgPoints As Double = If(totalCustomers > 0, totalPoints / totalCustomers, 0)
                
                ' Actualizar labels de estadísticas
                CType(Me.Controls.Find("lblTotalCustomers", True)(0), Label).Text = $"Total Clientes: {totalCustomers}"
                CType(Me.Controls.Find("lblTotalPoints", True)(0), Label).Text = $"Total Puntos: {totalPoints:N0}"
                CType(Me.Controls.Find("lblAvgPoints", True)(0), Label).Text = $"Promedio Puntos: {avgPoints:F1}"
                CType(Me.Controls.Find("lblActiveCustomers", True)(0), Label).Text = $"Clientes Activos: {activeCustomers}"
            Else
                ' Resetear estadísticas si no hay datos
                CType(Me.Controls.Find("lblTotalCustomers", True)(0), Label).Text = "Total Clientes: 0"
                CType(Me.Controls.Find("lblTotalPoints", True)(0), Label).Text = "Total Puntos: 0"
                CType(Me.Controls.Find("lblAvgPoints", True)(0), Label).Text = "Promedio Puntos: 0"
                CType(Me.Controls.Find("lblActiveCustomers", True)(0), Label).Text = "Clientes Activos: 0"
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al actualizar estadísticas: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs)
        ' Implementar búsqueda con delay para evitar muchas consultas
        Dim timer As New Timer()
        timer.Interval = 500 ' 500ms delay
        timer.Tag = CType(sender, TextBox).Text
        
        AddHandler timer.Tick, Sub(timerSender, timerE)
                                   Dim searchTimer As Timer = CType(timerSender, Timer)
                                   searchTimer.Stop()
                                   LoadCustomerReport(searchTimer.Tag.ToString())
                                   searchTimer.Dispose()
                               End Sub
        
        timer.Start()
    End Sub
    
    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs)
        Dim txtSearch As TextBox = CType(Me.Controls.Find("txtSearch", True)(0), TextBox)
        LoadCustomerReport(txtSearch.Text)
        MessageBox.Show("Reporte actualizado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    
    Private Sub BtnExport_Click(sender As Object, e As EventArgs)
        Try
            Dim dgv As DataGridView = CType(Me.Controls.Find("dgvReport", True)(0), DataGridView)
            
            If dgv.Rows.Count = 0 Then
                MessageBox.Show("No hay datos para exportar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            
            ' Crear SaveFileDialog
            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "Archivos CSV (*.csv)|*.csv|Archivos de Texto (*.txt)|*.txt"
            saveDialog.Title = "Exportar Reporte de Fidelidad"
            saveDialog.FileName = $"Reporte_Fidelidad_{DateTime.Now:yyyyMMdd_HHmmss}"
            
            If saveDialog.ShowDialog() = DialogResult.OK Then
                ExportToCSV(dgv, saveDialog.FileName)
                MessageBox.Show($"Reporte exportado exitosamente a:{vbCrLf}{saveDialog.FileName}", "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            
        Catch ex As Exception
            MessageBox.Show("Error al exportar reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    Private Sub ExportToCSV(dgv As DataGridView, fileName As String)
        Try
            Using writer As New System.IO.StreamWriter(fileName, False, System.Text.Encoding.UTF8)
                ' Escribir encabezados
                Dim headers As New List(Of String)
                For Each column As DataGridViewColumn In dgv.Columns
                    headers.Add($"""{column.HeaderText}""")
                Next
                writer.WriteLine(String.Join(",", headers))
                
                ' Escribir datos
                For Each row As DataGridViewRow In dgv.Rows
                    If Not row.IsNewRow Then
                        Dim values As New List(Of String)
                        For Each cell As DataGridViewCell In row.Cells
                            Dim value As String = If(cell.Value?.ToString(), "")
                            values.Add($"""{value}""")
                        Next
                        writer.WriteLine(String.Join(",", values))
                    End If
                Next
                
                ' Agregar estadísticas al final
                writer.WriteLine()
                writer.WriteLine("ESTADÍSTICAS GENERALES")
                writer.WriteLine($"Fecha de Generación,{DateTime.Now:dd/MM/yyyy HH:mm:ss}")
                writer.WriteLine($"Total de Clientes,{dgv.Rows.Count}")
                
                Dim totalPoints As Integer = 0
                Dim activeCustomers As Integer = 0
                For Each row As DataGridViewRow In dgv.Rows
                    If Not row.IsNewRow AndAlso row.Cells("CurrentPoints").Value IsNot Nothing Then
                        Dim points As Integer = Convert.ToInt32(row.Cells("CurrentPoints").Value)
                        totalPoints += points
                        If points > 0 Then activeCustomers += 1
                    End If
                Next
                
                writer.WriteLine($"Total de Puntos,{totalPoints}")
                writer.WriteLine($"Clientes Activos,{activeCustomers}")
                writer.WriteLine($"Promedio de Puntos,{If(dgv.Rows.Count > 0, totalPoints / dgv.Rows.Count, 0):F2}")
            End Using
            
        Catch ex As Exception
            Throw New Exception("Error al escribir archivo CSV: " & ex.Message)
        End Try
    End Sub
    
    Private Sub BtnBack_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
End Class
