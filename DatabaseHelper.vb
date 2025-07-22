Imports System.Data.SqlClient
Imports System.Configuration

Public Class DatabaseHelper
    ' Cadena de conexión - Actualizar según su configuración de SQL Server
    Private Shared connectionString As String = "Data Source=localhost;Initial Catalog=LoyaltySystemDB;Integrated Security=True;"
    
    ' Método para ejecutar consultas que no devuelven datos (INSERT, UPDATE, DELETE)
    Public Shared Function ExecuteNonQuery(query As String, Optional parameters As List(Of SqlParameter) = Nothing) As Integer
        Dim rowsAffected As Integer = 0
        
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    If parameters IsNot Nothing Then
                        command.Parameters.AddRange(parameters.ToArray())
                    End If
                    rowsAffected = command.ExecuteNonQuery()
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error de base de datos: " & ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return -1
            Catch ex As Exception
                MessageBox.Show("Error inesperado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return -1
            End Try
        End Using
        
        Return rowsAffected
    End Function
    
    ' Método para ejecutar consultas que devuelven datos (SELECT)
    Public Shared Function ExecuteReader(query As String, Optional parameters As List(Of SqlParameter) = Nothing) As SqlDataReader
        Dim connection As New SqlConnection(connectionString)
        Dim command As New SqlCommand(query, connection)
        
        If parameters IsNot Nothing Then
            command.Parameters.AddRange(parameters.ToArray())
        End If
        
        Try
            connection.Open()
            Return command.ExecuteReader(CommandBehavior.CloseConnection)
        Catch ex As SqlException
            MessageBox.Show("Error de base de datos: " & ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error)
            connection.Close()
            Return Nothing
        Catch ex As Exception
            MessageBox.Show("Error inesperado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            connection.Close()
            Return Nothing
        End Try
    End Function
    
    ' Método para obtener un DataTable
    Public Shared Function GetDataTable(query As String, Optional parameters As List(Of SqlParameter) = Nothing) As DataTable
        Dim dataTable As New DataTable()
        
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    If parameters IsNot Nothing Then
                        command.Parameters.AddRange(parameters.ToArray())
                    End If
                    
                    Using adapter As New SqlDataAdapter(command)
                        adapter.Fill(dataTable)
                    End Using
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error de base de datos: " & ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            Catch ex As Exception
                MessageBox.Show("Error inesperado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Using
        
        Return dataTable
    End Function
    
    ' Método para ejecutar procedimientos almacenados
    Public Shared Function ExecuteStoredProcedure(procedureName As String, Optional parameters As List(Of SqlParameter) = Nothing) As Object
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Using command As New SqlCommand(procedureName, connection)
                    command.CommandType = CommandType.StoredProcedure
                    
                    If parameters IsNot Nothing Then
                        command.Parameters.AddRange(parameters.ToArray())
                    End If
                    
                    Return command.ExecuteScalar()
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error de base de datos: " & ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            Catch ex As Exception
                MessageBox.Show("Error inesperado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Using
    End Function
    
    ' Método para probar la conexión
    Public Shared Function TestConnection() As Boolean
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Return True
            Catch ex As Exception
                MessageBox.Show("No se pudo conectar a la base de datos: " & ex.Message, "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Using
    End Function
    
    ' Método para obtener el siguiente ID disponible
    Public Shared Function GetNextId(tableName As String, idColumnName As String) As Integer
        Dim query As String = $"SELECT ISNULL(MAX({idColumnName}), 0) + 1 FROM {tableName}"
        
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    Dim result = command.ExecuteScalar()
                    Return Convert.ToInt32(result)
                End Using
            Catch ex As Exception
                MessageBox.Show("Error obteniendo siguiente ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return 1
            End Try
        End Using
    End Function
End Class
