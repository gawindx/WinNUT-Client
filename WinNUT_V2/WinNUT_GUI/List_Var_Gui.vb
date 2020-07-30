Public Class List_Var_Gui
    Private List_Var_Datas As List(Of UPS_Var_Node)
    Private LogFile As Logger
    Private Sub List_Var_Gui_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.LogFile = WinNUT.LogFile
        LogFile.LogTracing("Load List Var Gui", LogLvl.LOG_DEBUG, Me)
        Me.Icon = WinNUT.Icon
        Me.PopulateTreeView()
    End Sub
    Private Sub PopulateTreeView()
        LogFile.LogTracing("Populate TreeView", LogLvl.LOG_DEBUG, Me)
        List_Var_Datas = WinNUT.UPS_Network.ListUPSVars()
        TView_UPSVar.Nodes.Clear()
        TView_UPSVar.Nodes.Add(WinNUT_Params.Arr_Reg_Key.Item("UPSName"), WinNUT_Params.Arr_Reg_Key.Item("UPSName"))
        Dim TreeChild As New TreeNode
        Dim LastNode As New TreeNode
        LastNode = TView_UPSVar.Nodes(0)
        For Each UPS_Var In List_Var_Datas
            Dim FullPathNode = String.Empty
            For Each SubPath In (Strings.Split(UPS_Var.VarKey, "."))
                FullPathNode += SubPath & "."
                Dim Nodes = TView_UPSVar.Nodes.Find(FullPathNode, True)
                If Nodes.Length = 0 Then
                    If LastNode.Text = "" Then
                        LastNode = TView_UPSVar.Nodes.Add(FullPathNode, SubPath)
                    Else
                        LastNode = LastNode.Nodes.Add(FullPathNode, SubPath)
                    End If
                Else
                    LastNode = Nodes(0)
                End If
            Next
            LastNode = TView_UPSVar.Nodes(0)
        Next
    End Sub
    Private Sub Btn_Clear_Click(sender As Object, e As EventArgs) Handles Btn_Clear.Click
        TView_UPSVar.CollapseAll()
        Lbl_N_Value.Text = ""
        Lbl_V_Value.Text = ""
        Lbl_D_Value.Text = ""
    End Sub
    Private Function FindNodeByValue(ByVal value As String, ByVal nodes As TreeNodeCollection) As TreeNode
        For Each n As TreeNode In nodes
            If n.Text = value Then
                Return n
            Else
                'Recursively call the Function
                Dim nodeToFind As TreeNode = FindNodeByValue(value, n.Nodes)
                If nodeToFind IsNot Nothing Then
                    Return nodeToFind
                End If
            End If
        Next

        Return Nothing
    End Function
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer_Update_List.Tick
        Dim SelectedNode As TreeNode = TView_UPSVar.SelectedNode
        Dim UPSNAme = WinNUT.UPS_Network.NutUPS
        If SelectedNode IsNot Nothing Then
            If SelectedNode.Parent IsNot Nothing Then
                If SelectedNode.Parent.Text <> UPSNAme And SelectedNode.Nodes.Count = 0 Then
                    Dim VarName = Strings.Replace(TView_UPSVar.SelectedNode.FullPath, WinNUT.UPS_Network.NutUPS & ".", "")
                    LogFile.LogTracing("Update {VarName}", LogLvl.LOG_DEBUG, Me)
                    Lbl_V_Value.Text = WinNUT.UPS_Network.GetUPSVar(VarName)
                End If
            End If
        End If
    End Sub

    Private Sub Btn_Close_Click(sender As Object, e As EventArgs) Handles Btn_Close.Click
        LogFile.LogTracing("Close List Var Gui", LogLvl.LOG_DEBUG, Me)
        Me.Close()
    End Sub

    Private Sub Btn_Reload_Click(sender As Object, e As EventArgs) Handles Btn_Reload.Click
        LogFile.LogTracing("Reload Treeview from Button", LogLvl.LOG_DEBUG, Me)
        Lbl_N_Value.Text = ""
        Lbl_V_Value.Text = ""
        Lbl_D_Value.Text = ""
        TView_UPSVar.Nodes.Clear()
        Me.PopulateTreeView()
    End Sub

    Private Sub TView_UPSVar_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TView_UPSVar.NodeMouseClick
        Dim index As Integer = 0
        Dim UPSName = WinNUT_Params.Arr_Reg_Key.Item("UPSName")
        Dim SelectedChild = Strings.Replace(e.Node.FullPath, UPSName & ".", "")
        Dim FindChild As Predicate(Of UPS_Var_Node) = Function(ByVal x As UPS_Var_Node)
                                                          If x.VarKey = SelectedChild Then
                                                              Return True
                                                          Else
                                                              index += 1
                                                              Return False
                                                          End If
                                                      End Function
        If Not SelectedChild = UPSName And List_Var_Datas.FindIndex(FindChild) <> -1 Then
            LogFile.LogTracing("Select {List_Var_Datas.Item(index).VarKey} Node", LogLvl.LOG_DEBUG, Me)
            Lbl_N_Value.Text = List_Var_Datas.Item(index).VarKey
            Lbl_V_Value.Text = List_Var_Datas.Item(index).VarValue
            Lbl_D_Value.Text = List_Var_Datas.Item(index).VarDesc
        Else
            Lbl_N_Value.Text = ""
            Lbl_V_Value.Text = ""
            Lbl_D_Value.Text = ""
        End If
    End Sub

    Private Sub Btn_Clip_Click(sender As Object, e As EventArgs) Handles Btn_Clip.Click
        LogFile.LogTracing("Export TreeView To Clipboard", LogLvl.LOG_DEBUG, Me)
        Dim ToClipboard As String = WinNUT_Params.Arr_Reg_Key.Item("UPSName") & " (" & WinNUT.UPS_Mfr & "/" & WinNUT.UPS_Model & "/" & WinNUT.UPS_Firmware & ")" & vbNewLine
        For Each LDatas In List_Var_Datas
            ToClipboard &= LDatas.VarKey & " (" & LDatas.VarDesc & ") : " & LDatas.VarValue & vbNewLine
        Next
        My.Computer.Clipboard.SetText(ToClipboard)
    End Sub
    Function GetChildren(parentNode As TreeNode) As List(Of String)
        Dim nodes As List(Of String) = New List(Of String)
        GetAllChildren(parentNode, nodes)
        Return nodes
    End Function

    Sub GetAllChildren(parentNode As TreeNode, nodes As List(Of String))
        For Each childNode As TreeNode In parentNode.Nodes
            nodes.Add(childNode.Text)
            GetAllChildren(childNode, nodes)
        Next
    End Sub
End Class