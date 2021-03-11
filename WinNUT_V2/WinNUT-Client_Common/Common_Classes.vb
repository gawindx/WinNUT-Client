Public Class UPS_Var_Node
    Public VarKey As String
    Public VarValue As String
    Public VarDesc As String
End Class

Public Class UPS_List_Datas
    Public VarKey As String
    Public VarValue As String
    Public VarDesc As String
End Class

Public Class UPS_Values
    Public Batt_Charge As Double = Nothing
    Public Batt_Voltage As Double = Nothing
    Public Batt_Runtime As Double = Nothing
    Public Input_Voltage As Double = Nothing
    Public Output_Voltage As Double = Nothing
    Public Power_Frequency As Double = Nothing
    Public Load As Double = Nothing
    Public Output_Power As Double = Nothing
    Public Batt_Capacity As Double = Nothing
    Public UPS_Status As Integer = Nothing
End Class

Public Class UPS_Datas
    Public Name As String = ""
    Public Mfr As String = ""
    Public Model As String = ""
    Public Serial As String = ""
    Public Firmware As String = ""
    Public UPS_Value As New UPS_Values
End Class

Public Class Nut_Exception
    Inherits System.ApplicationException

    Public Sub New(ByVal Nut_Error_Lvl As Nut_Exception_Value)
        MyBase.New(StringEnum.GetStringValue(Nut_Error_Lvl))
    End Sub

    Public Sub New(ByVal Nut_Error_Lvl As Nut_Exception_Value, ByVal Message As String)
        MyBase.New(StringEnum.GetStringValue(Nut_Error_Lvl) & Message)
    End Sub
End Class

Public Class Nut_Parameter
    Public Host As String = ""
    Public Port As Integer = Nothing
    Public Login As String = ""
    Public Password As String = ""
    Public UPSName As String = ""
    Public AutoReconnect As Boolean = False
End Class
