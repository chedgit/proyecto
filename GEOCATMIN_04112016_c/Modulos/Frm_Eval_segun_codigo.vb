Imports System.IO
Imports System.Collections
Imports PORTAL_Clases
Imports ESRI.ArcGIS.Framework
Imports System.Drawing
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.DataSourcesOleDB
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geometry
Imports System.Xml

Imports ESRI.ArcGIS.Carto
Imports System.Windows.Forms
Imports Microsoft.Office.Interop
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.GeoDatabaseUI


Public Class Frm_Eval_segun_codigo
    Public m_Application As IApplication
    Private cls_Oracle As New cls_Oracle
    Private cls_Catastro As New cls_DM_1
    Private cls_DM_2 As New cls_DM_2
    Private cls_Prueba As New cls_Prueba
    Private cls_eval As New Cls_evaluacion
    Private cls_planos As New Cls_planos
    Private lodtbDatos As New DataTable
    Public p_Campo As String
    Public p_Dato As String
    Private Const Col_Codigo = 0
    Private Const Col_Nombre = 1
    Private Const Col_Carta = 2
    Private Const Col_Zona = 3
    Private Const Col_Tipo = 4
    Private Const Col_Naturaleza = 5
    Private Const Col_Hectarea = 6
    Private Const Col_Estado = 7
    Private Const Col_Vigcat = 8

    '************************
    'Ubigeo
    Private Const Col_Codi = 0
    Private Const Col_Dpto = 1
    Private Const Col_Prov = 2
    Private Const Col_Dist = 3
    Private Const Col_Cap_Dist = 4
    Private Const Col_Zonau = 5
    Private Const Col_Ubigeo = 6
    'Carta
    Private Const Col_Cod_Hoja = 0
    Private Const Col_Nom_Hoja = 1
    Private Const Col_Zona_Hoja = 2
    Private Const Col_xMin = 3
    Private Const Col_yMin = 4
    Private Const Col_xMax = 5
    Private Const Col_yMax = 6
    '************************

    ' Simultaneo ced
    Private Const Col_flg_sel = 0
    Private Const Col_grupo = 1
    Private Const Col_num_dm = 2
    Private Const Col_zonac = 3

    'Reserva
    Private Const Col_Sel_R = 0
    Private Const Col_Codigo_R = 1
    Private Const Col_Nombre_R = 2
    Private Const Col_Zona_R = 3
    Private Const Col_Descripcion_R = 4
    Private Const Col_Tipo_R = 5
    Private Const Col_Categoria_R = 6
    Private Const Col_Situacion_R = 7

    Private lo_Inicio_DM As Integer = 0
    Private lo_Zona_Carta As Integer = 0
    Private lo_Filtro_Ubigeo As String = ""
    Private lo_layer As String = ""
    Private cls_Usuario As New cls_Usuario
    Dim lodtbOracle As New DataTable
    Public clsData As New cls_Utilidades

    Private Property v_fecsimul As DataTable

    Private Property v_petsimul As DataTable

    Private Sub Frm_Eval_segun_codigo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Cbo_libdenu.Visible = False
        lista_nm_depa = ""
        Dim lodtbPerfilUsuario As New DataTable
        'glo_InformeDM = ""
        Dim MyDate As Date = Now

        btn_cargarcuadriculas.Enabled = False
        btn_cargarcuadriculas.Visible = False
        btn_procesar.Enabled = False
        btn_procesar.Visible = False

        lblfecha.Enabled = False
        lblfecha.Visible = False
        dtpFecha1.Enabled = False
        dtpFecha1.Visible = False
        dgdResultado.Enabled = False
        dgdResultado.Visible = False
        btnexp_excel.Enabled = False
        btnexp_excel.Visible = False

        btnActualizar.Enabled = False
        btnvermapa.Enabled = False
        btnActualizar.Visible = False
        btnvermapa.Visible = False
        lbl_nombre1.Visible = False
        lbl_nombre2.Visible = False
        cbotipo.Visible = False
        cboarea.Visible = False
        ' btnBuscar.Enabled = True
        lista_nm_depa = ""
        glo_Tool_EVA_02 = False
        v_fecha_dm = RellenarComodin(MyDate.Year, 2, "0") & "/" & RellenarComodin(MyDate.Month, 2, "0") & "/" & MyDate.Day
        fecha_tabla = RellenarComodin(MyDate.Day, 2, "0") & RellenarComodin(MyDate.Month, 2, "0") & RellenarComodin(MyDate.Year, 2, "0")
        glo_Inicio_SDE = False
        Dim lostr_Botones As String = ""
        Dim lostr_Boton As String = ""
        Dim lostr_BotonF As String = ""
        Dim lodtbCboTipo As New DataTable
        gloTool_Generar_Malla = False
        Me.clbLayer.SetItemCheckState(0, Windows.Forms.CheckState.Indeterminate)
        Me.clbLayer.SetItemCheckState(1, Windows.Forms.CheckState.Indeterminate)
        Me.cboConsulta.Enabled = False : Me.cboZona.SelectedIndex = 0 : Me.cboSistema.SelectedIndex = 0
        Me.txtConsulta.Text = ""
        Dim m_form As New GEOCATMIN_IniLogin ' LoginForm
        m_form.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
        'glo_int_Perfil = 0
        If gloint_Acceso = 0 Then
            m_form.ShowDialog()
            '  cls_Catastro.Conexion_SDE(m_Application)
            ' Select Case glo_Inicio_SDE
            '    Case False
            ' MsgBox("No tiene Acceso a la Geodatabase, ...", MsgBoxStyle.Information, "[BDGeocamin]")
            ' Me.Close()
            ' Exit Sub
            ' End Select
            pgloUsuConexionGIS = clsData.ConxGis(gstrUsuarioAcceso, gstrUsuarioClave, gstrDatabaseGIS)


            Dim lodtbRolesUsuario As New DataTable
            Try
                lodtbRolesUsuario = cls_Oracle.F_Obtiene_Role_Usuario("2", "ROL_CONSULTA_CM", pgloUsuConexionGIS)
                Inicializar_Variable_Global()
                'Imagenes Iniciales
                Me.imgMenu.ImageLocation = glo_pathPNG & "\menu.png"
                Me.img_DM.ImageLocation = glo_pathPNG & "\imagen_principal.png"
            Catch ex As Exception
                lodtbRolesUsuario = Nothing
            End Try

            If lodtbRolesUsuario Is Nothing Then
                gloint_Acceso = 0
                MsgBox("Lo siento, no tiene los privilegios para acceder al Sistema BDGeocatmin", MsgBoxStyle.Information, "[BDGeocatmin]")
                Me.Close()
                Exit Sub
                'DialogResult = Windows.Forms.DialogResult.Cancel
            ElseIf lodtbRolesUsuario.Rows.Count = 0 Then
                gloint_Acceso = 0
                MsgBox("Lo siento, no tiene acceso al Sistema BDGeocatmin", MsgBoxStyle.Information, "[BDGeocatmin]")
                Me.Close()
                Exit Sub
            Else
                Try
                    lodtbPerfilUsuario = cls_Oracle.F_Obtiene_Perfil_Usuario(gstrUsuarioAcceso)
                    glo_int_Perfil = CType(lodtbPerfilUsuario.Rows(0).Item("PERFIL"), Integer)
                Catch ex As Exception
                End Try

                For w As Integer = 0 To lodtbRolesUsuario.Rows.Count - 1
                    Select Case lodtbRolesUsuario.Rows(w).Item("GRANTED_ROLE")
                        Case "ROL_CONSULTA_CM" ', "ROL_CONSULTA_GRAL"
                            gstrPerfil_Usuario = "M�dulo: Consulta y Evaluaci�n"
                            Me.Text = gstrPerfil_Usuario
                            'glo_int_Perfil = 2
                            Exit For
                        Case ""
                            gstrPerfil_Usuario = "M�dulo: " & lodtbPerfil.Rows(0).Item("OBSERVACION") & " del Sistema GEOCATMIN" '"Acceso Total"
                            Me.Text = gstrPerfil_Usuario
                        Case ""
                            gstrPerfil_Usuario = "M�dulo: " & lodtbPerfil.Rows(0).Item("OBSERVACION") & " del Sistema GEOCATMIN" '"Consulta"
                            Me.Text = gstrPerfil_Usuario
                    End Select
                Next
            End If
        Else
            'glo_int_Perfil = 1
        End If
        If glo_int_Perfil <> 0 Then
            If m_form.DialogResult = Windows.Forms.DialogResult.Cancel Then
                Me.Close() : Exit Sub
            End If
            If m_form.DialogResult = Windows.Forms.DialogResult.OK Or gloint_Acceso = "1" Then
                gloint_Acceso = "1"
                'Dim cls_WURBINA As New cls_wurbina
                Dim CLS_CATASTRO As New cls_DM_1
                CLS_CATASTRO.Cargar_FeatureClass_SDE()
                Me.Text = gstrPerfil_Usuario
                Me.lblUsuario.Text = "Usuario: " & gstrNombre_Usuario '& gstrUsuarioAcceso
                Pintar_Formulario()
                lodtbCboTipo = cls_Oracle.F_Obtiene_Menu_Usuario_1(glo_int_Perfil, gstrUsuarioAcceso)  'carga opciones de consulta
                Me.lblProduccion.Text = glo_Desarrollo_BD '& " - " & gstrPerfil_Usuario

                'nuevo OP-20     Data Historica   
                Dim dRow As DataRow
                dRow = lodtbCboTipo.NewRow
                dRow.Item("CODIGO") = "OP_20"
                dRow.Item("DESCRIPCION") = "Data Hist�rica por DM"
                lodtbCboTipo.Rows.Add(dRow)

                'nuevo OP-28     Data Historica   
                Dim dRow4 As DataRow
                dRow4 = lodtbCboTipo.NewRow
                dRow4.Item("CODIGO") = "OP_28"
                dRow4.Item("DESCRIPCION") = "Data Hist�rica por Carta"
                lodtbCboTipo.Rows.Add(dRow4)

                'nuevo  OP-22
                'Dim dRow1 As DataRow
                'dRow1 = lodtbCboTipo.NewRow
                'dRow1.Item("CODIGO") = "OP_22"
                'dRow1.Item("DESCRIPCION") = "Demarcaci�n INEI"
                'lodtbCboTipo.Rows.Add(dRow1)

                'nuevo  OP-23
                'Dim dRow1 As DataRow
                'dRow1 = lodtbCboTipo.NewRow
                'dRow1.Item("CODIGO") = "OP_23"
                'dRow1.Item("DESCRIPCION") = "Superposici�n DM"
                'lodtbCboTipo.Rows.Add(dRow1)

                'nuevo(OP - 24)
                Dim dRow2 As DataRow
                dRow2 = lodtbCboTipo.NewRow
                dRow2.Item("CODIGO") = "OP_24"
                dRow2.Item("DESCRIPCION") = "DM Simultaneos"
                lodtbCboTipo.Rows.Add(dRow2)

                ''nuevo  OP-25
                'Dim dRow3 As DataRow
                'dRow3 = lodtbCboTipo.NewRow
                'dRow3.Item("CODIGO") = "OP_25"
                'dRow3.Item("DESCRIPCION") = "Petitorios PMA"
                'lodtbCboTipo.Rows.Add(dRow3)

                'nuevo(OP - 29)
                Dim dRow5 As DataRow
                dRow5 = lodtbCboTipo.NewRow
                dRow5.Item("CODIGO") = "OP_29"
                dRow5.Item("DESCRIPCION") = "DM Acumulaci�n"
                lodtbCboTipo.Rows.Add(dRow5)

                Dim lodtvCboTipo As New DataView(lodtbCboTipo)
                Me.cbo_tipo.DataSource = lodtvCboTipo
                Me.cbo_tipo.DisplayMember = "DESCRIPCION"
                Me.cbo_tipo.ValueMember = "CODIGO"

                If glo_int_Perfil = "2" Then
                    Me.cbo_tipo.SelectedIndex = 1
                    Me.txtConsulta.Focus()
                    Me.txtNorte.TabIndex = 3
                End If
                Me.rbt_Visualiza.Checked = True
                Me.rbt_NoVisualiza.Checked = False
                Me.cbo_tipo.Refresh()
            End If
            btnBuscar.Enabled = True
        Else
            ' MsgBox("Lo siento, no tiene los Roles para accesar al Sistema BDGeocatmin", MsgBoxStyle.Information, "[BDGeocatmin]")
            'Me.Close()
            'Exit Sub
        End If
        Me.txtConsulta.Focus()
        txtConsulta.Enabled = True
    End Sub
    Public Sub Inicializar_Variable_Global()
        Dim lodtbData As New DataTable
        Try
            lodtbData = cls_Oracle.FT_SEL_LISTA_DESCRIPCION("2", "SIGGCTMIN_SRV")
            If lodtbData.Rows.Count = 1 Then
                glo_pathServidor = lodtbData.Rows(0).Item("DESCRIPCION")
                glo_pathIGN = glo_pathServidor & "datos\ecw_cartas\"
                glo_pathImaSat = glo_pathServidor & "Geocatmin\Imagenes\"
                glo_pathMXT = glo_pathServidor & "Geocatmin\Plantillas\"
                glo_pathGEO = glo_pathServidor & "Geocatmin\Geologia\"
                glo_pathREP = glo_pathServidor & "Geocatmin\Reporte\"
                glo_pathPNG = glo_pathServidor & "Geocatmin\Menu_Imagen\"
                glo_pathPNG = glo_pathServidor & "Geocatmin\Menu_Imagen\"
                glo_pathStyle_paises = glo_pathServidor & "Geocatmin\Estilos\"
                lodtbData = New DataTable
                lodtbData = cls_Oracle.FT_SEL_LISTA_DESCRIPCION("2", "SIGGCTMIN_DIREC")
                glo_DirectorCM = lodtbData.Rows(0).Item("DESCRIPCION")
                lodtbData = New DataTable
                lodtbData = cls_Oracle.FT_SEL_LISTA_DESCRIPCION("2", "SIGGCTMIN_ELABO")
                glo_Elaboradop = lodtbData.Rows(0).Item("DESCRIPCION")
            Else
                MsgBox("Conexi�n al Servidor SIGGeocatmin Error ... ", MsgBoxStyle.Information, "[BDGeocatmin]")
                Exit Sub
            End If
        Catch ex As Exception
        End Try

    End Sub
    Public Sub Create_Barra(ByVal m_application As IApplication, ByVal p_Grilla As DataTable)
        Dim pMDoc As IDocument
        Dim pCmdBars As ICommandBars
        Dim pCmdBar As ICommandBar = Nothing
        pMDoc = m_application.Document
        pCmdBars = pMDoc.CommandBars
        Try
            pCmdBar = pCmdBars.Find("Project.BDGeocatmin", False, True)
        Catch ex As Exception
            MsgBox("Error en la creaci�n de la Barra")
        End Try
        If pCmdBar Is Nothing Then
            'pCmdBars.HideAllToolbars() 
            pCmdBar = pCmdBars.Create("BDGeocatmin", ESRI.ArcGIS.SystemUI.esriCmdBarType.esriCmdBarTypeToolbar)
            ' The built in ArcID module is used to find the ArcMap commands. 
            Dim pUID As UID
            For i As Integer = 0 To p_Grilla.Rows.Count - 1
                pUID = New UID
                pUID.Value = "{" & p_Grilla.Rows(i).Item("GUID") & "}" '"{22534c7c-6632-4c4f-9671-bce2954c82de}"
                pCmdBar.Add(pUID)
            Next
            pCmdBar.Dock(esriDockFlags.esriDockShow)
        Else
            If pCmdBar.IsVisible Then
                ' pCmdBar.Dock(esriDockFlags.esriDockHide)
            Else
                pCmdBar.Dock(esriDockFlags.esriDockShow)
            End If
        End If
    End Sub

    Public Sub Create_Barra_1(ByVal m_application As IApplication, ByVal p_Grilla As DataTable)
        Dim pMDocumento As IDocument
        Dim pTool_Bars As ICommandBars
        Dim pTool_Boton As ICommandBar = Nothing
        pMDocumento = m_application.Document
        pTool_Bars = pMDocumento.CommandBars
        Try
            ' pTool_Bars.HideAllToolbars()
            pTool_Boton = pTool_Bars.Find("Project.BDGeocatmin", False, True)
            If pTool_Boton Is Nothing Then
                'pTool_Bars.HideAllToolbars()
                pTool_Boton = pTool_Bars.Create("BDGeocatmin", ESRI.ArcGIS.SystemUI.esriCmdBarType.esriCmdBarTypeToolbar)
                ' The built in ArcID module is used to find the ArcMap commands. 
                Dim pUID As UID
                For i As Integer = 0 To p_Grilla.Rows.Count - 1
                    pUID = New UID
                    pUID.Value = "{" & p_Grilla.Rows(i).Item("GUID") & "}" '"{22534c7c-6632-4c4f-9671-bce2954c82de}"
                    pTool_Boton.Add(pUID)
                Next
                pTool_Boton.Dock(esriDockFlags.esriDockShow)
            Else
                If pTool_Boton.IsVisible Then
                    For w As Integer = 0 To pTool_Boton.Count - 1
                        pTool_Boton.Item(0).Delete()
                    Next
                    Dim pUID As UID
                    For i As Integer = 0 To p_Grilla.Rows.Count - 1
                        pUID = New UID
                        pUID.Value = "{" & p_Grilla.Rows(i).Item("GUID") & "}" '"{22534c7c-6632-4c4f-9671-bce2954c82de}"
                        pTool_Boton.Add(pUID)
                    Next
                Else
                    pTool_Boton.Dock(esriDockFlags.esriDockShow)
                End If
            End If
        Catch ex As Exception
            ' MsgBox("Error")
        End Try
    End Sub
    Private Sub Elimina_Archivo_Temporal()
        Try
            Kill(glo_pathTMP & "\*.*") ' Eliminar Directorio Temporal
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Inicializa_Layer_DM(ByVal p_Tipo As String)
        Select Case p_Tipo
            Case "OP_7", "OP_10", "OP_8", "OP_13"
                Me.clbLayer.Items.Add("Zona Reservada")
                Me.clbLayer.Items.Add("Area Urbana")
                Me.clbLayer.Items.Add("Capitales Distritales")
            Case "OP_2", "OP_12", "OP_11", "OP_1", "OP_18"
                Me.clbLayer.Items.Add("Zona Reservada")
                Me.clbLayer.Items.Add("Area Urbana")
                Me.clbLayer.Items.Add("Limite Departamental")
                Me.clbLayer.Items.Add("Limite Provincial")
                Me.clbLayer.Items.Add("Limite Distrital")
                Me.clbLayer.Items.Add("Centros Poblados")
                Me.clbLayer.Items.Add("Red Hidrografica")
                Me.clbLayer.Items.Add("Red Vial")
            Case "OP_3", "OP_9"
                Me.clbLayer.Items.Add("Zona Reservada")
                Me.clbLayer.Items.Add("Area Urbana")
                Me.clbLayer.Items.Add("Limite Distrital")
                Me.clbLayer.Items.Add("Limite Provincial")
                Me.clbLayer.Items.Add("Limite Departamental")
                Me.clbLayer.Items.Add("Capitales Distritales")
            Case Else
                Me.clbLayer.Items.Add("Zona Reservada")
                Me.clbLayer.Items.Add("Area Urbana")
        End Select
        Me.clbLayer.SetItemCheckState(0, Windows.Forms.CheckState.Indeterminate)
        Me.clbLayer.SetItemCheckState(1, Windows.Forms.CheckState.Indeterminate)

    End Sub

    Private Sub txtConsulta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtConsulta.KeyPress
        If e.KeyChar = Chr(13) Then
            Select Case Me.cbo_tipo.SelectedValue
                Case "OP_7", "OP_10", "OP_8"
                    Dim losender As New System.Object
                    Dim loe As New System.EventArgs
                    Me.btnBuscar_Click(losender, loe)
                Case "OP_20"
                    lo_Flag_DH = 0
                    Dim lodtbFecha As New DataTable
                    lodtbFecha = cls_Oracle.F_SEL_FECHA_DM_HISTORICO(txtConsulta.Text)
                    Dim lodtvFecha As New DataView(lodtbFecha)
                    cboAnio.DataSource = lodtvFecha
                    cboAnio.DisplayMember = "DESCRIPCION"
                    cboAnio.ValueMember = "CODIGO"
                    lo_Flag_DH = 1
            End Select
            Select Case Me.cboConsulta.SelectedIndex
                Case 0
                    Dim losender As New System.Object
                    Dim loe As New System.EventArgs
                    Me.btnBuscar_Click(losender, loe)
                Case 1
                    Dim losender As New System.Object
                    Dim loe As New System.EventArgs
                    Me.btnBuscar_Click(losender, loe)
                Case Else
                    Dim losender As New System.Object
                    Dim loe As New System.EventArgs
                    Me.btnagregar_Click(losender, loe)
            End Select
        End If
    End Sub
    Public Sub Pintar_Formulario()
        Me.BackColor = Drawing.Color.FromArgb(242, 242, 240)
    End Sub


    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If v_sistema = "" Then
            MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "GEOCATMIN")
            Exit Sub
        End If

        If cbo_tipo.SelectedValue = "OP_20" Or cbo_tipo.SelectedValue = "OP_28" Then
            v_sistema = "PSAD-56"
            V_Sistemasele = "01"
            catastro_h = "1"
            lo_fecha_h = fecha_h
        Else
            catastro_h = ""
        End If

        If cbo_tipo.SelectedValue = "OP_29" Then
            v_sistema = "WGS-84"
            V_Sistemasele = "02"
        End If

        If v_sistema = "PSAD-56" Then
            If cbo_tipo.SelectedValue = "OP_20" Or cbo_tipo.SelectedValue = "OP_28" Then
                gstrFC_Catastro_Minero = "CEGO0891.GPO_CATASTRO_MINERO_H"
            Else
                gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
            End If

            gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
            ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
            ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
            'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

            gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
            gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
            gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
            gstrFC_Rios = "GLI_RIO_RIOS"
            gstrFC_Carretera = "GLI_VIA_VIAS"
            gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
            'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"

            'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
            gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
            gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
            gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
            gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
            gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
            gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
            gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
            'gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
            'gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"     
            gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_18"
            gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_18"
            gstrFC_LHojas = "GPO_HOJ_HOJAS"
            gstrFC_Carta = "GPO_HOJ_HOJAS"

            'PARA WGS 84
        ElseIf v_sistema = "WGS-84" Then
            gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
            ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
            'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
            'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

            gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
            gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
            gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
            gstrFC_Rios = "GLI_RIO_RIOS_18"
            gstrFC_Carretera = "GLI_VIA_VIAS_18"
            gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
            'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
            gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
            gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
            'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
            'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
            gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
            gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
            gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
            gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
            gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
            gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
            'gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
            'gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
            gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS_18"
            gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS_18"
            gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
            gstrFC_Carta = "GPO_HOJ_HOJAS_18"

        End If


        Accion_proceso = True
        tipo_opcion = ""
        Dim lostrOpcion As String = ""
        'If valida_informe = "Guardado" Then  'Si ya guardo el anterior codigo su informe tecnico
        If var_fa_validaeval = False And var_fa_AreaSuper = False And var_Fa_AreaReserva = False And var_fa_Coord_SuperAreaReserva = False And var_Fa_Zonaurbana = False And var_Fa_AreasNaturales = False Then
            'nada
        ElseIf var_fa_validaeval = True Or var_fa_AreaSuper = True Or var_Fa_AreaReserva = True Or var_fa_Coord_SuperAreaReserva = True Or var_Fa_Zonaurbana = True Or var_Fa_AreasNaturales = True Then
            If valida_informe <> "Guardado" Then
                Dim m_form As New Form_DatosProcesados
                If Not m_form.Visible Then
                    m_form.m_application = m_Application
                    'm_form1.Show()
                    'SetWindowLong(m_form1.Handle.ToInt32(), GWL_HWNDPARENT, m_Application.hWnd)
                    m_form.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                    m_form.ShowDialog()
                    'If m_form.DialogResult = Windows.Forms.DialogResult.Cancel Then
                    'Exit Sub
                    'End If
                End If
            End If
        End If

        'If Accion_proceso = False Then
        'MsgBox("Ha Cancelado, Vuelva a Interntarlo Nuevamente...", MsgBoxStyle.Critical, "BDGEOCATMIN")
        'Exit Sub
        ' Else 'PROCESO SATISFACTORIO
        var_fa_validaeval = False

        var_fa_AreaSuper = False
        var_Fa_AreaReserva = False
        var_fa_Coord_SuperAreaReserva = False
        var_Fa_Zonaurbana = False
        var_Fa_AreaReserva = False
        var_Fa_AreasNaturales = False
        'End If
        lblRegistro.Text = ""
        Me.dgdDetalle.AllowUpdate = False
        btnBuscar.Cursor = System.Windows.Forms.Cursors.AppStarting
        cls_planos.CambiaADataView(m_Application)
        Pinta_Grilla_Dm()
        Dim lostr_Zona As String = ""
        If Len(Me.txtConsulta.Text) = 0 Then
            MsgBox("Debe ingresar informaci�n", MsgBoxStyle.Information, "Geocatmin")
            Me.txtConsulta.Focus()
            Exit Sub
        End If
        arch_cata = ""
        Dim loStrShapefile As String = "Catastro" & DateTime.Now.Ticks.ToString()
        Dim lo_Filtro_cartas As String = ""
        Dim cls_Usuario As New cls_Usuario
        Dim lodtbLeyenda As New DataTable
        pMxDoc = m_Application.Document
        pMap = pMxDoc.FocusMap
        tipo_seleccion = Me.cbo_tipo.SelectedValue
        Dim cls_eval As New Cls_evaluacion
        Select Case Me.cbo_tipo.SelectedValue 'tipo_seleccion
            Case "OP_7", "OP_10", "OP_8"

                caso_consulta1 = ""
                tipo_opcion = ""
                cls_Catastro.Borra_Todo_Feature("", m_Application)
                cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    'cls_eval.activadataframe("CATASTRO MINERO")
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                cls_eval.Eliminadataframe(caso_consulta)
                ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                'cls_eval.Eliminadataframe()
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                PT_Inicializar_Grilla_Ubigeo()
                Dim cls_Oracle As New cls_Oracle
                Dim lodbtZona As New DataTable : Dim lodbtUbigeo As New DataTable
                Select Case tipo_seleccion
                    Case "OP_7"
                        lo_layer = "Departamento"
                        lo_Filtro_Ubigeo = "NM_DEPA = '" & txtConsulta.Text.ToUpper & "'"
                        lodbtUbigeo = cls_Oracle.FT_Selecciona_x_Ubigeo(1, txtConsulta.Text.ToUpper)
                    Case "OP_10"
                        lo_layer = "Provincia"
                        lo_Filtro_Ubigeo = "NM_PROV LIKE '" & txtConsulta.Text.ToUpper & "%'"
                        lodbtUbigeo = cls_Oracle.FT_Selecciona_x_Ubigeo(2, txtConsulta.Text.ToUpper)
                    Case "OP_8"
                        lo_layer = "Distrito"
                        lo_Filtro_Ubigeo = "NM_DIST LIKE '" & txtConsulta.Text.ToUpper & "%'"
                        lodbtUbigeo = cls_Oracle.FT_Selecciona_x_Ubigeo(3, txtConsulta.Text.ToUpper)
                End Select
                If lodbtUbigeo.Rows.Count = 0 Then
                    MsgBox("No existe registro en la capa " & lo_layer & ", verificar datos ", MsgBoxStyle.Information, "[BDGeocatmin]")
                End If

                Me.lblRegistro.Text = "Se ha encontrado:  " & lodbtUbigeo.Rows.Count & "  Registro(s)"
                If lodbtUbigeo.Rows.Count = 0 Then
                    PT_Inicializar_Grilla_Ubigeo()
                    Exit Sub
                End If
                Select Case tipo_seleccion
                    Case "OP_7"
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento, m_Application, "1", True)
                        lo_Filtro_Ubigeo = "CD_DEPA = '" & lodbtUbigeo.Rows(0).Item("UBIGEO") & "'"
                        lodbtZona = cls_Oracle.FT_Selecciona_x_Zona(lodbtUbigeo.Rows(0).Item("UBIGEO") & "0000")
                    Case "OP_10"
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia, m_Application, "1", True)
                        lo_Filtro_Ubigeo = "CD_PROV = '" & Mid(lodbtUbigeo.Rows(0).Item("UBIGEO"), 1, 4) & "'"
                        lodbtZona = cls_Oracle.FT_Selecciona_x_Zona(lodbtUbigeo.Rows(0).Item("UBIGEO"))
                    Case "OP_8"
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito, m_Application, "1", True)
                        lo_Filtro_Ubigeo = "CD_DIST = '" & lodbtUbigeo.Rows(0).Item("UBIGEO") & "'"
                        lodbtZona = cls_Oracle.FT_Selecciona_x_Zona(lodbtUbigeo.Rows(0).Item("UBIGEO"))
                End Select
                If lodbtZona.Rows.Count = 2 Then 'And lodbtUbigeo.Rows.Count = 1 Then
                    Verificar_Zona_Ubigeo(lodbtZona, lodbtUbigeo)
                Else
                    Me.lblZona.Location = New System.Drawing.Point(236, 80)
                    Me.cboZona.Visible = True
                    Me.cboZona.Enabled = True
                    Me.lblZona.Visible = True
                    Me.lblZona.Text = "Esta " & lo_layer & " se encuentra entre " & vbNewLine & "2 ZONAS, seleccione una ZONA"
                    'Dim lodtvZona As New DataView(lodbtZona, Nothing, "DESCRIPCION ASC", DataViewRowState.CurrentRows)
                    cboZona.DataSource = lodbtZona
                    cboZona.DisplayMember = "DESCRIPCION"
                    cboZona.ValueMember = "CODIGO"
                    Me.cboZona.Enabled = True
                    Me.cboZona.SelectedIndex = 1
                    cls_Catastro.Actualizar_DM(Me.cboZona.SelectedValue)
                End If
                '********
                cls_Catastro.DefinitionExpression(lo_Filtro_Ubigeo, m_Application, lo_layer)
                cls_Usuario.Activar_Layer_True_False(True, m_Application)
                cls_Catastro.Seleccioname_Envelope(lo_Filtro_Ubigeo, lo_layer, xMin, yMin, xMax, yMax, Me.cboZona.SelectedValue, m_Application)
                'cls_Catastro.Seleccioname_Envelope(lo_Filtro_Ubigeo, lo_layer, v_xmin, v_ymin, v_xmax, v_ymax, Me.cboZona.SelectedValue, m_Application)

                cls_Catastro.ShowLabel_DM(lo_layer, m_Application)
                Select Case tipo_seleccion
                    Case "OP_7"
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento, m_Application, "1", True)
                    Case "OP_10"
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia, m_Application, "1", True)
                    Case "OP_8"
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito, m_Application, "1", True)
                End Select
                'cls_Catastro.Shade_Poligono(lo_layer, m_Application)
                cls_Catastro.Color_Poligono_Simple(m_Application, lo_layer)
                cls_Catastro.Genera_Imagen_DM("Vista_Ubigeo", "VistaPrevia")
                Me.img_DM.ImageLocation = glo_pathTMP & "\Vista_Ubigeo.jpg"
                cls_Catastro.Quitar_Layer(lo_layer, m_Application) : cls_Catastro.Quitar_Layer(lo_layer, m_Application)
                Me.dgdDetalle.DataSource = lodbtUbigeo
                PT_Agregar_Funciones_UBIGEO() : PT_Forma_Grilla_Ubigeo()
                If lodbtZona.Rows.Count > 2 Then Me.cboZona.SelectedIndex = 0
                Me.clbLayer.Enabled = True
                Me.clbLayer.SetItemCheckState(0, Windows.Forms.CheckState.Indeterminate)
                Me.clbLayer.SetItemCheckState(1, Windows.Forms.CheckState.Indeterminate)
            Case "OP_13"

                caso_consulta1 = ""
                tipo_opcion = ""
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                cls_eval.Eliminadataframe(caso_consulta)

                'cls_eval.Eliminadataframe()  'elimina dataframe
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                For i As Integer = 0 To Me.lstCoordenada.Items.Count - 1
                    If Me.lstCoordenada.GetSelected(i) Then
                        lo_Filtro_cartas = lo_Filtro_cartas & "'" & Mid(lstCoordenada.Items(i).ToString, 3, 4) & "',"
                    End If
                    If i = 0 Then
                        'Dim loBuscar01 As String = ""
                        'Dim loBuscar02 As String = ""
                        'Dim loBuscar03 As String = ""
                        ''loBuscar01 = InStr("ABCDEFG", Mid(Me.txtCarta.Text.ToUpper, InStr(Me.txtCarta.Text.ToUpper, "-") + 1, 1))
                        ''loBuscar02 = InStr("HIJKLMN�OPQR", Mid(Me.txtCarta.Text.ToUpper, InStr(Me.txtCarta.Text.ToUpper, "-") + 1, 1))
                        ''loBuscar03 = InStr("STUVWXYZ", Mid(Me.txtCarta.Text.ToUpper, InStr(Me.txtCarta.Text.ToUpper, "-") + 1, 1))
                        'If loBuscar01 >= 1 Then lostr_Zona = 17
                        'If loBuscar02 >= 1 Then lostr_Zona = 18
                        'If loBuscar03 >= 1 Then lostr_Zona = 19
                        lostr_Zona = "18"
                    End If
                Next
                lo_Filtro_cartas = "CD_HOJA IN (" & Mid(lo_Filtro_cartas, 1, Len(lo_Filtro_cartas) - 1) & ")"

                cls_Catastro.Delete_Rows_FC_GDB("Malla_" & lostr_Zona)
                cls_Catastro.Delete_Rows_FC_GDB("Mallap_" & lostr_Zona)
                cls_Catastro.Load_FC_GDB("Malla_" & lostr_Zona, "", m_Application, True)
                cls_Catastro.Load_FC_GDB("Mallap_" & lostr_Zona, "", m_Application, True)

                cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_LHojas, m_Application, "1", False)
                cls_Catastro.PT_CargarFeatureClass_SDE("GPO_CMI_CATASTRO_MINERO_18", m_Application, "1", False)
                'cls_Catastro.PT_CargarFeatureClass_SDE("GPO_CMI_CATASTRO_MINERO_WGS_18", m_Application, "1", False)

                cls_Prueba.IntersectSelect_por_Limite(m_Application, lo_Filtro_cartas, "", "Cuadrangulo", xMin, yMin, xMax, yMax, txtExiste)
                'cls_Catastro.Expor_Tema(loStrShapefile, "Denun=No", m_application)
                cls_Catastro.Expor_Tema(loStrShapefile, False, m_Application)
                cls_Catastro.Quitar_Layer("Catastro", m_Application)
                cls_Catastro.Quitar_Layer("Cuadrangulo", m_Application)
                cls_Prueba.AddFieldDM(loStrShapefile) : cls_Catastro.Quitar_Layer(loStrShapefile, m_Application)
                cls_Catastro.Add_ShapeFile(loStrShapefile, m_Application)
                cls_Catastro.UpdateValue(lo_Filtro_cartas, m_Application, "Catastro", "")

                lodtbLeyenda = cls_Prueba.f_Genera_Leyenda_DM("Catastro", lo_Filtro_cartas, m_Application)
                cls_Prueba.Poligono_Color_1("Catastro", lodtbLeyenda, glo_pathStyle & "\Catastro.style", "LEYENDA", "", m_Application)
                lodtbLeyenda = Nothing
                cls_Catastro.ShowLabel_DM("Catastro", m_Application)
                cls_Catastro.Genera_Malla_UTM(CType(Me.xMin.Text, Double), CType(yMin.Text, Double), CType(xMax.Text, Double), CType(yMax.Text, Double), lostr_Zona, m_Application)
                cls_Catastro.Rotular_texto_DM("Mallap_" & lostr_Zona, lostr_Zona, m_Application)
                cls_Catastro.Quitar_Layer("Mallap_" & lostr_Zona, m_Application)
            Case "OP_4" ' Area Restringida

                caso_consulta1 = ""
                tipo_opcion = "2"
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                cls_eval.Eliminadataframe(caso_consulta)
                'cls_eval.Eliminadataframe()  'elimina dataframe
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                Me.btnGraficar.Enabled = False
                If Len(Me.txtConsulta.Text) < 2 Then
                    MsgBox("Insuficiente n�mero de car�cteres de b�squeda..", MsgBoxStyle.Information, "SIGCAGMIN")
                    Exit Sub
                End If


                Dim lodtbArea_Reserva As New DataTable
                Select Case Me.cboConsulta.SelectedIndex + 1
                    Case 1
                        'lodtbArea_Reserva = cls_Oracle.F_Obtiene_Area_Reserva(iif(cboConsulta.SelectedIndex=0 then "CODIGO", "NOMBRE"), Me.txtConsulta.Text)
                        lodtbArea_Reserva = cls_Oracle.F_Obtiene_Area_Reserva("CODIGO", Me.txtConsulta.Text.ToUpper)

                    Case 2
                        lodtbArea_Reserva = cls_Oracle.F_Obtiene_Area_Reserva("NOMBRE", Me.txtConsulta.Text.ToUpper)

                End Select
                Me.lblRegistro.Text = "Se ha encontrado:  " & lodtbArea_Reserva.Rows.Count & "  Registro(s)"
                If lodtbArea_Reserva.Rows.Count = 0 Then
                    MsgBox("No Existe ning�n Item con esta consulta ", MsgBoxStyle.Information, "SIGCATMIN")
                    Me.txtConsulta.Text = ""
                    Me.btnGraficar.Enabled = False
                    Exit Sub
                End If
                Me.btnGraficar.Enabled = True
                Me.dgdDetalle.DataSource = lodtbArea_Reserva
                PT_Agregar_Funciones_Reserva() : PT_Forma_Grilla_Reserva()
                Me.chkEstado.Checked = False : Me.chkEstado.Checked = True
                Me.dgdDetalle.AllowUpdate = True
                tipo_seleccion = "OP_4"
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                cls_eval.Eliminadataframe(caso_consulta)
                'cls_eval.Eliminadataframe()
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
            Case "OP_27" ' Consulta de planos de Areas Restringidas

                caso_consulta1 = ""
                tipo_opcion = "3"
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                cls_eval.Eliminadataframe(caso_consulta)
                'cls_eval.Eliminadataframe()  'elimina dataframe
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                Me.btnGraficar.Enabled = False
                If Len(Me.txtConsulta.Text) < 2 Then
                    MsgBox("Insuficiente n�mero de car�cteres de b�squeda..", MsgBoxStyle.Information, "SIGCAGMIN")
                    Exit Sub
                End If

                Dim lodtbArea_Reserva As New DataTable
                Select Case Me.cboConsulta.SelectedIndex + 1
                    Case 1
                        lodtbArea_Reserva = cls_Oracle.F_Obtiene_Area_Reserva("CODIGO", Me.txtConsulta.Text.ToUpper)

                    Case 2
                        lodtbArea_Reserva = cls_Oracle.F_Obtiene_Area_Reserva("NOMBRE", Me.txtConsulta.Text.ToUpper)

                End Select
                Me.lblRegistro.Text = "Se ha encontrado:  " & lodtbArea_Reserva.Rows.Count & "  Registro(s)"
                If lodtbArea_Reserva.Rows.Count = 0 Then
                    MsgBox("No Existe ning�n Item con esta consulta ", MsgBoxStyle.Information, "SIGCAGMIN")
                    Me.txtConsulta.Text = ""
                    Me.btnGraficar.Enabled = False
                    Exit Sub
                End If
                Me.btnGraficar.Enabled = True
                Me.dgdDetalle.DataSource = lodtbArea_Reserva
                PT_Agregar_Funciones_Reserva() : PT_Forma_Grilla_Reserva()
                Me.chkEstado.Checked = False : Me.chkEstado.Checked = True
                Me.dgdDetalle.AllowUpdate = True
                tipo_seleccion = "OP_27"
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                cls_eval.Eliminadataframe(caso_consulta)
                'cls_eval.Eliminadataframe()
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()

            Case "OP_11", "OP_2", "OP_5", "OP_6", "OP_16", "OP_18", "OP_19", "OP_20", "OP_29"   'comentado por pruebas

                '  Case "OP_11", "OP_2", "OP_5", "OP_6", "OP_16", "OP_18"
                ' tipo_opcion = ""
                '  If tipo_seleccion = "OP_11" Or tipo_seleccion = "OP_2" Or tipo_seleccion = "OP_16" Or tipo_seleccion = "OP_18" Then
                If tipo_seleccion = "OP_11" Or tipo_seleccion = "OP_2" Or tipo_seleccion = "OP_18" Or tipo_seleccion = "OP_20" Or tipo_seleccion = "OP_29" Then
                    'If tipo_seleccion = "OP_11" Or tipo_seleccion = "OP_2" Or tipo_seleccion = "OP_3" Then
                    caso_consulta1 = ""
                    caso_consulta = "CATASTRO MINERO"
                    If pMap.Name <> "CATASTRO MINERO" Then
                        cls_planos.buscaadataframe(caso_consulta, False)
                        If valida = False Then
                            pMap.Name = "CATASTRO MINERO"
                            pMxDoc.UpdateContents()
                        End If
                        cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.Eliminadataframe(caso_consulta)  'elimina dataframe  ojo se cambio
                    'cls_eval.Eliminadataframe()  'elimina dataframe  ojo se cambio
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                    End If
                    pMxDoc.UpdateContents()
                    'ElseIf tipo_seleccion = "OP_19" Then
                    '   lst_listadm.Items.Add(txtConsulta.Text)


                ElseIf tipo_seleccion = "OP_5" Then
                    caso_consulta1 = "TIPO CARTA IGN"
                    caso_consulta = "CARTA IGN"
                    cls_planos.buscaadataframe(caso_consulta, False)
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    If valida = False Then
                        cls_eval.adicionadataframe(caso_consulta)
                        'cls_eval.activadataframe(caso_consulta)
                    End If
                    cls_Catastro.Borra_Todo_Feature("", m_Application)
                    cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                ElseIf tipo_seleccion = "OP_6" Then
                    caso_consulta1 = "TIPO DEMARCACION POLITICA"
                    caso_consulta = "DEMARCACION POLITICA"


                    caso_consulta1 = "TIPO DEMARCACION POLITICA"
                    cls_planos.buscaadataframe(caso_consulta, False)
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                    If valida = False Then
                        cls_eval.adicionadataframe(caso_consulta)
                    End If
                    cls_Catastro.Borra_Todo_Feature("", m_Application)
                    cls_Catastro.Limpiar_Texto_Pantalla(m_Application)

                    'caso_consulta1 = "TIPO MAPA GEOLOGICO"
                    'caso_consulta = "TIPO MAPA GEOLOGICO"
                    'cls_planos.buscaadataframe(caso_consulta, False)
                    'cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    'pMxDoc.UpdateContents()
                    'If valida = False Then
                    '    cls_eval.adicionadataframe(caso_consulta)
                    'End If
                    'cls_Catastro.Borra_Todo_Feature("", m_Application)
                    'cls_Catastro.Limpiar_Texto_Pantalla(m_Application)

                End If
                pMxDoc.UpdateContents()
                Dim cls_Prueba As New cls_Prueba
                Dim cls_Oracle As New cls_Oracle
                'Me.lblUsuario.Text = "Usuario: " & txhIngresoUsuario 'gstrUsuarioAcceso
                PT_Inicializar_Grilla_DM()
                Dim lostrRegistro As String = "" : Dim lodbListaDM As New DataTable
                lostrRegistro = cls_Oracle.FT_Cuenta_Registro(cboConsulta.SelectedIndex + 1, Me.txtConsulta.Text)
                If CType(lostrRegistro, Integer) > 150 Then
                    MsgBox("Sea Ud. mas espec�fico en la consulta, hay " & lostrRegistro & " Registro(s)", MsgBoxStyle.Information, "[BDGeocatmin]")
                    Exit Sub
                End If
                Me.dgdDetalle.Focus()
                If cbo_tipo.SelectedValue = "OP_20" Then
                    'fecha_h = cboAnio.SelectedItem.ToString()
                    lodbListaDM = cls_Oracle.F_SEL_LISTA_DM_HISTORICO(txtConsulta.Text, "", fecha_h & "1231", "1")
                Else
                    lodbListaDM = cls_Oracle.F_OBTIENE_DM_UNIQUE(txtConsulta.Text, CType(cboConsulta.SelectedIndex, Integer) + 1)   ' consulta BD petitorios
                End If
                Me.lblRegistro.Text = "Se ha encontrado:  " & lodbListaDM.Rows.Count & "  Registro(s)"
                Select Case lodbListaDM.Rows.Count
                    Case 0
                        MsgBox("No Existe ning�n Registo con esta consulta " & txtConsulta.Text, MsgBoxStyle.Information, "[BDGEOCATMIN]")
                        DialogResult = Windows.Forms.DialogResult.Cancel
                    Case Else

                        Me.dgdDetalle.DataSource = lodbListaDM     ' muestra en el DataGrid los atributos del Petitorio
                        PT_Agregar_Funciones_DM() : PT_Forma_Grilla_DM()
                        Me.cboZona.Text = lodbListaDM.Rows(0).Item("ZONA")
                        If tipo_seleccion = "OP_19" Then
                            lst_listadm.Items.Add(txtConsulta.Text)
                        End If
                        Dim losender As New System.Object
                        Dim loe As New System.EventArgs
                        Me.dgdDetalle_DoubleClick(losender, loe)  'Entra a consultar DM
                End Select
            Case "OP_1", "OP_14", "OP_28" 'DM por Carta y Carta Geologica

                tipo_opcion = "1"
                caso_consulta1 = ""
                cls_Catastro.Borra_Todo_Feature("", m_Application)
                cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                Dim lodtbOracle As New DataTable
                Dim cls_Oracle As New cls_Oracle
                Me.txtdato1.Text = "" : Me.txtdato2.Text = "" : Me.txtdato3.Text = "" : Me.txtdato4.Text = ""
                Me.cboZona.SelectedIndex = 0
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                clbLayer.Enabled = False
                p_Campo = Me.cboConsulta.Text
                p_Dato = Me.txtConsulta.Text
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                cls_eval.Eliminadataframe(caso_consulta)
                'cls_eval.Eliminadataframe()
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                Select Case Me.cboConsulta.SelectedIndex
                    Case 0
                        lodtbOracle = cls_Oracle.F_Obtiene_Carta_oficial("CODIGO", "%" & p_Dato.ToUpper, v_sistema)
                    Case 1
                        lodtbOracle = cls_Oracle.F_Obtiene_Carta_oficial("NOMBRE", p_Dato.ToUpper, v_sistema)
                End Select
                Me.lblRegistro.Text = "Se ha encontrado:  " & lodtbOracle.Rows.Count & "  Registro(s)"
                Me.dgdDetalle.DataSource = lodtbOracle
                PT_Agregar_Funciones_Carta() : PT_Forma_Grilla_Carta()
                Select Case lodtbOracle.Rows.Count
                    Case 0
                        MsgBox("No Existe la Hoja " & p_Dato & " en la Base de Datos para realizar esta consulta", MsgBoxStyle.Information, "BDGEOCATMIN")
                        DialogResult = Windows.Forms.DialogResult.OK
                        Exit Sub
                    Case Else
                        clbLayer.Enabled = True
                        Pintar_Formulario()
                        Me.cboZona.Text = v_zona_dm
                        cls_Catastro.Actualizar_DM(v_zona_dm)
                        Me.btnGraficar.Enabled = False
                        Dim losender As New System.Object
                        Dim loe As New System.EventArgs

                        Me.dgdDetalle_DoubleClick(losender, loe)  'PROCEDIMIENTO DONDE ENTRA LA CONSULTA DEL DM
                        Me.dgdDetalle.Focus()


                        'Case Else
                        '    clbLayer.Enabled = True
                        '    Pintar_Formulario()
                        '    Me.cboZona.Text = v_zona_dm
                        '    cls_Catastro.Actualizar_DM(v_zona_dm)
                        '    Me.btnGraficar.Enabled = False
                End Select
                'Case "OP_20"
                '    'caso_consulta1 = ""
                '    'cboAnio_SelectedIndexChanged(Nothing, Nothing)

                '    Dim lodtbDetalle As New DataTable
                '    If cboConsulta.SelectedIndex = 0 Then
                '        lodtbDetalle = cls_Oracle.F_SEL_LISTA_DM_HISTORICO(txtConsulta.Text, "", "", "")
                '    Else
                '        lodtbDetalle = cls_Oracle.F_SEL_LISTA_DM_HISTORICO("", txtConsulta.Text, "", "")
                '    End If
                '    If lodtbDetalle.Rows.Count > 0 Then
                '        cls_Catastro.Borra_Todo_Feature("", m_Application)
                '        cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                '        'cls_planos.cambiacolor_frame("PORCOLOR")
                '        Me.dgdDetalle.DataSource = lodtbDetalle
                '        Me.cboZona.Text = lodtbDetalle.Rows(0).Item("ZONA")
                '        dgdDetalle.Row = 0
                '        visualizar_datos_dh(dgdDetalle.Item(dgdDetalle.Row, "CODIGO"), dgdDetalle.Item(dgdDetalle.Row, "ANNO"), m_Application)
                '        'cboZona.SelectedValue = dgdDetalle.Item(dgdDetalle.Row, "ZONA").ToString
                '        btnGraficar.Enabled = True
                '    Else
                '        dgdDetalle.DataSource = Nothing
                '        btnGraficar.Enabled = False
                '    End If



                'Case "OP_19"   'prueba

                '    Dim lodtbDetalle As New DataTable
                '    If cboConsulta.SelectedIndex = 0 Then
                '        lodtbDetalle = cls_Oracle.F_SEL_LISTA_DM_HISTORICO(txtConsulta.Text, "", "")
                '    Else
                '        lodtbDetalle = cls_Oracle.F_SEL_LISTA_DM_HISTORICO("", txtConsulta.Text, "")
                '    End If
                '    If lodtbDetalle.Rows.Count > 0 Then
                '        cls_Catastro.Borra_Todo_Feature("", m_Application)
                '        cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                '        'cls_planos.cambiacolor_frame("PORCOLOR")
                '        Me.dgdDetalle.DataSource = lodtbDetalle
                '        Me.cboZona.Text = lodtbDetalle.Rows(0).Item("ZONA")
                '        dgdDetalle.Row = 0
                '        visualizar_datos_dh(dgdDetalle.Item(dgdDetalle.Row, "CODIGO"), dgdDetalle.Item(dgdDetalle.Row, "ANNO"), m_Application)
                '        'cboZona.SelectedValue = dgdDetalle.Item(dgdDetalle.Row, "ZONA").ToString
                '        btnGraficar.Enabled = True
                '    Else
                '        dgdDetalle.DataSource = Nothing
                '        btnGraficar.Enabled = False
                '    End If




                'Dim openFD As New OpenFileDialog
                'With openFD
                '    .Title = "Seleccionar archivos"
                '    .Filter = "Archivos Excel(*.xls;*.xlsx)|*.xls;*.xlsx|Todos los archivos (*.*)|*.*"
                '    .Multiselect = False
                '    .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop
                '    If .ShowDialog = Windows.Forms.DialogResult.OK Then
                '        glo_Path = .FileName
                '    End If
                'End With

                'cls_Catastro.importar()
        End Select
        cboAnio.Text = lo_fecha_h
        btnBuscar.Cursor = System.Windows.Forms.Cursors.Default
    End Sub
    Private Sub Verificar_Zona_Ubigeo(ByVal p_Zona As DataTable, Optional ByVal p_Ubigeo As DataTable = Nothing)
        Select Case p_Zona.Rows.Count
            Case 0
                MsgBox("No existe  " & lo_layer & ", ingrese nuevamente", MsgBoxStyle.Information, "[BDGEOCATMIN]")
                Me.txtConsulta.Text = ""
                Me.txtConsulta.Focus()
                Exit Sub
            Case 2
                Me.cboZona.Enabled = True
                Me.lblZona.Location = New System.Drawing.Point(290, 97)
                Me.lblZona.Text = "Zona: "
                'cls_Catastro.PT_CargarFeatureClass_SDE(lo_layer, m_application, "1", False)
                'Dim lodtvZona As New DataView(p_Zona, Nothing, "DESCRIPCION ASC", DataViewRowState.CurrentRows)
                cboZona.DataSource = p_Zona
                cboZona.DisplayMember = "DESCRIPCION"
                cboZona.ValueMember = "CODIGO"
                Me.cboZona.SelectedIndex = 1
                Me.cboZona.Enabled = False
                cls_Catastro.Actualizar_DM(Me.cboZona.SelectedValue)
            Case 3
                Me.lblZona.Location = New System.Drawing.Point(236, 80)
                Me.cboZona.Visible = True
                Me.cboZona.Enabled = True
                Me.lblZona.Visible = True
                Me.lblZona.Text = "Esta " & lo_layer & " se encuentra entre " & vbNewLine & "2 ZONAS, seleccione una ZONA"
                'Dim lodtvZona As New DataView(p_Zona, Nothing, "DESCRIPCION ASC", DataViewRowState.CurrentRows)
                cboZona.DataSource = p_Zona
                cboZona.DisplayMember = "DESCRIPCION"
                cboZona.ValueMember = "CODIGO"
                Me.cboZona.Enabled = True
        End Select
        Me.btnGraficar.Enabled = True
    End Sub

    Private Sub visualizar_datos_dh(ByVal p_Codigo As String, ByVal p_anno As String, ByVal p_App As IApplication)
        Dim lodtbDetalle As New DataTable
        Dim CLS_DM_1 As New cls_DM_1
        Dim CLS_DM_2 As New cls_DM_2
        If lo_Flag_DH = 0 Then Exit Sub

        Dim loStrShapefile As String = "Poligono_" & DateTime.Now.Ticks.ToString()
        CLS_DM_1.Create_Shapefile_DM_Prueba(glo_pathTMP, loStrShapefile, v_zona_dm, "Poligono")
        Try
            'lodtbDetalle = cls_Oracle.F_SEL_LISTA_DM_COORDENADA(dgdDetalle.Item(dgdDetalle.Row, "CODIGO"), dgdDetalle.Item(dgdDetalle.Row, "ANNO"))
            lodtbDetalle = cls_Oracle.F_SEL_LISTA_DM_COORDENADA(p_Codigo, p_anno)
            If lodtbDetalle.Rows.Count = 0 Then
                MsgBox(".. No Existe Regsitros, ", MsgBoxStyle.Information, "BDGEOCATMIN")
                DialogResult = Windows.Forms.DialogResult.OK : Exit Sub
            End If
            Dim lodtvOrdena_x As New DataView(lodtbDetalle, Nothing, "CD_COREST ASC", DataViewRowState.CurrentRows)
            xMin.Text = lodtvOrdena_x.Item(0).Row("CD_COREST")
            xMax.Text = lodtvOrdena_x.Item(lodtvOrdena_x.Count - 1).Row("CD_COREST")
            Dim lodtvOrdena_y As New DataView(lodtbDetalle, Nothing, "CD_CORNOR ASC", DataViewRowState.CurrentRows)
            yMin.Text = lodtvOrdena_y.Item(0).Row("CD_CORNOR")
            yMax.Text = lodtvOrdena_y.Item(lodtvOrdena_y.Count - 1).Row("CD_CORNOR")
            lstCoordenada.Items.Clear()
            CLS_DM_1.Borra_Todo_Feature("", m_Application)
            arch_cata = "union"
            CLS_DM_1.Load_FC_GDB("gpt_Vertice_DM", "", m_Application, True)
            CLS_DM_1.Delete_Rows_FC_GDB("gpt_Vertice_DM")
            CLS_DM_2.Genera_Polygono_DH(loStrShapefile, lodtbDetalle, 18, m_Application, True)
            cls_Catastro.ShowLabel_DM("gpt_Vertice_DM", m_Application)
            cls_Catastro.Color_Poligono_Simple(m_Application, "Poligono")
            cls_Catastro.Genera_Imagen_DM("Vista_Ubigeo", "VistaPrevia")
            img_DM.ImageLocation = glo_pathTMP & "\Vista_Ubigeo.jpg"
            cls_Catastro.Quitar_Layer("Poligono", m_Application)
            CLS_DM_1.Quitar_Layer("gpt_Vertice_DM", m_Application)
            lstCoordenada.Items.Add(" Vert. " & "        Este  " & "     " & "        Norte  ")
            lstCoordenada.Items.Add("--------------------------------------------")
            For i As Integer = 0 To lodtbDetalle.Rows.Count - 1
                lstCoordenada.Items.Add(" " & RellenarComodin(i + 1, 3, "0") & "   " & _
                Format(Math.Round(lodtbDetalle.Rows(i).Item("CD_COREST"), 3), "###,###.#0") & _
                "   " & Format(Math.Round(lodtbDetalle.Rows(i).Item("CD_CORNOR"), 3), "###,###.#0") & "")
            Next
            txtArea.Text = f_CalCular_Area(lodtbDetalle)
        Catch ex As Exception
        End Try
        txtArea.Enabled = True
        cboZona.Enabled = False
        txtRadio.Enabled = True
        Exit Sub
    End Sub

    Private Sub btnGraficar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraficar.Click
        v_calculAreaint = False
        procesoautmatico_eval = ""
        ' cadena_query_ubica_corre_fin = ""
        cadena_query_ar_corre = ""
        v_valida_canti_rese = "0"
        cadena_query_ar_corre = ""

        If v_sistema = "PSAD-56" Then
            m_Application.Caption = "SISTEMA DE INFORMACI�N GEOGR�FICA - SIGCATMIN...DATUM PSAD 1956"
        Else
            m_Application.Caption = "SISTEMA DE INFORMACI�N GEOGR�FICA - SIGCATMIN...DATUM WGS 1984"
        End If

        'If valida_region_dema = "" Then
        '    MsgBox("Usted indico no graficar el DM en la Provincia Colindante, debe indicar Si para graficar...", MsgBoxStyle.Critical, "BDGECATMIN")

        '    Exit Sub

        'End If
        ''PARA WGS 84
        'gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
        'gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
        'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
        'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"
        'gstrFC_Rios = "GLI_RIO_RIOS_18"
        'gstrFC_Carretera = "GLI_VIA_VIAS_18"
        'gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
        'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
        'gstrFC_Cuadricula = ""
        'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
        'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
        'gstrFC_ZUrbana56 = ""
        'gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
        'gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
        'gstrFC_Frontera = "GLI_FRO_FONTERA_18"
        'gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
        'gstrFC_Carta = "GPO_HOJ_HOJAS"
        ''PARA WGS 84

        GloInt_Opcion = 0
        fecha_archi3 = DateTime.Now.Ticks.ToString
        fecha_archi = DateTime.Now.Ticks.ToString

        Dim lostrOpcioncbo As String = cbo_tipo.Text
        tipo_seleccion = Me.cbo_tipo.SelectedValue
        consulta_dms = ""
        ' lista_dist = "" : lista_prov = "" : lista_depa = ""
        If V_caso_simu <> "SI" Then
            glo_xMin = 0 : glo_yMin = 0 : glo_xMax = 0 : glo_yMax = 0
        End If
        Select Case cbo_tipo.SelectedValue
            Case "OP_11" 'Evaluaci�n
                conta_botones_evaluacion = conta_botones_evaluacion + 1
                GloInt_Opcion = 1
            Case "OP_12" 'Simulacion
                conta_botones_evaluacion = conta_botones_evaluacion + 1
                GloInt_Opcion = 1
            Case "OP_5" 'carta ign
                conta_botones_evaluacion = conta_botones_evaluacion + 1
                GloInt_Opcion = 1
            Case "OP_6" 'demarcacion
                conta_botones_evaluacion = conta_botones_evaluacion + 1
                GloInt_Opcion = 1
            Case "OP_18" 'Acunulacion
                conta_botones_evaluacion = conta_botones_evaluacion + 1
                GloInt_Opcion = 1
            Case "OP_16" 'Libredenu
                conta_botones_libredenu = conta_botones_libredenu + 1
                GloInt_Opcion = 1
            Case "OP_20" 'Evaluaci�n
                conta_botones_evaluacion = conta_botones_evaluacion + 1
                GloInt_Opcion = 1
            Case Else
                GloInt_Opcion = 0
                conta_botones_consulta = conta_botones_consulta + 1
                'conta_botones_evaluacion = conta_botones_evaluacion + 1
        End Select
        HIDDEN_BOTONES_MENU()
        Dim lostrRetornoBD As String = cls_Oracle.FT_Registro(1, cbo_tipo.Text.ToString)        ' devuelve 362894
        Select Case Me.cbo_tipo.SelectedValue
            Case "OP_4"
            Case Else
                Dim RetVal
                btnGraficar.Cursor = System.Windows.Forms.Cursors.AppStarting
                RetVal = Shell(glo_Path & "\estilos\Barra_Proceso.exe", 1)
                Me.Hide()
        End Select
        s_tipo_plano = ""
        Dim lo_Filtro_Capital As String = ""
        Dim pPoint As ESRI.ArcGIS.Geometry.IPoint
        Dim loStrShapefile As String = "Catastro" & DateTime.Now.Ticks.ToString()
        If rbt_Visualiza.Checked = True Then sele_denu = True Else sele_denu = False
        Dim cls_catastro As New cls_DM_1
        'cls_catastro.Conexion_SDE(m_Application)

        Select Case Me.cbo_tipo.SelectedValue

            Case "OP_7", "OP_10", "OP_8"
                v_codigo = ""
                lo_Zona_Carta = Me.cboZona.SelectedValue
                v_zona_dm = lo_Zona_Carta

                Select Case tipo_seleccion.ToUpper
                    Case "OP_7"
                        ' If v_zona_dm = "18" Then
                        'cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento, m_Application, "1", False)
                        'Else
                        If v_sistema = "PSAD-56" Then
                            cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_Z & v_zona_dm, m_Application, "1", False)
                        Else
                            cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_WGS & v_zona_dm, m_Application, "1", False)
                        End If


                        ' End If


                    Case "OP_10"
                        'If v_zona_dm = "18" Then
                        'cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia, m_Application, "1", False)
                        'Else
                        If v_sistema = "PSAD-56" Then
                            cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia_Z & v_zona_dm, m_Application, "1", False)
                        Else
                            cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia_WGS & v_zona_dm, m_Application, "1", False)
                        End If

                        'End If

                    Case "OP_8"
                        'If v_zona_dm = "18" Then
                        ' cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito, m_Application, "1", False)
                        ' Else
                        If v_sistema = "PSAD-56" Then
                            cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito_Z & v_zona_dm, m_Application, "1", False)
                        Else
                            cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito_WGS & v_zona_dm, m_Application, "1", False)
                        End If


                        'End If

                End Select
                ' lo_Zona_Carta = Me.cboZona.SelectedValue
                ' v_zona_dm = lo_Zona_Carta

                Dim lodtbLeyenda As New DataTable
                Try
                    cls_catastro.Actualizar_DM(lo_Zona_Carta)
                    cls_catastro.Delete_Rows_FC("Malla_" & lo_Zona_Carta)
                    cls_catastro.Delete_Rows_FC("Mallap_" & lo_Zona_Carta)
                    cls_catastro.Load_FC("Malla_" & lo_Zona_Carta, "", m_Application, True)
                    cls_catastro.Load_FC("Mallap_" & lo_Zona_Carta, "", m_Application, False)

                    cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero & lo_Zona_Carta, m_Application, "1", False)
                    'cls_catastro.PT_CargarFeatureClass_SDE("GPO_CMI_CATASTRO_MINERO_" & lo_Zona_Carta, m_Application, "1", False)
                    'cls_catastro.Conexion_SDE(m_Application)
                    cls_Prueba.IntersectSelect_por_Limite(m_Application, lo_Filtro_Ubigeo, "", lo_layer, xMin, yMin, xMax, yMax, txtExiste)

                    'Exit Sub

                    Select Case txtExiste.Text
                        Case -1
                            cls_eval.cierra_ejecutable()
                            Me.Show()
                            cls_catastro.Borra_Todo_Feature("", m_Application)
                            cls_catastro.Limpiar_Texto_Pantalla(m_Application)
                            MsgBox("..No existe Denuncios en esta Zona..", MsgBoxStyle.Information, "BDGEOTCATMIN")
                            'DialogResult = Windows.Forms.DialogResult.Cancel
                        Case Else
                            cls_catastro.Expor_Tema(loStrShapefile, False, m_Application)
                            cls_catastro.Quitar_Layer("Catastro", m_Application)
                            'cls_Prueba.AddFieldDM(loStrShapefile)
                            'arch_cata = "Cat_exporta"
                            'cls_catastro.Exportando_Temas("Cat_exporta", loStrShapefile, m_Application)
                            'cls_catastro.Quitar_Layer(loStrShapefile, m_Application)
                            'cls_catastro.Add_ShapeFile(loStrShapefile_cat, m_Application)

                            cls_catastro.Add_ShapeFile_tmp(loStrShapefile, m_Application)
                            cls_eval.agregacampotema_tpm(loStrShapefile, "") 'se puso esto para evitar el bloque
                            cls_catastro.Add_ShapeFile(loStrShapefile, m_Application)

                            cls_catastro.UpdateValue(lo_Filtro_Ubigeo, m_Application, "Catastro")
                            lodtbLeyenda = cls_Prueba.f_Genera_Leyenda_DM("Catastro", lo_Filtro_Ubigeo, m_Application)
                            cls_Prueba.Poligono_Color_1("Catastro", lodtbLeyenda, glo_pathStyle & "\Catastro.style", "LEYENDA", "", m_Application)
                            'cls_Catastro.ShowLabel_DM("Catastro", m_application)
                            cls_catastro.rotulatexto_dm("Catastro", m_Application)

                            cls_catastro.Seleccioname_Envelope(lo_Filtro_Ubigeo, lo_layer, xMin, yMin, xMax, yMax, lo_Zona_Carta, m_Application)
                            'cls_catastro.Seleccioname_Envelope(lo_Filtro_Ubigeo, lo_layer, v_xmin, v_ymin, v_xmax, v_ymax, lo_Zona_Carta, m_Application)
                            ''comentar cuando es en arcgia 10.1, es solo para arcgis 10
                            'cls_catastro.Genera_Malla_UTM(CType(xMin.Text, Double), CType(yMin.Text, Double), CType(xMax.Text, Double), CType(yMax.Text, Double), lo_Zona_Carta, m_Application)
                            ''Esto aplicad a arcgis 10.1
                            cls_catastro.Genera_Malla_UTM(xMin.Text, yMin.Text, xMax.Text, yMax.Text, lo_Zona_Carta, m_Application)

                            cls_catastro.Rotular_texto_DM("Mallap_" & lo_Zona_Carta, lo_Zona_Carta, m_Application)
                            cls_catastro.Quitar_Layer("Mallap_" & lo_Zona_Carta, m_Application)
                            cls_catastro.DefinitionExpression(lo_Filtro_Ubigeo, m_Application, lo_layer)
                            'cls_catastro.Shade_Poligono(lo_layer, m_Application)
                            cls_catastro.Color_Poligono_Simple(m_Application, lo_layer)
                            'cls_Catastro.Style_Linea_GDB("Malla_" & lo_Zona_Carta, glo_pathStyle & "\malla.style", "CLASE", m_application, "GDB")
                            cls_catastro.ShowLabel_DM(lo_layer, m_Application)
                            glo_xMin = CType(Me.xMin.Text, Double) : glo_yMin = CType(Me.yMin.Text, Double)
                            glo_xMax = CType(Me.xMax.Text, Double) : glo_yMax = CType(Me.yMax.Text, Double)
                            cls_Usuario.Activar_Layer_True_False(True, m_Application)
                            Activar_Capas_DM("", clbLayer, lo_Zona_Carta)
                            Select Case tipo_seleccion.ToUpper
                                Case "OP_7"
                                    'cls_catastro.Zoom_to_Layer("Departamento")
                                    cls_Prueba.Zoom_to_Layer("Departamento", m_Application)
                                Case "OP_10"
                                    ' cls_catastro.Zoom_to_Layer("Provincia")
                                    cls_Prueba.Zoom_to_Layer("Provincia", m_Application)
                                Case "OP_8"
                                    '    cls_catastro.Zoom_to_Layer("Distrito")

                                    cls_Prueba.Zoom_to_Layer("Distrito", m_Application)
                                    '  cls_catastro.Zoom_to_Layer("Distrito")
                            End Select
                            cls_catastro.Zoom_to_Layer("Catastro")
                            Me.Close()
                            pMxDoc.ActiveView.Refresh()
                            pMxDoc.UpdateContents()

                            lodtbLeyenda = Nothing
                            Me.btnGraficar.Enabled = False
                            Me.btnOtraConsulta.Enabled = True
                            DialogResult = Windows.Forms.DialogResult.Cancel
                    End Select
                Catch ex As Exception
                End Try
            Case "OP_12"  'simulacion

                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If

                'cls_eval.Eliminadataframe()
                cls_eval.Eliminadataframe(caso_consulta)  'elimina dataframe  ojo se cambio
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                If Me.cboZona.SelectedIndex = 0 Then
                    MsgBox("Seleccione la Zona en que desea Graficar la consulta ..", MsgBoxStyle.Information, "[BBDGEOCATMIN]")
                    Me.cboZona.Focus()
                    Exit Sub
                End If
                v_zona_dm = Me.cboZona.Text
                v_radio = Me.txtRadio.Text
                v_dato1 = Me.xMin.Text
                v_dato3 = Me.xMax.Text
                v_dato2 = Me.yMin.Text
                v_dato4 = Me.yMax.Text
                v_nombre_dm = "DM_Simulado"
                cls_catastro.Consulta_Evaluacion_DM(m_Application, clbLayer)

                cls_Usuario.Activar_Layer_True_False(True, m_Application)
                Activar_Capas_DM("", clbLayer, v_zona_dm)
            Case "OP_13" 'VARIAS CARTAS

                If v_sistema = "" Then
                    MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "GEOCATMIN")
                    Exit Sub
                End If
                If v_sistema = "PSAD-56" Then

                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                    ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                    gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                    gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                    gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                    gstrFC_Rios = "GLI_RIO_RIOS"
                    gstrFC_Carretera = "GLI_VIA_VIAS"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                    'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS"
                    gstrFC_Carta = "GPO_HOJ_HOJAS"

                    'PARA WGS 84
                ElseIf v_sistema = "WGS-84" Then
                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                    'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                    gstrFC_Rios = "GLI_RIO_RIOS_18"
                    gstrFC_Carretera = "GLI_VIA_VIAS_18"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                    'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                    'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                    gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                    'PARA WGS 84
                End If

                v_codigo = ""
                caso_consulta = "CATASTRO MINERO"

                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'Elimina dataframe
                'cls_eval.Eliminadataframe()
                cls_eval.Eliminadataframe(caso_consulta)
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                cls_catastro.Borra_Todo_Feature("", m_Application)
                cls_catastro.Limpiar_Texto_Pantalla(m_Application)
                If Me.lstCoordenada.Items.Count = 0 Then Exit Sub
                Dim lostr_Filtro As String = ""
                For i As Integer = 0 To Me.lstCoordenada.Items.Count - 1
                    Me.lstCoordenada.SelectedIndex = i
                    lostr_Filtro = lostr_Filtro & "'" & RellenarComodin(Mid(lstCoordenada.Text, InStr(lstCoordenada.Text, ":") + 4), 4, "0") & "',"
                Next
                lostr_Filtro = "CD_HOJA IN (" & Mid(lostr_Filtro, 1, Len(lostr_Filtro) - 1) & ")"
                cls_catastro.Actualizar_DM(lo_Zona_Carta)
                cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Carta, m_Application, "1", False)
                cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero & lo_Zona_Carta, m_Application, "1", False)
                'cls_catastro.Conexion_SDE(m_Application)
                cls_Prueba.IntersectSelect_por_Limite(m_Application, lostr_Filtro, "ESTADO <> 'Y'", "Cuadrangulo", xMin, yMin, xMax, yMax, txtExiste)
                Select Case txtExiste.Text
                    Case -1
                        cls_catastro.Borra_Todo_Feature("", m_Application)
                        cls_catastro.Limpiar_Texto_Pantalla(m_Application)
                        MsgBox("..No existe Derechos Mineros en esta Hoja..", MsgBoxStyle.Information, "BDGEOTCATMIN")
                    Case Else
                        cls_catastro.Expor_Tema(loStrShapefile, False, m_Application)
                        cls_catastro.Quitar_Layer("Catastro", m_Application)
                        'cls_Prueba.AddFieldDM(loStrShapefile) ': cls_catastro.Quitar_Layer(loStrShapefile, m_Application)
                        'arch_cata = "Cat_exporta"
                        'cls_catastro.Exportando_Temas("Cat_exporta", loStrShapefile, m_Application)
                        'cls_catastro.Quitar_Layer(loStrShapefile, m_Application)
                        'cls_catastro.Add_ShapeFile(loStrShapefile_cat, m_Application)
                        cls_catastro.Add_ShapeFile_tmp(loStrShapefile, m_Application)
                        cls_eval.agregacampotema_tpm(loStrShapefile, "") 'se puso esto para evitar el bloque
                        cls_catastro.Add_ShapeFile(loStrShapefile, m_Application)

                        cls_catastro.UpdateValue(lostr_Filtro, m_Application, "Catastro", "")
                        Dim lodtbLeyenda As New DataTable
                        lodtbLeyenda = cls_Prueba.f_Genera_Leyenda_DM("Catastro", lostr_Filtro, m_Application)
                        cls_Prueba.Poligono_Color_1("Catastro", lodtbLeyenda, glo_pathStyle & "\Catastro.style", "LEYENDA", "", m_Application)
                        lodtbLeyenda = Nothing
                        'cls_Catastro.ShowLabel_DM("Catastro", m_application)
                        cls_catastro.rotulatexto_dm("Catastro", m_Application)
                        cls_catastro.Delete_Rows_FC_GDB("Malla_" & lo_Zona_Carta)
                        cls_catastro.Delete_Rows_FC_GDB("Mallap_" & lo_Zona_Carta)
                        cls_catastro.Load_FC_GDB("Malla_" & lo_Zona_Carta, "", m_Application, True)
                        cls_catastro.Load_FC_GDB("Mallap_" & lo_Zona_Carta, "", m_Application, True)
                        pPoint = New ESRI.ArcGIS.Geometry.Point
                        pPoint.X = xMin.Text : pPoint.Y = yMin.Text
                        ' pPoint.SpatialReference = Datum_PSAD_18
                        cls_catastro.Actualizar_DM(lo_Zona_Carta)
                        'Select Case lo_Zona_Carta
                        '    Case 17
                        '        pPoint.Project(Datum_PSAD_17)
                        '        xMin.Text = pPoint.X : yMin.Text = pPoint.Y
                        '        pPoint = New ESRI.ArcGIS.Geometry.Point
                        '        pPoint.X = xMax.Text : pPoint.Y = yMax.Text
                        '        pPoint.SpatialReference = Datum_PSAD_18
                        '        pPoint.Project(Datum_PSAD_17)
                        '        xMax.Text = pPoint.X : yMax.Text = pPoint.Y
                        '    Case 19
                        '        pPoint.Project(Datum_PSAD_19)
                        '        xMin.Text = pPoint.X : yMin.Text = pPoint.Y
                        '        pPoint = New ESRI.ArcGIS.Geometry.Point
                        '        pPoint.X = xMax.Text : pPoint.Y = yMax.Text
                        '        pPoint.SpatialReference = Datum_PSAD_18
                        '        pPoint.Project(Datum_PSAD_19)
                        '        xMax.Text = pPoint.X : yMax.Text = pPoint.Y
                        'End Select
                        cls_catastro.Genera_Malla_UTM(CType(Me.xMin.Text, Double), CType(yMin.Text, Double), CType(xMax.Text, Double), CType(yMax.Text, Double), lo_Zona_Carta, m_Application)
                        cls_catastro.Rotular_texto_DM("Mallap_" & lo_Zona_Carta, lo_Zona_Carta, m_Application)
                        cls_catastro.Quitar_Layer("Mallap_" & lo_Zona_Carta, m_Application)
                        'cls_Catastro.Shade_Poligono("Cuadrangulo", m_application)
                        glo_xMin = CType(Me.xMin.Text, Double) : glo_yMin = CType(Me.yMin.Text, Double)
                        glo_xMax = CType(Me.xMax.Text, Double) : glo_yMax = CType(Me.yMax.Text, Double)
                        Me.btnOtraConsulta.Enabled = True : Me.btnGraficar.Enabled = False

                        Activar_Capas_DM("CARTA IN " & Mid(lostr_Filtro, InStr(lostr_Filtro, "(")).ToUpper, clbLayer, lo_Zona_Carta)
                        cls_catastro.Quitar_Layer("Cuadrangulo", m_Application)
                        cls_catastro.Zoom_to_Layer("Catastro")

                End Select
                Me.Close()
            Case "OP_3" 'SEGUN XY

                '''''''''''''''''''''''  se agrega para WGS 84 ''''''''''''''''''''''''''''''''''''''''''
                If v_sistema = "" Then
                    MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "GEOCATMIN")
                    Exit Sub
                End If

                If v_sistema = "PSAD-56" Then

                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                    ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                    gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                    gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                    gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                    gstrFC_Rios = "GLI_RIO_RIOS"
                    gstrFC_Carretera = "GLI_VIA_VIAS"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                    'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                    'gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                    'gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS"
                    gstrFC_Carta = "GPO_HOJ_HOJAS"

                    'PARA WGS 84
                ElseIf v_sistema = "WGS-84" Then
                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                    'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                    gstrFC_Rios = "GLI_RIO_RIOS_18"
                    gstrFC_Carretera = "GLI_VIA_VIAS_18"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                    'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                    'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                    'gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                    'gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS_"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS_"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                    gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                    'PARA WGS 84
                End If
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'fecha_archi = DateTime.Now.Ticks.ToString()
                tipo_opcion = ""
                v_codigo = ""
                pMxDoc = m_Application.Document
                pMap = pMxDoc.FocusMap
                v_dato1 = Me.txtdato1.Text
                v_dato2 = Me.txtdato2.Text
                v_zona_dm = Trim(Me.cboZona.Text)
                v_radio = txtRadio.Text
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                cls_eval.Eliminadataframe(caso_consulta)
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                'Dim p_Existe As Boolean = False


                ''Condicion para las regiones  --- NUEVO
                'Dim ptoregion As String = ""
                'ptoregion = cls_Oracle.F_Pto_Region(v_dato1, v_dato2, v_zona_dm)
                'If Mid(ptoregion, 1, 2) <> cd_region_sele Then
                '    MsgBox("Las coordenadas ingresadas no se ubican en la Regi�n..", MsgBoxStyle.Information, "Observaci�n...")
                '    Exit Sub
                'End If


                cls_catastro.Consulta_XY(m_Application, clbLayer, txtExiste)


                Select Case txtExiste.Text
                    Case "1"
                        cls_Usuario.Activar_Layer_True_False(True, m_Application)
                        Activar_Capas_DM("", clbLayer, v_zona_dm) : Me.Close()
                        cls_catastro.Zoom_to_Layer("Catastro")
                    Case "-1"
                        Me.Show()
                        MsgBox("No existe Derechos Mineros en el Area de Interes indicado..", MsgBoxStyle.Information, "[BDGeocatmin]")
                End Select
            Case "OP_16" 'LIBRE DENUNCIABILIDAD SEGUN XY "

                'If v_sistema = "" Then
                '    MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "GEOCATMIN")
                '    Exit Sub
                'End If
                'If v_sistema = "PSAD-56" Then

                '    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                '    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                '    ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                '    'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                '    gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                '    gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                '    gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                '    gstrFC_Rios = "GLI_RIO_RIOS"
                '    gstrFC_Carretera = "GLI_VIA_VIAS"
                '    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                '    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                '    'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                '    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                '    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                '    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                '    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                '    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                '    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                '    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                '    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                '    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                '    gstrFC_LHojas = "GPO_HOJ_HOJAS"
                '    gstrFC_Carta = "GPO_HOJ_HOJAS"

                '    'PARA WGS 84
                'ElseIf v_sistema = "WGS-84" Then
                '    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                '    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                '    'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                '    'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                '    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                '    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                '    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                '    gstrFC_Rios = "GLI_RIO_RIOS_18"
                '    gstrFC_Carretera = "GLI_VIA_VIAS_18"
                '    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                '    'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                '    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                '    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                '    'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                '    'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                '    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                '    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                '    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                '    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                '    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                '    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                '    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                '    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                '    gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                '    gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                '    'PARA WGS 84
                'End If

                'tipo_opcion = ""
                tipo_seleccion = "OP_16"
                pMxDoc = m_Application.Document
                pMap = pMxDoc.FocusMap
                'v_dato1 = Me.txtdato1.Text
                'v_dato2 = Me.txtdato2.Text
                v_zona_dm = Trim(Me.cboZona.Text)
                v_radio = txtRadio.Text
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                'cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                cls_eval.Eliminadataframe(caso_consulta)
                'cls_eval.Eliminadataframe()

                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                'Dim p_Existe As Boolean = False
                cls_catastro.Consulta_DM_LibDenu(m_Application, clbLayer, txtExiste)
                '  Exit Sub

                cls_eval.cierra_ejecutable()
                cls_catastro.Zoom_to_Layer("Catastro")
                cls_eval.asigna_escaladataframe("CATASTRO MINERO")
                ' escala_plano_eval = pMap.MapScale
                conta_libredenu = 0
                MsgBox("El Proceso Termino Satisfactoriamente...", MsgBoxStyle.Exclamation, "GEOCATMIN...")



            Case "OP_19" 'MAPA GEOLOGICO"

                If v_sistema = "" Then
                    MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "GEOCATMIN")
                    Exit Sub
                End If
                If v_sistema = "PSAD-56" Then

                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                    ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                    gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                    gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                    gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                    gstrFC_Rios = "GLI_RIO_RIOS"
                    gstrFC_Carretera = "GLI_VIA_VIAS"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                    'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS"
                    gstrFC_Carta = "GPO_HOJ_HOJAS"

                    'PARA WGS 84
                ElseIf v_sistema = "WGS-84" Then
                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                    'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                    gstrFC_Rios = "GLI_RIO_RIOS_18"
                    gstrFC_Carretera = "GLI_VIA_VIAS_18"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                    'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                    'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                    gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                    'PARA WGS 84
                End If

                tipo_seleccion = "OP_19"
                Dim cadena As String

                pMxDoc = m_Application.Document
                pMap = pMxDoc.FocusMap

                'v_zona_dm = Trim(Me.cboZona.Text)
                v_radio = txtRadio.Text
                caso_consulta = "MAPA GEOLOGICO"
                If pMap.Name <> caso_consulta Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = caso_consulta
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                'cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                cls_eval.Eliminadataframe(caso_consulta)
                'cls_eval.Eliminadataframe()

                cls_planos.buscaadataframe(caso_consulta, False)
                If valida = False Then
                    pMap.Name = caso_consulta
                End If
                pMxDoc.UpdateContents()


                caso_consulta1 = "TIPO MAPA GEOLOGICO"


                If Cbo_libdenu.Text = "C�digo DM" Then
                    Tipo_lib = "Individual"

                    lista_Cd_bol_Geologia = ""
                    lista_Cd_Geologia_fran50 = ""
                    lista_Cd_Geologia_fran100 = ""
                    lista_Cd_boletin = ""
                    lista_Cd_bol_Geologia_bufer = ""
                    lista_Cd_Geologia_bufer100 = ""
                    lista_Cd_Geologia_bufer50 = ""

                    cls_catastro.Consulta_Segun_CartaIGN(m_Application)

                ElseIf Cbo_libdenu.Text = "Masivo" Then
                    Dim cuenta As Long = 0

                    Dim lodbListaDM As DataTable

                    'For elemento As Integer = 0 To colecciones_dm_geolo.Count - 1

                    For elemento As Integer = 0 To lst_listadm.Items.Count - 1
                        cuenta = cuenta + 1
                        v_codigo = lst_listadm.Items.Item(elemento)
                        ' cadena = Microsoft.VisualBasic.Right(lst_listadm.Items.Item(elemento), 2)
                        ' v_codigo = Microsoft.VisualBasic.Left(lst_listadm.Items.Item(elemento), Len((lst_listadm.Items.Item(elemento))) - 2)
                        lodbListaDM = cls_Oracle.F_OBTIENE_DM_UNIQUE(v_codigo, 1)
                        cadena = (lodtbDatos.Rows(0).Item("PE_ZONCAT").ToString)

                        m_Application.Caption = "PROCESO  MASIVO DE GENERACION DE MAPAS GEOLOGICOS :  " & v_codigo & "  " & cuenta.ToString & "...DE " & lst_listadm.Items.Count
                        cls_catastro.Consulta_masiva_mapas_geologicos(m_Application, lst_listadm, txtExiste, v_codigo, cadena)

                        lista_Cd_bol_Geologia = ""
                        lista_Cd_Geologia_fran50 = ""
                        lista_Cd_Geologia_fran100 = ""
                        lista_Cd_boletin = ""
                        lista_Cd_bol_Geologia_bufer = ""
                        lista_Cd_Geologia_bufer100 = ""
                        lista_Cd_Geologia_bufer50 = ""


                        'cadena = Microsoft.VisualBasic.Right(colecciones_dm_geolo.Item(elemento), 2)
                        'v_codigo = Microsoft.VisualBasic.Left(colecciones_dm_geolo.Item(elemento), Len((colecciones_dm_geolo.Item(elemento))) - 2)
                        'm_Application.Caption = "PROCESO  MASIVO DE GENERACION DE MAPAS GEOLOGICOS :  " & v_codigo & "  " & cuenta.ToString & "...DE " & lst_listadm.Items.Count
                        'cls_catastro.Consulta_masiva_mapas_geologicos(m_Application, lst_listadm, txtExiste, v_codigo, cadena)
                    Next elemento
                    cuenta = 0
                    '  Exit Sub

                    cls_eval.cierra_ejecutable()
                    'cls_catastro.Zoom_to_Layer("Catastro")
                    '   cls_eval.asigna_escaladataframe(caso_consulta)
                    ' escala_plano_eval = pMap.MapScale
                    '  conta_libredenu = 0
                    MsgBox("El Proceso Termino Satisfactoriamente...", MsgBoxStyle.Exclamation, "SIGCATMIN...")

                End If

            Case "OP_17" 'PROCESO PADRON MINERO

                If v_sistema = "" Then
                    MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "GEOCATMIN")
                    Exit Sub
                End If
                If v_sistema = "PSAD-56" Then

                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                    ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                    gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                    gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                    gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                    gstrFC_Rios = "GLI_RIO_RIOS"
                    gstrFC_Carretera = "GLI_VIA_VIAS"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                    'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS"
                    gstrFC_Carta = "GPO_HOJ_HOJAS"

                    'PARA WGS 84
                ElseIf v_sistema = "WGS-84" Then
                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                    'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                    gstrFC_Rios = "GLI_RIO_RIOS_18"
                    gstrFC_Carretera = "GLI_VIA_VIAS_18"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                    'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                    'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                    gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                    'PARA WGS 84
                End If


                tipo_opcion = ""

                pMxDoc = m_Application.Document
                pMap = pMxDoc.FocusMap

                v_zona_dm = Trim(Me.cboZona.Text)
                v_radio = txtRadio.Text
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                cls_eval.Eliminadataframe(caso_consulta)

                ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                'cls_eval.Eliminadataframe()
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                'Procedimiento para Padron Minero - Calculo Areas Netas

                cls_catastro.Consulta_DM_PadronMinero(m_Application, clbLayer, txtExiste)
                MsgBox("Ha terminado Satisfactoriamiente el Proceso de Calculo de Areas Netas")
                Exit Sub

                'Select Case txtExiste.Text
                '    Case "1"
                '        'cls_Usuario.Activar_Layer_True_False(True, m_Application)
                '        'Activar_Capas_DM("", clbLayer, v_zona_dm) : Me.Close()
                '    Case "-1"
                '        Me.Show()
                '        'MsgBox("No existe Derechos Mineros en el Area de Interes indicado..", MsgBoxStyle.Information, "[BDGeocatmin]")
                'End Select

            Case "OP_9"  'LIMTES MAXIMOS

                If v_sistema = "" Then
                    MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "GEOCATMIN")
                    Exit Sub
                End If
                If v_sistema = "PSAD-56" Then

                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                    ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                    gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                    gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                    gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                    gstrFC_Rios = "GLI_RIO_RIOS"
                    gstrFC_Carretera = "GLI_VIA_VIAS"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                    'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS"
                    gstrFC_Carta = "GPO_HOJ_HOJAS"

                    'PARA WGS 84
                ElseIf v_sistema = "WGS-84" Then
                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                    'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                    gstrFC_Rios = "GLI_RIO_RIOS_18"
                    gstrFC_Carretera = "GLI_VIA_VIAS_18"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                    'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                    'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                    gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                    'PARA WGS 84
                End If

                v_codigo = ""
                v_dato1 = Me.txtdato1.Text
                tipo_opcion = ""
                v_dato2 = Me.txtdato2.Text
                v_dato3 = Me.txtdato3.Text
                v_dato4 = Me.txtdato4.Text
                v_zona_dm = Me.cboZona.Text
                caso_consulta = "CATASTRO MINERO"
                pMxDoc = m_Application.Document
                pMap = pMxDoc.FocusMap
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                cls_eval.Eliminadataframe(caso_consulta)
                ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                'cls_eval.Eliminadataframe()
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()
                cls_catastro.Borra_Todo_Feature("", m_Application)
                cls_catastro.Limpiar_Texto_Pantalla(m_Application)
                cls_catastro.consulta_limitesmaximos(m_Application, clbLayer)
                cls_Usuario.Activar_Layer_True_False(True, m_Application)
                Activar_Capas_DM("", clbLayer, v_zona_dm)
                cls_catastro.Zoom_to_Layer("Catastro")
                Me.Close()

            Case "OP_1", "OP_14", "OP_28" 'CARTA DM Y CARTA GEOLOGICA

                'If v_sistema = "" Then
                '    MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "GEOCATMIN")
                '    Exit Sub
                'End If
                'If v_sistema = "PSAD-56" Then

                '    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                '    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                '    ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                '    'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                '    gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                '    gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                '    gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                '    gstrFC_Rios = "GLI_RIO_RIOS"
                '    gstrFC_Carretera = "GLI_VIA_VIAS"
                '    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                '    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                '    'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                '    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                '    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                '    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                '    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                '    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                '    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                '    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                '    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                '    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                '    gstrFC_LHojas = "GPO_HOJ_HOJAS"
                '    gstrFC_Carta = "GPO_HOJ_HOJAS"

                '    'PARA WGS 84
                'ElseIf v_sistema = "WGS-84" Then
                '    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                '    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                '    'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                '    'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                '    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                '    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                '    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                '    gstrFC_Rios = "GLI_RIO_RIOS_18"
                '    gstrFC_Carretera = "GLI_VIA_VIAS_18"
                '    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                '    'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                '    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                '    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                '    'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                '    'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                '    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                '    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                '    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                '    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                '    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                '    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                '    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                '    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                '    gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                '    gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                '    'PARA WGS 84
                'End If

                pMxDoc.UpdateContents()
                v_dato1 = Me.txtdato1.Text : v_dato2 = Me.txtdato2.Text
                v_dato3 = Me.txtdato3.Text : v_dato4 = Me.txtdato4.Text
                v_zona_dm = Me.cboZona.Text
                cls_catastro.Consulta_Segun_Carta_DM(m_Application, clbLayer, Me.cbo_tipo.SelectedValue)

                If pMap.LayerCount <> 0 Then
                    cls_Usuario.Activar_Layer_True_False(True, m_Application)
                    Activar_Capas_DM(tipo_opcion, clbLayer, v_zona_dm)
                    cls_catastro.Zoom_to_Layer("Catastro")
                    Me.Close()
                Else
                    Me.Show()
                End If
                ' cls_catastro.Zoom_to_Layer("Catastro")
            Case "OP_4" 'AREA RESTRINGIDA

                'If v_sistema = "" Then
                '    MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "GEOCATMIN")
                '    Exit Sub
                'End If
                'If v_sistema = "PSAD-56" Then

                '    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                '    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                '    ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                '    'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                '    gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                '    gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                '    gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                '    gstrFC_Rios = "GLI_RIO_RIOS"
                '    gstrFC_Carretera = "GLI_VIA_VIAS"
                '    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                '    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                '    'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                '    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                '    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                '    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                '    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                '    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                '    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                '    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                '    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                '    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                '    gstrFC_LHojas = "GPO_HOJ_HOJAS"
                '    gstrFC_Carta = "GPO_HOJ_HOJAS"

                '    'PARA WGS 84
                'ElseIf v_sistema = "WGS-84" Then
                '    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                '    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                '    'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                '    'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                '    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                '    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                '    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                '    gstrFC_Rios = "GLI_RIO_RIOS_18"
                '    gstrFC_Carretera = "GLI_VIA_VIAS_18"
                '    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                '    'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                '    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                '    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                '    'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                '    'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                '    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                '    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                '    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                '    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                '    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                '    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                '    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                '    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                '    gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                '    gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                '    'PARA WGS 84
                'End If

                Dim loBoo_flg As Boolean = False
                For w As Integer = 0 To Me.dgdDetalle.RowCount - 1
                    If Me.dgdDetalle.Item(w, "FLG_SEL") Then
                        loBoo_flg = True
                    End If
                Next
                If loBoo_flg = False Then
                    MsgBox("Seleccione un Item de la Lista", MsgBoxStyle.Information, "[SIGCATMIN]")
                    Exit Sub
                End If
                Me.Hide() : Dim RetVal
                btnGraficar.Cursor = System.Windows.Forms.Cursors.AppStarting
                RetVal = Shell(glo_Path & "\estilos\Barra_Proceso.exe", 1)


                cls_catastro.Consulta_Area_Restringida(m_Application, Me.dgdDetalle, txtExiste)


                Select Case txtExiste.Text
                    Case -1
                        'Me.Close() : cls_eval.cierra_ejecutable()
                        'MsgBox("..No existe Derechos Mineros en esta Hoja..", MsgBoxStyle.Information, "BDGEOTCATMIN")

                        'nuevo
                        ' cls_Usuario.Activar_Layer_True_False(True, m_Application)
                        Activar_Capas_DM("", clbLayer, v_zona_dm)
                        cod_opcion_Rese = ""
                        Me.Close()
                    Case -2
                        Me.Show() : cls_eval.cierra_ejecutable()
                        MsgBox("No Existe �l area en consulta", MsgBoxStyle.Information, "SIGCATMIN")
                    Case -3
                        Me.Show() : cls_eval.cierra_ejecutable()
                        MsgBox("Usted est� seleccionando diferentes Areas, debe seleccionar el mismo codigo area reserva", MsgBoxStyle.Information, "SIGCATMIN")
                    Case -4
                        Me.Show() : cls_eval.cierra_ejecutable()
                        MsgBox("Usted est� seleccionando diferentes zonas, debe seleccionar la misma zona para graficar", MsgBoxStyle.Information, "SIGCATMIN")
                    Case -5
                        Me.Show() : cls_eval.cierra_ejecutable()
                        MsgBox("Solo debe seleccionar hasta 2 registros : Nucleo y Amortiguamiento del Mismo Codigo Seleccionado..", MsgBoxStyle.Information, "SIGCATMIN")
                    Case ""
                        Dim losender As New System.Object
                        Dim loe As New System.EventArgs
                        Me.btnBuscar_Click(losender, loe)
                        Me.Show()
                    Case Else
                        cls_Usuario.Activar_Layer_True_False(True, m_Application)
                        Activar_Capas_DM("", clbLayer, v_zona_dm)
                        cod_opcion_Rese = ""
                        Me.Close()
                End Select

            Case "OP_27" 'CONSULTA DE PLANOS DE AREAS RESTRINGIDAS

                If v_sistema = "" Then
                    MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "SIGCATMIN")
                    Exit Sub
                End If
                If v_sistema = "PSAD-56" Then

                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                    ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                    gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                    gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                    gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                    gstrFC_Rios = "GLI_RIO_RIOS"
                    gstrFC_Carretera = "GLI_VIA_VIAS"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                    'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS"
                    gstrFC_Carta = "GPO_HOJ_HOJAS"

                    'PARA WGS 84
                ElseIf v_sistema = "WGS-84" Then
                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                    'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                    gstrFC_Rios = "GLI_RIO_RIOS_18"
                    gstrFC_Carretera = "GLI_VIA_VIAS_18"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                    'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                    'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                    gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                    'PARA WGS 84
                End If

                Dim loBoo_flg As Boolean = False
                For w As Integer = 0 To Me.dgdDetalle.RowCount - 1
                    If Me.dgdDetalle.Item(w, "FLG_SEL") Then
                        loBoo_flg = True
                    End If
                Next
                If loBoo_flg = False Then
                    MsgBox("Seleccione un Item de la Lista", MsgBoxStyle.Information, "SIGCATMIN")
                    Exit Sub
                End If
                Me.Hide() : Dim RetVal
                btnGraficar.Cursor = System.Windows.Forms.Cursors.AppStarting
                RetVal = Shell(glo_Path & "\estilos\Barra_Proceso.exe", 1)
                cls_catastro.Consulta_Area_Restringida_rese(m_Application, Me.dgdDetalle, txtExiste)



                Select Case txtExiste.Text
                    Case -1

                        Activar_Capas_DM("", clbLayer, v_zona_dm)
                        cod_opcion_Rese = ""
                        Me.Close()
                    Case -2
                        Me.Show() : cls_eval.cierra_ejecutable()
                        MsgBox("No Existe C�digo del �rea en consulta", MsgBoxStyle.Information, "[SIGCATMIN]")
                    Case -3
                        Me.Show() : cls_eval.cierra_ejecutable()
                        MsgBox("Usted est� seleccionando diferentes Areas, debe seleccionar el mismo codigo area reserva", MsgBoxStyle.Information, "SIGCATMIN")
                    Case -4
                        Me.Show() : cls_eval.cierra_ejecutable()
                        MsgBox("Usted est� seleccionando diferentes zonas, debe seleccionar la misma zona para graficar", MsgBoxStyle.Information, "SIGCATMIN")
                    Case -5
                        Me.Show() : cls_eval.cierra_ejecutable()
                        MsgBox("Solo debe seleccionar hasta 2 registros : Nucleo y Amortiguamiento del Mismo Codigo Seleccionado..", MsgBoxStyle.Information, "SIGCATMIN")
                    Case ""
                        Dim losender As New System.Object
                        Dim loe As New System.EventArgs
                        Me.btnBuscar_Click(losender, loe)
                        Me.Show()
                    Case Else
                        cls_Usuario.Activar_Layer_True_False(True, m_Application)
                        Activar_Capas_DM("", clbLayer, v_zona_dm)
                        cod_opcion_Rese = ""
                        Me.Close()
                End Select


            Case "OP_11", "OP_2", "OP_18", "OP_20" 'EVALUACION (11) Y CONSULTA (02)
                lista_dist = "" : lista_prov = "" : lista_depa = ""
                If Me.cboZona.SelectedIndex = 0 Then
                    MsgBox("Seleccione la Zona en que desea Graficar la consulta ..", MsgBoxStyle.Information, "SIGCATMIN")
                    Me.cboZona.Focus()
                    Exit Sub
                End If

                'Volver afuera, se puseo dento del op_11
                v_radio = Me.txtRadio.Text
                v_dato1 = Me.xMin.Text
                v_dato3 = Me.xMax.Text
                v_dato2 = Me.yMin.Text
                v_dato4 = Me.yMax.Text
                If tipo_seleccion = "OP_11" Or tipo_seleccion = "OP_18" Or tipo_seleccion = "OP_20" Then
                    'v_radio = Me.txtRadio.Text
                    'v_dato1 = Me.xMin.Text
                    'v_dato3 = Me.xMax.Text
                    'v_dato2 = Me.yMin.Text
                    'v_dato4 = Me.yMax.Text
                    Select Case v_vigcat
                        Case "G"

                            cls_catastro.Consulta_Evaluacion_DM(m_Application, clbLayer)

                            'Exit Sub

                            'hay que eliminar estas dos lineas     OJO
                            'pLayer.Name = "Prioritarios" & v_codigo & fecha_archi_prioritario
                            'cls_catastro.Quitar_Layer("Prioritarios" & v_codigo & fecha_archi_prioritario, m_Application)

                        Case Else

                            If v_sistema = "" Then
                                MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "SIGCATMIN")
                                Exit Sub
                            End If
                            If v_sistema = "PSAD-56" Then

                                gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                                ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                                ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                                'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                                gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                                gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                                gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                                gstrFC_Rios = "GLI_RIO_RIOS"
                                gstrFC_Carretera = "GLI_VIA_VIAS"
                                gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                                gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                                'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                                gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                                gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                                gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                                gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                                gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                                gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                                gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                                gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                                gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                                gstrFC_LHojas = "GPO_HOJ_HOJAS"
                                gstrFC_Carta = "GPO_HOJ_HOJAS"

                                'PARA WGS 84
                            ElseIf v_sistema = "WGS-84" Then
                                gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                                ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                                'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                                'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                                gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                                gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                                gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                                gstrFC_Rios = "GLI_RIO_RIOS_18"
                                gstrFC_Carretera = "GLI_VIA_VIAS_18"
                                gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                                'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                                gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                                gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                                'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                                'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                                gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                                gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                                gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                                gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                                gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                                gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                                gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                                gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                                gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                                gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                                'PARA WGS 84
                            End If

                            tipo_seleccion = "OP_2"

                            cls_catastro.Consulta_segun_codigoDM(m_Application, clbLayer)

                            Me.btnOtraConsulta.Enabled = True
                            'Me.Close()



                            Exit Sub
                    End Select


                    Select Case loglo_MensajeDM

                        Case "6"

                            If v_sistema = "" Then
                                MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "SIGCATMIN")
                                Exit Sub
                            End If
                            If v_sistema = "PSAD-56" Then

                                If cbo_tipo.SelectedValue = "OP_20" Then
                                    gstrFC_Catastro_Minero = "CEGO0891.GPO_CATASTRO_MINERO_H"
                                Else
                                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                                End If

                                gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                                ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                                ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                                'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                                gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                                gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                                gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                                gstrFC_Rios = "GLI_RIO_RIOS"
                                gstrFC_Carretera = "GLI_VIA_VIAS"
                                gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                                'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"

                                'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                                gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                                gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                                gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                                gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                                gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                                gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                                gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                                gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                                gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                                gstrFC_LHojas = "GPO_HOJ_HOJAS"
                                gstrFC_Carta = "GPO_HOJ_HOJAS"

                                'PARA WGS 84
                            ElseIf v_sistema = "WGS-84" Then
                                gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                                ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                                'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                                'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                                gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                                gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                                gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                                gstrFC_Rios = "GLI_RIO_RIOS_18"
                                gstrFC_Carretera = "GLI_VIA_VIAS_18"
                                gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                                'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                                gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                                gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                                'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                                'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                                gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                                gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                                gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                                gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                                gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                                gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                                gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                                gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                                gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                                gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                                'PARA WGS 84
                            End If

                            cls_Usuario.Activar_Layer_True_False(True, m_Application)


                            Activar_Capas_DM(lo_Filtro_Ubigeo, clbLayer, Me.cboZona.SelectedItem)


                            cls_catastro.Zoom_to_Layer("Catastro")
                            cls_eval.asigna_escaladataframe("CATASTRO MINERO")
                            escala_plano_eval = pMap.MapScale
                            var_fa_validaeval = False
                            var_fa_AreaSuper = False
                            var_Fa_AreaReserva = False
                            var_fa_Coord_SuperAreaReserva = False
                            var_Fa_Zonaurbana = False
                            var_Fa_AreaReserva = False
                            var_Fa_AreasNaturales = False
                            Accion_proceso = True
                            ' MsgBox(lista_dist)

                            ' FT_Cargar_Botones_EVA("Catastro")
                            Me.Close()
                        Case "7"
                            Me.Show()
                    End Select
                ElseIf tipo_seleccion = "OP_2" Then

                    If v_sistema = "" Then
                        MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "SIGCATMIN")
                        Exit Sub
                    End If
                    If v_sistema = "PSAD-56" Then

                        gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                        ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                        ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                        'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                        gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                        gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                        gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                        gstrFC_Rios = "GLI_RIO_RIOS"
                        gstrFC_Carretera = "GLI_VIA_VIAS"
                        gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                        gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                        'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                        gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                        gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                        gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                        gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                        gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                        gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                        gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                        gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                        gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                        gstrFC_LHojas = "GPO_HOJ_HOJAS"
                        gstrFC_Carta = "GPO_HOJ_HOJAS"

                        'PARA WGS 84
                    ElseIf v_sistema = "WGS-84" Then
                        gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                        ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                        'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                        'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                        gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                        gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                        gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                        gstrFC_Rios = "GLI_RIO_RIOS_18"
                        gstrFC_Carretera = "GLI_VIA_VIAS_18"
                        gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                        'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                        gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                        gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                        'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                        'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                        gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                        gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                        gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                        gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                        gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                        gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                        gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                        gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                        gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                        gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                        'PARA WGS 84
                    End If

                    cls_catastro.Consulta_segun_codigoDM(m_Application, clbLayer)


                    If loglo_MensajeDM = "6" Then
                        cls_Usuario.Activar_Layer_True_False(True, m_Application)
                        Activar_Capas_DM(lo_Filtro_Ubigeo, clbLayer, Me.cboZona.Text)
                    Else
                        Me.Show()
                    End If


                    'tipo_opcion = ""
                    'pMxDoc = m_Application.Document
                    'pMap = pMxDoc.FocusMap
                    'v_zona_dm = Trim(Me.cboZona.Text)
                    'v_radio = txtRadio.Text
                    'caso_consulta = "CATASTRO MINERO"
                    'If pMap.Name <> "CATASTRO MINERO" Then
                    '    cls_planos.buscaadataframe(caso_consulta, False)
                    '    If valida = False Then
                    '        pMap.Name = "CATASTRO MINERO"
                    '        pMxDoc.UpdateContents()
                    '    End If
                    '    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    '    pMxDoc.UpdateContents()
                    'End If
                    'cls_eval.Eliminadataframe()  'elimina dataframe
                    'cls_planos.buscaadataframe("CATASTRO MINERO", False)
                    'If valida = False Then
                    '    pMap.Name = "CATASTRO MINERO"
                    'End If
                    'pMxDoc.UpdateContents()

                    'cls_catastro.Consulta_DM_LibDenu(m_Application, clbLayer, txtExiste)
                    ''  Exit Sub

                End If

            Case "OP_5", "OP_6" 'CARTA IGN (5), DEMARCA (6)

                If v_sistema = "" Then
                    MsgBox("Debe seleccionar el Datum de Trabajo...", MsgBoxStyle.Critical, "SIGCATMIN")
                    Exit Sub
                End If
                If v_sistema = "PSAD-56" Then

                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
                    ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"

                    gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"
                    gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
                    gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
                    gstrFC_Rios = "GLI_RIO_RIOS"
                    gstrFC_Carretera = "GLI_VIA_VIAS"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                    'gstrFC_Catastro_Minero = "DESA_GIS.GPO_CMI_CATASTRO_MINERO_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS"
                    gstrFC_Carta = "GPO_HOJ_HOJAS"

                    'PARA WGS 84
                ElseIf v_sistema = "WGS-84" Then
                    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
                    ' gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
                    'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
                    'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"

                    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
                    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
                    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
                    gstrFC_Rios = "GLI_RIO_RIOS_18"
                    gstrFC_Carretera = "GLI_VIA_VIAS_18"
                    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
                    'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"     ''''''''''''''''''''''''''''''''''''
                    'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
                    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
                    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
                    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
                    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
                    gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
                    gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
                    gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
                    gstrFC_LHojas = "GPO_HOJ_HOJAS_18"
                    gstrFC_Carta = "GPO_HOJ_HOJAS_18"
                    'PARA WGS 84
                End If

                Me.Show()
                cls_catastro.Borra_Todo_Feature("", m_Application)
                cls_catastro.Limpiar_Texto_Pantalla(m_Application)
                Select Case v_vigcat
                    Case "G"
                        GloInt_Opcion = 1

                        conta_botones_otro = conta_botones_otro + 1
                        If tipo_seleccion = "OP_5" Then
                            caso_consulta1 = "TIPO CARTA IGN"
                            lista_depa_sele = ""
                        ElseIf tipo_seleccion = "OP_6" Then
                            caso_consulta1 = "TIPO DEMARCACION POLITICA"
                            lista_depa_sele = ""
                            'ElseIf tipo_seleccion = "OP_6" Then
                            '    caso_consulta1 = "TIPO MAPA GEOLOGICO"
                            '    lista_depa_sele = ""
                        End If
                        cls_catastro.Consulta_Segun_CartaIGN(m_Application)

                        Me.Close()
                    Case Else
                        cls_eval.cierra_ejecutable()
                        MsgBox("Las Coordenadas de este Derecho Minero no" & vbNewLine & "est�n Habilitadas para el Sistema de Graficaci�n", MsgBoxStyle.OkOnly, "SIGCATMIN")
                        Me.Show()
                End Select
                'Case "OP_20"
                '    v_radio = txtRadio.Text
                '    v_dato1 = xMin.Text
                '    v_dato3 = xMax.Text
                '    v_dato2 = yMin.Text
                '    v_dato4 = yMax.Text
                '    v_zona_dm = cboZona.Text
                '    'v_codigo = txtConsulta.Text
                '    v_codigo = dgdDetalle.Item(dgdDetalle.Row, "CODIGO")
                '    'v_fecha_dh = dgdDetalle.Item(dgdDetalle.Row, "AAMMDD")
                '    v_fecha_dh = dgdDetalle.Item(dgdDetalle.Row, "ANNO")
                '    cls_catastro.Consulta_Evaluacion_DM_DH(m_Application, clbLayer)
                '    lo_glo_xMin = xMin.Text
                '    lo_glo_yMin = yMin.Text
                '    lo_glo_xMax = xMax.Text
                '    lo_glo_yMax = yMax.Text
                '    cls_catastro.PT_CargarFeatureClass_SDE("GPO_DIS_DISTRITO_CAT", m_Application, "1", False)
                '    cls_eval.DefinitionExpressiontema("GPO_DIS_DISTRITO_CAT", m_Application, "Distrito")
                '    arch_cata = "Distrito"
                '    cls_catastro.Color_Poligono_Simple(m_Application, "Distrito")
                '    cls_catastro.ShowLabel_DM("Distrito", m_Application)

                '    cls_catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_" & v_zona_dm, m_Application, "1", False)
                '    Dim lo_Filtro_Zona_Urbana As String = cls_catastro.f_Intercepta_FC("Zona Urbana", lo_glo_xMin, glo_yMin, lo_glo_xMax, lo_glo_yMax, m_Application)
                '    If lo_Filtro_Zona_Urbana = "" Then
                '        arch_cata = "Zona Urbana"
                '        cls_catastro.Color_Poligono_Simple(m_Application, "Zona Urbana")
                '        ' cls_catastro.Quitar_Layer("Zona Urbana", m_Application)
                '    Else
                '        cls_catastro.DefinitionExpression(lo_Filtro_Zona_Urbana, m_Application, "Zona Urbana")
                '        cls_catastro.Color_Poligono_Simple(m_Application, "Zona Urbana")
                '        cls_catastro.rotulatexto_dm("Zona Urbana", m_Application)
                '    End If

                '    cls_catastro.PT_CargarFeatureClass_SDE("GPO_ARE_AREA_RESERVADA_" & v_zona_dm, m_Application, "1", False)
                '    Dim lo_Filtro_Area_Reserva As String = cls_catastro.f_Intercepta_FC("Zona Reservada", lo_glo_xMin, glo_yMin, lo_glo_xMax, lo_glo_yMax, m_Application)
                '    If lo_Filtro_Area_Reserva = "" Then
                '        arch_cata = "Zona Reservada"
                '        cls_catastro.Color_Poligono_Simple(m_Application, "Zona Reservada")
                '        ' cls_catastro.Quitar_Layer("Zona Reservada", m_Application)
                '    Else
                '        cls_catastro.Expor_Tema("Zona Reservada", True, m_Application)
                '        cls_catastro.Quitar_Layer("Zona Reservada", m_Application)
                '        cls_catastro.Add_ShapeFile1("Zona Reservada" & fecha_archi, m_Application, "Zona Reservada")
                '        cls_catastro.Shade_Poligono("Zona Reservada", m_Application)
                '        cls_catastro.rotulatexto_dm("Zona Reservada", m_Application)
                '    End If
                '    cls_catastro.Zoom_to_Layer("Catastro")

            Case "OP_24" 'DM Simultaneos
                Dim cls_planos As New Cls_planos
                cls_planos.CambiaADataView(m_Application)
                cls_catastro.Limpiar_Texto_Pantalla(m_Application)
                pMxDoc.UpdateContents()
                Dim loBoo_flg As Boolean = False
                For w As Integer = 0 To Me.dgdDetalle.RowCount - 1
                    If Me.dgdDetalle.Item(w, "FLG_SEL") Then
                        loBoo_flg = True
                    End If
                Next
                If loBoo_flg = False Then
                    MsgBox("Seleccione un Item de la Lista", MsgBoxStyle.Information, "[BDGeocatmin]")
                    Exit Sub
                End If
                Me.Hide() : Dim RetVal
                btnGraficar.Cursor = System.Windows.Forms.Cursors.AppStarting

                cls_catastro.Consulta_DM_Simultaneoc(m_Application, Me.dgdDetalle, txtExiste)

                'dtCoordenada = Feature_Vertice("Cuadricula_sim", m_Application)

                'cls_catastro.ShowLabel_DM3("DM_simultaneos", m_Application)
                'cls_catastro.ShowLabel_DM3("Cuadricula_sim", m_Application)

                'If Val(num_cuasim) = "1" Then
                '    cls_catastro.ShowLabel_DM3("Cuadricula_sim", m_Application)
                'Else
                '    cls_catastro.ShowLabel_DM3("Cuadricula_dsim", m_Application)
                'End If
                '------------------------------------------------------------------   se inserto este bloque en Consulta_DM_Simultaneoc
                'cls_catastro.rotulatexto_dm("Catastro_sim", m_Application)       
                'If Val(num_cuasim) = "1" Then
                '    cls_catastro.rotulatexto_dm("Cuadricula_sim", m_Application)
                'Else
                '    cls_catastro.rotulatexto_dm("Cuadricula_dsim", m_Application)
                'End If
                '-----------------------------------------------------------------
                'cls_catastro.ShowLabel_DM3("Catastro_sim", m_Application)
                'caso_consulta = "CATASTRO MINERO"
                'caso_consulta = "DM_simultaneos"
                cls_planos.generaplanosimultaneos(m_Application)

                Me.Show()
        End Select
        v_fecha_dh = ""
        Dim lostrRetorno As String = cls_Oracle.FT_Registro(1, lostrOpcioncbo)
        cls_eval.cierra_ejecutable()
        'Abre los botones de las herramientas
        BOTON_MENU(True)
        btnGraficar.Cursor = System.Windows.Forms.Cursors.Default

        'Me.Show()
    End Sub
    Private Sub HIDDEN_BOTONES_MENU()
        Hide_Barra(m_Application, Nothing, "Evaluaci�n")
        'Hide_Barra(m_application, Nothing, "Opciones")
        Hide_Barra(m_Application, Nothing, "Consulta")
    End Sub
    Public Sub Hide_Barra(ByVal m_application As IApplication, ByVal p_Grilla As DataTable, ByVal p_NomVentana As String)
        Dim pMDocumento As IDocument
        Dim pTool_Bars As ICommandBars
        Dim pTool_Boton As ICommandBar = Nothing
        pMDocumento = m_application.Document
        pTool_Bars = pMDocumento.CommandBars
        Try
            pTool_Boton = pTool_Bars.Find("Project." & p_NomVentana, False, True)
            Select Case p_NomVentana
                Case "Consulta"
                    pTool_Boton.Dock(esriDockFlags.esriDockHide)
                Case "Evaluaci�n"
                    pTool_Boton.Dock(esriDockFlags.esriDockHide)
                Case "Opciones"
                    'pTool_Boton.Dock(esriDockFlags.esriDockHide)
            End Select
            Exit Sub
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btnOtraConsulta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOtraConsulta.Click
        Me.lstCoordenada.Items.Clear()
        Me.btnGraficar.Enabled = False
        Me.txtConsulta.Text = ""
        Me.txtConsulta.Focus()
        Me.cboZona.SelectedIndex = 0
        Me.cboZona.Enabled = False
        Dim lodbtNothing As New DataTable
        Select Case Me.cbo_tipo.SelectedValue
            Case "OP_7", "OP_10", "OP_8"
                PT_Inicializar_Grilla_Ubigeo()
            Case "OP_1", "OP_13", "OP_14"
                PT_Inicializar_Grilla_Carta()
            Case "OP_11", "OP_2", "OP_5", "OP_6", "OP_18"
                PT_Inicializar_Grilla_DM()
            Case "OP_12"
                Me.dgdDetalle.DataSource = lodbtNothing
            Case "OP_13"
                Me.dgdDetalle.DataSource = lodbtNothing
            Case "OP_3"
                Me.dgdDetalle.DataSource = lodbtNothing
            Case "OP_9"
                Me.dgdDetalle.DataSource = lodbtNothing
            Case "OP_4"
                PT_Inicializar_Grilla_Reserva()
            Case "OP_27"
                PT_Inicializar_Grilla_Reserva()
            Case "OP_17"
                Me.btnGraficar.Enabled = True
            Case "OP_23"
                Me.dgdResultado.DataSource = lodbtNothing
            Case "OP_24"
                PT_Inicializar_Grilla_Simultaneoc()
            Case "OP_25"
                Me.dgdResultado.DataSource = lodbtNothing
            Case Else
                Me.dgdDetalle.DataSource = lodbtNothing
        End Select
    End Sub

    Public Sub PT_Inicializar_Grilla_Reserva()
        Dim lodtbLista_DM As New DataTable
        lodtbLista_DM.Columns.Add("FLG_SEL", GetType(String))
        lodtbLista_DM.Columns.Add("CG_CODIGO", GetType(String))
        lodtbLista_DM.Columns.Add("PE_NOMARE", GetType(String))
        lodtbLista_DM.Columns.Add("ZA_ZONA", GetType(String))
        lodtbLista_DM.Columns.Add("PA_DESCRI", GetType(String))
        lodtbLista_DM.Columns.Add("TN_DESTIP", GetType(String))
        lodtbLista_DM.Columns.Add("CA_DESCAT", GetType(String))
        lodtbLista_DM.Columns.Add("SE_SITUEX", GetType(String))
        PT_Estilo_Grilla_Reserva(lodtbLista_DM) : PT_Cargar_Grilla_Reserva(lodtbLista_DM)
        PT_Agregar_Funciones_Reserva() : PT_Forma_Grilla_Reserva()
    End Sub

    Public Sub PT_Inicializar_Grilla_Simultaneoc()
        Dim v_petsimul As New DataTable
        v_petsimul.Columns.Add("FLG_SEL", GetType(String))
        v_petsimul.Columns.Add("GRUPO", GetType(String))
        v_petsimul.Columns.Add("NUM_DM", GetType(String))
        v_petsimul.Columns.Add("ZONAC", GetType(String))
        PT_Estilo_Grilla_Simultaneoc(v_petsimul) : PT_Cargar_Grilla_Simultaneoc(v_petsimul)
        PT_Agregar_Funciones_Simultaneoc() : PT_Forma_Grilla_Simultaneoc()
    End Sub

    Private Sub PT_Estilo_Grilla_Reserva(ByRef padtbDetalle As DataTable)
        padtbDetalle.Columns.Item(Col_Sel_R).DefaultValue = 0
        padtbDetalle.Columns.Item(Col_Codigo_R).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Nombre_R).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Zona_R).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Descripcion_R).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Tipo_R).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Categoria_R).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Situacion_R).DefaultValue = ""
    End Sub

    Private Sub PT_Estilo_Grilla_Simultaneoc(ByRef padtbDetalle As DataTable)
        padtbDetalle.Columns.Item(Col_flg_sel).DefaultValue = 0
        padtbDetalle.Columns.Item(Col_grupo).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_num_dm).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_zonac).DefaultValue = ""
    End Sub

    Private Sub PT_Cargar_Grilla_Reserva(ByVal padtbDetalle As DataTable)
        Dim dvwDetalle As New DataView(padtbDetalle)
        Me.dgdDetalle.DataSource = dvwDetalle
        Me.dgdDetalle.Columns(Col_Sel_R).Value = False
        Pinta_Grilla_Ubigeo()
    End Sub

    Private Sub PT_Cargar_Grilla_Simultaneoc(ByVal padtbDetalle As DataTable)
        Dim v_petsimul As New DataView(padtbDetalle)
        Me.dgdDetalle.DataSource = v_petsimul
        Me.dgdDetalle.Columns(Col_flg_sel).Value = False
        Pinta_Grilla_Ubigeo()
    End Sub

    Private Sub PT_Agregar_Funciones_Reserva()
        Me.dgdDetalle.Columns(Col_Codigo_R).DefaultValue = 0
        'Me.dgdDetalle.Columns(Col_Sel_R).ValueItems.Presentation = C1.Win.C1TrueDBGrid.PresentationEnum.CheckBox
        'Me.dgdDetalle.Columns(Col_Sel_R).ValueItems.CycleOnClick = False
        Me.dgdDetalle.Columns(Col_Codigo_R).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Nombre_R).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Zona_R).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Descripcion_R).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Tipo_R).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Categoria_R).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Situacion_R).DefaultValue = ""
    End Sub

    Private Sub PT_Agregar_Funciones_Simultaneoc()
        Me.dgdDetalle.Columns(Col_flg_sel).DefaultValue = 0
        Me.dgdDetalle.Columns(Col_grupo).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_num_dm).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_zonac).DefaultValue = ""
    End Sub

    Private Sub PT_Forma_Grilla_Reserva()
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Sel_R).Width = 25
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codigo_R).Width = 85
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nombre_R).Width = 130
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zona_R).Width = 50
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Descripcion_R).Width = 100
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Tipo_R).Width = 140
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Categoria_R).Width = 150
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Situacion_R).Width = 60
        Me.dgdDetalle.Columns("FLG_SEL").Caption = "Sel."
        Me.dgdDetalle.Columns("CG_CODIGO").Caption = "C�digo"
        Me.dgdDetalle.Columns("PE_NOMARE").Caption = "Nombre"
        Me.dgdDetalle.Columns("ZA_ZONA").Caption = "Zona"
        Me.dgdDetalle.Columns("PA_DESCRI").Caption = "Descripci�n"
        Me.dgdDetalle.Columns("TN_DESTIP").Caption = "Tipo"
        Me.dgdDetalle.Columns("CA_DESCAT").Caption = "Categoria"
        Me.dgdDetalle.Columns("SE_SITUEX").Caption = "Situaci�n"

        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codigo_R).Locked = True
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nombre_R).Locked = True
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zona_R).Locked = True
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Descripcion_R).Locked = True
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Tipo_R).Locked = True
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Categoria_R).Locked = True
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Situacion_R).Locked = True

        Me.dgdDetalle.Splits(0).HeadingStyle.ForeColor = Color.Black
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Sel_R).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codigo_R).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nombre_R).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zona_R).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Descripcion_R).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Tipo_R).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Categoria_R).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Situacion_R).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Sel_R).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codigo_R).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nombre_R).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zona_R).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Descripcion_R).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Tipo_R).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Categoria_R).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Situacion_R).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        'Me.dgdDetalle.Columns(Col_Sel_R).ValueItems.CycleOnClick = False
        'Me.dgdDetalle.Columns(Col_Sel_R).ValueItems.Presentation = C1.Win.C1TrueDBGrid.PresentationEnum.CheckBox
    End Sub

    Private Sub PT_Forma_Grilla_Simultaneoc()
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_flg_sel).Width = 20
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_grupo).Width = 80
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_num_dm).Width = 80
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_zonac).Width = 50

        Me.dgdDetalle.Columns("FLG_SEL").Caption = "Sel."
        Me.dgdDetalle.Columns("GRUPO").Caption = "Grupo"
        Me.dgdDetalle.Columns("NUM_DM").Caption = "N�mero_DM"
        Me.dgdDetalle.Columns("ZONAC").Caption = "Zona"

        Me.dgdDetalle.Splits(0).DisplayColumns(Col_flg_sel).Locked = True
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_grupo).Locked = True
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_num_dm).Locked = True
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_zonac).Locked = True

        Me.dgdDetalle.Splits(0).HeadingStyle.ForeColor = Color.Black
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_flg_sel).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_grupo).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_num_dm).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_zonac).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center

        Me.dgdDetalle.Splits(0).DisplayColumns(Col_flg_sel).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_grupo).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_num_dm).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_zonac).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
    End Sub

    Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
        If cbo_tipo.SelectedValue = "OP_24" Then
            cbo_tipo.SelectedIndex = 1
            Me.cboSistema.Visible = True
            Me.lbl_sistema.Visible = True
        Else
            Me.Close()
        End If
    End Sub

    Private Sub visualizar_datos_dm(ByVal p_consulta As String, ByVal p_App As IApplication)
        Dim escala As Long
        'Dim pactive As ESRI.ArcGIS.Carto.IActiveView
        Me.lstCoordenada.Items.Clear()
        Select Case Me.cbo_tipo.SelectedValue

            Case "OP_18"  'Para opcion Acumulacion
                If v_sistema = "PSAD-56" Then  'PROCEDIMIENTO PARA OBTENER DM EN DATUN 56
                    'lodtbDatos = cls_Oracle.F_Obtiene_Datos_DM(p_consulta)
                    lodtbDatos = cls_Oracle.F_Obtiene_Datos_DM_ACUMULACION(p_consulta)
                ElseIf v_sistema = "WGS-84" Then 'PROCEDIMIENTO PARA OBTENER DM EN DATUN 84
                    lodtbDatos = cls_Oracle.F_Obtiene_Datos_DM_84(p_consulta)
                End If
            Case Else
                If v_sistema = "PSAD-56" Then  'PROCEDIMIENTO PARA OBTENER DM EN DATUN 56
                    If cbo_tipo.SelectedValue = "OP_20" Then
                        'lodbListaDM = cls_Oracle.F_SEL_LISTA_DM_HISTORICO(txtConsulta.Text, "", fecha_h & "1231")
                        lodtbDatos = cls_Oracle.F_SEL_LISTA_DM_HISTORICO(txtConsulta.Text, "", fecha_h & "1231", "2")
                    Else
                        lodtbDatos = cls_Oracle.F_Obtiene_Datos_DM_84(p_consulta)
                    End If
                    'lodtbDatos = cls_Oracle.F_Obtiene_Datos_DM(p_consulta)
                    'lodtbDatos = cls_Oracle.F_Obtiene_Datos_DM_84(p_consulta)                '  TRAE COORDENADAS DEL DM Y OTROS DATOS
                ElseIf v_sistema = "WGS-84" Then 'PROCEDIMIENTO PARA OBTENER DM EN DATUN 84
                    lodtbDatos = cls_Oracle.F_Obtiene_Datos_DM_84(p_consulta)     ' Trae coordenadas del DM
                End If
        End Select

        Select Case lodtbDatos.Rows.Count
            Case 0
                MsgBox("No Existe Codigo: " & p_consulta & " en la Base de Datos....")
                Me.txtConsulta.Focus()
            Case Else

                'Termino
                '  Dim v_equivalente As String

                cls_Catastro.Actualizar_DM(lodtbDatos.Rows(0).Item("PE_ZONCAT"))

                If cbo_tipo.SelectedValue = "OP_20" Then
                    v_nombre_dm = lodtbDatos.Rows(0).Item("PE_NOMDER").ToString
                    v_equivalente = "01"
                    v_titular_dm = lodtbDatos.Rows(0).Item("TITULAR").ToString
                    v_codigo = lodtbDatos.Rows(0).Item("CG_CODIGO").ToString
                    v_zona_dm = (lodtbDatos.Rows(0).Item("PE_ZONCAT").ToString)
                    v_carta_dm = lodtbDatos.Rows(0).Item("CA_CODCAR").ToString
                    v_vigcat = lodtbDatos.Rows(0).Item("PE_VIGCAT").ToString
                    v_tipo_exp = lodtbDatos.Rows(0).Item("TE_TIPOEX").ToString
                    v_ESTADO_dm = lodtbDatos.Rows(0).Item("ESTADO").ToString
                Else
                    v_nombre_dm = lodtbDatos.Rows(0).Item("PE_NOMDER").ToString
                    v_equivalente = lodtbDatos.Rows(0).Item("SC_CODDAT").ToString
                    v_titular_dm = lodtbDatos.Rows(0).Item("TITULAR").ToString
                    v_codigo = lodtbDatos.Rows(0).Item("CG_CODIGO").ToString
                    v_zona_dm = (lodtbDatos.Rows(0).Item("PE_ZONCAT").ToString)
                    v_carta_dm = lodtbDatos.Rows(0).Item("CA_CODCAR").ToString
                    v_vigcat = lodtbDatos.Rows(0).Item("PE_VIGCAT").ToString
                    v_tipo_exp = lodtbDatos.Rows(0).Item("TE_TIPOEX").ToString
                    'v_situacion_dm = lodtbDatos.Rows(0).Item("SITUACION").ToString     ' comentado x wgs84
                    v_ESTADO_dm = lodtbDatos.Rows(0).Item("ESTADO").ToString
                End If

                'v_estado = lodtbDatos.Rows(0).Item("EE_ESTAEX").ToString
                ' If v_vigcat = "G" Then

                ' colecciones_dm_geolo.Add(v_codigo & v_zona_dm)
                ' End If


                cls_Catastro.Borra_Todo_Feature("", m_Application)
                'cd_region_sele = "15"       'Lima
                valida_region = False
                'Encontrando Si el DM esta superpuestosa a la Region solicitada, este procedimiento aplica para usuarios regionales
                '-------------------------------------------------------------------------------------------------------------------
                ' cd_region_encontrado = "00"
                valida_region_dema = ""
                If cd_region_sele <> "00" Then  'PARA USUARIOS FUERA DE LIMA
                    'If glo_User_SDE = "WURB0904" Then
                    Dim lodbtExiste_depa As New DataTable
                    'Verifica si esta dentro de su Region
                    Try
                        lodbtExiste_depa = cls_Oracle.FT_Intersecta_Fclass_Oracle_1(8, gstrFC_Catastro_Minero & v_zona_dm, gstrFC_Departamento, v_codigo)

                    Catch ex As Exception
                    End Try

                    Dim cd_depa As String = ""
                    ' Dim cd_region_sele As String = ""
                    ' cd_region_sele = "15"  'asignacion temporal
                    ' Dim valida As Boolean = False
                    Dim pregunta As Boolean = False
                    If lodbtExiste_depa.Rows.Count > 1 Then
                        For contador As Integer = 0 To lodbtExiste_depa.Rows.Count - 1
                            cd_depa = lodbtExiste_depa.Rows(contador).Item("CD_DEPA")
                            If cd_region_sele = cd_depa Then
                                cd_region_encontrado = cd_region_sele

                                lista_region_sele = "CD_DEPA =  '" & cd_region_sele & "'"
                                lista_cod_region_sele = "CODREGI =  '" & cd_region_sele & "'"
                                valida_region = True
                            Else

                                'cd_region_encontrado = cd_depa
                                'lista_region_sele = "CD_DEPA =  '" & cd_depa & "'"
                                'lista_cod_region_sele = "CODREGI =  '" & cd_depa & "'"
                            End If
                        Next contador
                    ElseIf lodbtExiste_depa.Rows.Count = 1 Then


                        For contador As Integer = 0 To lodbtExiste_depa.Rows.Count - 1
                            cd_depa = lodbtExiste_depa.Rows(contador).Item("CD_DEPA")
                            If cd_region_sele = cd_depa Then
                                cd_region_encontrado = cd_region_sele

                                lista_region_sele = "CD_DEPA =  '" & cd_region_sele & "'"
                                lista_cod_region_sele = "CODREGI =  '" & cd_region_sele & "'"
                                valida_region = True
                            Else
                                cd_region_encontrado = cd_depa

                            End If
                        Next contador

                    End If

                    'Validad Regiones Colindantes
                    Dim lodbtExiste_region As New DataTable
                    'Verifica si esta dentro de su Region
                    Try
                        lodbtExiste_region = cls_Oracle.FT_Intersecta_Fclass_Oracle_1(25, gstrFC_Departamento, gstrFC_Departamento, cd_region_sele)


                    Catch ex As Exception
                    End Try
                    Dim codigo_depa As String = ""
                    Dim codigo_depa1 As String = ""
                    lista_codigo_depa = ""
                    If lodbtExiste_region.Rows.Count >= 1 Then
                        For contador As Integer = 0 To lodbtExiste_region.Rows.Count - 1
                            codigo_depa = lodbtExiste_region.Rows(contador).Item("CD_DEPA")
                            colecciones_colind.Add(codigo_depa)

                            If codigo_depa <> "99" Then
                                If codigo_depa <> cd_region_sele Then
                                    If contador = 0 Then
                                        lista_codigo_depa = "CD_DEPA =  '" & codigo_depa & "'"

                                    ElseIf contador > 0 Then
                                        lista_codigo_depa = lista_codigo_depa & " OR " & "CD_DEPA =  '" & codigo_depa & "'"

                                    End If
                                End If
                            End If


                        Next contador
                    End If
                    Dim cadena As String
                    For cuenta As Integer = 1 To colecciones_colind.Count
                        cadena = colecciones_colind.Item(cuenta)


                    Next cuenta
                    If cadena <> cd_region_encontrado Then
                        '        MsgBox("no esta")
                    End If
                    Dim cls_eval As New Cls_evaluacion

                    cls_eval.Consulta_tabla_ProvColin()  'Busca en la tablas las provincias colindantes segun las regiones colindante
                    'If valida_region = False Then
                    '    MsgBox("El DM a evaluar no se encuentra en la Region Solicitada ni en su Provincia Colindante, por lo tanto no se grafica...", MsgBoxStyle.Critical, "GEOCATMIN")
                    '    Me.btnGraficar.Enabled = False
                    '    valida_region_dema = ""
                    '    Exit Sub
                    'End If
                    '    Select Case pregunta
                    '        Case 6

                    '            valida_region_dema = "DEMARACION"

                    '        Case 7
                    '            valida_region_dema = ""
                    '            'Exit Sub
                    '        Case 7
                    '    End Select
                    'End If

                End If


                'If caso_consulta = "CARTA IGN" Then
                '    carta_v = v_carta_dm.Replace("-", "")
                '    Dim filtro_carta As String = "NAME = '" & LCase(carta_v) & ".ecw'"
                '    'cls_Catastro.AddImagen(glo_pathIGN & carta_v & ".ECW", "1", carta_v, m_Application, True)

                '    cls_Catastro.Conexion_SDE(m_Application)
                '    'pactive = pMap
                '    pRasterCatalog = cls_Catastro.GetRasterCatalog(m_Application, "DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOPSA56")
                '    cls_DM_1.AddRasterCatalogLayer(m_Application, pRasterCatalog)
                '    cls_Catastro.MyDefinitionQuery("DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOPSA56", m_Application, carta_v, filtro_carta)
                '    cls_Catastro.ZOOM_SELECTED("DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOPSA56", m_Application)
                '    pFSelQuery.Clear()

                '    escala = pMap.MapScale
                '    escala = escala * 3
                '    pMxDoc.UpdateContents()
                '    pMap.MapScale = escala
                '    pMxDoc.UpdateContents()
                'End If
                cls_Catastro.Load_FC_GDB("gpt_Vertice_DM", "", m_Application, True)
                cls_Catastro.Delete_Rows_FC_GDB("gpt_Vertice_DM")

                v_zona_dm = lodtbDatos.Rows(0).Item("PE_ZONCAT")     ' se esta agregando para probar acumulacion


                cls_DM_2.Genera_Catastro_DM(p_consulta, lodtbDatos.Rows(0).Item("PE_ZONCAT"), p_App)
                ' Exit Sub

                If cbo_tipo.SelectedValue = "OP_29" Then
                    cls_eval.Geoprocesamiento_temas_convert("FCToShp", p_App, "Acumulacion")   ' se esta agregando para acumulacion
                End If

                cls_Catastro.Rotular_texto_DM("gpt_Vertice_DM", v_zona_dm, m_Application)

                cls_Catastro.Quitar_Layer("gpt_Vertice_DM", m_Application)
                ' If caso_consulta1 = "TIPO DEMARCACION POLITICA" Then

                If caso_consulta1 = "TIPO MAPA GEOLOGICO" Or caso_consulta1 = "TIPO DEMARCACION POLITICA" Then
                    'cls_eval.obtienelimitesmaximos("Catastro")

                    'Procedimiento para obtener limites maximos y minimos del DM
                    '**************************************************************

                    Dim lodbtExiste_MAX As New DataTable
                    Try
                        ' lodbtExiste_MAX = cls_Oracle.FT_Obtiene_datos_XYminmax(v_codigo, gstrFC_Catastro_Minero & gloZona)
                        lodbtExiste_MAX = cls_Oracle.FT_Obtiene_datos_XYminmax(v_codigo, gstrFC_Catastro_Minero & v_zona_dm)
                    Catch ex As Exception
                    End Try
                    If lodbtExiste_MAX.Rows.Count >= 1 Then
                        For contador As Integer = 0 To lodbtExiste_MAX.Rows.Count - 1
                            v_este_min = lodbtExiste_MAX.Rows(contador).Item("MINX")
                            v_norte_min = lodbtExiste_MAX.Rows(contador).Item("MINY")
                            v_este_max = lodbtExiste_MAX.Rows(contador).Item("MAXX")
                            v_norte_max = lodbtExiste_MAX.Rows(contador).Item("MAXY")
                        Next contador
                    End If

                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito, m_Application, "1", True)
                    Dim lo_Filtro_Dist As String = cls_Catastro.f_Intercepta_FC("Distrito", v_este_min, v_norte_min, v_este_max, v_norte_max, m_Application)
                    cls_Catastro.DefinitionExpression(lo_Filtro_Dist, m_Application, "Distrito")
                    pFeatureLayer = pMap.Layer(0)
                    pFeatureLayer.Visible = True
                    'cls_Catastro.Shade_Poligono("Distrito", m_Application)
                    cls_Catastro.Color_Poligono_Simple(m_Application, "Distrito")
                    cls_Catastro.ShowLabel_DM("Distrito", m_Application)
                    escala = pMap.MapScale
                    escala = escala * 100
                    pMxDoc.UpdateContents()
                    pMap.MapScale = escala
                    pMxDoc.UpdateContents()
                End If

                Dim lo_Filtro As String = "CODIGOU = '" & v_codigo & "'"

                ' cls_Catastro.Seleccioname_Envelope(lo_Filtro, "Catastro", xMin, yMin, xMax, yMax, v_zona_dm, m_Application)
                cls_Catastro.Seleccioname_Envelope(lo_Filtro, "Catastro_" & v_zona_dm, xMin, yMin, xMax, yMax, v_zona_dm, m_Application)

                Me.lstCoordenada.Items.Add(" Vert. " & "        Este  " & "     " & "        Norte  ")
                Me.lstCoordenada.Items.Add("--------------------------------------------")

                If v_equivalente = "01" Then
                    v_equivalente = "PSAD-56"
                Else
                    v_equivalente = "WGS-84"
                End If
                'If v_equivalente = v_sistema Then
                If v_equivalente = v_sistema Then
                    For i As Integer = 0 To lodtbDatos.Rows.Count - 1
                        Me.lstCoordenada.Items.Add(" " & RellenarComodin(i + 1, 3, "0") & "   " & _
                        Format(Math.Round(lodtbDatos.Rows(i).Item("CD_COREST"), 3), "###,###.#0") & _
                        "   " & Format(Math.Round(lodtbDatos.Rows(i).Item("CD_CORNOR"), 3), "###,###.#0") & "")
                    Next
                Else
                    For i As Integer = 0 To lodtbDatos.Rows.Count - 1
                        Me.lstCoordenada.Items.Add(" " & RellenarComodin(i + 1, 3, "0") & "   " & _
                        Format(Math.Round(lodtbDatos.Rows(i).Item("CD_COREST_E"), 3), "###,###.#0") & _
                        "   " & Format(Math.Round(lodtbDatos.Rows(i).Item("CD_CORNOR_E"), 3), "###,###.#0") & "")
                    Next
                End If

                cls_Catastro.ClearLayerSelection(pFeatureLayer)
                If pMap.LayerCount > 0 Then
                    pFeatureLayer = pMap.Layer(0)
                    pFeatureLayer.Name = "Catastro"
                    pMxDoc.ActiveView.Refresh()
                    pMxDoc.UpdateContents()
                Else
                    MsgBox("No se generado la capa correctamente del DM buscado...", MsgBoxStyle.Critical, "Verificar...")
                    Exit Sub

                End If

                arch_cata = "Catastro"


                cls_Catastro.Color_Poligono_Simple(m_Application, "Catastro")
                cls_Catastro.Genera_Imagen_DM("VistaPrevia", "VistaPrevia")
                img_DM.ImageLocation = glo_pathTMP & "\VistaPrevia.jpg"

                arch_cata = "Catastro"
                cls_Catastro.Color_Poligono_Simple7(m_Application, "Catastro")
                'cls_Catastro.Color_Poligono_Simple(m_Application, "Catastro")
                cls_Catastro.Genera_Imagen_DM("VistaPrevia_1", "vistaprevia")
                cls_planos.cambiacolor_frame("NORMAL")
                cls_Catastro.Borra_Todo_Feature("", m_Application)
                cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                If v_equivalente = v_sistema Then
                    Dim lodtvOrdena_x As New DataView(lodtbDatos, Nothing, "CD_COREST ASC", DataViewRowState.CurrentRows)
                    xMin.Text = lodtvOrdena_x.Item(0).Row("CD_COREST")
                    xMax.Text = lodtvOrdena_x.Item(lodtvOrdena_x.Count - 1).Row("CD_COREST")

                    Dim lodtvOrdena_y As New DataView(lodtbDatos, Nothing, "CD_CORNOR ASC", DataViewRowState.CurrentRows)
                    yMin.Text = lodtvOrdena_y.Item(0).Row("CD_CORNOR")
                    yMax.Text = lodtvOrdena_y.Item(lodtvOrdena_y.Count - 1).Row("CD_CORNOR")
                Else
                    'Cuando Sistema seleccionado sea distinto al Origen
                    Dim lodtvOrdena_x As New DataView(lodtbDatos, Nothing, "CD_COREST_E ASC", DataViewRowState.CurrentRows)
                    xMin.Text = lodtvOrdena_x.Item(0).Row("CD_COREST_E")
                    xMax.Text = lodtvOrdena_x.Item(lodtvOrdena_x.Count - 1).Row("CD_COREST_E")

                    Dim lodtvOrdena_y As New DataView(lodtbDatos, Nothing, "CD_CORNOR_E ASC", DataViewRowState.CurrentRows)
                    yMin.Text = lodtvOrdena_y.Item(0).Row("CD_CORNOR_E")
                    yMax.Text = lodtvOrdena_y.Item(lodtvOrdena_y.Count - 1).Row("CD_CORNOR_E")

                End If

                Me.btnGraficar.Enabled = True
                Me.btnReporte.Enabled = True
                txtRadio.Enabled = True
                clbLayer.Enabled = True


        End Select
        Dim item As Windows.Forms.ListViewItem
        If lodtbDatos.Rows.Count = 0 Then
            MsgBox("No Existe ning�n Item con esta consulta ", MsgBoxStyle.Information, "[BDGEOCIENT�FICA]")
            Me.btnGraficar.Enabled = False
            Exit Sub
        End If
        For i As Integer = 0 To lodtbDatos.Rows.Count - 1
            Dim ArrFila(5) As String
            ArrFila(0) = lodtbDatos.Rows(i).Item("CG_CODIGO").ToString
            ArrFila(1) = lodtbDatos.Rows(i).Item("PE_NOMDER").ToString
            ArrFila(2) = lodtbDatos.Rows(i).Item("PE_ZONCAT").ToString
            ArrFila(3) = lodtbDatos.Rows(i).Item("TITULAR").ToString
            ArrFila(4) = lodtbDatos.Rows(i).Item("CA_CODCAR").ToString
            item = New Windows.Forms.ListViewItem(ArrFila)
            lsvListaAreas.Items.Add(item)
        Next
    End Sub

    Private Sub cboConsulta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboConsulta.SelectedIndexChanged
        Me.txtConsulta.Text = ""
    End Sub

    Private Sub cbo_tipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_tipo.SelectedIndexChanged
        lo_Inicio_DM = 0
        v_este_min = 0
        v_norte_min = 0
        v_este_max = 0
        v_norte_min = 0
        btnBuscar.Enabled = True
        Me.lblVertice.Text = ""
        Me.lblRegistro.Text = ""
        If cbo_tipo.SelectedIndex = 0 Then
            Me.cboConsulta.Enabled = False
            Me.txtConsulta.Enabled = False
            Me.btnGenera_Poligono.Visible = False : Me.btnGenera_Poligono.Enabled = False
            Exit Sub
        End If
        If cbo_tipo.SelectedValue <> "OP_15" Then
            Me.btnCerrar.Visible = True
            Me.btnOtraConsulta.Visible = True
            Me.btnReporte.Visible = True
            Me.btnGraficar.Visible = True
            Me.GroupBox1.Visible = True
            Me.clbLayer.Visible = True
            Me.rbt_NoVisualiza.Visible = True
            Me.rbt_Visualiza.Visible = True
            Cbo_libdenu.Visible = False
            Cbo_libdenu.Enabled = False


        End If
        Me.lsvListaAreas.Visible = False : Me.lsvListaAreas.Enabled = False
        Me.dgdDetalle.Visible = True : Me.dgdDetalle.Enabled = True
        btnCerrar.Enabled = True
        lsvListaAreas.Enabled = True
        Me.clbLayer.Items.Clear()
        Inicializa_Layer_DM(Me.cbo_tipo.SelectedValue)
        loglo_Titulo = Me.cbo_tipo.SelectedValue
        Dim losender As New System.Object
        Dim loe As New System.EventArgs
        Me.btnOtraConsulta_Click(losender, loe)
        'Select Case cbo_tipo.Text.ToUpper
        Select Case cbo_tipo.SelectedValue
            Case "OP_11", "OP_2", "OP_18" 'Evaluaci�n, consulta dm
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                'Case "OP_11"
                'txtArea.Visible = True
                'lblArea.Visible = True
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btneliminar.Enabled = False
                btneliminar.Visible = False
                btnlimpiar.Enabled = False
                btnlimpiar.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                img_DM1.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.txtRadio.Visible = True : Me.txtRadio.Enabled = True
                Me.cboZona.Visible = True : Me.cboZona.Enabled = True
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                Me.lblRadio.Visible = True : Me.lblRadio.Enabled = True
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.lblConsultar.Text = "Consultar"
                Me.lblDato.Text = "Dato"
                cboConsulta.Enabled = True : cboConsulta.Visible = True
                txtConsulta.Visible = True : txtConsulta.Enabled = False
                Me.lbt_dato1.Visible = False : Me.lbt_dato1.Enabled = False
                Me.lbt_dato2.Visible = False : Me.lbt_dato2.Enabled = False
                Me.lbt_dato3.Visible = False : Me.lbt_dato3.Enabled = False
                Me.lbt_dato4.Visible = False : Me.lbt_dato4.Enabled = False
                Me.btnLimpia.Visible = False : Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False : Me.btnElimina.Enabled = False
                Me.btnGenera_Poligono.Visible = False
                Me.lstCoordenada.Items.Clear()
                Me.btnagregar.Visible = False
                Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False
                Me.lbl_Previo.Text = "Vista Previa, DM y V�rtices"
                Me.btnBuscar.Visible = True
                cboConsulta.Items.Clear()
                cboConsulta.Enabled = True
                txtConsulta.Enabled = False
                btnBuscar.Enabled = True
                lstCoordenada.Visible = True
                Txtdato5.Visible = False : Txtdato5.Enabled = False
                Txtdato6.Visible = False : Txtdato6.Enabled = False
                btnagregar.Visible = False : btnagregar.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                cboZona.Enabled = False
                btnGraficar.Enabled = False
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                cboConsulta.Items.Add("C�digo DM")
                cboConsulta.Items.Add("Nombre DM")
                Me.cboConsulta.SelectedIndex = 0
                'Me.txtConsulta.Focus()
                img_DM.ImageLocation = glo_pathPNG & "\evaluacion.png"


                'Case "OP_2"
                '    'txtArea.Visible = True
                '    'lblArea.Visible = True
                '    img_DM1.Visible = False
                '    cboarea.Visible = False
                '    cbotipo.Visible = False
                '    lbl_nombre1.Visible = False
                '    lbl_nombre2.Visible = False
                '    btnvermapa.Visible = False
                '    btnActualizar.Visible = False
                '    btnvermapa.Enabled = False
                '    btnActualizar.Enabled = False
                '    Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                '    Me.cboZona.Location = New System.Drawing.Point(392, 113)
                '    Me.lblZona.Location = New System.Drawing.Point(392, 98)
                '    Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                '    Me.txtRadio.Visible = True : Me.txtRadio.Enabled = True
                '    Me.cboZona.Visible = True : Me.cboZona.Enabled = True
                '    Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                '    Me.lblRadio.Visible = True : Me.lblRadio.Enabled = True
                '    Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                '    Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                '    Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                '    Me.lblConsultar.Text = "Consultar"
                '    Me.lblDato.Text = "Dato"
                '    cboConsulta.Enabled = True : cboConsulta.Visible = True
                '    txtConsulta.Visible = True : txtConsulta.Enabled = True
                '    Me.lbt_dato1.Visible = False : Me.lbt_dato1.Enabled = False
                '    Me.lbt_dato2.Visible = False : Me.lbt_dato2.Enabled = False
                '    Me.lbt_dato3.Visible = False : Me.lbt_dato3.Enabled = False
                '    Me.lbt_dato4.Visible = False : Me.lbt_dato4.Enabled = False
                '    Me.btnLimpia.Visible = False : Me.btnLimpia.Visible = False
                '    Me.btnElimina.Visible = False : Me.btnElimina.Enabled = False
                '    Me.btnGenera_Poligono.Visible = False
                '    Me.lstCoordenada.Items.Clear()
                '    Me.btnagregar.Visible = False
                '    Me.btnLimpia.Visible = False
                '    Me.btnElimina.Visible = False
                '    Me.lbl_Previo.Text = "Vista Previa, DM y V�rtices"
                '    Me.btnBuscar.Visible = True
                '    cboConsulta.Items.Clear()
                '    cboConsulta.Enabled = True
                '    txtConsulta.Enabled = True
                '    btnBuscar.Enabled = True
                '    lstCoordenada.Visible = True
                '    Txtdato5.Visible = False : Txtdato5.Enabled = False
                '    Txtdato6.Visible = False : Txtdato6.Enabled = False
                '    btnagregar.Visible = False : btnagregar.Enabled = False
                '    txtdato1.Visible = False : txtdato1.Enabled = False
                '    txtdato2.Visible = False : txtdato2.Enabled = False
                '    txtdato3.Visible = False : txtdato3.Enabled = False
                '    txtdato4.Visible = False : txtdato4.Enabled = False
                '    cboZona.Enabled = False
                '    btnGraficar.Enabled = False
                '    txtRadio.Enabled = False
                '    btnReporte.Enabled = False
                '    cboConsulta.Items.Add("C�digo DM")
                '    cboConsulta.Items.Add("Masivo")
                '    Me.cboConsulta.SelectedIndex = 0
                '    Me.txtConsulta.Focus()
                '    img_DM.ImageLocation = glo_pathPNG & "\evaluacion.png"

                If Me.cbo_tipo.SelectedValue = "OP_11" Then
                    Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                    btn_cargarcuadriculas.Visible = False
                    btn_cargarcuadriculas.Enabled = False
                    btn_procesar.Visible = False
                    btn_procesar.Enabled = False

                    lblfecha.Enabled = False
                    lblfecha.Visible = False
                    dtpFecha1.Enabled = False
                    dtpFecha1.Visible = False
                    dgdResultado.Enabled = False
                    dgdResultado.Visible = False
                    btnexp_excel.Enabled = False
                    btnexp_excel.Visible = False

                    tipo_seleccion = "OP_11"
                    V_caso_simu = ""
                    txtArea.Visible = True
                    lblArea.Visible = True

                ElseIf Me.cbo_tipo.SelectedValue = "OP_18" Then
                    Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                    btn_cargarcuadriculas.Visible = False
                    btn_cargarcuadriculas.Enabled = False
                    btn_procesar.Visible = False
                    btn_procesar.Enabled = False

                    lblfecha.Enabled = False
                    lblfecha.Visible = False
                    dtpFecha1.Enabled = False
                    dtpFecha1.Visible = False
                    dgdResultado.Enabled = False
                    dgdResultado.Visible = False
                    btnexp_excel.Enabled = False
                    btnexp_excel.Visible = False

                    tipo_seleccion = "OP_18"
                    V_caso_simu = ""
                    txtArea.Visible = True
                    lblArea.Visible = True

                ElseIf Me.cbo_tipo.SelectedValue = "OP_2" Then
                    Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                    btn_cargarcuadriculas.Visible = False
                    btn_cargarcuadriculas.Enabled = False
                    btn_procesar.Visible = False
                    btn_procesar.Enabled = False

                    lblfecha.Enabled = False
                    lblfecha.Visible = False
                    dtpFecha1.Enabled = False
                    dtpFecha1.Visible = False
                    dgdResultado.Enabled = False
                    dgdResultado.Visible = False
                    btnexp_excel.Enabled = False
                    btnexp_excel.Visible = False

                    txtArea.Visible = False
                    lblArea.Visible = False
                    tipo_seleccion = "OP_2"
                    V_caso_simu = "SI"
                    loglo_Titulo = "C�digo / Nombre"
                    Me.lblDato.Visible = True
                    Me.lblConsultar.Text = "Consulta: "
                    'Me.lblConsultar.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                    Me.cboConsulta.Visible = True
                    Me.lblConsultar.Text = "Consultar"
                    Me.lblDato.Text = "Dato"
                    Me.txtEste.Visible = False
                    Me.txtNorte.Visible = False
                    Me.txtConsulta.Visible = True
                    Me.btnGenera_Poligono.Visible = False

                End If

            Case "OP_16" 'libre denunciabilidad
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                tipo_seleccion = "OP_16"
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                img_DM1.Visible = False
                cboarea.Visible = False
                ' cbotipo.Visible = True
                Me.cbo_tipo.Visible = True
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.txtRadio.Visible = True : Me.txtRadio.Enabled = True
                Me.cboZona.Visible = True : Me.cboZona.Enabled = True
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                Me.lblRadio.Visible = True : Me.lblRadio.Enabled = True
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.lblConsultar.Text = "Consultar"
                Me.lblDato.Text = "Dato"
                Me.Cbo_libdenu.Enabled = True : Cbo_libdenu.Visible = True
                txtConsulta.Visible = True : txtConsulta.Enabled = True
                Me.lbt_dato1.Visible = False : Me.lbt_dato1.Enabled = False
                Me.lbt_dato2.Visible = False : Me.lbt_dato2.Enabled = False
                Me.lbt_dato3.Visible = False : Me.lbt_dato3.Enabled = False
                Me.lbt_dato4.Visible = False : Me.lbt_dato4.Enabled = False
                Me.btnLimpia.Visible = False : Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False : Me.btnElimina.Enabled = False
                Me.btnGenera_Poligono.Visible = False
                Me.lstCoordenada.Items.Clear()
                Me.btnagregar.Visible = False
                Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False
                Me.lbl_Previo.Text = "Vista Previa, DM y V�rtices"
                Me.btnBuscar.Visible = True
                Cbo_libdenu.Items.Clear()
                Cbo_libdenu.Enabled = True
                'btnBuscar.Enabled = True
                lstCoordenada.Visible = True
                Txtdato5.Visible = False : Txtdato5.Enabled = False
                Txtdato6.Visible = False : Txtdato6.Enabled = False
                btnagregar.Visible = False : btnagregar.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                cboZona.Enabled = False
                btnGraficar.Enabled = False
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                Cbo_libdenu.Items.Add("C�digo DM")
                Cbo_libdenu.Items.Add("Masivo")
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btneliminar.Enabled = False
                btneliminar.Visible = False
                btnlimpiar.Enabled = False
                btnlimpiar.Visible = False
                Me.Cbo_libdenu.SelectedIndex = 0
                Me.txtConsulta.Focus()
                img_DM.ImageLocation = glo_pathPNG & "\evaluacion.png"

                ' Me.cboZona.SelectedIndex = 1

            Case "OP_19x" 'Planos Geologico Masivo

                tipo_seleccion = "OP_19"
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                txtArea.Visible = False
                lblArea.Visible = False
                img_DM1.Visible = False
                cboarea.Visible = False
                ' cbotipo.Visible = True
                Me.cbo_tipo.Visible = True
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.txtRadio.Visible = False : Me.txtRadio.Visible = False
                Me.cboZona.Visible = False : Me.cboZona.Enabled = False
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                Me.lblRadio.Visible = False : Me.lblRadio.Enabled = False
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.lblConsultar.Text = "Consultar"
                Me.lblDato.Text = "Dato"
                Me.Cbo_libdenu.Enabled = True : Cbo_libdenu.Visible = True
                txtConsulta.Visible = True : txtConsulta.Enabled = True
                Me.lbt_dato1.Visible = False : Me.lbt_dato1.Enabled = False
                Me.lbt_dato2.Visible = False : Me.lbt_dato2.Enabled = False
                Me.lbt_dato3.Visible = False : Me.lbt_dato3.Enabled = False
                Me.lbt_dato4.Visible = False : Me.lbt_dato4.Enabled = False
                Me.btnLimpia.Visible = False : Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False : Me.btnElimina.Enabled = False
                Me.btnGenera_Poligono.Visible = False
                Me.lstCoordenada.Items.Clear()
                Me.btnagregar.Visible = False
                Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False
                Me.lbl_Previo.Text = "Vista Previa, DM y V�rtices"
                Me.btnBuscar.Visible = True
                Cbo_libdenu.Items.Clear()
                Cbo_libdenu.Enabled = True
                'btnBuscar.Enabled = True
                lstCoordenada.Visible = True
                Txtdato5.Visible = False : Txtdato5.Enabled = False
                Txtdato6.Visible = False : Txtdato6.Enabled = False
                btnagregar.Visible = False : btnagregar.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                cboZona.Enabled = False
                btnGraficar.Enabled = False
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                Cbo_libdenu.Items.Add("C�digo DM")
                Cbo_libdenu.Items.Add("Masivo")
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btneliminar.Enabled = False
                btneliminar.Visible = False
                btnlimpiar.Enabled = False
                btnlimpiar.Visible = False
                Me.Cbo_libdenu.SelectedIndex = 0
                Me.txtConsulta.Focus()
                img_DM.ImageLocation = glo_pathPNG & "\evaluacion.png"
                lblZona.Visible = False


            Case "OP_19" 'Planos Geologico Masivo

                tipo_seleccion = "OP_19"
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                txtArea.Visible = False
                lblArea.Visible = False
                img_DM1.Visible = False
                cboarea.Visible = False
                ' cbotipo.Visible = True
                Me.cbo_tipo.Visible = True
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.txtRadio.Visible = False : Me.txtRadio.Visible = False
                Me.cboZona.Visible = False : Me.cboZona.Enabled = False
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                Me.lblRadio.Visible = False : Me.lblRadio.Enabled = False
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.lblConsultar.Text = "Consultar"
                Me.lblDato.Text = "Dato"
                Me.Cbo_libdenu.Enabled = True : Cbo_libdenu.Visible = True
                txtConsulta.Visible = True : txtConsulta.Enabled = True
                Me.lbt_dato1.Visible = False : Me.lbt_dato1.Enabled = False
                Me.lbt_dato2.Visible = False : Me.lbt_dato2.Enabled = False
                Me.lbt_dato3.Visible = False : Me.lbt_dato3.Enabled = False
                Me.lbt_dato4.Visible = False : Me.lbt_dato4.Enabled = False
                Me.btnLimpia.Visible = False : Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False : Me.btnElimina.Enabled = False
                Me.btnGenera_Poligono.Visible = False
                Me.lstCoordenada.Items.Clear()
                Me.btnagregar.Visible = False
                Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False
                Me.lbl_Previo.Text = "Vista Previa, DM y V�rtices"
                Me.btnBuscar.Visible = True
                Cbo_libdenu.Items.Clear()
                Cbo_libdenu.Enabled = True
                'btnBuscar.Enabled = True
                lstCoordenada.Visible = True
                Txtdato5.Visible = False : Txtdato5.Enabled = False
                Txtdato6.Visible = False : Txtdato6.Enabled = False
                btnagregar.Visible = False : btnagregar.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                cboZona.Enabled = False
                btnGraficar.Enabled = False
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                Cbo_libdenu.Items.Add("C�digo DM")
                Cbo_libdenu.Items.Add("Masivo")
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btneliminar.Enabled = False
                btneliminar.Visible = False
                btnlimpiar.Enabled = False
                btnlimpiar.Visible = False
                Me.Cbo_libdenu.SelectedIndex = 0
                Me.txtConsulta.Focus()
                img_DM.ImageLocation = glo_pathPNG & "\evaluacion.png"
                lblZona.Visible = False
            Case "OP_12"
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btneliminar.Enabled = False
                btneliminar.Visible = False
                btnlimpiar.Enabled = False
                btnlimpiar.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                img_DM1.Visible = False
                cboarea.Visible = False
                btnvermapa.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                btnActualizar.Visible = False
                Me.lstCoordenada.Items.Clear()
                cboConsulta.Items.Clear()
                Me.txtRadio.Visible = True : Me.txtRadio.Enabled = True
                Me.lblRadio.Visible = True : Me.lblRadio.Enabled = True
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                cboConsulta.Enabled = False : cboConsulta.Visible = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                cboZona.Visible = True : cboZona.Enabled = True
                Me.lbt_dato1.Visible = False : Me.lbt_dato1.Enabled = False
                Me.lbt_dato2.Visible = False : Me.lbt_dato2.Enabled = False
                Me.lbt_dato3.Visible = False : Me.lbt_dato3.Enabled = False
                Me.lbt_dato4.Visible = False : Me.lbt_dato4.Enabled = False
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                Me.txtEste.Visible = True : Me.txtEste.Enabled = True
                Me.txtNorte.Visible = True : Me.txtNorte.Enabled = True
                Me.btnLimpia.Visible = True : Me.btnLimpia.Visible = True
                Me.btnElimina.Visible = True : Me.btnElimina.Enabled = True
                Me.btnBuscar.Visible = False : btnReporte.Enabled = False
                Me.btnagregar.Visible = True : Me.btnagregar.Enabled = True
                lstCoordenada.Visible = True
                Me.btnGraficar.Enabled = False
                Me.lblConsultar.Visible = True : Me.lblConsultar.Text = "        Este: "
                Me.lblDato.Text = "Norte: "
                Me.txtEste.Text = "" : Me.txtNorte.Text = ""
                Me.txtConsulta.Visible = False
                tipo_seleccion = "OP_12" '"SIMULACI�N DM"
                Me.btnGenera_Poligono.Visible = True
                img_DM.ImageLocation = glo_pathPNG & "\simulacion.png"

            Case "OP_1", "OP_14", "OP_28" 'CARTA DM
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btneliminar.Enabled = False
                btneliminar.Visible = False
                btnlimpiar.Enabled = False
                btnlimpiar.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                img_DM1.Visible = False
                btnActualizar.Visible = False
                cboarea.Visible = False
                btnvermapa.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                Me.txtConsulta.Visible = True : Me.cboConsulta.Enabled = True
                Me.lblConsultar.Visible = True : Me.txtConsulta.Enabled = True
                Me.cboConsulta.Visible = True : Me.cboConsulta.Enabled = True
                Me.lbt_dato1.Visible = True : Me.lbt_dato1.Enabled = True
                Me.lbt_dato2.Visible = True : Me.lbt_dato2.Enabled = True
                Me.lbt_dato3.Visible = True : Me.lbt_dato3.Enabled = True
                Me.lbt_dato4.Visible = True : Me.lbt_dato4.Enabled = True
                Me.lbt_dato1.Text = "Este Min.:" : Me.lbt_dato2.Text = "Norte Min.:"
                Me.lbt_dato3.Text = "Este Max.:" : Me.lbt_dato4.Text = "Norte Max.:"
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.btnGenera_Poligono.Visible = False : Me.btnGenera_Poligono.Enabled = False
                Me.txtRadio.Visible = False : Me.txtRadio.Enabled = False
                Me.cboZona.Visible = True : Me.cboZona.Enabled = True
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                Me.lblRadio.Visible = False : Me.lblRadio.Enabled = False
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                Me.lbl_Previo.Text = "L�mite de la Carta"
                Me.lblConsultar.Text = "Consultar:"
                btnBuscar.Visible = True
                clbLayer.Enabled = False
                cboConsulta.Items.Clear()
                cboConsulta.Enabled = True
                txtConsulta.Enabled = True
                ' btnBuscar.Enabled = True
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                Txtdato5.Enabled = False
                Txtdato6.Enabled = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = True : txtdato1.Enabled = True
                txtdato2.Visible = True : txtdato2.Enabled = True
                txtdato3.Visible = True : txtdato3.Enabled = True
                txtdato4.Visible = True : txtdato4.Enabled = True
                lstCoordenada.Visible = False
                btnGraficar.Enabled = False
                txtRadio.Enabled = False
                cboZona.Enabled = False
                cboConsulta.Items.Add("C�digo Carta")
                cboConsulta.Items.Add("Nombre Carta")
                cboConsulta.SelectedIndex = 0
                btnReporte.Enabled = False
                Select Case Me.cbo_tipo.SelectedValue
                    Case "OP_1"
                        tipo_seleccion = "OP_1"
                        img_DM.ImageLocation = glo_pathPNG & "\carta_ign.png"
                        cboConsulta.Refresh()

                    Case "OP_14"
                        tipo_seleccion = "OP_14"
                        img_DM.ImageLocation = glo_pathPNG & "\Geologia.png"
                        'img_DM.ImageLocation = glo_Path & "\Vista_Previa\Geologia.png"
                        cboConsulta.Refresh()

                    Case "OP_28"
                        tipo_seleccion = "OP_28"

                        lblAnio.Visible = True
                        cboAnio.Enabled = True
                        cboAnio.Visible = True
                        Me.cboSistema.Visible = False
                        Me.lbl_sistema.Visible = False
                        Me.cboAnio.Focus()

                        img_DM.ImageLocation = glo_pathPNG & "\carta_ign.png"
                        cboConsulta.Refresh()
                End Select

            Case "OP_3gg"

                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                btneliminar.Enabled = False
                btneliminar.Visible = False
                btnlimpiar.Enabled = False
                btnlimpiar.Visible = False
                img_DM1.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.txtRadio.Visible = True : Me.txtRadio.Enabled = True
                Me.cboZona.Visible = True : Me.cboZona.Enabled = True
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                Me.lblRadio.Visible = True : Me.lblRadio.Enabled = True
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.lblConsultar.Text = "Consultar"
                Me.lblDato.Text = "Dato"
                cboConsulta.Enabled = True : cboConsulta.Visible = True
                txtConsulta.Visible = True : txtConsulta.Enabled = True
                Me.lbt_dato1.Visible = False : Me.lbt_dato1.Enabled = False
                Me.lbt_dato2.Visible = False : Me.lbt_dato2.Enabled = False
                Me.lbt_dato3.Visible = False : Me.lbt_dato3.Enabled = False
                Me.lbt_dato4.Visible = False : Me.lbt_dato4.Enabled = False
                Me.btnLimpia.Visible = False : Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False : Me.btnElimina.Enabled = False
                Me.btnGenera_Poligono.Visible = False
                Me.lstCoordenada.Items.Clear()
                Me.btnagregar.Visible = False
                Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False
                Me.lbl_Previo.Text = "Vista Previa, DM y V�rtices"
                Me.btnBuscar.Visible = True
                cboConsulta.Items.Clear()
                cboConsulta.Enabled = True
                txtConsulta.Enabled = True
                btnBuscar.Enabled = True
                lstCoordenada.Visible = True
                Txtdato5.Visible = False : Txtdato5.Enabled = False
                Txtdato6.Visible = False : Txtdato6.Enabled = False
                btnagregar.Visible = False : btnagregar.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                cboZona.Enabled = False
                btnGraficar.Enabled = False
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                cboConsulta.Items.Add("C�digo DM")
                cboConsulta.Items.Add("Masivo")
                Me.cboConsulta.SelectedIndex = 0
                Me.txtConsulta.Focus()
                img_DM.ImageLocation = glo_pathPNG & "\evaluacion.png"

            Case "OP_3"
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                img_DM1.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                txtArea.Visible = False
                lblArea.Visible = False

                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                Me.lblDato.Visible = False : Me.lblDato.Enabled = False
                Me.lblConsultar.Visible = False : Me.lblConsultar.Enabled = False
                Me.txtConsulta.Visible = False : Me.txtConsulta.Enabled = False
                Me.cboConsulta.Visible = False : Me.cboConsulta.Enabled = False
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.btnLimpia.Visible = False : Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False : Me.btnElimina.Enabled = False
                Me.btnGenera_Poligono.Visible = False : Me.btnGenera_Poligono.Enabled = False
                'Me.lblZona.Location = New System.Drawing.Point(392, 103)
                Me.txtRadio.Visible = True : Me.txtRadio.Enabled = True
                Me.cboZona.Visible = True : Me.cboZona.Enabled = True
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                Me.lblRadio.Visible = True : Me.lblRadio.Enabled = True
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                Me.lbt_dato1.Visible = True : Me.lbt_dato1.Enabled = True
                Me.lbt_dato2.Visible = True : Me.lbt_dato2.Enabled = True
                Me.lbt_dato1.Text = "Este:" : Me.lbt_dato2.Text = "Norte:"
                Me.btnBuscar.Visible = False : Me.btnBuscar.Enabled = False
                cboConsulta.Items.Clear()
                clbLayer.Enabled = True
                cboConsulta.Enabled = False
                txtConsulta.Enabled = False
                btnBuscar.Enabled = True
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = True
                txtdato1.Visible = True : txtdato1.Enabled = True
                txtdato2.Visible = True : txtdato2.Enabled = True
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                lstCoordenada.Visible = False
                cboZona.Enabled = True
                txtRadio.Enabled = True
                btnGraficar.Enabled = False
                btnReporte.Enabled = False
                tipo_seleccion = "OP_3"
                img_DM.ImageLocation = glo_pathPNG & "\b_carta.png"

            Case "OP_9" ' Limites M�ximos
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                img_DM1.Visible = False
                cboarea.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                Me.lblConsultar.Visible = False : Me.lblConsultar.Enabled = False
                Me.cboConsulta.Visible = False : Me.cboConsulta.Enabled = False
                Me.lblDato.Visible = False : Me.lblDato.Enabled = False
                Me.cboZona.Visible = True : Me.cboZona.Enabled = True
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                Me.lblConsultar.Text = "Consultar:"
                Me.btnBuscar.Visible = False : Me.btnBuscar.Enabled = False
                Me.txtConsulta.Visible = False : Me.txtRadio.Enabled = False
                Me.txtRadio.Visible = False : Me.txtRadio.Enabled = False
                Me.lblRadio.Visible = False : Me.lblRadio.Enabled = False
                Me.lbt_dato1.Visible = True : Me.lbt_dato1.Enabled = True
                Me.lbt_dato2.Visible = True : Me.lbt_dato2.Enabled = True
                Me.lbt_dato3.Visible = True : Me.lbt_dato3.Enabled = True
                Me.lbt_dato4.Visible = True : Me.lbt_dato4.Enabled = True
                Me.lbt_dato1.Text = "Este Min."
                Me.lbt_dato2.Text = "Norte Min."
                Me.lbt_dato3.Text = "Este Max."
                Me.lbt_dato4.Text = "Norte Max."
                cboConsulta.Items.Clear()
                clbLayer.Enabled = True
                cboConsulta.Enabled = False
                txtConsulta.Enabled = False
                btnBuscar.Enabled = True
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = True
                txtdato1.Visible = True
                txtdato1.Enabled = True
                txtdato2.Visible = True
                txtdato2.Enabled = True
                txtdato3.Visible = True
                txtdato3.Enabled = True
                txtdato4.Visible = True
                txtdato4.Enabled = True
                lstCoordenada.Visible = False
                btnGraficar.Enabled = True
                btnOtraConsulta.Enabled = True
                btnReporte.Enabled = False
                cboZona.Enabled = True
                txtRadio.Enabled = False
                btnGraficar.Enabled = False
                tipo_seleccion = "OP_9" ' "DM SEG�N L�MITES M�XIMOS"
                img_DM.ImageLocation = glo_pathPNG & "\limite.png"

            Case "OP_13" 'VARIAS CARTAS
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                img_DM1.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.txtNorte.Enabled = False : Me.txtNorte.Visible = False
                Me.cboConsulta.Visible = False
                Me.lbt_dato1.Visible = False
                Me.lbt_dato2.Visible = False
                Me.lbt_dato3.Visible = False
                Me.lbt_dato4.Visible = False
                cboConsulta.Items.Clear()
                cboConsulta.Enabled = False
                txtConsulta.Visible = True : txtConsulta.Enabled = True
                btnBuscar.Enabled = True
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                lstCoordenada.Visible = True
                cboZona.Enabled = False
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                btnGraficar.Enabled = False
                Me.lblZona.Visible = False
                Me.lbl_Previo.Text = "Cartas a Graficar:"
                Me.lblDato.Text = "Carta: "
                Me.lblConsultar.Visible = False
                Me.lblRadio.Visible = False
                Me.cboZona.Visible = False
                Me.txtRadio.Visible = False
                Me.btnagregar.Visible = True
                Me.btnElimina.Visible = True
                Me.btnLimpia.Visible = True
                Me.btnBuscar.Visible = False
                Me.btnagregar.Enabled = True
                Me.btnElimina.Enabled = True
                Me.btnLimpia.Enabled = True
                Me.btnGenera_Poligono.Visible = False
                tipo_seleccion = "OP_13"
                img_DM.ImageLocation = glo_pathPNG & "\vcartas.png"
                Me.txtEste.Visible = False
                'Case "OP_4" 'AREA RESTRINGIDA
                '    txtArea.Visible = False
                '    lblArea.Visible = False
                '    btnvermapa.Enabled = False
                '    btnActualizar.Enabled = False
                '    img_DM1.Visible = False
                '    btnActualizar.Visible = False
                '    cboarea.Visible = False
                '    cbotipo.Visible = False
                '    lbl_nombre1.Visible = False
                '    lbl_nombre2.Visible = False
                '    Me.lblConsultar.Text = "Consultar:"
                '    Me.lblDato.Text = "Dato:"
                '    Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                '    Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                '    Me.lsvListaAreas.Visible = True : Me.lsvListaAreas.Enabled = True
                '    Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                '    Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                '    Me.txtConsulta.Visible = True : Me.txtConsulta.Enabled = True
                '    Me.cboConsulta.Visible = True : Me.cboConsulta.Enabled = True
                '    Me.btnBuscar.Visible = True 'Me.btnBuscar.Enabled = True
                '    Me.txtRadio.Visible = False : Me.txtRadio.Enabled = False
                '    Me.cboZona.Visible = False : Me.cboZona.Enabled = False
                '    Me.lblRadio.Visible = False : Me.lblRadio.Enabled = False
                '    Me.lblZona.Visible = False : Me.lblZona.Enabled = False
                '    cboConsulta.Items.Clear()
                '    cboConsulta.Enabled = True : txtConsulta.Enabled = True
                '    btnBuscar.Enabled = True
                '    Txtdato5.Visible = False
                '    Txtdato6.Visible = False
                '    btnagregar.Visible = False
                '    txtRadio.Enabled = False
                '    txtdato1.Visible = False : txtdato1.Enabled = False
                '    txtdato2.Visible = False : txtdato2.Enabled = False
                '    txtdato3.Visible = False : txtdato3.Enabled = False
                '    txtdato4.Visible = False : txtdato4.Enabled = False
                '    lstCoordenada.Visible = False
                '    cboZona.Enabled = False
                '    txtRadio.Enabled = False
                '    btnGraficar.Enabled = False
                '    btnReporte.Enabled = False
                '    txtdato1.Visible = False : txtdato1.Enabled = False
                '    txtdato2.Visible = False : txtdato2.Enabled = False
                '    txtdato3.Visible = False : txtdato3.Enabled = False
                '    txtdato4.Visible = False : txtdato4.Enabled = False
                '    lbt_dato1.Visible = False : lbt_dato2.Visible = False
                '    lbt_dato3.Visible = False : lbt_dato4.Visible = False
                '    Me.lbl_Previo.Text = "CONSULTA AREA RESTRINGIDA"
                '    cboConsulta.Items.Add("Codigo Area")
                '    cboConsulta.Items.Add("Nombre Area")
                '    Me.cboConsulta.SelectedIndex = 1
                '    'cboConsulta.Text = "NOMBRE"
                '    txtConsulta.Text = ""
                '    ' txtConsulta.Text = "PARA"
                '    tipo_seleccion = "OP_4"
                '    img_DM.ImageLocation = glo_pathPNG & "\Area_Restringida.png"

            Case "OP_4" 'AREA RESTRINGIDA
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                img_DM1.Visible = False
                btnActualizar.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                Me.lblConsultar.Text = "Consultar:"
                Me.lblDato.Text = "Dato:"
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.lsvListaAreas.Visible = True : Me.lsvListaAreas.Enabled = True
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                Me.txtConsulta.Visible = True : Me.txtConsulta.Enabled = True
                Me.cboConsulta.Visible = True : Me.cboConsulta.Enabled = True
                Me.btnBuscar.Visible = True 'Me.btnBuscar.Enabled = True
                Me.txtRadio.Visible = False : Me.txtRadio.Enabled = False
                Me.cboZona.Visible = False : Me.cboZona.Enabled = False
                Me.lblRadio.Visible = False : Me.lblRadio.Enabled = False
                Me.lblZona.Visible = False : Me.lblZona.Enabled = False
                cboConsulta.Items.Clear()
                cboConsulta.Enabled = True : txtConsulta.Enabled = True
                btnBuscar.Enabled = True
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                lstCoordenada.Visible = False
                cboZona.Enabled = False
                txtRadio.Enabled = False
                btnGraficar.Enabled = False
                btnReporte.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                lbt_dato1.Visible = False : lbt_dato2.Visible = False
                lbt_dato3.Visible = False : lbt_dato4.Visible = False
                Me.lbl_Previo.Text = "CONSULTA AREA RESTRINGIDA"
                cboConsulta.Items.Add("Codigo Area")
                cboConsulta.Items.Add("Nombre Area")
                Me.cboConsulta.SelectedIndex = 1
                'cboConsulta.Text = "NOMBRE"
                txtConsulta.Text = ""
                ' txtConsulta.Text = "PARA"
                tipo_seleccion = "OP_4"
                img_DM.ImageLocation = glo_pathPNG & "\Area_Restringida.png"

            Case "OP_8", "OP_10", "OP_7"
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                img_DM1.Visible = False
                btnActualizar.Visible = False
                btnvermapa.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                lblConsultar.Visible = True : lblConsultar.Enabled = True
                Me.btnGenera_Poligono.Visible = False
                Me.cboZona.Location = New System.Drawing.Point(290, 113)
                Me.lblZona.Location = New System.Drawing.Point(290, 97)
                Me.lblZona.Text = "Zona: "
                Me.txtEste.Visible = False
                Me.lbt_dato1.Visible = False : Me.lbt_dato1.Enabled = False
                Me.lbt_dato2.Visible = False : Me.lbt_dato2.Enabled = False
                Me.lbt_dato3.Visible = False : Me.lbt_dato3.Enabled = False
                Me.lbt_dato4.Visible = False : Me.lbt_dato4.Enabled = False

                Me.btnElimina.Visible = False : Me.btnLimpia.Visible = False
                Me.btnBuscar.Visible = True 'Me.btnBuscar.Enabled = True
                cboConsulta.Items.Clear()
                cboConsulta.Enabled = True
                txtConsulta.Visible = True : txtConsulta.Enabled = True
                btnBuscar.Enabled = True
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                lstCoordenada.Visible = False
                cboZona.Enabled = False
                clbLayer.Enabled = False
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                btnGraficar.Enabled = False
                Me.cboConsulta.Visible = False
                Me.lblDato.Visible = False
                Me.txtRadio.Visible = False
                Me.lblRadio.Visible = False
                Me.lbl_Previo.Visible = False
                'Dim losender As New System.Object
                'Dim loe As New System.EventArgs
                'Me.btnOtraConsulta_Click(losender, loe)
                Me.lblConsultar.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                If Me.cbo_tipo.SelectedValue = "OP_8" Then
                    img_DM.ImageLocation = glo_pathPNG & "\distrito.png"
                    Me.lblConsultar.Text = "Nombre del Distrito a Graficar: "
                    tipo_seleccion = "Nombre del Distrito a Graficar: "
                ElseIf Me.cbo_tipo.SelectedValue = "OP_10" Then
                    img_DM.ImageLocation = glo_pathPNG & "\provincia.png"
                    Me.lblConsultar.Text = "Nombre de la Provincia a Graficar: "
                    tipo_seleccion = "DM SEG�N PROVINCIA"
                ElseIf Me.cbo_tipo.SelectedValue = "OP_7" Then
                    img_DM.ImageLocation = glo_pathPNG & "\depa.png"
                    Me.lblConsultar.Text = "Nombre del Departam. a Graficar: "
                    tipo_seleccion = "DM SEG�N DEPARTAMENTO"
                End If
            Case "OP_5", "OP_6"
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                img_DM1.Visible = False
                btnActualizar.Visible = False
                btnvermapa.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                Me.cboConsulta.Items.Clear()
                cboConsulta.Items.Add("C�digo DM")
                cboConsulta.Items.Add("Nombre DM")
                cboConsulta.SelectedIndex = 0
                Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                Me.cboConsulta.Visible = True : Me.cboConsulta.Enabled = True
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.txtConsulta.Visible = True : Me.txtConsulta.Enabled = True
                lblConsultar.Text = "Consultar:"
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                lbt_dato1.Visible = False : lbt_dato2.Visible = False
                lbt_dato3.Visible = False : lbt_dato4.Visible = False
                Me.lblRadio.Visible = False : Me.lblRadio.Enabled = False
                Me.lblZona.Visible = False : Me.lblZona.Enabled = False
                Me.txtRadio.Visible = False : Me.txtRadio.Enabled = False
                Me.cboZona.Visible = False : Me.cboZona.Enabled = False
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                Me.btnBuscar.Visible = True : btnBuscar.Enabled = True
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                lstCoordenada.Visible = True : lstCoordenada.Enabled = True
                cboZona.Enabled = False
                clbLayer.Enabled = False
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                btnGraficar.Enabled = False
                Me.txtRadio.Visible = False : Me.txtRadio.Enabled = False
                If cbo_tipo.SelectedValue = "OP_5" Then
                    Me.lbl_Previo.Text = "DM SEG�N CARTA IGN"
                    tipo_seleccion = "OP_5"
                    img_DM.ImageLocation = glo_pathPNG & "\dm_carta.png"
                ElseIf cbo_tipo.SelectedValue = "OP_6" Then

                    Me.lbl_Previo.Text = "DM SEG�N DEMARCACI�N"
                    tipo_seleccion = "OP_6" ' "DM SEG�N DEMARCACI�N"
                    img_DM.ImageLocation = glo_pathPNG & "\dm_demarca.png"
                End If

            Case "OP_15"
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                txtConsulta.Visible = False
                btnLimpia.Visible = False
                btnCerrar.Visible = True
                img_DM1.Visible = True
                cbotipo.Enabled = False
                cboarea.Visible = True
                cbotipo.Visible = True
                btnElimina.Visible = False
                btnGenera_Poligono.Visible = False
                lbl_nombre1.Visible = True
                lbl_nombre2.Visible = True
                btnvermapa.Visible = True
                btnActualizar.Visible = True
                Me.cboConsulta.Items.Clear()
                cboConsulta.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblConsultar.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblDato.Visible = False
                Me.txtConsulta.Visible = False
                lblConsultar.Visible = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lbt_dato1.Visible = False : lbt_dato2.Visible = False
                lbt_dato3.Visible = False : lbt_dato4.Visible = False
                Me.lblRadio.Visible = False
                Me.lblZona.Visible = False
                Me.txtRadio.Visible = False
                Me.cboZona.Visible = False
                Me.lbl_Previo.Visible = False
                Me.btnBuscar.Visible = False
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lstCoordenada.Visible = False
                Me.txtRadio.Visible = False
                btnReporte.Visible = False
                rbt_NoVisualiza.Visible = False
                rbt_Visualiza.Visible = False
                btnGraficar.Visible = False
                btnOtraConsulta.Visible = False
                clbLayer.Visible = False
                GroupBox1.Visible = False
                dgdDetalle.Visible = False
                img_DM.ImageLocation = glo_pathPNG & "\Area_Restringida.png"
                img_DM1.ImageLocation = glo_pathPNG & "\Repositorio.png"

            Case "OP_17"
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                txtConsulta.Visible = False
                btnLimpia.Visible = False
                btnCerrar.Visible = True
                img_DM1.Visible = True
                ' cbotipo.Enabled = False
                cboarea.Visible = False
                cbotipo.Visible = False
                btnElimina.Visible = False
                btnGenera_Poligono.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                Me.cboConsulta.Items.Clear()
                cboConsulta.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblConsultar.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblDato.Visible = False
                Me.txtConsulta.Visible = False
                lblConsultar.Visible = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lbt_dato1.Visible = False : lbt_dato2.Visible = False
                lbt_dato3.Visible = False : lbt_dato4.Visible = False
                Me.lblRadio.Visible = False
                Me.lblZona.Visible = False
                Me.txtRadio.Visible = False
                Me.cboZona.Visible = False
                Me.lbl_Previo.Visible = False
                Me.btnBuscar.Visible = False
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lstCoordenada.Visible = False
                Me.txtRadio.Visible = False
                btnReporte.Visible = False
                rbt_NoVisualiza.Visible = False
                rbt_Visualiza.Visible = False

                btnOtraConsulta.Visible = False
                clbLayer.Visible = False
                GroupBox1.Visible = False
                dgdDetalle.Visible = False
                btnGraficar.Enabled = True
                btnGraficar.Visible = True
                img_DM.ImageLocation = glo_pathPNG & "\Area_Neta.png"
                img_DM1.ImageLocation = glo_pathPNG & "\Repositorio.png"

            Case "OP_21"

                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                'ESTADISTICAS DE AREAS RESTRINGIDAS
                Me.Hide()
                'comentado(temporalmente)
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                Dim pForm As New Frm_Estadisticas_Caram

                pForm.m_application = m_Application
                pForm.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                pForm.ShowDialog()
                SetWindowLong(pForm.Handle.ToInt32(), GWL_HWNDPARENT, m_Application.hWnd)
                Select Case pForm.DialogResult
                    Case DialogResult.OK
                        cbo_tipo.SelectedIndex = 0
                        Me.Show()
                    Case DialogResult.Cancel
                        cbo_tipo.SelectedIndex = 0
                        Me.Show()
                End Select

            Case "OP_22"  'Demarcacion INEI

                btn_cargarcuadriculas.Visible = True
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = True
                btn_procesar.Enabled = True

                dgdResultado.Enabled = False
                dgdResultado.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                txtConsulta.Visible = False
                btnLimpia.Visible = False
                btnCerrar.Visible = True
                img_DM1.Visible = True
                ' cbotipo.Enabled = False
                cboarea.Visible = False
                cbotipo.Visible = False
                btnElimina.Visible = False
                btnGenera_Poligono.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                Me.cboConsulta.Items.Clear()
                cboConsulta.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblConsultar.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblDato.Visible = False
                Me.txtConsulta.Visible = False
                lblConsultar.Visible = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lbt_dato1.Visible = False : lbt_dato2.Visible = False
                lbt_dato3.Visible = False : lbt_dato4.Visible = False
                Me.lblRadio.Visible = False
                Me.lblZona.Visible = False
                Me.txtRadio.Visible = False
                Me.cboZona.Visible = False
                Me.lbl_Previo.Visible = False
                Me.btnBuscar.Visible = False
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lstCoordenada.Visible = False
                Me.txtRadio.Visible = False
                btnReporte.Visible = False
                rbt_NoVisualiza.Visible = False
                rbt_Visualiza.Visible = False

                btnOtraConsulta.Visible = False
                clbLayer.Visible = False
                GroupBox1.Visible = False
                dgdDetalle.Visible = False
                btnGraficar.Enabled = False
                btnGraficar.Visible = False
                img_DM.ImageLocation = glo_pathPNG & "\regiones.png"
                img_DM1.ImageLocation = glo_pathPNG & "\Repositorio.png"

            Case "OP_23"  'Reporte DM superpuestos
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = True
                btn_procesar.Enabled = True

                lblfecha.Enabled = True
                lblfecha.Visible = True
                dtpFecha1.Enabled = True
                dtpFecha1.Visible = True

                lbl_sistema.Enabled = False
                lbl_sistema.Visible = False
                cboSistema.Enabled = False
                cboSistema.Visible = False

                'dgdResultado.ClearSelection()
                'dgdResultado.Rows.Clear()

                dgdResultado.Enabled = True
                dgdResultado.Visible = True
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = True

                txtArea.Visible = False
                lblArea.Visible = False
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                txtConsulta.Visible = False
                btnLimpia.Visible = False
                btnCerrar.Visible = True
                img_DM1.Visible = True
                ' cbotipo.Enabled = False
                cboarea.Visible = False
                cbotipo.Visible = False
                btnElimina.Visible = False
                btnGenera_Poligono.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                Me.cboConsulta.Items.Clear()
                cboConsulta.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblConsultar.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblDato.Visible = False
                Me.txtConsulta.Visible = False
                lblConsultar.Visible = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lbt_dato1.Visible = False : lbt_dato2.Visible = False
                lbt_dato3.Visible = False : lbt_dato4.Visible = False
                Me.lblRadio.Visible = False
                Me.lblZona.Visible = False
                Me.txtRadio.Visible = False
                Me.cboZona.Visible = False
                Me.lbl_Previo.Visible = False
                Me.btnBuscar.Visible = False
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lstCoordenada.Visible = False
                Me.txtRadio.Visible = False
                btnReporte.Visible = False
                rbt_NoVisualiza.Visible = False
                rbt_Visualiza.Visible = False

                btnOtraConsulta.Visible = False
                clbLayer.Visible = False
                GroupBox1.Visible = False
                dgdDetalle.Visible = False
                btnGraficar.Enabled = False
                btnGraficar.Visible = False
                img_DM.ImageLocation = glo_pathPNG & "\simulacion.png"
                img_DM1.ImageLocation = glo_pathPNG & "\Repositorio.png"

            Case "OP_24"  'DM Simultaneos
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = True
                btn_procesar.Enabled = True

                lblfecha.Enabled = True
                lblfecha.Visible = True
                dtpFecha1.Enabled = True
                dtpFecha1.Visible = True
                'dgdResultado.Enabled = True
                'dgdResultado.Visible = True
                dgdDetalle.Enabled = True
                dgdDetalle.Visible = True
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                Me.cboSistema.Visible = False
                Me.lbl_sistema.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                txtConsulta.Visible = False
                btnLimpia.Visible = False
                btnCerrar.Visible = True
                img_DM1.Visible = True
                ' cbotipo.Enabled = False
                cboarea.Visible = False
                cbotipo.Visible = False
                btnElimina.Visible = False
                btnGenera_Poligono.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                Me.cboConsulta.Items.Clear()
                cboConsulta.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblConsultar.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblDato.Visible = False
                Me.txtConsulta.Visible = False
                lblConsultar.Visible = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lbt_dato1.Visible = False : lbt_dato2.Visible = False
                lbt_dato3.Visible = False : lbt_dato4.Visible = False
                Me.lblRadio.Visible = False
                Me.lblZona.Visible = False
                Me.txtRadio.Visible = False
                Me.cboZona.Visible = False
                Me.lbl_Previo.Visible = False
                Me.btnBuscar.Visible = False
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lstCoordenada.Visible = False
                Me.txtRadio.Visible = False
                btnReporte.Visible = False
                rbt_NoVisualiza.Visible = False
                rbt_Visualiza.Visible = False

                btnOtraConsulta.Visible = False
                clbLayer.Visible = False
                GroupBox1.Visible = False
                'dgdDetalle.Visible = False
                btnGraficar.Enabled = False
                btnGraficar.Visible = False
                img_DM.ImageLocation = glo_pathPNG & "\simulacion.png"
                img_DM1.ImageLocation = glo_pathPNG & "\Repositorio.png"

            Case "OP_25"  'Petitorios PMA
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = True
                btn_procesar.Enabled = True

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False

                'dgdResultado.ClearSelection()
                'dgdResultado.Rows.Clear()

                dgdResultado.Enabled = True
                dgdResultado.Visible = True
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = True

                txtArea.Visible = False
                lblArea.Visible = False
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                txtConsulta.Visible = False
                btnLimpia.Visible = False
                btnCerrar.Visible = True
                img_DM1.Visible = True
                ' cbotipo.Enabled = False
                cboarea.Visible = False
                cbotipo.Visible = False
                btnElimina.Visible = False
                btnGenera_Poligono.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                Me.cboConsulta.Items.Clear()
                cboConsulta.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblConsultar.Visible = False
                Me.cboConsulta.Visible = False
                Me.lblDato.Visible = False
                Me.txtConsulta.Visible = False
                lblConsultar.Visible = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lbt_dato1.Visible = False : lbt_dato2.Visible = False
                lbt_dato3.Visible = False : lbt_dato4.Visible = False
                Me.lblRadio.Visible = False
                Me.lblZona.Visible = False
                Me.txtRadio.Visible = False
                Me.cboZona.Visible = False
                Me.lbl_Previo.Visible = False
                Me.btnBuscar.Visible = False
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = False
                txtdato2.Visible = False
                txtdato3.Visible = False
                txtdato4.Visible = False
                lstCoordenada.Visible = False
                Me.txtRadio.Visible = False
                btnReporte.Visible = False
                rbt_NoVisualiza.Visible = False
                rbt_Visualiza.Visible = False

                btnOtraConsulta.Visible = False
                clbLayer.Visible = False
                GroupBox1.Visible = False
                dgdDetalle.Visible = False
                btnGraficar.Enabled = False
                btnGraficar.Visible = False
                img_DM.ImageLocation = glo_pathPNG & "\simulacion.png"
                img_DM1.ImageLocation = glo_pathPNG & "\Repositorio.png"

            Case "OP_26"

                Dim pForm1 As New Frm_Ingreso_AreasRestringidas

                pForm1.m_application = m_Application
                pForm1.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                pForm1.ShowDialog()
                SetWindowLong(pForm1.Handle.ToInt32(), GWL_HWNDPARENT, m_Application.hWnd)

            Case "OP_27" 'CONSULTAR AREA RESTRINGIDA
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                lblfecha.Enabled = False
                lblfecha.Visible = False
                dtpFecha1.Enabled = False
                dtpFecha1.Visible = False
                dgdResultado.Enabled = False
                dgdResultado.Visible = False
                btnexp_excel.Enabled = False
                btnexp_excel.Visible = False

                txtArea.Visible = False
                lblArea.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                img_DM1.Visible = False
                btnActualizar.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                Me.lblConsultar.Text = "Consultar:"
                Me.lblDato.Text = "Dato:"
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.lsvListaAreas.Visible = True : Me.lsvListaAreas.Enabled = True
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                Me.txtConsulta.Visible = True : Me.txtConsulta.Enabled = True
                Me.cboConsulta.Visible = True : Me.cboConsulta.Enabled = True
                Me.btnBuscar.Visible = True 'Me.btnBuscar.Enabled = True
                Me.txtRadio.Visible = False : Me.txtRadio.Enabled = False
                Me.cboZona.Visible = False : Me.cboZona.Enabled = False
                Me.lblRadio.Visible = False : Me.lblRadio.Enabled = False
                Me.lblZona.Visible = False : Me.lblZona.Enabled = False
                cboConsulta.Items.Clear()
                cboConsulta.Enabled = True : txtConsulta.Enabled = True
                btnBuscar.Enabled = True
                Txtdato5.Visible = False
                Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                lstCoordenada.Visible = False
                cboZona.Enabled = False
                txtRadio.Enabled = False
                btnGraficar.Enabled = False
                btnReporte.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                lbt_dato1.Visible = False : lbt_dato2.Visible = False
                lbt_dato3.Visible = False : lbt_dato4.Visible = False
                Me.lbl_Previo.Text = "CONSULTA AREA RESTRINGIDA"
                cboConsulta.Items.Add("Codigo Area")
                cboConsulta.Items.Add("Nombre Area")
                Me.cboConsulta.SelectedIndex = 1
                'cboConsulta.Text = "NOMBRE"
                txtConsulta.Text = ""
                ' txtConsulta.Text = "PARA"
                tipo_seleccion = "OP_4"
                img_DM.ImageLocation = glo_pathPNG & "\Area_Restringida.png"

            Case "OP_20"   'Data Historica
                lblAnio.Visible = True
                cboAnio.Enabled = True
                cboAnio.Visible = True

                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                dgdResultado.Enabled = False
                dgdResultado.Visible = False

                img_DM1.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.txtRadio.Visible = True : Me.txtRadio.Enabled = True
                Me.cboZona.Visible = True : Me.cboZona.Enabled = True
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                Me.lblRadio.Visible = True : Me.lblRadio.Enabled = True
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.lblConsultar.Text = "Consultar"
                Me.lblDato.Text = "Dato"
                cboConsulta.Enabled = True : cboConsulta.Visible = True
                txtConsulta.Visible = True : txtConsulta.Enabled = True
                Me.lbt_dato1.Visible = False : Me.lbt_dato1.Enabled = False
                Me.lbt_dato2.Visible = False : Me.lbt_dato2.Enabled = False
                Me.lbt_dato3.Visible = False : Me.lbt_dato3.Enabled = False
                Me.lbt_dato4.Visible = False : Me.lbt_dato4.Enabled = False
                Me.btnLimpia.Visible = False : Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False : Me.btnElimina.Enabled = False
                Me.btnGenera_Poligono.Visible = False
                Me.lstCoordenada.Items.Clear()
                Me.btnagregar.Visible = False
                Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False
                Me.lbl_Previo.Text = "Vista Previa, DM y V�rtices"
                Me.btnBuscar.Visible = True
                cboConsulta.Items.Clear()
                cboConsulta.Enabled = True
                txtConsulta.Enabled = True
                btnBuscar.Enabled = True
                lstCoordenada.Visible = True
                Txtdato5.Visible = False : Txtdato5.Enabled = False
                Txtdato6.Visible = False : Txtdato6.Enabled = False
                btnagregar.Visible = False : btnagregar.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                cboZona.Enabled = False
                btnGraficar.Enabled = False
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                cboConsulta.Items.Add("C�digo DM")
                cboConsulta.Items.Add("Nombre DM")
                Me.cboConsulta.SelectedIndex = 0

                Me.cboAnio.SelectedIndex = 0

                Me.cboAnio.Focus()

                'Me.txtConsulta.Focus()

                img_DM.ImageLocation = glo_pathPNG & "\evaluacion.png"

                lbl_sistema.Visible = False
                cboSistema.Enabled = False
                cboSistema.Visible = False

            Case "OP_29"   'DM Acumulacion

                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btn_cargarcuadriculas.Visible = False
                btn_cargarcuadriculas.Enabled = False
                btn_procesar.Visible = False
                btn_procesar.Enabled = False

                dgdResultado.Enabled = False
                dgdResultado.Visible = False

                img_DM1.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                Me.lblConsultar.Visible = True : Me.lblConsultar.Enabled = True
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.txtRadio.Visible = True : Me.txtRadio.Enabled = True
                Me.cboZona.Visible = True : Me.cboZona.Enabled = True
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True
                Me.lblRadio.Visible = True : Me.lblRadio.Enabled = True
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.lblConsultar.Text = "Consultar"
                Me.lblDato.Text = "Dato"
                cboConsulta.Enabled = True : cboConsulta.Visible = True
                txtConsulta.Visible = True : txtConsulta.Enabled = True
                Me.lbt_dato1.Visible = False : Me.lbt_dato1.Enabled = False
                Me.lbt_dato2.Visible = False : Me.lbt_dato2.Enabled = False
                Me.lbt_dato3.Visible = False : Me.lbt_dato3.Enabled = False
                Me.lbt_dato4.Visible = False : Me.lbt_dato4.Enabled = False
                Me.btnLimpia.Visible = False : Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False : Me.btnElimina.Enabled = False
                Me.btnGenera_Poligono.Visible = False
                Me.lstCoordenada.Items.Clear()
                Me.btnagregar.Visible = False
                Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False
                Me.lbl_Previo.Text = "Vista Previa, DM y V�rtices"
                Me.btnBuscar.Visible = True
                cboConsulta.Items.Clear()
                cboConsulta.Enabled = True
                txtConsulta.Enabled = True
                btnBuscar.Enabled = True
                lstCoordenada.Visible = True
                Txtdato5.Visible = False : Txtdato5.Enabled = False
                Txtdato6.Visible = False : Txtdato6.Enabled = False
                btnagregar.Visible = False : btnagregar.Enabled = False
                txtdato1.Visible = False : txtdato1.Enabled = False
                txtdato2.Visible = False : txtdato2.Enabled = False
                txtdato3.Visible = False : txtdato3.Enabled = False
                txtdato4.Visible = False : txtdato4.Enabled = False
                cboZona.Enabled = False
                btnGraficar.Enabled = False
                txtRadio.Enabled = False
                btnReporte.Enabled = False
                cboConsulta.Items.Add("C�digo DM")
                cboConsulta.Items.Add("Nombre DM")
                Me.cboConsulta.SelectedIndex = 0

                Me.txtConsulta.Focus()

                img_DM.ImageLocation = glo_pathPNG & "\evaluacion.png"

                lbl_sistema.Visible = False
                cboSistema.Enabled = False
                cboSistema.Visible = False

            Case Else
        End Select
        btnBuscar.Enabled = True

    End Sub

    Public Sub PT_Inicializar_Grilla_DM()
        Dim lodtbLista_DM As New DataTable
        lodtbLista_DM.Columns.Add("CODIGO", GetType(String))
        lodtbLista_DM.Columns.Add("NOMBRE", GetType(String))
        lodtbLista_DM.Columns.Add("CARTA", GetType(String))
        lodtbLista_DM.Columns.Add("ZONA", GetType(String))
        lodtbLista_DM.Columns.Add("TIPO", GetType(String))
        lodtbLista_DM.Columns.Add("NATURALEZA", GetType(String))
        lodtbLista_DM.Columns.Add("HECTAREA", GetType(String))
        lodtbLista_DM.Columns.Add("ESTADO", GetType(String))
        lodtbLista_DM.Columns.Add("PE_VIGCAT", GetType(String))
        PT_Estilo_Grilla_DM(lodtbLista_DM) : PT_Cargar_Grilla_DM(lodtbLista_DM)
        PT_Agregar_Funciones_DM() : PT_Forma_Grilla_DM()
    End Sub
    Private Sub PT_Estilo_Grilla_DM(ByRef padtbDetalle As DataTable)
        padtbDetalle.Columns.Item(Col_Codigo).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Nombre).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Carta).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Zona).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Tipo).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Naturaleza).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Hectarea).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Estado).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Vigcat).DefaultValue = ""
    End Sub
    Private Sub PT_Cargar_Grilla_DM(ByVal padtbDetalle As DataTable)
        Dim dvwDetalle As New DataView(padtbDetalle)
        Me.dgdDetalle.DataSource = dvwDetalle
        Pinta_Grilla_Dm()
    End Sub
    Public Sub Pinta_Grilla_Dm()
        Me.dgdDetalle.BackColor = Color.FromArgb(242, 242, 240)
        Me.dgdDetalle.HeadingStyle.BackColor = Color.FromArgb(207, 209, 221) 'Color.Black CFD1DD
        Me.dgdDetalle.OddRowStyle.BackColor = Color.FromArgb(229, 232, 239) 'E5E8EF
        Me.dgdDetalle.EvenRowStyle.BackColor = Color.FromArgb(242, 242, 240)
    End Sub
    Private Sub PT_Agregar_Funciones_DM()
        Me.dgdDetalle.Columns(Col_Codigo).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Nombre).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Carta).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Zona).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Tipo).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Naturaleza).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Hectarea).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Estado).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Vigcat).DefaultValue = ""
    End Sub
    Private Sub PT_Forma_Grilla_DM()
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codigo).Width = 80
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nombre).Width = 150
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Carta).Width = 45
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zona).Width = 40
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Tipo).Width = 40
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Naturaleza).Width = 65
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Hectarea).Width = 55
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Estado).Width = 40
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Vigcat).Width = 40
            Me.dgdDetalle.Columns("CODIGO").Caption = "C�digo"
            Me.dgdDetalle.Columns("NOMBRE").Caption = "Derecho Minero"
            Me.dgdDetalle.Columns("CARTA").Caption = "Carta"
            Me.dgdDetalle.Columns("ZONA").Caption = "Zona"
            Me.dgdDetalle.Columns("TIPO").Caption = "Tipo"
            Me.dgdDetalle.Columns("NATURALEZA").Caption = "Naturaleza"
            Me.dgdDetalle.Columns("HECTAREA").Caption = "Hectarea"
            Me.dgdDetalle.Columns("ESTADO").Caption = "Estado"
            Me.dgdDetalle.Columns("PE_VIGCAT").Caption = "Gr�fica"
            'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nombre).Locked = True
            'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codigo).Locked = True
            'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Tipo).Locked = True
            'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Carta).Locked = True
            'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Situacion).Locked = True
            'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Titular).Locked = True
            'Me.dgdDetalle.Splits(0).DisplayColumns("COD_SERVICIO").AllowSizing = True
            'Me.dgdDetalle.Splits(0).DisplayColumns("COD_SERVICIO").Style.WrapText = True
            'Me.dgdDetalle.Splits(0).DisplayColumns("DESCRIPCION").AllowSizing = True
            'Me.dgdDetalle.Splits(0).DisplayColumns("DESCRIPCION").Style.WrapText = True
            'Me.dgdDetalle.Splits(0).DisplayColumns("OBSERVACION").AllowSizing = True
            'Me.dgdDetalle.Splits(0).DisplayColumns("OBSERVACION").Style.WrapText = True
            'Me.dgd_Orden_Trabajo.Splits(0).AllowRowSizing = RowSizingEnum.AllRows
            'Me.dgd_Orden_Trabajo.AllowRowSizing = RowSizingEnum.IndividualRows
            'Me.dgd_Orden_Trabajo.Splits(0).Rows(0).Height = 50
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codigo).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nombre).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Carta).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zona).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Tipo).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Naturaleza).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Hectarea).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Estado).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Vigcat).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codigo).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Near
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nombre).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zona).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Carta).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Tipo).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Naturaleza).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Hectarea).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Estado).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
            Me.dgdDetalle.Splits(0).DisplayColumns(Col_Vigcat).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
    End Sub

    Private Sub dgdDetalle_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgdDetalle.DoubleClick

        Select Case Me.cbo_tipo.SelectedValue 'tipo_seleccion
            Case "OP_24" : Exit Sub
            Case "OP_4"
            Case "OP_27"
            Case "OP_13"
            Case "OP_7", "OP_10", "OP_8"
                cls_Catastro.Borra_Todo_Feature("", m_Application)
                cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                Dim lodbtZona As New DataTable : Dim lodbtUbigeo As New DataTable
                'lodbtZona = cls_Prueba.ShowUniqueValues_Zona("ZONA_DIST", "ZONA", lo_Filtro_Ubigeo)
                Select Case Me.cbo_tipo.SelectedValue
                    Case "OP_7"
                        lo_Filtro_Ubigeo = "CD_DEPA = '" & dgdDetalle.Item(dgdDetalle.Row, "UBIGEO") & "0000'"
                    Case "OP_10"
                        lo_Filtro_Ubigeo = "CD_PROV = '" & Mid(dgdDetalle.Item(dgdDetalle.Row, "UBIGEO"), 1, 4) & "'"
                        lodbtZona = cls_Oracle.FT_Selecciona_x_Zona(dgdDetalle.Item(dgdDetalle.Row, "UBIGEO"))
                    Case "OP_8"
                        lo_Filtro_Ubigeo = "CD_DIST = '" & dgdDetalle.Item(dgdDetalle.Row, "UBIGEO") & "'"
                        lodbtZona = cls_Oracle.FT_Selecciona_x_Zona(dgdDetalle.Item(dgdDetalle.Row, "UBIGEO"))
                End Select
                'lodbtZona = cls_Prueba.ShowUniqueValues_Zona_SDE("DBF_DISTRITO_CATASTRO", "ZONA", lo_Filtro_Ubigeo, m_application)
                Verificar_Zona_Ubigeo(lodbtZona) : cls_Catastro.Actualizar_DM(Me.cboZona.SelectedValue)
                '****************
                Select Case tipo_seleccion
                    Case "OP_7"
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento, m_Application, "1", True)
                    Case "OP_10"
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia, m_Application, "1", True)
                    Case "OP_8"
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito, m_Application, "1", True)
                End Select
                Select Case tipo_seleccion
                    Case "OP_7"
                        cls_Catastro.DefinitionExpression(lo_Filtro_Ubigeo, m_Application, lo_layer)
                        cls_Usuario.Activar_Layer_True_False(True, m_Application)
                        cls_Catastro.Seleccioname_Envelope(lo_Filtro_Ubigeo, lo_layer, xMin, yMin, xMax, yMax, Me.cboZona.SelectedValue, m_Application)
                        'cls_Catastro.Seleccioname_Envelope(lo_Filtro_Ubigeo, lo_layer, v_xmin, v_ymin, v_xmax, v_ymax, Me.cboZona.SelectedValue, m_Application)
                        cls_Catastro.ShowLabel_DM(lo_layer, m_Application)
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento, m_Application, "1", True)
                    Case "OP_10"
                        cls_Catastro.DefinitionExpression(lo_Filtro_Ubigeo, m_Application, lo_layer)
                        cls_Usuario.Activar_Layer_True_False(True, m_Application)
                        cls_Catastro.Seleccioname_Envelope(lo_Filtro_Ubigeo, lo_layer, xMin, yMin, xMax, yMax, Me.cboZona.SelectedValue, m_Application)
                        'cls_Catastro.Seleccioname_Envelope(lo_Filtro_Ubigeo, lo_layer, v_xmin, v_ymin, v_xmax, v_ymax, Me.cboZona.SelectedValue, m_Application)
                        cls_Catastro.ShowLabel_DM(lo_layer, m_Application)
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia, m_Application, "1", True)
                    Case "OP_8"
                        cls_Catastro.DefinitionExpression(lo_Filtro_Ubigeo, m_Application, lo_layer)
                        cls_Usuario.Activar_Layer_True_False(True, m_Application)
                        cls_Catastro.Seleccioname_Envelope(lo_Filtro_Ubigeo, lo_layer, xMin, yMin, xMax, yMax, Me.cboZona.SelectedValue, m_Application)
                        'cls_Catastro.Seleccioname_Envelope(lo_Filtro_Ubigeo, lo_layer, v_xmin, v_ymin, v_xmax, v_ymax, Me.cboZona.SelectedValue, m_Application)
                        cls_Catastro.ShowLabel_DM(lo_layer, m_Application)
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito, m_Application, "1", True)
                End Select

                ' cls_Catastro.Shade_Poligono(lo_layer, m_application)
                cls_Catastro.Color_Poligono_Simple(m_Application, lo_layer)
                cls_Catastro.Genera_Imagen_DM("Vista_Ubigeo", "vistaprevia")
                Me.img_DM.ImageLocation = glo_pathTMP & "\Vista_Ubigeo.jpg"
                cls_Catastro.Quitar_Layer(lo_layer, m_Application) : cls_Catastro.Quitar_Layer(lo_layer, m_Application)
                Me.btnGraficar.Enabled = True
            Case "OP_1", "OP_14", "OP_28"
                Me.btnGraficar.Enabled = True
                Dim lo_Filtro As String = "CARTA = '" & dgdDetalle.Item(dgdDetalle.Row, "CODIGO") & "'"
                v_dato1 = 0 : v_dato2 = 0 : v_dato3 = 0 : v_dato4 = 0
                'cls_Catastro.AddImagen("U:\DATOS\ECW_CARTAS\" & dgdDetalle.Item(dgdDetalle.Row, "CODIGO").Replace("-", "") & ".ECW", "", m_application, True)
                Select Case Me.cbo_tipo.SelectedValue
                    Case "OP_1", "OP_28"
                        'cls_Catastro.AddImagen(glo_pathIGN & dgdDetalle.Item(dgdDetalle.Row, "CODIGO").Replace("-", "") & ".ECW", "", dgdDetalle.Item(dgdDetalle.Row, "CODIGO"), m_Application, True)
                        v_carta_dm = dgdDetalle.Item(dgdDetalle.Row, "CODIGO")
                        'carta_v = v_carta_dm.Replace("-", "")
                        carta_v = v_carta_dm
                        'Dim filtro_carta As String = "NAME = '" & LCase(carta_v) & ".ecw'"
                        Dim filtro_carta As String = "NAME = '" & LCase(carta_v.Replace("-", "")) & ".ecw'"

                        'cls_Catastro.AddImagen(glo_pathIGN & carta_v & ".ECW", "1", carta_v, m_Application, True)

                        cls_Catastro.Conexion_SDE(m_Application)
                        If v_sistema = "PSAD-56" Then
                            pRasterCatalog = cls_Catastro.GetRasterCatalog(m_Application, "DATA_GIS.DS_IGN_CARTA_NACIONAL_2016_P56")
                            'pRasterCatalog = cls_Catastro.GetRasterCatalog(m_Application, " DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOPSA56")
                            '    DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOPSA56()
                            cls_DM_1.AddRasterCatalogLayer(m_Application, pRasterCatalog)

                            'cls_Catastro.MyDefinitionQuery("DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOPSA56", m_Application, carta_v, filtro_carta)
                            'cls_Catastro.ZOOM_SELECTED("DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOPSA56", m_Application)
                            cls_Catastro.MyDefinitionQuery("DATA_GIS.DS_IGN_CARTA_NACIONAL_2016_P56", m_Application, carta_v, filtro_carta)
                            cls_Catastro.ZOOM_SELECTED("DATA_GIS.DS_IGN_CARTA_NACIONAL_2016_P56", m_Application)
                            pFSelQuery.Clear()
                        Else

                            'pRasterCatalog = cls_Catastro.GetRasterCatalog(m_Application, "DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOWGS84")
                            pRasterCatalog = cls_Catastro.GetRasterCatalog(m_Application, "DATA_GIS.DS_IGN_CARTA_NACIONAL_2016_W84")
                            'pRasterCatalog = cls_Catastro.GetRasterCatalog(m_Application, " DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOPSA56")
                            '    DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOPSA56()
                            cls_DM_1.AddRasterCatalogLayer(m_Application, pRasterCatalog)

                            'cls_Catastro.MyDefinitionQuery("DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOWGS84", m_Application, carta_v, filtro_carta)
                            'cls_Catastro.ZOOM_SELECTED("DATA_GIS.DS_IGN_CARTA_NACIONAL_GEOWGS84", m_Application)
                            cls_Catastro.MyDefinitionQuery("DATA_GIS.DS_IGN_CARTA_NACIONAL_2016_W84", m_Application, carta_v, filtro_carta)
                            cls_Catastro.ZOOM_SELECTED("DATA_GIS.DS_IGN_CARTA_NACIONAL_2016_W84", m_Application)
                            pFSelQuery.Clear()
                            'escala = pMap.MapScale
                            'escala = escala * 3
                            'pMxDoc.UpdateContents()
                            'pMap.MapScale = escala
                            'pMxDoc.UpdateContents()      'SE COMENTO 14072016
                        End If


                        '  Exit Sub

                    Case "OP_14"
                        'cls_Catastro.AddImagen(glo_pathGEO & dgdDetalle.Item(dgdDetalle.Row, "CODIGO").Replace("-", "") & ".ECW", 3, dgdDetalle.Item(dgdDetalle.Row, "CODIGO"), m_Application, True)
                        v_carta_dm = dgdDetalle.Item(dgdDetalle.Row, "CODIGO")
                        carta_v = v_carta_dm.Replace("-", "")
                        'carta_v = v_carta_dm
                        Dim filtro_carta As String = "NAME = '" & LCase(carta_v) & ".ecw'"
                        'cls_Catastro.AddImagen(glo_pathIGN & carta_v & ".ECW", "1", carta_v, m_Application, True)

                        cls_Catastro.Conexion_SDE(m_Application)

                        pRasterCatalog = cls_Catastro.GetRasterCatalog(m_Application, "DATA_GIS.DS_IMAGEN_CARTA_GEOLOGICA")
                        cls_DM_1.AddRasterCatalogLayer(m_Application, pRasterCatalog)
                        cls_Catastro.MyDefinitionQuery("DATA_GIS.DS_IMAGEN_CARTA_GEOLOGICA", m_Application, carta_v, filtro_carta)
                        cls_Catastro.ZOOM_SELECTED("DATA_GIS.DS_IMAGEN_CARTA_GEOLOGICA", m_Application)
                        pFSelQuery.Clear()

                        'escala = pMap.MapScale
                        'escala = escala * 3
                        'pMxDoc.UpdateContents()
                        'pMap.MapScale = escala
                        pMxDoc.UpdateContents()

                End Select

                If cbo_tipo.SelectedValue = "OP_20" Or cbo_tipo.SelectedValue = "OP_28" Then
                    gstrFC_Catastro_Minero = "CEGO0891.GPO_CATASTRO_MINERO_H"
                Else
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                End If

                'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero & dgdDetalle.Item(dgdDetalle.Row, "ZONA"), m_Application, "1", True)  'PSAD56
                cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero & "18", m_Application, "1", False)
                '     cls_Catastro.PT_CargarFeatureClass_SDE("GPO_CMI_CATASTRO_MINERO_18", m_Application, "1", False)

                '  cls_Catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_CMI_CATASTRO_MINERO_" & dgdDetalle.Item(dgdDetalle.Row, "ZONA"), m_Application, "1", True)  'PSAD56
                'cls_Catastro.PT_CargarFeatureClass_SDE("GPO_CMI_CATASTRO_MINERO_WGS_" & dgdDetalle.Item(dgdDetalle.Row, "ZONA"), m_Application, "1", True)  'WGS64
                'cls_Catastro.Conexion_SDE(m_Application)
                cls_Catastro.DefinitionExpression(lo_Filtro, m_Application, "Catastro")

                'cls_Catastro.Poligono_Color_GDB(gstrFC_Catastro_Minero & dgdDetalle.Item(dgdDetalle.Row, "ZONA"), glo_pathStyle & "\CATASTRO1.style", "ESTADO", "", "Cadena", "", m_application, "")
                cls_Catastro.Color_Poligono_Simple(m_Application, "Catastro")  'se cambio por la anterior linea, 
                cls_planos.cambiacolor_frame("PORCOLOR")


                cls_Catastro.Genera_Imagen_DM("VistaPrevia", "vista previa")
                Me.img_DM.ImageLocation = glo_pathTMP & "\VistaPrevia.jpg"
                cls_planos.cambiacolor_frame("NORMAL")

                v_codigo = dgdDetalle.Item(dgdDetalle.Row, "CODIGO") 'lodtbOracle.Rows(0).Item("CODIGO")
                v_nombre = dgdDetalle.Item(dgdDetalle.Row, "NOMBRE") ' lodtbOracle.Rows(0).Item("NOMBRE").ToString
                Me.cboZona.Text = dgdDetalle.Item(dgdDetalle.Row, "ZONA") 'lodtbOracle.Rows(0).Item("ZONA").ToString
                v_dato1 = dgdDetalle.Item(dgdDetalle.Row, "XMIN") 'lodtbOracle.Rows(0).Item("XMIN")
                v_dato3 = dgdDetalle.Item(dgdDetalle.Row, "XMAX") 'lodtbOracle.Rows(0).Item("XMAX")
                v_dato2 = dgdDetalle.Item(dgdDetalle.Row, "YMIN") 'lodtbOracle.Rows(0).Item("YMIN")
                v_dato4 = dgdDetalle.Item(dgdDetalle.Row, "YMAX") 'lodtbOracle.Rows(0).Item("YMAX")
                cls_Catastro.Actualizar_DM(Me.cboZona.Text)
                cls_Catastro.Borra_Todo_Feature("", m_Application)
                txtdato1.Visible = True : txtdato1.Enabled = True
                txtdato2.Visible = True : txtdato2.Enabled = True
                txtdato3.Visible = True : txtdato3.Enabled = True
                txtdato4.Visible = True : txtdato4.Enabled = True
                If v_dato1 = 0 Then
                    If v_dato1 = 0 Then txtdato1.Text = ""
                    If v_dato2 = 0 Then txtdato2.Text = ""
                    If v_dato3 = 0 Then txtdato3.Text = ""
                    If v_dato4 = 0 Then txtdato4.Text = ""
                Else
                    txtdato1.Text = v_dato1 : txtdato2.Text = v_dato2
                    txtdato3.Text = v_dato3 : txtdato4.Text = v_dato4
                    lbt_dato1.Visible = True : lbt_dato2.Visible = True
                    lbt_dato3.Visible = True : lbt_dato4.Visible = True
                    lbt_dato1.Text = "Este Min" : lbt_dato2.Text = "Norte Min"
                    lbt_dato3.Text = "Este Max" : lbt_dato4.Text = "Norte Max"
                End If
            Case Else
                If dgdDetalle.RowCount = 0 Then Exit Sub
                cls_Catastro.Borra_Todo_Feature("", m_Application)
                cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                cls_planos.cambiacolor_frame("PORCOLOR")
                visualizar_datos_dm(dgdDetalle.Item(dgdDetalle.Row, "CODIGO"), m_Application) 'AQUI CONSULTA A LA BD
                '  Exit Sub


                Try
                    Me.cboZona.Text = dgdDetalle.Item(dgdDetalle.Row, "ZONA")
                    glostrNaturaleza = dgdDetalle.Item(dgdDetalle.Row, "NATURALEZA")
                    'Me.txtArea.Text = dgdDetalle.Item(dgdDetalle.Row, "HECTAREA")
                    Me.txtArea.Text = Area_utm
                    Me.cboZona.Enabled = False
                    Me.lblVertice.Text = "El D.M.tiene " & Me.lstCoordenada.Items.Count - 2 & "  V�rtices"
                Catch ex As Exception
                End Try
        End Select

    End Sub

    Private Sub btnReporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReporte.Click
        Dim frm_Rpt As New rpt_Reporte_1 ' rptOrden_Detalle
        Dim lodtbItem As New DataTable
        Dim controw As Integer
        Dim lostrHaGIS As String = ""
        Dim m_ReportDefinitionFile As String

        lodtbItem = cls_Oracle.F_Item_Data_DM(v_codigo)
        controw = lodtbItem.Rows.Count
        'controw = 0     'para forzar

        If controw > 0 Then
            lostrHaGIS = lodtbItem.Rows(0).Item("HECTAGIS")
            frm_Rpt.C1Report1.DataSource.Recordset = New DataView(lodtbDatos)

            m_ReportDefinitionFile = glo_pathREP & "rpt_Reporte.xml"
            frm_Rpt.C1Report1.Load(m_ReportDefinitionFile, "Derecho_Minero")
            frm_Rpt.C1Report1.DataSource.Recordset = New DataView(lodtbDatos)
            frm_Rpt.C1Report1.Fields("IMA_DM").Picture = glo_pathTMP & "\VistaPrevia.jpg"
            frm_Rpt.C1Report1.Fields("IMA_DM1").Picture = glo_pathTMP & "\VistaPrevia_1.jpg"
            Select Case glostrNaturaleza
                Case "M"
                    frm_Rpt.C1Report1.Fields("NATURALEZA").Text = "Met�lico"
                Case "N"
                    frm_Rpt.C1Report1.Fields("NATURALEZA").Text = "No Met�lico"
            End Select
            frm_Rpt.C1Report1.Fields("HECTAREA_C").Text = lostrHaGIS
            'frm_Rpt.C1Report1.Fields("PARTIDA").Text = glostrPartida
            'frm_Rpt.C1Report1.Fields("PADRON").Text = glostrPadron
            'frm_Rpt.C1Report1.Fields("JEFATURA").Text = glostrJefatura
            'frm_Rpt.C1Report1.Fields("TIPO_DERECHO").Text = glostrTipoDer
            frm_Rpt.C1Report1.Fields("VERTICE").Text = lodtbDatos.Rows.Count
            frm_Rpt.Show()
            frm_Rpt.C1Report1.Render()
        Else
            lodtbItem = cls_Oracle.F_Item_Data_DM_AREA(v_codigo)
            lostrHaGIS = lodtbItem.Rows(0).Item("PE_HECFOR")
            frm_Rpt.C1Report1.DataSource.Recordset = New DataView(lodtbDatos)

            m_ReportDefinitionFile = glo_pathREP & "rpt_Reporte.xml"
            frm_Rpt.C1Report1.Load(m_ReportDefinitionFile, "Derecho_Minero")
            frm_Rpt.C1Report1.DataSource.Recordset = New DataView(lodtbDatos)
            frm_Rpt.C1Report1.Fields("IMA_DM").Picture = glo_pathTMP & "\VistaPrevia.jpg"
            frm_Rpt.C1Report1.Fields("IMA_DM1").Picture = glo_pathTMP & "\VistaPrevia_1.jpg"
            Select Case glostrNaturaleza
                Case "M"
                    frm_Rpt.C1Report1.Fields("NATURALEZA").Text = "Met�lico"
                Case "N"
                    frm_Rpt.C1Report1.Fields("NATURALEZA").Text = "No Met�lico"
            End Select
            frm_Rpt.C1Report1.Fields("HECTAREA_C").Text = lostrHaGIS
            'frm_Rpt.C1Report1.Fields("PARTIDA").Text = glostrPartida
            'frm_Rpt.C1Report1.Fields("PADRON").Text = glostrPadron
            'frm_Rpt.C1Report1.Fields("JEFATURA").Text = glostrJefatura
            'frm_Rpt.C1Report1.Fields("TIPO_DERECHO").Text = glostrTipoDer
            frm_Rpt.C1Report1.Fields("VERTICE").Text = lodtbDatos.Rows.Count
            frm_Rpt.Show()
            frm_Rpt.C1Report1.Render()
        End If
    End Sub

    Private Sub btnagregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnagregar.Click
        Select Case Me.cbo_tipo.SelectedValue
            Case "OP_12"
                Dim valor As String
                ' If Not IsNumeric(Me.txtNorte.Text) Or Len(Me.txtNorte.Text) <> 7 Then 'NORTE
                If Not IsNumeric(Me.txtNorte.Text) Then 'NORTE
                    MsgBox("Ingrese Correctamente la Coordenada Este", MsgBoxStyle.Critical, "Observaci�n...")
                    Me.txtNorte.Focus()
                    Exit Sub
                End If
                ' If Not IsNumeric(Me.txtEste.Text) Or Len(Me.txtEste.Text) <> 6 Then
                If Not IsNumeric(Me.txtEste.Text) Then
                    MsgBox("Ingrese Correctamente la Coordenada Norte", MsgBoxStyle.Critical, "Observaci�n...")
                    Me.txtEste.Focus()
                    Exit Sub
                End If
                valor = "Punto " & Me.lstCoordenada.Items.Count + 1 & ":  " & Val(txtEste.Text) & "; " & Val(txtNorte.Text)
                Me.lstCoordenada.Items.Add(valor)
                'If Me.lstCoordenada.Items.Count >= 3 Then Me.btnGraficar.Enabled = True
            Case Else
                If txtConsulta.Text = "" Or InStr(txtConsulta.Text, "-") = 0 Then Exit Sub
                If Me.lstCoordenada.Items.Count = 0 Then lo_Inicio_DM = 0
                p_Dato = Me.txtConsulta.Text.ToUpper
                Dim lostrParte_1 As String = RellenarComodin(Mid(p_Dato, 1, InStr(p_Dato, "-") - 1), 2, "0")
                Dim lostrParte_2 As String = Mid(p_Dato, InStr(p_Dato, "-"))
                p_Dato = lostrParte_1 & lostrParte_2
                Select Case lo_Inicio_DM
                    Case 0
                        'lodtbOracle = cls_Oracle.F_Obtiene_Carta("CODIGO", RellenarComodin(p_Dato, 4, "0"))
                        lodtbOracle = cls_Oracle.F_Obtiene_Carta_oficial("CODIGO", RellenarComodin(p_Dato, 4, "0"), v_sistema)
                        If lodtbOracle.Rows.Count = 0 Then
                            MsgBox(".. No Existe Hoja, " & p_Dato, MsgBoxStyle.Information, "BDGEOCATMIN")
                            DialogResult = Windows.Forms.DialogResult.OK : Exit Sub
                        End If
                        Try
                            gloEsteMin = lodtbOracle.Rows(0).Item("XMIN") * 1000
                            gloEsteMax = lodtbOracle.Rows(0).Item("XMAX") * 1000
                            gloNorteMin = lodtbOracle.Rows(0).Item("YMIN") * 1000
                            gloNorteMax = lodtbOracle.Rows(0).Item("YMAX") * 1000
                            lo_Zona_Carta = lodtbOracle.Rows(0).Item("ZONA")
                            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Carta, m_Application, "1", False)
                            cls_Catastro.Actualizar_DM(lodtbOracle.Rows(0).Item("ZONA"))
                        Catch ex As Exception
                            MsgBox("No Existe la Hoja " & p_Dato, MsgBoxStyle.Information, "[BDGEOCATMIN]")
                            Me.txtConsulta.Text = ""
                            Me.txtConsulta.Focus()
                            Exit Sub
                        End Try
                        Dim lo_Filtro As String = "hoja = '" & p_Dato.ToUpper & "'"
                        'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Carta, m_application, "1", False)
                        lo_Index_Carta = cls_Catastro.f_Intercepta_Carta("Cuadrangulo", CType(gloEsteMin, Double), CType(gloNorteMin, Double), CType(gloEsteMax, Double), CType(gloNorteMax, Double), lodtbOracle.Rows(0).Item("ZONA"), m_Application)
                        cls_Catastro.Quitar_Layer("Cuadrangulo", m_Application)
                        lo_Inicio_DM = 1
                        Me.lstCoordenada.Items.Add("   Carta " & lstCoordenada.Items.Count + 1 & ":   " & Me.txtConsulta.Text)
                        Me.txtConsulta.Text = "" : Me.txtConsulta.Focus()
                        Me.btnGraficar.Enabled = True
                        Me.clbLayer.Enabled = True
                        Me.dgdDetalle.DataSource = lodtbOracle
                        PT_Agregar_Funciones_Carta() : PT_Forma_Grilla_Carta()
                    Case 1
                        For i As Integer = 0 To Me.lstCoordenada.Items.Count - 1
                            Me.lstCoordenada.SelectedIndex = i
                            If RellenarComodin(Mid(lstCoordenada.Text.ToUpper, InStr(lstCoordenada.Text, ":") + 3), 4, "0") = p_Dato Then
                                MsgBox("Carta ha sido seleccionado ...", MsgBoxStyle.Information, "[BDGEOCATMIN]")
                                Me.txtConsulta.Text = "" : Me.txtConsulta.Focus()
                                Exit Sub
                            End If
                        Next
                        Dim loint_Encontre As Integer = InStr(lo_Index_Carta, p_Dato)
                        If loint_Encontre > 0 Then
                            Dim lodtbOracleTmp As New DataTable
                            'lodtbOracleTmp = cls_Oracle.F_Obtiene_Carta("CODIGO", RellenarComodin(p_Dato, 4, "0"))
                            lodtbOracleTmp = cls_Oracle.F_Obtiene_Carta_oficial("CODIGO", RellenarComodin(p_Dato, 4, "0"), v_sistema)
                            If lodtbOracleTmp.Rows.Count = 0 Then
                                MsgBox("No Existe la Hoja " & p_Dato, MsgBoxStyle.Information, "[BDGEOCATMIN]")
                                Exit Sub
                            End If
                            Me.lstCoordenada.Items.Add("   Carta " & lstCoordenada.Items.Count + 1 & ":   " & Me.txtConsulta.Text)
                            Dim lodtrDetalle As DataRow
                            lodtrDetalle = lodtbOracle.NewRow()
                            lodtrDetalle(0) = lodtbOracleTmp.Rows(0).Item("CODIGO")
                            lodtrDetalle(1) = lodtbOracleTmp.Rows(0).Item("NOMBRE")
                            lodtrDetalle(2) = lodtbOracleTmp.Rows(0).Item("ZONA")
                            lodtrDetalle(3) = lodtbOracleTmp.Rows(0).Item("XMIN")
                            lodtrDetalle(4) = lodtbOracleTmp.Rows(0).Item("YMIN")
                            lodtrDetalle(5) = lodtbOracleTmp.Rows(0).Item("XMAX")
                            lodtrDetalle(6) = lodtbOracleTmp.Rows(0).Item("YMAX")
                            lodtbOracle.Rows.Add(lodtrDetalle)
                            Me.dgdDetalle.DataSource = lodtbOracle
                            PT_Agregar_Funciones_Carta() : PT_Forma_Grilla_Carta()
                            Me.txtConsulta.Text = "" : Me.txtConsulta.Focus()
                        Else
                            MsgBox("Error la Carta: " & p_Dato & " No es Colindante .... ", MsgBoxStyle.Information, "[BDGEOCATMIN]")
                            Me.txtConsulta.Text = "" : Me.txtConsulta.Focus()
                            Exit Sub
                        End If
                End Select
        End Select
        If lstCoordenada.Items.Count >= 3 Then Me.btnGenera_Poligono.Enabled = True
        Me.cboZona.Enabled = True
    End Sub

    Private Sub btnElimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElimina.Click
        Dim lostrHoja As String = RellenarComodin(Mid(lstCoordenada.Text.ToUpper, InStr(lstCoordenada.Text, ":") + 3), 4, "0")
        If lstCoordenada.Items.Count = 0 Or Me.lstCoordenada.SelectedIndex = -1 Then Exit Sub
        Select Case Me.cbo_tipo.SelectedValue
            Case "OP_12"
                lstCoordenada.Items.RemoveAt(lstCoordenada.SelectedIndex)
            Case Else
                Dim loint_Mensaje As Integer = 0
                If lstCoordenada.Items.Count = 0 Or Me.lstCoordenada.SelectedIndex = -1 Then Exit Sub
                If lstCoordenada.SelectedIndex = 0 Then
                    loint_Mensaje = MsgBox("� Desea Eliminar Carta Principal de selecci�n de Colindantes ?", MsgBoxStyle.YesNo, "BDGEOCATMIN")
                    If loint_Mensaje = 6 Then
                        Me.lstCoordenada.Items.Clear()
                        lo_Inicio_DM = 0
                    End If
                Else
                    lstCoordenada.Items.RemoveAt(lstCoordenada.SelectedIndex)
                End If
        End Select

        For i As Integer = 0 To Me.dgdDetalle.RowCount - 1
            Select Case lo_Inicio_DM
                Case 0
                    dgdDetalle.Delete(i)
                Case Else
                    If dgdDetalle.Item(i, 0) = lostrHoja Then
                        dgdDetalle.Delete(i)
                    End If
            End Select
        Next
    End Sub

    Private Sub btnLimpia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpia.Click
        For i As Integer = 0 To Me.dgdDetalle.RowCount - 1
            dgdDetalle.Delete(i)
        Next
        lo_Inicio_DM = 0
        lstCoordenada.Items.Clear()
        Me.cboZona.Enabled = True
    End Sub

    Private Sub txtdato1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtdato1.KeyDown
        If e.KeyCode = 9 Then 'tab
            Me.txtdato2.Focus()
        End If
    End Sub

    Private Sub txtdato1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtdato1.KeyPress
        '        cls_Prueba.PT_validarNumeros(e, 3, 0, txtdato1)
        'Select Case tipo_seleccion
        '    Case "OP_1"
        '        cls_Prueba.PT_validarNumeros(e, 3, 0, txtdato1)
        '    Case "OP_3"
        '        cls_Prueba.PT_validarNumeros(e, 6, 0, txtdato1)
        '    Case "OP_9"
        '        cls_Prueba.PT_validarNumeros(e, 6, 0, txtdato1)
        '    Case Else
        'End Select
    End Sub


    Private Sub txtdato4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtdato4.KeyPress
        'cls_Prueba.PT_validarNumeros(e, 4, 0, txtdato4)
        'Select Case tipo_seleccion
        '    Case "OP_1"
        '        cls_Prueba.PT_validarNumeros(e, 4, 0, txtdato4)
        '    Case "OP_9"
        '        cls_Prueba.PT_validarNumeros(e, 7, 0, txtdato4)
        'End Select
    End Sub

    Private Sub txtdato2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtdato2.KeyDown
        If e.KeyCode = Windows.Forms.Keys.ShiftKey Then
            Me.txtdato1.Focus()
        ElseIf e.KeyCode = Windows.Forms.Keys.Tab Then
            Me.txtRadio.Focus()
        End If
    End Sub

    Private Sub txtdato2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtdato2.KeyPress
        'Select Case tipo_seleccion
        '    Case "OP_1"
        '        cls_Prueba.PT_validarNumeros(e, 4, 0, txtdato2)
        '    Case "OP_3"
        '        cls_Prueba.PT_validarNumeros(e, 7, 0, txtdato2)
        '    Case "OP_9"
        '        cls_Prueba.PT_validarNumeros(e, 7, 0, txtdato2)
        '    Case Else
        'End Select
    End Sub

    Private Sub txtdato3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtdato3.KeyPress
        'cls_Prueba.PT_validarNumeros(e, 3, 0, txtdato3)
        'Select Case tipo_seleccion
        '    Case "OP_1"
        '        cls_Prueba.PT_validarNumeros(e, 3, 0, txtdato3)
        '    Case "OP_9"
        '        cls_Prueba.PT_validarNumeros(e, 6, 0, txtdato3)
        '    Case Else
        'End Select
    End Sub

    Public Sub PT_Inicializar_Grilla_Ubigeo()
        Dim lodtbLista_DM As New DataTable
        'Dim dvwLista_DM As New DataView(lodtbLista_DM)
        lodtbLista_DM.Columns.Add("CODIGO", GetType(String))
        lodtbLista_DM.Columns.Add("DPTO", GetType(String))
        lodtbLista_DM.Columns.Add("PROV", GetType(String))
        lodtbLista_DM.Columns.Add("DIST", GetType(String))
        lodtbLista_DM.Columns.Add("CAP_DIST", GetType(String))
        lodtbLista_DM.Columns.Add("ZONA", GetType(String))
        lodtbLista_DM.Columns.Add("UBIGEO", GetType(String))
        PT_Estilo_Grilla_UBIGEO(lodtbLista_DM) : PT_Cargar_Grilla_UBIGEO(lodtbLista_DM)
        PT_Agregar_Funciones_UBIGEO() : PT_Forma_Grilla_Ubigeo()
    End Sub
    Private Sub PT_Estilo_Grilla_UBIGEO(ByRef padtbDetalle As DataTable)
        padtbDetalle.Columns.Item(Col_Codi).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Dpto).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Prov).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Dist).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Cap_Dist).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Zonau).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Ubigeo).DefaultValue = ""
    End Sub
    Public Sub Pinta_Grilla_Ubigeo()
        Me.dgdDetalle.BackColor = Color.FromArgb(242, 242, 240)
        Me.dgdDetalle.HeadingStyle.BackColor = Color.FromArgb(207, 209, 221) 'Color.Black CFD1DD
        Me.dgdDetalle.OddRowStyle.BackColor = Color.FromArgb(229, 232, 239) 'E5E8EF
        Me.dgdDetalle.EvenRowStyle.BackColor = Color.FromArgb(242, 242, 240)
    End Sub
    Private Sub PT_Agregar_Funciones_UBIGEO()
        Me.dgdDetalle.Columns(Col_Codi).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Dpto).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Prov).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Dist).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Cap_Dist).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Zonau).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Ubigeo).DefaultValue = ""
    End Sub
    Private Sub PT_Forma_Grilla_Ubigeo()
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codi).Width = 40
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Dpto).Width = 120
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Prov).Width = 120
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Dist).Width = 150
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Cap_Dist).Width = 100
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zonau).Width = 0
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Ubigeo).Width = 50
        Me.dgdDetalle.Columns("CODIGO").Caption = "C�digo"
        Me.dgdDetalle.Columns("DPTO").Caption = "Departamento"
        Me.dgdDetalle.Columns("PROV").Caption = "Provincia"
        Me.dgdDetalle.Columns("DIST").Caption = "Distrito"
        Me.dgdDetalle.Columns("CAP_DIST").Caption = "Capital Distrito"
        Me.dgdDetalle.Columns("ZONA").Caption = "Zona"
        Me.dgdDetalle.Columns("UBIGEO").Caption = "Ubigeo"
        'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nombre).Locked = True
        'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codigo).Locked = True
        'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Tipo).Locked = True
        'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Carta).Locked = True
        'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Situacion).Locked = True
        'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Titular).Locked = True
        'Me.dgdDetalle.Splits(0).DisplayColumns("COD_SERVICIO").AllowSizing = True
        'Me.dgdDetalle.Splits(0).DisplayColumns("COD_SERVICIO").Style.WrapText = True
        'Me.dgdDetalle.Splits(0).DisplayColumns("DESCRIPCION").AllowSizing = True
        'Me.dgdDetalle.Splits(0).DisplayColumns("DESCRIPCION").Style.WrapText = True
        'Me.dgdDetalle.Splits(0).DisplayColumns("OBSERVACION").AllowSizing = True
        'Me.dgdDetalle.Splits(0).DisplayColumns("OBSERVACION").Style.WrapText = True
        'Me.dgd_Orden_Trabajo.Splits(0).AllowRowSizing = RowSizingEnum.AllRows
        'Me.dgd_Orden_Trabajo.AllowRowSizing = RowSizingEnum.IndividualRows
        'Me.dgd_Orden_Trabajo.Splits(0).Rows(0).Height = 50
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codi).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        'Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codi).HeadingStyle.ForeColor = Color.Red
        Me.dgdDetalle.Splits(0).HeadingStyle.ForeColor = Color.Black
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Dpto).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Prov).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Dist).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Cap_Dist).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zonau).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Ubigeo).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center

        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Codi).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Dpto).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Prov).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Dist).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Cap_Dist).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zonau).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Ubigeo).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
    End Sub

    Private Sub cboZona_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboZona.SelectedIndexChanged
        Select Case Me.cbo_tipo.SelectedValue
            Case "OP_12"
                btnGraficar.Enabled = False
            Case Else
                If Me.cboZona.SelectedIndex <> 0 Then
                    btnGraficar.Enabled = True
                Else
                    btnGraficar.Enabled = False
                End If
        End Select
    End Sub
    Public Sub Activar_Capas_DM(ByVal p_Filtro_Capital As String, ByVal p_clbLayer As Object, ByVal p_Zona As String)
        Try
            ''para capas 84 sin utilizar tabla sg_M_descripcion

            'gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
            'gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_18"
            'gstrFC_Provincia = "GPO_PRO_PROVINCIA_18"
            'gstrFC_Distrito = "GPO_DIS_DISTRITO_18"
            'gstrFC_Rios = "GLI_RIO_RIOS_18"
            'gstrFC_Carretera = "GLI_VIA_VIAS_18"
            'gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
            'gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
            'gstrFC_Cuadricula = ""
            'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
            'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
            'gstrFC_ZUrbana56 = ""
            'gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
            'gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
            'gstrFC_Frontera = "GLI_FRO_FONTERA_18"
            ' gstrFC_LHojas = "GPO_LIM_HOJAS"
            'gstrFC_Carta = "GPO_HOJ_HOJAS"

            Dim cls_eval As New Cls_evaluacion
            Dim lo_Filtro_Dpto As String = ""
            Dim lo_Filtro_Dpto_mod As String = ""
            Dim lo_Filtro_Dpto2 As String = ""


            '**************************************************************
            Dim glo_xMin_1, glo_yMin_1, glo_xMax_1, glo_yMax_1 As Double
            glo_xMin_1 = glo_xMin : glo_xMax_1 = glo_xMax
            glo_yMin_1 = glo_yMin : glo_yMax_1 = glo_yMax
            Dim v_filtro As Integer

            If p_clbLayer.CheckedItems.Count <= 0 Then Exit Sub

            If glo_xMin <> 0 And glo_yMin <> 0 And glo_xMax <> 0 And glo_yMax <> 0 Then
                If tipo_seleccion = "OP_11" Or tipo_seleccion = "OP_2" Or tipo_seleccion = "OP_16" Or tipo_seleccion = "OP_18" Or tipo_seleccion = "OP_20" Then
                    lista_depa = lista_depa
                    'A�adir capa de paises si esta en Mar
                    'Dim busca_filtro_MAR As Integer = InStr(lista_depa, "MAR")
                    If busca_filtro_MAR > 0 Or distancia_fron <= 50 Then
                        'If lista_depa = "SAN MARTIN" Then
                        'ElseIf lista_depa = "CAJAMARCA" Then
                        'Else
                        cls_eval.AddLayerFromFile(m_Application)
                        cls_Catastro.Ordenacapa_vista("Catastro")
                        'End If
                    End If

                Else



                    'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_Z & p_Zona, m_Application, "2", False)

                    If v_sistema = "PSAD-56" Then
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_Z & p_Zona, m_Application, "2", False)
                    Else
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_WGS & p_Zona, m_Application, "2", False)
                    End If


                    lo_Filtro_Dpto = cls_eval.f_Intercepta_temas("Departamento", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                    lo_Filtro_Dpto_mod = lo_Filtro_Dpto
                    v_filtro = InStr(lo_Filtro_Dpto, "99")
                    If v_filtro > 0 Then  'valida <> 99
                        Dim nm_depa As String = ""
                        Dim nm_depa1 As String = ""

                        If colecciones_depa.Count = 0 Then
                            lista_depa = ""

                        End If
                        For contador As Integer = 1 To colecciones_depa.Count
                            nm_depa = colecciones_depa.Item(contador)
                            If contador = 1 Then
                                lista_depa = nm_depa
                                lista_depa_mod = nm_depa
                                lista_nm_depa = "NM_DEPA =  '" & nm_depa & "'"

                                nm_depa1 = nm_depa

                            ElseIf contador > 1 Then
                                If nm_depa <> nm_depa1 Then
                                    lista_depa = lista_depa & "," & nm_depa
                                    lista_nm_depa = lista_nm_depa & " OR " & "NM_DEPA =  '" & nm_depa & "'"
                                    nm_depa1 = nm_depa
                                End If
                            End If
                        Next contador
                    End If
                    'A�adir capa de paises si esta en Mar
                    Dim busca_filtro As Integer = InStr(lista_nm_depa, "MAR")
                    If busca_filtro > 0 Then
                        cls_eval.AddLayerFromFile(m_Application)
                        cls_Catastro.Ordenacapa_vista("Catastro")
                    End If
                End If

                'chequear en simulado edicion no terminada

                'Capa hojas

                'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_LHojas, m_Application, "2", False)

                'antes estab de poner wgs84 y psad 56 fecha 13-10-2015
                'cls_Catastro.PT_CargarFeatureClass_SDE("GPO_HOJ_HOJAS", m_Application, "2", False)
                cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Carta, m_Application, "2", False)

                If p_Filtro_Capital = "1" Then
                    lista_nmhojas = v_codigo
                Else

                    'Dim lo_Filtro_Dpto2 As String = cls_eval.f_Intercepta_temas("Cuadrangulo", v_este_min, v_norte_min, v_este_max, v_norte_max, pApp, loStrShapefile)
                    If tipo_seleccion = "OP_11" Or tipo_seleccion = "OP_2" Or tipo_seleccion = "OP_16" Or tipo_seleccion = "OP_18" Or tipo_seleccion = "OP_20" Then
                        lista_cartas = lista_cartas  'el mismo
                    Else  'Aplica a otra opcion
                        lo_Filtro_Dpto2 = cls_eval.f_Intercepta_temas("Cuadrangulo", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                        lista_hojas = lo_Filtro_Dpto2
                        Dim cd_carta As String
                        For contador As Integer = 1 To colecciones.Count
                            cd_carta = colecciones.Item(contador)
                            If contador = 1 Then
                                lista_cartas = cd_carta
                            ElseIf contador > 1 Then
                                lista_cartas = lista_cartas & "," & cd_carta
                            End If
                        Next contador

                        For contador As Integer = 1 To colecciones_nmhojas.Count
                            cd_carta = colecciones_nmhojas.Item(contador)
                            If contador = 1 Then
                                lista_nmhojas = cd_carta
                            ElseIf contador > 1 Then
                                lista_nmhojas = lista_nmhojas & "," & cd_carta
                            End If
                        Next contador
                        colecciones_nmhojas.Clear()
                        colecciones.Clear()
                    End If
                End If


            Else
                ' MsgBox("La variable glo_xMin es NUll", MsgBoxStyle.Information, "[BDGeocatmin]")
                Exit Sub
            End If
            Dim lo_Filtro_Area_Reserva As String

            For i As Integer = 0 To p_clbLayer.CheckedItems.Count - 1

                Select Case p_clbLayer.CheckedItems.Item(i) '.SelectedItem '.GetItemText(i) clbLayer.CheckedItems.Item(1)

                    Case "Zona Reservada"


                        If tipo_opcion = "2" Or tipo_opcion = "3" Then  'Solo si es consulta por Areas Restringida

                            If cod_opcion_Rese = "AN" Then  'Solo capas Reservas se agrega ya no capas Urbanas
                                'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada56, m_Application, "1", False)
                                cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & p_Zona, m_Application, "1", False)  'se comento
                                ' cls_Catastro.Poligono_Color_GDB("GPO_ARE_AREA_RESERVADA_" & v_zona_dm, glo_pathStyle & "\AREA_RESERVA_" & v_zona_dm & ".style", "CODIGO", "", "Cadena", "", m_Application, "")
                                'Dim lo_Filtro_Area_Reserva As String = cls_Catastro.f_Intercepta_FC("Zona Reservada", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)

                                'If lo_Filtro_Area_Reserva = "" Then
                                'cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                                'Else
                                'cls_Catastro.DefinitionExpression(lo_Filtro_Area_Reserva, m_Application, "Zona Reservada")
                                If v_valida_canti_rese = "1" Then
                                    cls_Catastro.DefinitionExpression(cadena_query_ar_corre, m_Application, "Zona Reservada")  'cuando clase tiene muchos espacios vacioes
                                Else
                                    cls_Catastro.DefinitionExpression(cadena_query_ar, m_Application, "Zona Reservada")
                                End If

                                cls_Catastro.Expor_Tema("Zona Reservada", True, m_Application)
                                cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                                cls_Catastro.Add_ShapeFile1("Zona Reservada" & fecha_archi, m_Application, "Zona Reservada")
                                ' cls_Catastro.Shade_Poligono("Zona Reservada", m_Application)
                                cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Reservada")

                                'If tipo_seleccion = "OP_4" Then
                                'cls_Catastro.DefinitionExpression(cadena_query_ar, m_Application, "Zona Reservada")
                                'End If


                                'cls_Catastro.ShowLabel_DM("Zona Reservada", m_Application)

                                cls_Catastro.rotulatexto_dm("Zona Reservada", m_Application)
                                'End If
                                cod_opcion_Rese = ""
                            End If


                            Else
                                '  If tipo_opcion <> "2" Then 'Para Cualquier tipo de consulta

                                'cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ARE_AREA_RESERVADA_WGS_" & v_zona_dm, m_Application, "1", False)
                                'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & v_zona_dm, m_Application, "1", False)
                                '  cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & p_Zona, m_Application, "1", False)

                                If tipo_seleccion = "OP_7" Or tipo_seleccion = "OP_10" Or tipo_seleccion = "OP_8" Then
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & p_Zona, m_Application, "1", False)
                                    lo_Filtro_Area_Reserva = "1"
                                    cls_Prueba.Intersect_Dema_AreaRestringida(m_Application, lo_Filtro_Ubigeo, "", lo_layer, xMin, yMin, xMax, yMax, txtExiste)
                                Else
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & p_Zona, m_Application, "2", False)    '''''   ojo validar caso de varias cartas: variable:lo_Zona_Carta
                                    lo_Filtro_Area_Reserva = cls_Catastro.f_Intercepta_FC("Zona Reservada", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                                    cantidad_reg_areas = 1
                                End If

                                ' Exit Sub

                                '   Dim lo_Filtro_Area_Reserva As String = cls_Catastro.f_Intercepta_FC("Zona Reservada", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)

                                ' cls_Catastro.PT_CargarFeatureClass_SDE("GPO_CAR_CARAM_18", m_Application, "1", True)
                                ' Dim lo_Filtro_Area_Reserva As String = cls_Catastro.f_Intercepta_FC("Zona Reservada", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                                '  Dim lo_Filtro_Area_Reserva As String
                                If cantidad_reg_areas = 0 Then
                                    ' If lo_Filtro_Area_Reserva = "" Then
                                    cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                                ElseIf lo_Filtro_Area_Reserva = "" Then
                                    cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                                Else
                                    cls_Catastro.Expor_Tema("Zona Reservada", True, m_Application)

                                    ' cls_Catastro.Export_data_select("Zona Reservada", "X1", m_Application)
                                    '   cls_Catastro.Expor_Tema("Zona Reservada", True, m_Application)
                                    ' cls_Catastro.Exportando_Temas("Zona Reservada", "Zona Reservada", m_Application)
                                    cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                                    cls_Catastro.Expor_Tema("Area_Natural", True, m_Application)
                                    Try
                                        Kill(glo_pathTMP & "\*.sbn")
                                        Kill(glo_pathTMP & "\*.sbx")

                                    Catch ex As Exception
                                    End Try
                                    cls_Catastro.Add_ShapeFile1("Zona Reservada" & fecha_archi, m_Application, "Zona Reservada")
                                    cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Reservada")

                                    cls_Catastro.rotulatexto_dm("Zona Reservada", m_Application)

                                End If
                                'ElseIf tipo_opcion = "2" Then  'Solo si es consulta por Areas Restringida

                                'If cod_opcion_Rese = "AN" Then  'Solo capas Reservas se agrega ya no capas Urbanas
                                '    'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada56, m_Application, "1", False)
                                '    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & p_Zona, m_Application, "1", False)  'se comento
                                '    ' cls_Catastro.Poligono_Color_GDB("GPO_ARE_AREA_RESERVADA_" & v_zona_dm, glo_pathStyle & "\AREA_RESERVA_" & v_zona_dm & ".style", "CODIGO", "", "Cadena", "", m_Application, "")
                                '    'Dim lo_Filtro_Area_Reserva As String = cls_Catastro.f_Intercepta_FC("Zona Reservada", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)

                                '    'If lo_Filtro_Area_Reserva = "" Then
                                '    'cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                                '    'Else
                                '    'cls_Catastro.DefinitionExpression(lo_Filtro_Area_Reserva, m_Application, "Zona Reservada")
                                '    cls_Catastro.DefinitionExpression(cadena_query_ar, m_Application, "Zona Reservada")
                                '    cls_Catastro.Expor_Tema("Zona Reservada", True, m_Application)
                                '    cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                                '    cls_Catastro.Add_ShapeFile1("Zona Reservada" & fecha_archi, m_Application, "Zona Reservada")
                                '    ' cls_Catastro.Shade_Poligono("Zona Reservada", m_Application)
                                '    cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Reservada")

                                '    'If tipo_seleccion = "OP_4" Then
                                '    'cls_Catastro.DefinitionExpression(cadena_query_ar, m_Application, "Zona Reservada")
                                '    'End If


                                '    'cls_Catastro.ShowLabel_DM("Zona Reservada", m_Application)

                                '    cls_Catastro.rotulatexto_dm("Zona Reservada", m_Application)
                                '    'End If
                                '    cod_opcion_Rese = ""
                                'End If
                            End If

                    Case "Area Urbana"
                            'If p_clbLayer.GetItemChecked(i) = True Then



                            If tipo_opcion = "2" Then  'Solo si es consulta por Dm x Areas Restringida
                                If cod_opcion_Rese = "ZU" Then  'Solo capas Urbanas se agrega, ya no capa Reservas
                                    'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & p_Zona, m_Application, "1", False)
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & p_Zona, m_Application, "1", False)
                                    'cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_WGS_" & v_zona_dm, m_Application, "1", False)
                                    Dim lo_Filtro_Zona_Urbana As String = cls_Catastro.f_Intercepta_FC("Zona Urbana", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                                    If lo_Filtro_Zona_Urbana = "" Then
                                        cls_Catastro.Quitar_Layer("Zona Urbana", m_Application)
                                    Else
                                        cls_Catastro.DefinitionExpression(lo_Filtro_Zona_Urbana, m_Application, "Zona Urbana")
                                        cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Urbana")
                                        cls_Catastro.rotulatexto_dm("Zona Urbana", m_Application)
                                    End If
                                End If
                                cod_opcion_Rese = ""


                            ElseIf tipo_opcion = "3" Then  'esto es para consulta de planos de areas restringidas
                                If cod_opcion_Rese = "ZU" Then  'Solo capas Urbanas se agrega, ya no capa Reservas

                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & p_Zona, m_Application, "1", False)
                                If v_valida_canti_rese = "1" Then
                                    cls_Catastro.DefinitionExpression(cadena_query_ar_corre, m_Application, "Zona Urbana")
                                Else

                                    cls_Catastro.DefinitionExpression(cadena_query_ar, m_Application, "Zona Urbana")
                                End If

                                cls_Catastro.Expor_Tema("Zona Urbana", True, m_Application)
                                cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Urbana")
                                cls_Catastro.rotulatexto_dm("Zona Urbana", m_Application)
                                cls_Catastro.Zoom_to_Layer("Zona Urbana")


                            End If
                                cod_opcion_Rese = ""
                            Else

                                ' If tipo_opcion <> "2" Then 'antes estaba Para Cualquier tipo de consulta
                                cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & p_Zona, m_Application, "1", False)
                                'cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_WGS_" & v_zona_dm, m_Application, "1", False)
                                Dim lo_Filtro_Zona_Urbana As String = cls_Catastro.f_Intercepta_FC("Zona Urbana", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                                If lo_Filtro_Zona_Urbana = "" Then
                                    cls_Catastro.Quitar_Layer("Zona Urbana", m_Application)
                                Else
                                    cls_Catastro.DefinitionExpression(lo_Filtro_Zona_Urbana, m_Application, "Zona Urbana")
                                    cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Urbana")
                                    cls_Catastro.rotulatexto_dm("Zona Urbana", m_Application)
                                End If



                                cod_opcion_Rese = ""


                            End If

                    Case "Capitales Distritales"
                            'If p_clbLayer.GetItemChecked(i) = True Then
                            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_CDistrito, m_Application, "1", False)
                            Dim lo_Filtro_cp As String = cls_Catastro.f_Intercepta_FC("Capitales Distritales", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                            If lo_Filtro_cp <> "" Then
                                cls_Catastro.DefinitionExpression(lo_Filtro_cp, m_Application, "Capitales Distritales")
                                'cls_Catastro.Shade_Poligono("Capitales Distritales", m_Application)
                                cls_Catastro.Color_Poligono_Simple(m_Application, "Capitales Distritales")
                            Else
                                cls_Catastro.Quitar_Layer("Capitales Distritales", m_Application)
                            End If
                            'End If
                    Case "Limite Departamental"
                            If v_filtro > 0 Then  'valida <> 99
                                'antes 13-102015
                                '  cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento, m_Application, "1", False)
                                'hoy 13-10-2015
                                If v_sistema = "PSAD-56" Then
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_Z & p_Zona, m_Application, "1", False)
                                Else
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_WGS & p_Zona, m_Application, "1", False)
                                End If


                                cls_Catastro.DefinitionExpression(lista_nm_depa, m_Application, "Departamento")
                            Else
                                ' cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento, m_Application, "1", False)
                                If v_sistema = "PSAD-56" Then
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_Z & p_Zona, m_Application, "1", False)
                                Else
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_WGS & p_Zona, m_Application, "1", False)
                                End If

                                cls_Catastro.DefinitionExpression(lo_Filtro_Dpto, m_Application, "Departamento")
                            End If
                            ' cls_Catastro.Shade_Poligono("Departamento", m_Application)
                            arch_cata = "Departamento"
                            cls_Catastro.Color_Poligono_Simple(m_Application, "Departamento")
                            arch_cata = ""
                            pFeatureLayer = pMap.Layer(0)
                            pFeatureLayer.ShowTips = True
                    Case "Limite Provincial"
                            If v_filtro > 0 Then  'valida <> 99
                                If v_sistema = "PSAD-56" Then
                                    '  cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia_Z & p_Zona, m_Application, "1", False)
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia_Z & p_Zona, m_Application, "1", False)
                                Else
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia_WGS & p_Zona, m_Application, "1", False)
                                End If

                                cls_Catastro.DefinitionExpression(lista_nm_depa, m_Application, "Provincia")
                            Else
                                '  cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia_Z & p_Zona, m_Application, "1", False)
                                If v_sistema = "PSAD-56" Then
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia_Z & p_Zona, m_Application, "1", False)
                                Else
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia_WGS & p_Zona, m_Application, "1", False)

                                End If
                                cls_Catastro.DefinitionExpression(lo_Filtro_Dpto, m_Application, "Provincia")
                            End If
                            ' cls_Catastro.Shade_Poligono("Provincia", m_Application)
                            arch_cata = "Provincia"
                            cls_Catastro.Color_Poligono_Simple(m_Application, "Provincia")
                            arch_cata = ""
                            pFeatureLayer = pMap.Layer(0)
                            pFeatureLayer.ShowTips = True
                    Case "Limite Distrital"
                            If v_filtro > 0 Then  'valida <> 99
                                'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito_Z & p_Zona, m_Application, "1", False)
                                If v_sistema = "PSAD-56" Then
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito_Z & p_Zona, m_Application, "1", False)
                                Else
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito_WGS & p_Zona, m_Application, "1", False)
                                End If
                                cls_Catastro.DefinitionExpression(lista_nm_depa, m_Application, "Distrito")
                            Else
                                ' cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito_Z & p_Zona, m_Application, "1", False)
                                If v_sistema = "PSAD-56" Then
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito_Z & p_Zona, m_Application, "1", False)
                                Else
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito_WGS & p_Zona, m_Application, "1", False)
                                End If
                                cls_Catastro.DefinitionExpression(lo_Filtro_Dpto, m_Application, "Distrito")
                            End If
                            ' cls_Catastro.Shade_Poligono("Distrito", m_Application)
                            arch_cata = "Distrito"
                            cls_Catastro.Color_Poligono_Simple(m_Application, "Distrito")
                            pFeatureLayer = pMap.Layer(0)
                            pFeatureLayer.ShowTips = True
                            arch_cata = ""
                    Case "Red Hidrografica"
                            If v_filtro = 0 Then  'valida <> 99
                                cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Rios, m_Application, "1", False)
                                cls_Catastro.DefinitionExpression(lo_Filtro_Dpto_mod, m_Application, "Drenaje")
                                cls_Catastro.UniqueSymbols(m_Application, "Drenaje")
                            End If

                    Case "Red Vial"
                            If v_filtro = 0 Then  'valida <> 99
                                cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Carretera, m_Application, "1", False)
                                cls_Catastro.DefinitionExpression(lo_Filtro_Dpto, m_Application, "Vias")
                                cls_Catastro.Shade_Poligono("Vias", m_Application)
                            End If
                    Case "Centros Poblados"
                            'If p_clbLayer.GetItemChecked(i) = True Then
                            If v_filtro = 0 Then  'valida <> 99)
                                cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_CPoblado, m_Application, "1", False)
                                cls_Catastro.DefinitionExpression(lo_Filtro_Dpto_mod, m_Application, "Centro Poblado")
                                cls_Catastro.UniqueSymbols(m_Application, "Centro Poblado")
                                'cls_Catastro.Style_Linea_GDB(glo_Owner_Layer_SDE & "GPT_GPO_CENTRO_POBLADOS", glo_pathStyle & "\PUEBLO.style", "DEPARTA", m_application, "SDE", "Nombre")
                            End If
                End Select
            Next i

            If tipo_seleccion = "OP_5" Or tipo_seleccion = "OP_6" Or tipo_seleccion = "OP_12" Or tipo_seleccion = "OP_11" Or tipo_seleccion = "OP_2" Or tipo_seleccion = "OP_18" Or tipo_seleccion = "OP_20" Then

            Else

                'comentado porque se pidio no agregar capa de certificacion ambiental, se comento el 21-12-15

                'cls_Catastro.PT_CargarFeatureClass_SDE("GPT_CAM_CERTIFICACION_AMB_G56", m_Application, "1", False)
                '' cls_Catastro.Poligono_Color_GDBp("GPT_CAM_CERTIFICACION_AMB_G56", glo_pathStyle & "\Certificacion.style", "TC_ESTCER", "", "Cadena", "", m_application, "")
                'Dim lo_Filtro_certificacion As String = cls_Catastro.f_Intercepta_FC("Certificacion Ambiental", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                'If canti_capa_certi > 0 Then
                '    cls_Catastro.DefinitionExpression(lo_Filtro_certificacion, m_Application, "Certificacion Ambiental")
                '    Dim tipocerti As New DataTable
                '    tipocerti = cls_Catastro.f_Genera_Leyenda_DM("Certificacion Ambiental", m_Application)
                '    cls_Catastro.Poligono_Color_GDB3("Certificacion Ambiental", tipocerti, glo_pathStyle & "\Certificacion.style", "TC_ESTCER", "", m_Application)

                '    cls_eval.consultacapaDM("", "Certificacion", "Certificacion Ambiental")
                '    pFeatureLayer = pMap.Layer(0)
                '    pFeatureSelection = pFeatureLayer
                '    pSelectionSet = pFeatureSelection.SelectionSet
                '    If pFeatureLayer.Name = "Certificacion Ambiental" Then
                '        pFeatureLayer = pFeatureSelection
                '        pFeatureClass = pFeatureLayer.FeatureClass
                '        Dim v_explo_v_expta As Integer
                '        Dim v_explo As Integer
                '        Dim v_expta As Integer
                '        Dim v_cierre As Integer
                '        Dim v_tipo As String
                '        v_explo_v_expta = 0
                '        v_explo = 0
                '        v_expta = 0
                '        v_cierre = 0
                '        If pSelectionSet.Count > 0 Then

                '            'If pFeatureLayer.FeatureClass.FeatureCount(Nothing) > 0 Then
                '            'pFeatureCursor = pFeatureClass.Search(Nothing, True)
                '            pSelectionSet.Search(Nothing, True, pFeatureCursor)
                '            pFields = pFeatureClass.Fields
                '            Dim v_area_f As Double = 0
                '            pFeature = pFeatureCursor.NextFeature
                '            Do Until pFeature Is Nothing
                '                v_tipo = pFeature.Value(pFields.FindField("TC_ESTCER"))
                '                If v_tipo = "Exploraci�n o Explotaci�n" Then
                '                    v_explo_v_expta = v_explo_v_expta + 1
                '                ElseIf v_tipo = "Exploraci�n" Then
                '                    v_explo = v_explo + 1
                '                ElseIf v_tipo = "Explotaci�n" Then
                '                    v_expta = v_expta + 1
                '                ElseIf v_tipo = "Cierre" Then
                '                    v_cierre = v_cierre + 1
                '                End If

                '                pFeature = pFeatureCursor.NextFeature
                '            Loop
                '        End If
                '        v_cierre1 = v_cierre
                '        v_expta1 = v_expta
                '        v_explo_v_expta1 = v_explo_v_expta
                '        v_explo1 = v_explo

                '        pFeatureSelection.Clear()           'linea agregada para limpiar la seleccion
                '    End If
                'Else
                '    ' cls_Catastro.Quitar_Layer("Certificacion Ambiental", m_Application)
                'End If

                'Agregando capa de uso minero y actvidad minera
                '  cls_Catastro.PT_CargarFeatureClass_SDE("GPO_AUM_AREA_USO_MINERO_G56", m_Application, "1", False)



                ' cls_Catastro.Poligono_Color_GDB("GPO_MEM_USO_MINERO", glo_pathStyle & "\Uso_Minero.style", "ANOPRO", "", "Cadena", "", m_Application, "")
                'comentado el 16-15-2015
                'Dim lo_Filtro_Area_Uso_Minero As String = cls_Catastro.f_Intercepta_FC("DM_Uso_Minero", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                'If canti_capa_usomin > 0 Then
                '    cls_Catastro.DefinitionExpression(lo_Filtro_Area_Uso_Minero, m_Application, "DM_Uso_Minero")

                '    arch_cata = "DM_Uso_Minero"

                '    cls_Catastro.Color_Poligono_Simple(m_Application, "DM_Uso_Minero")
                '    arch_cata = ""
                'Else
                '    cls_Catastro.Quitar_Layer("DM_Uso_Minero", m_Application)
                'End If
                'comentado el 16-15-2015

                'cls_Catastro.PT_CargarFeatureClass_SDE("GPO_AAC_AREA_ACT_MINERA_G56", m_Application, "1", False)

                'Dim lo_Filtro_Area_act_Minera As String = cls_Catastro.f_Intercepta_FC("DM_Actividad_Minera", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                'If canti_capa_actmin > 0 Then
                '    cls_Catastro.DefinitionExpression(lo_Filtro_Area_act_Minera, m_Application, "DM_Actividad_Minera")
                '    arch_cata = "DM_Actividad_Minera"
                '    cls_Catastro.Color_Poligono_Simple(m_Application, "DM_Actividad_Minera")
                '    arch_cata = ""
                'Else
                '    cls_Catastro.Quitar_Layer("DM_Actividad_Minera", m_Application)
                'End If
            End If


            If cd_region_sele <> "00" Then   'PARA REGIONALES

                'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia, m_Application, "1", False)
                If valida_region_dema <> "" Then  '


                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia_Z & p_Zona, m_Application, "1", False)
                    cls_Catastro.DefinitionExpression(lista_prov_colin, m_Application, "Prov_Colindantes")

                    cls_Catastro.Color_Poligono_Simple(m_Application, "Prov_Colindantes")


                    '  cls_Catastro.f_Intercepta_FC("Prov_Colindantes", v_este_min, v_norte_min, v_este_max, v_norte_max, m_Application)


                    'Exit Sub

                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Cuadricula, m_Application, "1", False)
                    cls_Catastro.DefinitionExpression(lista_region_sele, m_Application, "Cuadricula Regional")
                    cls_Catastro.Color_Poligono_Simple(m_Application, "Cuadricula Regional")


                    If canti_capa_provcol = 0 Then
                        'Exit Sub
                        '
                        cls_Catastro.Borra_Todo_Feature("", m_Application)
                        cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                        caso_consulta = "CATASTRO MINERO"
                        If pMap.Name <> "CATASTRO MINERO" Then
                            'cls_eval.activadataframe("CATASTRO MINERO")
                            cls_planos.buscaadataframe(caso_consulta, False)
                            If valida = False Then
                                pMap.Name = "CATASTRO MINERO"
                                pMxDoc.UpdateContents()
                            End If
                            cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                            pMxDoc.UpdateContents()
                        End If
                        cls_eval.Eliminadataframe(caso_consulta)
                        ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
                        'cls_eval.Eliminadataframe()
                        cls_planos.buscaadataframe("CATASTRO MINERO", False)
                        If valida = False Then
                            pMap.Name = "CATASTRO MINERO"
                        End If
                        pMxDoc.UpdateContents()
                        MsgBox("El DM :" & v_codigo & " no se encuentra ni en la provincia colindante de su Region,por lo tanto no graficara...", MsgBoxStyle.Critical, "BDGEOCATMIN")

                        Exit Sub

                    Else
                        If valida_region_dema = "" Then  'CUANDO ESTA PROV COLINDANTES
                            cls_Catastro.Borra_Todo_Feature("", m_Application)
                            cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                            caso_consulta = "CATASTRO MINERO"
                            If pMap.Name <> "CATASTRO MINERO" Then
                                'cls_eval.activadataframe("CATASTRO MINERO")
                                cls_planos.buscaadataframe(caso_consulta, False)
                                If valida = False Then
                                    pMap.Name = "CATASTRO MINERO"
                                    pMxDoc.UpdateContents()
                                End If
                                cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                                pMxDoc.UpdateContents()
                                cls_eval.ActivaDataframe_Opcion("DEMARCACION POLITICA", m_Application)
                            End If
                        End If

                    End If


                ElseIf valida_region_dema = "" Then  'CUANDO ESTA PROV COLINDANTES
                    cls_Catastro.Borra_Todo_Feature("", m_Application)
                    cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
                    caso_consulta = "CATASTRO MINERO"
                    If pMap.Name <> "CATASTRO MINERO" Then
                        'cls_eval.activadataframe("CATASTRO MINERO")
                        cls_planos.buscaadataframe(caso_consulta, False)
                        If valida = False Then
                            pMap.Name = "CATASTRO MINERO"
                            pMxDoc.UpdateContents()
                        End If
                        cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                        pMxDoc.UpdateContents()
                        cls_eval.ActivaDataframe_Opcion("DEMARCACION POLITICA", m_Application)
                    End If
                    pMxDoc.UpdateContents()
                    caso_consulta = "DEMARCACION POLITICA"
                    cls_eval.ActivaDataframe_Opcion("DEMARCACION POLITICA", m_Application)
                    pMxDoc.UpdateContents()
                End If
            End If

            Select Case Me.cbo_tipo.SelectedValue 'tipo_seleccion
                Case "OP_20", "OP_28"

                Case Else
                    cls_eval.AddLayerFromFile_acceditario(m_Application, p_Zona)

            End Select

            'If (cbo_tipo.SelectedValue <> "OP_20" Or cbo_tipo.SelectedValue <> "OP_28") Then
            '    cls_eval.AddLayerFromFile_acceditario(m_Application, p_Zona)
            'End If

            'cls_eval.AddLayerFromFile_acceditario(m_Application, p_Zona)

            ' Exit Sub
            'Empieza COMENTADO ZONA DE FRONTERa
            'Frontera
            Me.lstCoordenada.Items.Clear()

            'comentado porque no aplica a hacer por zona
            'Select Case p_Zona
            '    Case 17
            '        pPoint = New ESRI.ArcGIS.Geometry.Point
            '        pPoint.X = glo_xMin : pPoint.Y = glo_yMin
            '        pPoint.SpatialReference = Datum_PSAD_17 : pPoint.Project(Datum_PSAD_18)
            '        xMin.Text = pPoint.X : yMin.Text = pPoint.Y
            '        pPoint = New ESRI.ArcGIS.Geometry.Point
            '        pPoint.X = glo_xMax : pPoint.Y = glo_yMax
            '        pPoint.SpatialReference = Datum_PSAD_17 : pPoint.Project(Datum_PSAD_18)
            '        xMax.Text = pPoint.X : yMax.Text = pPoint.Y
            '        glo_xMin = Int(xMin.Text / 1000) * 1000 : glo_xMax = Int(xMax.Text / 1000) * 1000
            '        glo_yMin = Int(yMin.Text / 1000) * 1000 : glo_yMax = Int(yMax.Text / 1000) * 1000
            '    Case 19
            '        pPoint = New ESRI.ArcGIS.Geometry.Point
            '        pPoint.X = glo_xMin : pPoint.Y = glo_yMin
            '        pPoint.SpatialReference = Datum_PSAD_19
            '        MsgBox(pPoint.X, MsgBoxStyle.Critical, pPoint.Y)
            '        pPoint.Project(Datum_PSAD_18)
            '        MsgBox(pPoint.X, MsgBoxStyle.Critical, pPoint.Y)
            '        xMin.Text = pPoint.X : yMin.Text = pPoint.Y
            '        pPoint = New ESRI.ArcGIS.Geometry.Point
            '        pPoint.X = glo_xMax : pPoint.Y = glo_yMax
            '        pPoint.SpatialReference = Datum_PSAD_19 : pPoint.Project(Datum_PSAD_18)
            '        xMax.Text = pPoint.X : yMax.Text = pPoint.Y
            '        glo_xMin = Int(xMin.Text / 1000) * 1000 : glo_xMax = Int(xMax.Text / 1000) * 1000
            '        glo_yMin = Int(yMin.Text / 1000) * 1000 : glo_yMax = Int(yMax.Text / 1000) * 1000
            'End Select

            'If glo_xMin > 0 Or glo_xMax > 0 Then
            '    Me.lstCoordenada.Items.Add("Punto 1:  " & Val(glo_xMax) & "; " & Val(glo_yMax))
            '    Me.lstCoordenada.Items.Add("Punto 2:  " & Val(glo_xMax) & "; " & Val(glo_yMin))
            '    Me.lstCoordenada.Items.Add("Punto 3:  " & Val(glo_xMin) & "; " & Val(glo_yMin))
            '    Me.lstCoordenada.Items.Add("Punto 4:  " & Val(glo_xMin) & "; " & Val(glo_yMax))
            '    cls_Catastro.Genera_Poligono(lstCoordenada, p_Zona, m_Application)
            'End If
            'termino COMENTADO ZONA DE FRONTERa

            colecciones_depa.Clear()
            cls_Catastro.Ordenacapa_vista("Catastro")
            cls_Catastro.HazZoom(glo_xMin_1 - loint_Intervalo, glo_yMin_1 - loint_Intervalo, glo_xMax_1 + loint_Intervalo, glo_yMax_1 + loint_Intervalo, 0, m_Application)
            cls_Catastro.Style_Linea_GDB("Malla_" & p_Zona, glo_pathStyle & "\malla.style", "CLASE", m_Application, "GDB")
            cls_Catastro.Quitar_Layer("DM_Simulado", m_Application)
            '  cls_Catastro.Zoom_to_Layer("Catastro")
        Catch ex As Exception
            MsgBox("Error al etiquetar capa", MsgBoxStyle.Critical, "Observaci�n...")
        End Try
    End Sub


    Public Sub Activar_Capas_DM_Masivo(ByVal p_Filtro_Capital As String, ByVal p_Zona As String)
        Try


            Dim cls_eval As New Cls_evaluacion
            Dim lo_Filtro_Dpto As String = ""
            Dim lo_Filtro_Dpto_mod As String = ""
            Dim lo_Filtro_Dpto2 As String = ""


            '**************************************************************
            Dim glo_xMin_1, glo_yMin_1, glo_xMax_1, glo_yMax_1 As Double
            glo_xMin_1 = glo_xMin : glo_xMax_1 = glo_xMax
            glo_yMin_1 = glo_yMin : glo_yMax_1 = glo_yMax
            Dim v_filtro As Integer

            'If p_clbLayer.CheckedItems.Count <= 0 Then Exit Sub

            '  If glo_xMin <> 0 And glo_yMin <> 0 And glo_xMax <> 0 And glo_yMax <> 0 Then
            If tipo_seleccion = "OP_11" Or tipo_seleccion = "OP_2" Or tipo_seleccion = "OP_16" Or tipo_seleccion = "OP_18" Then
                lista_depa = lista_depa
                'A�adir capa de paises si esta en Mar
                'Dim busca_filtro_MAR As Integer = InStr(lista_depa, "MAR")
                If busca_filtro_MAR > 0 Or distancia_fron <= 50 Then
                    'If lista_depa = "SAN MARTIN" Then
                    'ElseIf lista_depa = "CAJAMARCA" Then
                    'Else
                    cls_eval.AddLayerFromFile(m_Application)
                    cls_Catastro.Ordenacapa_vista("Catastro")
                    'End If
                End If

            Else



                'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_Z & p_Zona, m_Application, "2", False)

                ' If v_sistema = "PSAD-56" Then
                cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_Z & p_Zona, m_Application, "2", False)
                ' Else
                ' cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_WGS & p_Zona, m_Application, "2", False)
                '  End If


                lo_Filtro_Dpto = cls_eval.f_Intercepta_temas("Departamento", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                lo_Filtro_Dpto_mod = lo_Filtro_Dpto
                v_filtro = InStr(lo_Filtro_Dpto, "99")
                If v_filtro > 0 Then  'valida <> 99
                    Dim nm_depa As String = ""
                    Dim nm_depa1 As String = ""

                    If colecciones_depa.Count = 0 Then
                        lista_depa = ""

                    End If
                    For contador As Integer = 1 To colecciones_depa.Count
                        nm_depa = colecciones_depa.Item(contador)
                        If contador = 1 Then
                            lista_depa = nm_depa
                            lista_depa_mod = nm_depa
                            lista_nm_depa = "NM_DEPA =  '" & nm_depa & "'"

                            nm_depa1 = nm_depa

                        ElseIf contador > 1 Then
                            If nm_depa <> nm_depa1 Then
                                lista_depa = lista_depa & "," & nm_depa
                                lista_nm_depa = lista_nm_depa & " OR " & "NM_DEPA =  '" & nm_depa & "'"
                                nm_depa1 = nm_depa
                            End If
                        End If
                    Next contador
                End If
                'A�adir capa de paises si esta en Mar
                Dim busca_filtro As Integer = InStr(lista_nm_depa, "MAR")
                If busca_filtro > 0 Then
                    cls_eval.AddLayerFromFile(m_Application)
                    cls_Catastro.Ordenacapa_vista("Catastro")
                End If
            End If

            'chequear en simulado edicion no terminada


            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Carta, m_Application, "2", False)

            If p_Filtro_Capital = "1" Then
                lista_nmhojas = v_codigo
            Else

                'Dim lo_Filtro_Dpto2 As String = cls_eval.f_Intercepta_temas("Cuadrangulo", v_este_min, v_norte_min, v_este_max, v_norte_max, pApp, loStrShapefile)
                If tipo_seleccion = "OP_11" Or tipo_seleccion = "OP_2" Or tipo_seleccion = "OP_16" Or tipo_seleccion = "OP_18" Then
                    lista_cartas = lista_cartas  'el mismo
                Else  'Aplica a otra opcion
                    lo_Filtro_Dpto2 = cls_eval.f_Intercepta_temas("Cuadrangulo", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                    lista_hojas = lo_Filtro_Dpto2
                    Dim cd_carta As String
                    For contador As Integer = 1 To colecciones.Count
                        cd_carta = colecciones.Item(contador)
                        If contador = 1 Then
                            lista_cartas = cd_carta
                        ElseIf contador > 1 Then
                            lista_cartas = lista_cartas & "," & cd_carta
                        End If
                    Next contador

                    For contador As Integer = 1 To colecciones_nmhojas.Count
                        cd_carta = colecciones_nmhojas.Item(contador)
                        If contador = 1 Then
                            lista_nmhojas = cd_carta
                        ElseIf contador > 1 Then
                            lista_nmhojas = lista_nmhojas & "," & cd_carta
                        End If
                    Next contador
                    colecciones_nmhojas.Clear()
                    colecciones.Clear()
                End If
            End If


            ' Else
            ' MsgBox("La variable glo_xMin es NUll", MsgBoxStyle.Information, "[BDGeocatmin]")
            ' Exit Sub
            'End If
            Dim lo_Filtro_Area_Reserva As String

            ' For i As Integer = 0 To p_clbLayer.CheckedItems.Count - 1

            'Select Case p_clbLayer.CheckedItems.Item(i) '.SelectedItem '.GetItemText(i) clbLayer.CheckedItems.Item(1)

            '  Case "Zona Reservada"


            '  If tipo_opcion <> "2" Then 'Para Cualquier tipo de consulta


            'If tipo_seleccion = "OP_7" Or tipo_seleccion = "OP_10" Or tipo_seleccion = "OP_8" Then
            '    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & p_Zona, m_Application, "1", False)
            '    lo_Filtro_Area_Reserva = "1"
            '    cls_Prueba.Intersect_Dema_AreaRestringida(m_Application, lo_Filtro_Ubigeo, "", lo_layer, xMin, yMin, xMax, yMax, txtExiste)
            'Else
            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & v_zona_dm, m_Application, "2", False)
            lo_Filtro_Area_Reserva = cls_Catastro.f_Intercepta_FC("Zona Reservada", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
            cantidad_reg_areas = 1
            ' End If


            If cantidad_reg_areas = 0 Then
                ' If lo_Filtro_Area_Reserva = "" Then
                cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
            ElseIf lo_Filtro_Area_Reserva = "" Then
                cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
            Else
                cls_Catastro.Expor_Tema("Zona Reservada", True, m_Application)

                ' cls_Catastro.Export_data_select("Zona Reservada", "X1", m_Application)
                '   cls_Catastro.Expor_Tema("Zona Reservada", True, m_Application)
                ' cls_Catastro.Exportando_Temas("Zona Reservada", "Zona Reservada", m_Application)
                cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                cls_Catastro.Expor_Tema("Area_Natural", True, m_Application)
                Try
                    Kill(glo_pathTMP & "\*.sbn")
                    Kill(glo_pathTMP & "\*.sbx")

                Catch ex As Exception
                End Try
                cls_Catastro.Add_ShapeFile1("Zona Reservada" & fecha_archi, m_Application, "Zona Reservada")
                cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Reservada")

                cls_Catastro.rotulatexto_dm("Zona Reservada", m_Application)

            End If

            '  End If

            ' Case "Area Urbana"
            'If p_clbLayer.GetItemChecked(i) = True Then



            '   Else

            ' If tipo_opcion <> "2" Then 'antes estaba Para Cualquier tipo de consulta
            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & p_Zona, m_Application, "1", False)
            'cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_WGS_" & v_zona_dm, m_Application, "1", False)
            Dim lo_Filtro_Zona_Urbana As String = cls_Catastro.f_Intercepta_FC("Zona Urbana", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
            If lo_Filtro_Zona_Urbana = "" Then
                cls_Catastro.Quitar_Layer("Zona Urbana", m_Application)
            Else
                cls_Catastro.DefinitionExpression(lo_Filtro_Zona_Urbana, m_Application, "Zona Urbana")
                cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Urbana")
                cls_Catastro.rotulatexto_dm("Zona Urbana", m_Application)
            End If



            cod_opcion_Rese = ""


            '  End If




            'colecciones_depa.Clear()
            cls_Catastro.Ordenacapa_vista("Catastro")
            cls_Catastro.HazZoom(glo_xMin_1 - loint_Intervalo, glo_yMin_1 - loint_Intervalo, glo_xMax_1 + loint_Intervalo, glo_yMax_1 + loint_Intervalo, 0, m_Application)
            cls_Catastro.Style_Linea_GDB("Malla_" & p_Zona, glo_pathStyle & "\malla.style", "CLASE", m_Application, "GDB")
            cls_Catastro.Quitar_Layer("DM_Simulado", m_Application)
            '  cls_Catastro.Zoom_to_Layer("Catastro")
        Catch ex As Exception
            MsgBox("Error al etiquetar capa", MsgBoxStyle.Critical, "Observaci�n...")
        End Try
    End Sub





    Public Sub Activar_Capas_DM_PSAD56(ByVal p_Filtro_Capital As String, ByVal p_clbLayer As Object, ByVal p_Zona As String)
        Dim cls_eval As New Cls_evaluacion
        Dim lo_Filtro_Dpto As String = ""
        Dim lo_Filtro_Dpto_mod As String = ""
        Dim lo_Filtro_Dpto2 As String = ""


        '**************************************************************
        Dim glo_xMin_1, glo_yMin_1, glo_xMax_1, glo_yMax_1 As Double
        glo_xMin_1 = glo_xMin : glo_xMax_1 = glo_xMax
        glo_yMin_1 = glo_yMin : glo_yMax_1 = glo_yMax
        Dim v_filtro As Integer

        If p_clbLayer.CheckedItems.Count <= 0 Then Exit Sub

        If glo_xMin <> 0 And glo_yMin <> 0 And glo_xMax <> 0 And glo_yMax <> 0 Then
            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento, m_Application, "2", False)
            lo_Filtro_Dpto = cls_eval.f_Intercepta_temas("Departamento", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
            lo_Filtro_Dpto_mod = lo_Filtro_Dpto
            v_filtro = InStr(lo_Filtro_Dpto, "99")
            If v_filtro > 0 Then  'valida <> 99
                Dim nm_depa As String = ""
                Dim nm_depa1 As String = ""

                If colecciones_depa.Count = 0 Then
                    lista_depa = ""

                End If
                For contador As Integer = 1 To colecciones_depa.Count
                    nm_depa = colecciones_depa.Item(contador)
                    If contador = 1 Then
                        lista_depa = nm_depa
                        lista_depa_mod = nm_depa
                        lista_nm_depa = "NM_DEPA =  '" & nm_depa & "'"

                        nm_depa1 = nm_depa

                    ElseIf contador > 1 Then
                        If nm_depa <> nm_depa1 Then
                            lista_depa = lista_depa & "," & nm_depa
                            lista_nm_depa = lista_nm_depa & " OR " & "NM_DEPA =  '" & nm_depa & "'"
                            nm_depa1 = nm_depa
                        End If
                    End If
                Next contador
            End If
            Dim busca_filtro As Integer = InStr(lista_nm_depa, "MAR")
            If busca_filtro > 0 Then
                cls_eval.AddLayerFromFile(m_Application)
                cls_Catastro.Ordenacapa_vista("Catastro")
            End If

            'hojas

            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_HOJ_HOJAS", m_Application, "2", False)
            If p_Filtro_Capital = "1" Then
                lista_nmhojas = v_codigo
            Else

                'Dim lo_Filtro_Dpto2 As String = cls_eval.f_Intercepta_temas("Cuadrangulo", v_este_min, v_norte_min, v_este_max, v_norte_max, pApp, loStrShapefile)
                lo_Filtro_Dpto2 = cls_eval.f_Intercepta_temas("Cuadrangulo", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                lista_hojas = lo_Filtro_Dpto2
                Dim cd_carta As String
                For contador As Integer = 1 To colecciones.Count
                    cd_carta = colecciones.Item(contador)
                    If contador = 1 Then
                        lista_cartas = cd_carta
                    ElseIf contador > 1 Then
                        lista_cartas = lista_cartas & "," & cd_carta
                    End If
                Next contador

                For contador As Integer = 1 To colecciones_nmhojas.Count
                    cd_carta = colecciones_nmhojas.Item(contador)
                    If contador = 1 Then
                        lista_nmhojas = cd_carta
                    ElseIf contador > 1 Then
                        lista_nmhojas = lista_nmhojas & "," & cd_carta
                    End If
                Next contador
                colecciones_nmhojas.Clear()
                colecciones.Clear()
            End If


        Else
            ' MsgBox("La variable glo_xMin es NUll", MsgBoxStyle.Information, "[BDGeocatmin]")
            Exit Sub
        End If


        For i As Integer = 0 To p_clbLayer.CheckedItems.Count - 1

            Select Case p_clbLayer.CheckedItems.Item(i) '.SelectedItem '.GetItemText(i) clbLayer.CheckedItems.Item(1)

                Case "Zona Reservada"

                    If tipo_opcion <> "2" Then 'Para Cualquier tipo de consulta
                        'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada56, m_Application, "1", False)
                        cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ARE_AREA_RESERVADA_" & v_zona_dm, m_Application, "1", False)
                        'cls_Catastro.Poligono_Color_GDB(gstrFC_AReservada56, glo_pathStyle & "\AREA_RESERVA.style", "CODIGO", "", "Cadena", "", m_Application, "")
                        'Se comento estilo para reserva
                        'cls_Catastro.Poligono_Color_GDB("GPO_ARE_AREA_RESERVADA_" & v_zona_dm, glo_pathStyle & "\AREA_RESERVA_" & v_zona_dm & ".style", "CODIGO", "", "Cadena", "", m_Application, "")

                        Dim lo_Filtro_Area_Reserva As String = cls_Catastro.f_Intercepta_FC("Zona Reservada", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                        If lo_Filtro_Area_Reserva = "" Then
                            cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                        Else

                            cls_Catastro.Expor_Tema("Zona Reservada", True, m_Application)
                            'cls_Catastro.Add_ShapeFile1("Zona Reservada" & fecha_archi, m_Application, "Zona Reservada")
                            cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                            Try
                                Kill(glo_pathTMP & "\*.sbn")
                                Kill(glo_pathTMP & "\*.sbx")

                            Catch ex As Exception
                            End Try
                            cls_Catastro.Add_ShapeFile1("Zona Reservada" & fecha_archi, m_Application, "Zona Reservada")
                            cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Reservada")

                            'cls_Catastro.Shade_Poligono("Zona Reservada", m_Application)
                            'cls_Catastro.DefinitionExpression(lo_Filtro_Area_Reserva, m_Application, "Zona Reservada")  'se comento esta parte

                            'If tipo_seleccion = "OP_4" Then
                            'cls_Catastro.DefinitionExpression(cadena_query_ar, m_Application, "Zona Reservada")
                            'End If

                            'cls_Catastro.Poligono_Color_GDB(gstrFC_AReservada56, glo_pathStyle & "\AREA_RESERVA.style", "CODIGO", "", "Cadena", "", m_Application, "")
                            'cls_Catastro.ShowLabel_DM("Zona Reservada", m_Application)

                            cls_Catastro.rotulatexto_dm("Zona Reservada", m_Application)

                        End If
                    ElseIf tipo_opcion = "2" Then  'Solo si es consulta por Areas Restringida

                        If cod_opcion_Rese = "AN" Then  'Solo capas Reservas se agrega ya no capas Urbanas
                            'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada56, m_Application, "1", False)
                            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ARE_AREA_RESERVADA_" & v_zona_dm, m_Application, "1", False)  'se comento
                            'cls_Catastro.Poligono_Color_GDB("GPO_ARE_AREA_RESERVADA_" & v_zona_dm, glo_pathStyle & "\AREA_RESERVA_" & v_zona_dm & ".style", "CODIGO", "", "Cadena", "", m_Application, "")
                            'Dim lo_Filtro_Area_Reserva As String = cls_Catastro.f_Intercepta_FC("Zona Reservada", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)

                            'If lo_Filtro_Area_Reserva = "" Then
                            'cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                            'Else
                            'cls_Catastro.DefinitionExpression(lo_Filtro_Area_Reserva, m_Application, "Zona Reservada")
                            cls_Catastro.DefinitionExpression(cadena_query_ar, m_Application, "Zona Reservada")
                            cls_Catastro.Expor_Tema("Zona Reservada", True, m_Application)
                            cls_Catastro.Quitar_Layer("Zona Reservada", m_Application)
                            cls_Catastro.Add_ShapeFile1("Zona Reservada" & fecha_archi, m_Application, "Zona Reservada")
                            ' cls_Catastro.Shade_Poligono("Zona Reservada", m_Application)
                            cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Reservada")

                            'If tipo_seleccion = "OP_4" Then
                            'cls_Catastro.DefinitionExpression(cadena_query_ar, m_Application, "Zona Reservada")
                            'End If


                            'cls_Catastro.ShowLabel_DM("Zona Reservada", m_Application)

                            cls_Catastro.rotulatexto_dm("Zona Reservada", m_Application)
                            'End If
                            cod_opcion_Rese = ""
                        End If
                    End If

                Case "Area Urbana"
                    'If p_clbLayer.GetItemChecked(i) = True Then
                    If tipo_opcion <> "2" Then 'Para Cualquier tipo de consulta
                        'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & p_Zona, m_Application, "1", False)
                        cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_" & v_zona_dm, m_Application, "1", False)
                        Dim lo_Filtro_Zona_Urbana As String = cls_Catastro.f_Intercepta_FC("Zona Urbana", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                        If lo_Filtro_Zona_Urbana = "" Then
                            cls_Catastro.Quitar_Layer("Zona Urbana", m_Application)
                        Else
                            cls_Catastro.DefinitionExpression(lo_Filtro_Zona_Urbana, m_Application, "Zona Urbana")
                            cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Urbana")
                            cls_Catastro.rotulatexto_dm("Zona Urbana", m_Application)
                        End If
                    ElseIf tipo_opcion = "2" Then  'Solo si es consulta por Areas Restringida
                        If cod_opcion_Rese = "ZU" Then  'Solo capas Urbanas se agrega, ya no capa Reservas
                            'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & p_Zona, m_Application, "1", False)
                            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_" & v_zona_dm, m_Application, "1", False)
                            Dim lo_Filtro_Zona_Urbana As String = cls_Catastro.f_Intercepta_FC("Zona Urbana", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                            If lo_Filtro_Zona_Urbana = "" Then
                                cls_Catastro.Quitar_Layer("Zona Urbana", m_Application)
                            Else
                                cls_Catastro.DefinitionExpression(lo_Filtro_Zona_Urbana, m_Application, "Zona Urbana")
                                cls_Catastro.Color_Poligono_Simple(m_Application, "Zona Urbana")
                                cls_Catastro.rotulatexto_dm("Zona Urbana", m_Application)
                            End If
                        End If
                        cod_opcion_Rese = ""
                    End If

                Case "Capitales Distritales"
                    'If p_clbLayer.GetItemChecked(i) = True Then
                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_CDistrito, m_Application, "1", False)
                    Dim lo_Filtro_cp As String = cls_Catastro.f_Intercepta_FC("Capitales Distritales", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
                    If lo_Filtro_cp <> "" Then
                        cls_Catastro.DefinitionExpression(lo_Filtro_cp, m_Application, "Capitales Distritales")
                        'cls_Catastro.Shade_Poligono("Capitales Distritales", m_Application)
                        cls_Catastro.Color_Poligono_Simple(m_Application, "Capitales Distritales")
                    Else
                        cls_Catastro.Quitar_Layer("Capitales Distritales", m_Application)
                    End If
                    'End If
                Case "Limite Departamental"
                    If v_filtro > 0 Then  'valida <> 99
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento, m_Application, "1", False)
                        cls_Catastro.DefinitionExpression(lista_nm_depa, m_Application, "Departamento")
                    Else
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento, m_Application, "1", False)
                        cls_Catastro.DefinitionExpression(lo_Filtro_Dpto, m_Application, "Departamento")
                    End If
                    ' cls_Catastro.Shade_Poligono("Departamento", m_Application)
                    cls_Catastro.Color_Poligono_Simple(m_Application, "Departamento")
                    pFeatureLayer = pMap.Layer(0)
                    pFeatureLayer.ShowTips = True
                Case "Limite Provincial"
                    If v_filtro > 0 Then  'valida <> 99
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia, m_Application, "1", False)
                        cls_Catastro.DefinitionExpression(lista_nm_depa, m_Application, "Provincia")
                    Else
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia, m_Application, "1", False)
                        cls_Catastro.DefinitionExpression(lo_Filtro_Dpto, m_Application, "Provincia")
                    End If
                    ' cls_Catastro.Shade_Poligono("Provincia", m_Application)
                    cls_Catastro.Color_Poligono_Simple(m_Application, "Provincia")
                    pFeatureLayer = pMap.Layer(0)
                    pFeatureLayer.ShowTips = True
                Case "Limite Distrital"
                    If v_filtro > 0 Then  'valida <> 99
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito, m_Application, "1", False)
                        cls_Catastro.DefinitionExpression(lista_nm_depa, m_Application, "Distrito")
                    Else
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Distrito, m_Application, "1", False)
                        cls_Catastro.DefinitionExpression(lo_Filtro_Dpto, m_Application, "Distrito")
                    End If
                    ' cls_Catastro.Shade_Poligono("Distrito", m_Application)
                    cls_Catastro.Color_Poligono_Simple(m_Application, "Distrito")
                    pFeatureLayer = pMap.Layer(0)
                    pFeatureLayer.ShowTips = True

                Case "Red Hidrografica"
                    If v_filtro = 0 Then  'valida <> 99
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Rios, m_Application, "1", False)
                        cls_Catastro.DefinitionExpression(lo_Filtro_Dpto_mod, m_Application, "Drenaje")
                        cls_Catastro.UniqueSymbols(m_Application, "Drenaje")
                    End If

                Case "Red Vial"
                    If v_filtro = 0 Then  'valida <> 99
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Carretera, m_Application, "1", False)
                        cls_Catastro.DefinitionExpression(lo_Filtro_Dpto, m_Application, "Vias")
                        cls_Catastro.Shade_Poligono("Vias", m_Application)
                    End If
                Case "Centros Poblados"
                    'If p_clbLayer.GetItemChecked(i) = True Then
                    If v_filtro = 0 Then  'valida <> 99)
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_CPoblado, m_Application, "1", False)
                        cls_Catastro.DefinitionExpression(lo_Filtro_Dpto_mod, m_Application, "Centro Poblado")
                        cls_Catastro.UniqueSymbols(m_Application, "Centro Poblado")
                        'cls_Catastro.Style_Linea_GDB(glo_Owner_Layer_SDE & "GPT_GPO_CENTRO_POBLADOS", glo_pathStyle & "\PUEBLO.style", "DEPARTA", m_application, "SDE", "Nombre")
                    End If
            End Select
        Next i

        If tipo_seleccion = "OP_5" Or tipo_seleccion = "OP_6" Or tipo_seleccion = "OP_12" Or tipo_seleccion = "OP_11" Or tipo_seleccion = "OP_2" Then

        Else
            cls_Catastro.PT_CargarFeatureClass_SDE("GPT_CAM_CERTIFICACION_AMB_G56", m_Application, "1", False)
            ' cls_Catastro.Poligono_Color_GDBp("GPT_CAM_CERTIFICACION_AMB_G56", glo_pathStyle & "\Certificacion.style", "TC_ESTCER", "", "Cadena", "", m_application, "")
            Dim lo_Filtro_certificacion As String = cls_Catastro.f_Intercepta_FC("Certificacion Ambiental", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
            If canti_capa_certi > 0 Then
                cls_Catastro.DefinitionExpression(lo_Filtro_certificacion, m_Application, "Certificacion Ambiental")
                Dim tipocerti As New DataTable
                tipocerti = cls_Catastro.f_Genera_Leyenda_DM("Certificacion Ambiental", m_Application)
                cls_Catastro.Poligono_Color_GDB3("Certificacion Ambiental", tipocerti, glo_pathStyle & "\Certificacion.style", "TC_ESTCER", "", m_Application)

                cls_eval.consultacapaDM("", "Certificacion", "Certificacion Ambiental")
                pFeatureLayer = pMap.Layer(0)
                pFeatureSelection = pFeatureLayer
                pSelectionSet = pFeatureSelection.SelectionSet
                If pFeatureLayer.Name = "Certificacion Ambiental" Then
                    pFeatureLayer = pFeatureSelection
                    pFeatureClass = pFeatureLayer.FeatureClass
                    Dim v_explo_v_expta As Integer
                    Dim v_explo As Integer
                    Dim v_expta As Integer
                    Dim v_cierre As Integer
                    Dim v_tipo As String
                    v_explo_v_expta = 0
                    v_explo = 0
                    v_expta = 0
                    v_cierre = 0
                    If pSelectionSet.Count > 0 Then

                        'If pFeatureLayer.FeatureClass.FeatureCount(Nothing) > 0 Then
                        'pFeatureCursor = pFeatureClass.Search(Nothing, True)
                        pSelectionSet.Search(Nothing, True, pFeatureCursor)
                        pFields = pFeatureClass.Fields
                        Dim v_area_f As Double = 0
                        pFeature = pFeatureCursor.NextFeature
                        Do Until pFeature Is Nothing
                            v_tipo = pFeature.Value(pFields.FindField("TC_ESTCER"))
                            If v_tipo = "Exploraci�n o Explotaci�n" Then
                                v_explo_v_expta = v_explo_v_expta + 1
                            ElseIf v_tipo = "Exploraci�n" Then
                                v_explo = v_explo + 1
                            ElseIf v_tipo = "Explotaci�n" Then
                                v_expta = v_expta + 1
                            ElseIf v_tipo = "Cierre" Then
                                v_cierre = v_cierre + 1
                            End If

                            pFeature = pFeatureCursor.NextFeature
                        Loop
                    End If
                    v_cierre1 = v_cierre
                    v_expta1 = v_expta
                    v_explo_v_expta1 = v_explo_v_expta
                    v_explo1 = v_explo

                End If
            Else
                cls_Catastro.Quitar_Layer("Certificacion Ambiental", m_Application)
            End If

            'Agregando capa de uso minero y actvidad minera
            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_AUM_AREA_USO_MINERO_G56", m_Application, "1", False)

            ' cls_Catastro.Poligono_Color_GDB("GPO_MEM_USO_MINERO", glo_pathStyle & "\Uso_Minero.style", "ANOPRO", "", "Cadena", "", m_Application, "")
            Dim lo_Filtro_Area_Uso_Minero As String = cls_Catastro.f_Intercepta_FC("DM_Uso_Minero", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
            If canti_capa_usomin > 0 Then
                cls_Catastro.DefinitionExpression(lo_Filtro_Area_Uso_Minero, m_Application, "DM_Uso_Minero")
                cls_Catastro.Color_Poligono_Simple(m_Application, "DM_Uso_Minero")
            Else
                cls_Catastro.Quitar_Layer("DM_Uso_Minero", m_Application)
            End If

            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_AAC_AREA_ACT_MINERA_G56", m_Application, "1", False)

            Dim lo_Filtro_Area_act_Minera As String = cls_Catastro.f_Intercepta_FC("DM_Actividad_Minera", glo_xMin, glo_yMin, glo_xMax, glo_yMax, m_Application)
            If canti_capa_actmin > 0 Then
                cls_Catastro.DefinitionExpression(lo_Filtro_Area_act_Minera, m_Application, "DM_Actividad_Minera")
                cls_Catastro.Color_Poligono_Simple(m_Application, "DM_Actividad_Minera")
            Else
                cls_Catastro.Quitar_Layer("DM_Actividad_Minera", m_Application)
            End If
        End If


        'Frontera
        'Me.lstCoordenada.Items.Clear()
        'Select Case p_Zona
        '    Case 17
        '        pPoint = New ESRI.ArcGIS.Geometry.Point
        '        pPoint.X = glo_xMin : pPoint.Y = glo_yMin
        '        pPoint.SpatialReference = Datum_PSAD_17 : pPoint.Project(Datum_PSAD_18)
        '        xMin.Text = pPoint.X : yMin.Text = pPoint.Y
        '        pPoint = New ESRI.ArcGIS.Geometry.Point
        '        pPoint.X = glo_xMax : pPoint.Y = glo_yMax
        '        pPoint.SpatialReference = Datum_PSAD_17 : pPoint.Project(Datum_PSAD_18)
        '        xMax.Text = pPoint.X : yMax.Text = pPoint.Y
        '        glo_xMin = Int(xMin.Text / 1000) * 1000 : glo_xMax = Int(xMax.Text / 1000) * 1000
        '        glo_yMin = Int(yMin.Text / 1000) * 1000 : glo_yMax = Int(yMax.Text / 1000) * 1000
        '    Case 19
        '        pPoint = New ESRI.ArcGIS.Geometry.Point
        '        pPoint.X = glo_xMin : pPoint.Y = glo_yMin
        '        pPoint.SpatialReference = Datum_PSAD_19 : pPoint.Project(Datum_PSAD_18)
        '        xMin.Text = pPoint.X : yMin.Text = pPoint.Y
        '        pPoint = New ESRI.ArcGIS.Geometry.Point
        '        pPoint.X = glo_xMax : pPoint.Y = glo_yMax
        '        pPoint.SpatialReference = Datum_PSAD_19 : pPoint.Project(Datum_PSAD_18)
        '        xMax.Text = pPoint.X : yMax.Text = pPoint.Y
        '        glo_xMin = Int(xMin.Text / 1000) * 1000 : glo_xMax = Int(xMax.Text / 1000) * 1000
        '        glo_yMin = Int(yMin.Text / 1000) * 1000 : glo_yMax = Int(yMax.Text / 1000) * 1000
        'End Select
        'If glo_xMin > 0 Or glo_xMax > 0 Then
        '    Me.lstCoordenada.Items.Add("Punto 1:  " & Val(glo_xMax) & "; " & Val(glo_yMax))
        '    Me.lstCoordenada.Items.Add("Punto 2:  " & Val(glo_xMax) & "; " & Val(glo_yMin))
        '    Me.lstCoordenada.Items.Add("Punto 3:  " & Val(glo_xMin) & "; " & Val(glo_yMin))
        '    Me.lstCoordenada.Items.Add("Punto 4:  " & Val(glo_xMin) & "; " & Val(glo_yMax))
        '    cls_Catastro.Genera_Poligono(lstCoordenada, p_Zona, m_Application)
        'End If
        colecciones_depa.Clear()
        cls_Catastro.Ordenacapa_vista("Catastro")
        cls_Catastro.HazZoom(glo_xMin_1 - loint_Intervalo, glo_yMin_1 - loint_Intervalo, glo_xMax_1 + loint_Intervalo, glo_yMax_1 + loint_Intervalo, 0, m_Application)
        cls_Catastro.Style_Linea_GDB("Malla_" & p_Zona, glo_pathStyle & "\malla.style", "CLASE", m_Application, "GDB")

    End Sub

    Private Sub btnGenera_Poligono_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenera_Poligono.Click
        Try
            caso_consulta = "CATASTRO MINERO"
            V_caso_simu = "Si"
            pMxDoc = m_Application.Document
            pMap = pMxDoc.FocusMap
            If pMap.Name <> "CATASTRO MINERO" Then
                cls_planos.buscaadataframe(caso_consulta, False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                    pMxDoc.UpdateContents()
                End If
                cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                pMxDoc.UpdateContents()
            End If
            cls_eval.Eliminadataframe(caso_consulta)
            ' cls_eval.Eliminadataframe("CATASTRO MINERO")  'elimina dataframe
            ' cls_eval.Eliminadataframe()
            cls_planos.buscaadataframe("CATASTRO MINERO", False)
            If valida = False Then
                pMap.Name = "CATASTRO MINERO"
            End If
            pMxDoc.UpdateContents()

            cls_Catastro.Borra_Todo_Feature("", m_Application)
            cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
            If Me.lstCoordenada.Items.Count < 2 Then Exit Sub
            If Me.cboZona.SelectedIndex = 0 Then
                MsgBox("Seleccione una Zona ", MsgBoxStyle.Information, "[BDGEOCIENT�FICA]")
                Exit Sub
            End If
            Dim loStrShapefile As String = "Simulado" & DateTime.Now.Ticks.ToString()
            fecha_archi2 = DateTime.Now.Ticks.ToString
            glo_Layer_Simulado = loStrShapefile
            V_zona_simu = Me.cboZona.Text
            V_caso_simu = "SI"
            Codigo_dm_v = "000000001"
            Dim lodtRegistro As New DataTable
            If Me.lstCoordenada.Items.Count = 2 Then
                MsgBox("Para Evaluar m�nimo 3 V�rtices...", MsgBoxStyle.Information, "[BDGEOCATMIN]")
                Exit Sub
            End If
            For i As Integer = 0 To Me.lstCoordenada.Items.Count - 1
                Me.lstCoordenada.SelectedIndex = i
                Select Case i
                    Case 0
                        lodtRegistro.Columns.Add("CG_CODIGO", Type.GetType("System.String"))
                        lodtRegistro.Columns.Add("PE_NOMDER", Type.GetType("System.String"))
                        lodtRegistro.Columns.Add("CD_COREST", Type.GetType("System.Double"))
                        lodtRegistro.Columns.Add("CD_CORNOR", Type.GetType("System.Double"))
                        lodtRegistro.Columns.Add("CD_NUMVER", Type.GetType("System.Double"))
                End Select
                Dim dRow As DataRow
                dRow = lodtRegistro.NewRow
                dRow.Item("CG_CODIGO") = Codigo_dm_v
                dRow.Item("PE_NOMDER") = "DM Simulado"
                Dim lostrEste As String = Mid(Me.lstCoordenada.Text, 1, InStr(Me.lstCoordenada.Text, ";") - 1)
                dRow.Item("CD_COREST") = CType(Mid(lostrEste, InStr(lostrEste, ":") + 3), Double)
                dRow.Item("CD_CORNOR") = CType(Mid(Me.lstCoordenada.Text, InStr(Me.lstCoordenada.Text, ";") + 2), Double)
                dRow.Item("CD_NUMVER") = i + 1
                lodtRegistro.Rows.Add(dRow)
            Next
            Dim lodtvOrdena_xEste As New DataView(lodtRegistro, Nothing, "CD_COREST ASC", DataViewRowState.CurrentRows)
            glo_xMin = lodtvOrdena_xEste.Item(0).Row("CD_COREST")
            glo_xMax = lodtvOrdena_xEste.Item(lodtvOrdena_xEste.Count - 1).Row("CD_COREST")
            Dim lodtvOrdena_y As New DataView(lodtRegistro, Nothing, "CD_CORNOR ASC", DataViewRowState.CurrentRows)
            glo_yMin = lodtvOrdena_y.Item(0).Row("CD_CORNOR")
            glo_yMax = lodtvOrdena_y.Item(lodtvOrdena_y.Count - 1).Row("CD_CORNOR")
            cls_Catastro.Load_FC_GDB("gpt_Vertice_DM", "", m_Application, True)
            cls_Catastro.Delete_Rows_FC_GDB("gpt_Vertice_DM")
            'Se comento para arcgis 10.1 porque obtiene para jalar del shape, debe jala de la gdb
            '  cls_Prueba.Create_Shapefile_DM(glo_pathTMP, loStrShapefile, V_zona_simu, "Poligono")
            ' cls_DM_2.Genera_Catastro_NuevoM(loStrShapefile, lodtRegistro, V_zona_simu, m_Application, txtExiste)
            'Este procedimiento se aumento un parametro para que sea estandar tanto de la bd como simular por coordenadas

            'cls_DM_2.Genera_Catastro_DM_simu(lodtRegistro, V_zona_simu, m_Application)


            '  If CType(txtExiste.Text, Integer) <> -1 Then  'comentado
            cls_DM_2.Genera_Catastro_DM_simu(lodtRegistro, V_zona_simu, m_Application)
            v_zona_dm = V_zona_simu

            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Departamento_Z & V_zona_simu, m_Application, "1", False)
            '  cls_Prueba.IntersectSelect_Feature_Class("DM_Simulado", "", "Departamento", "", xMin, yMin, xMax, yMax, txtExiste, m_Application)
            cls_Prueba.IntersectSelect_Feature_Class("Catastro_" & V_zona_simu, "", "Departamento", "", xMin, yMin, xMax, yMax, txtExiste, m_Application)



            v_este_min = glo_xMin : v_este_max = glo_xMax
            v_norte_min = glo_yMin : v_norte_max = glo_yMax
            Select Case txtExiste.Text
                Case Is <> -1
                    cls_Catastro.Actualizar_DM(V_zona_simu)
                    cls_Catastro.Rotular_texto_DM("gpt_Vertice_DM", V_zona_simu, m_Application)
                    cls_Catastro.Quitar_Layer("gpt_Vertice_DM", m_Application)
                    '  cls_Prueba.Zoom_to_Layer("DM_Simulado", m_Application)
                    cls_Prueba.Zoom_to_Layer("Catastro_" & V_zona_simu, m_Application)
                    cls_Catastro.Color_Poligono_Simple(m_Application, "DM_Simulado")

                    cls_Catastro.Genera_Imagen_DM("VistaPrevia", "VistaPrevia")
                    img_DM.ImageLocation = glo_pathTMP & "\VistaPrevia.jpg"
                    'cls_Catastro.Shade_Poligono("DM_Simulado", m_Application)
                    cls_Catastro.Genera_Imagen_DM("VistaPrevia_1", "VistaPrevia")
                    Me.btnGraficar.Enabled = True
                Case Else
                    MsgBox("El poligono esta fuera de los L�mites, verificar coordenadas", MsgBoxStyle.Information, "[BDGEOCATMIN]")
            End Select
            'End If
            Me.cboZona.Enabled = True
            cls_Catastro.Borra_Todo_Feature("", m_Application)
            cls_Catastro.Limpiar_Texto_Pantalla(m_Application)
        Catch ex As Exception
            MsgBox(".::Error - en Simular Poligono::.", vbInformation, "Aviso")
        End Try
    End Sub
    Public Sub PT_Inicializar_Grilla_Carta()
        Dim lodtbLista_DM As New DataTable
        lodtbLista_DM.Columns.Add("CODIGO", GetType(String))
        lodtbLista_DM.Columns.Add("NOMBRE", GetType(String))
        lodtbLista_DM.Columns.Add("ZONA", GetType(String))
        lodtbLista_DM.Columns.Add("XMIN", GetType(String))
        lodtbLista_DM.Columns.Add("YMIN", GetType(String))
        lodtbLista_DM.Columns.Add("XMAX", GetType(String))
        lodtbLista_DM.Columns.Add("YMAX", GetType(String))
        PT_Estilo_Grilla_Carta(lodtbLista_DM) : PT_Cargar_Grilla_Carta(lodtbLista_DM)
        PT_Agregar_Funciones_Carta() : PT_Forma_Grilla_Carta()
    End Sub

    Private Sub PT_Estilo_Grilla_Carta(ByRef padtbDetalle As DataTable)
        padtbDetalle.Columns.Item(Col_Cod_Hoja).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Nom_Hoja).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_Zona_Hoja).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_xMin).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_yMin).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_xMax).DefaultValue = ""
        padtbDetalle.Columns.Item(Col_yMax).DefaultValue = ""
    End Sub
    Private Sub PT_Cargar_Grilla_UBIGEO(ByVal padtbDetalle As DataTable)
        Dim dvwDetalle As New DataView(padtbDetalle)
        Me.dgdDetalle.DataSource = dvwDetalle
        Pinta_Grilla_Ubigeo()
    End Sub
    Private Sub PT_Forma_Grilla_Carta()
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Cod_Hoja).Width = 60
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nom_Hoja).Width = 120
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zona_Hoja).Width = 40
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_xMin).Width = 80
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_yMin).Width = 80
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_xMax).Width = 80
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_yMax).Width = 80
        Me.dgdDetalle.Columns("CODIGO").Caption = "C�digo"
        Me.dgdDetalle.Columns("NOMBRE").Caption = "Nombre"
        Me.dgdDetalle.Columns("ZONA").Caption = "Zona"
        Me.dgdDetalle.Columns("XMIN").Caption = "Este Min."
        Me.dgdDetalle.Columns("YMIN").Caption = "Norte Min."
        Me.dgdDetalle.Columns("XMAX").Caption = "Este Max."
        Me.dgdDetalle.Columns("YMAX").Caption = "Norte Max."
        Me.dgdDetalle.Splits(0).HeadingStyle.ForeColor = Color.Black
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Cod_Hoja).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nom_Hoja).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zona_Hoja).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_xMin).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_yMin).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_xMax).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_yMax).HeadingStyle.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Cod_Hoja).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Nom_Hoja).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_Zona_Hoja).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_xMin).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_yMin).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_xMax).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Center
        Me.dgdDetalle.Splits(0).DisplayColumns(Col_yMax).Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.General
    End Sub
    Private Sub PT_Cargar_Grilla_Carta(ByVal padtbDetalle As DataTable)
        Dim dvwDetalle As New DataView(padtbDetalle)
        Me.dgdDetalle.DataSource = dvwDetalle
        Pinta_Grilla_Ubigeo()
    End Sub
    Private Sub PT_Agregar_Funciones_Carta()
        Me.dgdDetalle.Columns(Col_Cod_Hoja).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Nom_Hoja).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_Zona_Hoja).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_xMin).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_yMin).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_xMax).DefaultValue = ""
        Me.dgdDetalle.Columns(Col_yMax).DefaultValue = ""
    End Sub
    Private Sub dgdDetalle_RowColChange(ByVal sender As Object, ByVal e As C1.Win.C1TrueDBGrid.RowColChangeEventArgs) Handles dgdDetalle.RowColChange


        Dim losender As New System.Object
        Dim loe As New System.EventArgs
        Me.dgdDetalle_DoubleClick(losender, loe)
        Me.dgdDetalle.Focus()
    End Sub
    Private Sub BOTON_MENU(ByVal p_Estado As Boolean)
        Dim lodtbBotones As New DataTable
        Dim v_loOpcion As String = ""
        Select Case GloInt_Opcion
            Case 0
                lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("XXX")
                Hide_Barra(m_Application, lodtbBotones, "Evaluaci�n")
                lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("XXX")
                Hide_Barra(m_Application, lodtbBotones, "Opciones")

                tipo_seleccion = " "

                If Me.cbo_tipo.SelectedValue = "OP_28" Then
                    v_loOpcion = "OP_1"
                    lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion(v_loOpcion)
                Else
                    lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion(Me.cbo_tipo.SelectedValue)
                End If

                'lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion(Me.cbo_tipo.SelectedValue)

                If ((conta_botones_consulta = 1) And (tipo_seleccion = " ")) Then
                    'Create_Barra_1(m_Application, lodtbBotones)

                    'Create_Barra_2(m_Application, lodtbBotones, "Evaluaci�n")
                    'End If

                    'Formato Autom�tico - BOTONES INFORMES TECNICOS DE EVALUACION
                    ' lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("EVA2")
                    'Create_Barra_2(m_Application, lodtbBotones, "Opciones")
                    'Botones Principales

                    If Me.cbo_tipo.SelectedValue = "OP_28" Then
                        lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion(v_loOpcion)
                    Else
                        lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion(Me.cbo_tipo.SelectedValue)
                    End If

                    'lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion(Me.cbo_tipo.SelectedValue)

                    Create_Barra_1(m_Application, lodtbBotones)

                    'If tipo_seleccion = "OP_16" Then
                    'If (conta_botones_libredenu = 1) And (tipo_seleccion = "OP_16") Then
                    '    'Create_Barra_2(m_Application, lodtbBotones, "Opciones")


                    '    lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("EVA1")
                    '    Create_Barra_2(m_Application, lodtbBotones, "Opciones")

                    '    lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("LIBDEN")
                    '    Create_Barra_2(m_Application, lodtbBotones, "Libredenu")


                    'End If
                Else

                    lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("EVA1")
                    'If ((conta_botones_evaluacion = 1) And ((tipo_seleccion = "OP_11") Or (tipo_seleccion = "OP_12") Or (tipo_seleccion = "OP_5") Or (tipo_seleccion = "OP_6") Or (tipo_seleccion = "OP_16"))) Then
                    If ((conta_botones_evaluacion = 1) And ((tipo_seleccion = "OP_11") Or (tipo_seleccion = "OP_12") Or (tipo_seleccion = "OP_5") Or (tipo_seleccion = "OP_6") Or (tipo_seleccion = "OP_18") Or (tipo_seleccion = "OP_20"))) Then
                        Create_Barra_2(m_Application, lodtbBotones, "Evaluaci�n")

                        'Formato Autom�tico
                        lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("EVA2")
                        Create_Barra_2(m_Application, lodtbBotones, "Opciones")
                        'Botones Principales
                        lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion(Me.cbo_tipo.SelectedValue)
                        Create_Barra_1(m_Application, lodtbBotones)

                    ElseIf (conta_botones_libredenu = 1) And (tipo_seleccion = "OP_16") Then
                        'Create_Barra_2(m_Application, lodtbBotones, "Opciones")

                        ' lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("EVA1")
                        ' Create_Barra_2(m_Application, lodtbBotones, "Formato Tecnico")

                        lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("LIBDEN")  'BOTONES PARA LIBRE DENU
                        Create_Barra_2(m_Application, lodtbBotones, "Libredenu")

                    End If

                    ''Botones Principales
                    ''  tipo_seleccion = ""  'por  mientras boton libredenu
                    'lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion(Me.cbo_tipo.SelectedValue)
                    'conta_botones_consulta = 1
                    'If ((conta_botones_consulta = 1) And (tipo_seleccion = "")) Then
                    '    'Create_Barra_1(m_Application, lodtbBotones)

                    '    'Create_Barra_2(m_Application, lodtbBotones, "Evaluaci�n")
                    '    'End If

                    '    'Formato Autom�tico
                    '    ' lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("EVA2")
                    '    'Create_Barra_2(m_Application, lodtbBotones, "Opciones")
                    '    'Botones Principales
                    '    lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion(Me.cbo_tipo.SelectedValue)
                    '    Create_Barra_1(m_Application, lodtbBotones)
                    'End If


                End If

            Case 1
                lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("EVA1")
                'If ((conta_botones_evaluacion = 1) And ((tipo_seleccion = "OP_11") Or (tipo_seleccion = "OP_12") Or (tipo_seleccion = "OP_5") Or (tipo_seleccion = "OP_6") Or (tipo_seleccion = "OP_16"))) Then
                If ((conta_botones_evaluacion = 1) And ((tipo_seleccion = "OP_11") Or (tipo_seleccion = "OP_12") Or (tipo_seleccion = "OP_5") Or (tipo_seleccion = "OP_6") Or (tipo_seleccion = "OP_18") Or (tipo_seleccion = "OP_20"))) Then
                    Create_Barra_2(m_Application, lodtbBotones, "Evaluaci�n")
                    'End If

                    'Formato Autom�tico
                    lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("EVA2")
                    Create_Barra_2(m_Application, lodtbBotones, "Informes T�cnico")
                    'Botones Principales
                    lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion(Me.cbo_tipo.SelectedValue)
                    Create_Barra_1(m_Application, lodtbBotones)
                ElseIf (conta_botones_libredenu = 1) And (tipo_seleccion = "OP_16") Then 'BOTONES PARA LIBRE DENU

                    lodtbBotones = cls_Oracle.F_Obtiene_Menu_x_Opcion_1("LIBDEN")
                    Create_Barra_2(m_Application, lodtbBotones, "Libredenu")
                End If
        End Select
        'v_loOpcion = Me.cbo_tipo.SelectedValue.ToString

    End Sub
    Public Sub Create_Barra_2(ByVal m_application As IApplication, ByVal p_Grilla As DataTable, ByVal p_NomVentana As String)
        Dim pMDocumento As IDocument
        Dim pTool_Bars As ICommandBars
        Dim pTool_Boton As ICommandBar = Nothing
        pMDocumento = m_application.Document
        pTool_Bars = pMDocumento.CommandBars
        Try
            'pTool_Bars.HideAllToolbars()
            pTool_Boton = pTool_Bars.Find("Project." & p_NomVentana, False, True)
            If pTool_Boton Is Nothing Then
                pTool_Boton = pTool_Bars.Create(p_NomVentana, ESRI.ArcGIS.SystemUI.esriCmdBarType.esriCmdBarTypeToolbar)
                ' The built in ArcID module is used to find the ArcMap commands. 
                Dim pUID As UID
                For i As Integer = 0 To p_Grilla.Rows.Count - 1
                    pUID = New UID
                    pUID.Value = "{" & p_Grilla.Rows(i).Item("GUID") & "}" '"{22534c7c-6632-4c4f-9671-bce2954c82de}"
                    pTool_Boton.Add(pUID)
                Next
                pTool_Boton.Dock(esriDockFlags.esriDockShow)
            Else
                If pTool_Boton.IsVisible Or pTool_Boton.Count <> 0 Then
                    For w As Integer = 0 To pTool_Boton.Count - 1
                        pTool_Boton.Item(0).Delete()
                    Next
                    Dim pUID As UID
                    For i As Integer = 0 To p_Grilla.Rows.Count - 1
                        Try
                            pUID = New UID
                            pUID.Value = "{" & p_Grilla.Rows(i).Item("GUID") & "}" '"{22534c7c-6632-4c4f-9671-bce2954c82de}"
                            pTool_Boton.Add(pUID)
                            pTool_Boton.Dock(esriDockFlags.esriDockShow)
                            pTool_Boton.Dock(esriDockFlags.esriDockRight)
                        Catch ex As Exception
                            MsgBox("Error linea: " & i)
                        End Try
                    Next
                Else
                    pTool_Boton.Dock(esriDockFlags.esriDockShow)
                End If
            End If
        Catch ex As Exception
            ' MsgBox("Error")
        End Try
    End Sub


    Private Sub INICIALIZA_BOTON(ByVal p_Estado As Boolean)
        glo_Tool_BT_1 = p_Estado
        glo_Tool_BT_2 = p_Estado
        glo_Tool_BT_3 = p_Estado
        glo_Tool_BT_4 = p_Estado
        glo_Tool_BT_5 = p_Estado
        glo_Tool_BT_6 = p_Estado
        glo_Tool_BT_7 = p_Estado
        glo_Tool_BT_8 = p_Estado
        glo_Tool_BT_9 = p_Estado
        glo_Tool_BT_10 = p_Estado
        glo_Tool_BT_11 = p_Estado
        glo_Tool_BT_12 = p_Estado
        glo_Tool_BT_13 = p_Estado
        glo_Tool_BT_14 = p_Estado
        glo_Tool_BT_15 = p_Estado
        glo_Tool_BT_16 = p_Estado
        glo_Tool_BT_17 = p_Estado
        glo_Tool_BT_18 = p_Estado
        glo_Tool_BT_19 = p_Estado
        glo_Tool_BT_20 = p_Estado
        glo_Tool_BT_21 = p_Estado
        glo_Tool_BT_22 = p_Estado
        glo_Tool_BT_23 = p_Estado
        glo_Tool_BT_24 = p_Estado
        glo_Tool_BT_25 = p_Estado
        glo_Tool_BT_26 = p_Estado
    End Sub

    Private Sub txtRadio_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRadio.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Tab Then
            Me.cboZona.Focus()
        End If
    End Sub

    Private Sub chkEstado_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEstado.CheckedChanged
        Dim items As C1.Win.C1TrueDBGrid.ValueItems = Me.dgdDetalle.Columns("FLG_SEL").ValueItems
        If Me.chkEstado.Checked Then
            ' we're going to translate values - the datasource needs to hold at least 3 states
            items.Translate = True
            ' each click will cycle thru the various checkbox states
            items.CycleOnClick = True
            ' display the cell as a checkbox
            items.Presentation = C1.Win.C1TrueDBGrid.PresentationEnum.CheckBox
            ' now associate underlying db values with the checked state
            items.Values.Clear()
            items.Values.Add(New C1.Win.C1TrueDBGrid.ValueItem("0", False)) ' unchecked
            items.Values.Add(New C1.Win.C1TrueDBGrid.ValueItem("1", True)) ' checked
            '  items.Values.Add(New C1.Win.C1TrueDBGrid.ValueItem("2", "INDETERMINATE")) ' indeterminate state
        Else
            items.Translate = False
            items.CycleOnClick = False
            items.Presentation = C1.Win.C1TrueDBGrid.PresentationEnum.Normal
        End If
    End Sub

    Private Sub btnActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizar.Click
        pMap = pMxDoc.FocusMap
        Dim cls_catastro As New cls_DM_1
        cls_catastro.Borra_Todo_Feature("", m_Application)
        'Dim pOutFeatureClass As IFeatureClass
        Dim pQueryFilter As IQueryFilter
        Dim pFeatureCursor As IFeatureCursor
        Dim pFeature As IFeature
        Dim pFeatureCursorpout As IFeatureCursor
        Dim pFeaturepout As IFeature

        Dim v_nmrese As String = ""
        Dim v_cdrese As String = ""
        Dim v_nombre As String = ""
        Dim v_area As Double = 0.0
        Dim v_tprese As String = ""
        Dim v_categori As String = ""
        Dim v_clase As String = ""
        Dim v_titular As String = ""
        Dim v_zona As String = ""
        Dim v_zonex As String = ""
        Dim v_obs As String = ""
        Dim v_norma As String = ""
        Dim v_archivo As String = ""
        Dim v_fechaing As String = ""
        Dim v_entidad As String = ""
        Dim v_uso As String = ""
        Dim v_estado As String = ""
        Dim canti_reg As Integer
        Dim lostrObervacion As String = ""
        Dim cadena As String
        Dim nm_archivo As String
        Dim cls_consulta As New cls_DM_1
        Dim fecha As String
        Dim pFeatureTable As ITable
        Dim contador As Integer
        Dim MyDate As Date
        Dim v_leyenda As String = ""
        MyDate = Now
        Dim oSW As New StreamWriter(glo_pathTMP & "\prueba.txt")
        Dim Linea As String = ""
        fecha = RellenarComodin(MyDate.Day, 2, "0") & RellenarComodin(MyDate.Month, 2, "0") & RellenarComodin(MyDate.Year, 2, "0")
        Using archivo1 As IO.StreamReader = New IO.StreamReader("C:\" & fecha & ".txt")
            Dim line As String
            Do
                line = archivo1.ReadLine
                If (line <> "") Then
                    cadena = Microsoft.VisualBasic.Left(line, 1)
                    If (cadena = "s") Then  'solo caso featureclass
                        nm_archivo = Microsoft.VisualBasic.Right(line, Len(line) - 2)
                    End If

                    If line = "17" Or line = "18" Or line = "19" Then
                        oSW.WriteLine(nm_archivo & line)
                        oSW.Flush()
                    End If
                End If
            Loop Until line Is Nothing
            archivo1.Close()
        End Using
        oSW.Close()
        Try
            nm_archivo = UCase(nm_archivo)
            Using archivo As IO.StreamReader = New IO.StreamReader(glo_pathTMP & "\prueba.txt")
                Dim line As String
                Do
                    line = archivo.ReadLine
                    If (line <> "") Then
                        cadena = Microsoft.VisualBasic.Right(line, 2)
                        nm_archivo = Microsoft.VisualBasic.Left(line, Len(line) - 2)
                        nm_archivo = UCase(nm_archivo)
                        For i As Integer = 17 To 21
                            If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Or v_tipoproceso = "ELIMINAR" Then

                                If i = cadena Then
                                    contador = 1
                                    tipo_areas = "ReseUTM"

                                    If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                        '    ruta_shputm = "U:\DATOS\SHAPE\urbanas\Historico\"
                                        cls_consulta.Add_capa_catnomin(nm_archivo, m_Application, fecha, ruta_shputm & fecha)
                                    End If
                                    If tipo_catanominero = "AREA RESERVADA" Then
                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_AREA_RESERVADA_" & cadena, m_Application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & cadena, m_Application, "1", False)
                                        ' cls_catastro.PT_CargarFeatureClass_SDE("FLAT1065.GPO_ARE_AREA_RESERVADA_17", m_Application, "1", False)
                                        ' cls_catastro.PT_CargarFeatureClass_SDE("DATA_GIS.AREA_RESERVADA_17", m_Application, "1", False)

                                    ElseIf tipo_catanominero = "ZONA URBANA" Then
                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZON_ZONA_URBANA_" & cadena, m_application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & cadena, m_Application, "1", False)
                                    End If
                                ElseIf i = 20 Then
                                    contador = 1
                                    tipo_areas = "ReseGeo"
                                    If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                        cls_consulta.Add_capa_catnomin(nm_archivo & "g", m_Application, fecha, ruta_shp)
                                    End If

                                    If tipo_catanominero = "AREA RESERVADA" Then
                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_AREA_RESERVADA_G56", m_Application, "1", False)
                                        ''cls_catastro.PT_CargarFeatureClass_SDE("FLAT1065.GPO_ARE_AREA_RESERVADA_G56", m_Application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & "G56", m_Application, "1", False)
                                        '''cls_catastro.PT_CargarFeatureClass_SDE("CEGO0891.GPO_ARE_AREA_RESERVADA_G56", m_Application, "1", False)
                                    ElseIf tipo_catanominero = "ZONA URBANA" Then

                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZUR_ZONA_URBANA_G56", m_application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE("GIS.GPO_ZUR_ZONA_URBANA_G56", m_Application, "1", False)
                                    End If
                                ElseIf i = 21 Then
                                    contador = 1
                                    tipo_areas = "ReseGeo"
                                    If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                        cls_consulta.Add_capa_catnomin(nm_archivo & "w", m_Application, fecha, ruta_shp)
                                    End If
                                    If tipo_catanominero = "AREA RESERVADA" Then
                                        '   cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_AREA_RESERVADA_G84", m_Application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE("GPO_ARE_AREA_RESERVADA_G84", m_Application, "1", False)
                                    ElseIf tipo_catanominero = "ZONA URBANA" Then
                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZUR_ZONA_URBANA_G84", m_application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_G84", m_Application, "1", False)
                                    End If
                                Else
                                    contador = 1
                                    tipo_areas = "ReseUTM"
                                    i = "18"
                                    If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then

                                        cls_consulta.Add_capa_catnomin(nm_archivo & i, m_Application, fecha, ruta_shp)
                                    End If
                                    If tipo_catanominero = "AREA RESERVADA" Then
                                        '     cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_AREA_RESERVADA_" & i, m_Application, "1", False)
                                        ' cls_catastro.PT_CargarFeatureClass_SDE("FLAT1065.GPO_ARE_AREA_RESERVADA_" & i, m_Application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & i, m_Application, "1", False)
                                        ''cls_catastro.PT_CargarFeatureClass_SDE("CEGO0891.GPO_ARE_AREA_RESERVADA_" & i, m_Application, "1", False)
                                    ElseIf tipo_catanominero = "ZONA URBANA" Then
                                        ' cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZUR_ZONA_URBANA_" & i, m_application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & i, m_Application, "1", False)
                                    End If
                                End If
                                'pMap = pMxDoc.FocusMap
                                'pFeatureLayer = pMap.Layer(0)
                                'pOutFeatureClass = pFeatureLayer.FeatureClass
                                'pMap.DeleteLayer(pFeatureLayer)
                            End If
                            If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                'cls_consulta.Add_capa_catnomin(nm_archivo, m_application, fecha)
                                'pQueryFilter = New QueryFilter
                                'pQueryFilter.WhereClause = "ARCHIVO = '" & nm_archivo & "'"
                                'pFeatureCursorpout = pOutFeatureClass.Update(pQueryFilter, True)
                                'pFeaturepout = pFeatureCursorpout.NextFeature
                                'pFeatureTable = pOutFeatureClass
                                nm_archivo = UCase(nm_archivo)


                                If pFeatureLayeras_rese.FeatureClass.FeatureCount(Nothing) = 0 Then
                                    MsgBox("No hay registros en la capa ", MsgBoxStyle.Critical, "Observaci�n...")
                                    Exit Sub
                                Else
                                    canti_reg = pFeatureLayeras_rese.FeatureClass.FeatureCount(Nothing)
                                End If
                                pQueryFilter = New QueryFilter
                                pFeatureCursor = pInFeatureClass.Search(pQueryFilter, True)

                                pFeature = pFeatureCursor.NextFeature
                                Do While Not pFeature Is Nothing
                                    contador = contador + 1
                                    If tipo_catanominero = "AREA RESERVADA" Then
                                        If pFeatureCursor.FindField("CODIGO") = -1 Then
                                            v_cdrese = " "
                                        Else
                                            v_cdrese = pFeature.Value(pFeatureCursor.FindField("CODIGO")).ToString
                                        End If
                                        If pFeatureCursor.FindField("NM_RESE") = -1 Then
                                            v_nmrese = " "
                                        Else
                                            v_nmrese = pFeature.Value(pFeatureCursor.FindField("NM_RESE")).ToString
                                        End If
                                        If pFeatureCursor.FindField("NOMBRE") = -1 Then
                                            v_nombre = " "
                                        Else
                                            v_nombre = pFeature.Value(pFeatureCursor.FindField("NOMBRE")).ToString
                                        End If
                                        If pFeatureCursor.FindField("HAS") = -1 Then
                                            v_area = 0.0
                                        Else
                                            v_area = pFeature.Value(pFeatureCursor.FindField("HAS")).ToString
                                        End If
                                        If pFeatureCursor.FindField("TP_RESE") = -1 Then
                                            v_tprese = " "
                                        Else
                                            v_tprese = pFeature.Value(pFeatureCursor.FindField("TP_RESE")).ToString
                                        End If
                                        If pFeatureCursor.FindField("CATEGORI") = -1 Then
                                            v_categori = " "
                                        Else
                                            v_categori = pFeature.Value(pFeatureCursor.FindField("CATEGORI")).ToString
                                        End If
                                        If pFeatureCursor.FindField("CLASE") = -1 Then
                                            v_clase = " "
                                        Else
                                            v_clase = pFeature.Value(pFeatureCursor.FindField("CLASE")).ToString
                                        End If

                                        If pFeatureCursor.FindField("TITULAR") = -1 Then
                                            v_titular = " "
                                        Else
                                            v_titular = pFeature.Value(pFeatureCursor.FindField("TITULAR")).ToString
                                        End If
                                        If pFeatureCursor.FindField("ZONA") = -1 Then
                                            v_zona = " "
                                        Else
                                            v_zona = pFeature.Value(pFeatureCursor.FindField("ZONA")).ToString
                                        End If
                                        If pFeatureCursor.FindField("ZONEX") = -1 Then
                                            v_zonex = " "
                                        Else
                                            v_zonex = pFeature.Value(pFeatureCursor.FindField("ZONEX")).ToString
                                        End If

                                        If pFeatureCursor.FindField("OBS") = -1 Then
                                            v_obs = " "
                                        Else
                                            v_obs = pFeature.Value(pFeatureCursor.FindField("OBS")).ToString
                                        End If
                                        If pFeatureCursor.FindField("NORMA") = -1 Then
                                            v_norma = " "
                                        Else
                                            v_norma = pFeature.Value(pFeatureCursor.FindField("NORMA")).ToString
                                        End If
                                        If pFeatureCursor.FindField("ARCHIVO") = -1 Then
                                            v_archivo = " "
                                        Else
                                            v_archivo = pFeature.Value(pFeatureCursor.FindField("ARCHIVO")).ToString
                                            v_archivo = UCase(v_archivo)
                                        End If

                                        If pFeatureCursor.FindField("FEC_ING") = -1 Then
                                            v_fechaing = " "
                                        Else
                                            v_fechaing = pFeature.Value(pFeatureCursor.FindField("FEC_ING")).ToString
                                        End If

                                        If pFeatureCursor.FindField("ENTIDAD") = -1 Then
                                            v_entidad = " "
                                        Else
                                            v_entidad = pFeature.Value(pFeatureCursor.FindField("ENTIDAD")).ToString
                                        End If

                                        If pFeatureCursor.FindField("USO") = -1 Then
                                            v_uso = " "
                                        Else
                                            v_uso = pFeature.Value(pFeatureCursor.FindField("USO")).ToString
                                        End If

                                        If pFeatureCursor.FindField("EST_GRAF") = -1 Then
                                            v_estado = " "
                                        Else
                                            v_estado = pFeature.Value(pFeatureCursor.FindField("EST_GRAF")).ToString
                                        End If

                                        If pFeatureCursor.FindField("LEYENDA") = -1 Then
                                            v_leyenda = " "
                                        Else
                                            ' v_estado = pFeature.Value(pFeatureCursor.FindField("ESTADO")).ToString
                                            v_leyenda = pFeature.Value(pFeatureCursor.FindField("LEYENDA")).ToString
                                        End If

                                    ElseIf tipo_catanominero = "ZONA URBANA" Then

                                        If pFeatureCursor.FindField("CODIGO") = -1 Then
                                            v_cdrese = " "
                                        Else
                                            v_cdrese = pFeature.Value(pFeatureCursor.FindField("CODIGO")).ToString
                                        End If
                                        If pFeatureCursor.FindField("NM_URBA") = -1 Then
                                            v_nmrese = " "
                                        Else
                                            v_nmrese = pFeature.Value(pFeatureCursor.FindField("NM_URBA")).ToString
                                        End If
                                        If pFeatureCursor.FindField("NOMBRE") = -1 Then
                                            v_nombre = " "
                                        Else
                                            v_nombre = pFeature.Value(pFeatureCursor.FindField("NOMBRE")).ToString
                                        End If
                                        If pFeatureCursor.FindField("HAS") = -1 Then
                                            v_area = 0.0
                                        Else
                                            v_area = pFeature.Value(pFeatureCursor.FindField("HAS")).ToString
                                        End If
                                        If pFeatureCursor.FindField("TP_URBA") = -1 Then
                                            v_tprese = " "
                                        Else
                                            v_tprese = pFeature.Value(pFeatureCursor.FindField("TP_URBA")).ToString
                                        End If
                                        If pFeatureCursor.FindField("CATEGORI") = -1 Then
                                            v_categori = " "
                                        Else
                                            v_categori = pFeature.Value(pFeatureCursor.FindField("CATEGORI")).ToString
                                        End If
                                        If pFeatureCursor.FindField("ORDENANZA") = -1 Then
                                            v_clase = " "
                                        Else
                                            v_clase = pFeature.Value(pFeatureCursor.FindField("ORDENANZA")).ToString
                                        End If

                                        'If pFeatureCursor.FindField("TITULAR") = -1 Then
                                        'v_titular = " "
                                        'Else
                                        '   v_titular = pFeature.Value(pFeatureCursor.FindField("TITULAR")).ToString
                                        'End If
                                        If pFeatureCursor.FindField("ZONA") = -1 Then
                                            v_zona = " "
                                        Else
                                            v_zona = pFeature.Value(pFeatureCursor.FindField("ZONA")).ToString
                                        End If
                                        If pFeatureCursor.FindField("ZONEX") = -1 Then
                                            v_zonex = " "
                                        Else
                                            v_zonex = pFeature.Value(pFeatureCursor.FindField("ZONEX")).ToString
                                        End If

                                        If pFeatureCursor.FindField("OBS") = -1 Then
                                            v_obs = " "
                                        Else
                                            v_obs = pFeature.Value(pFeatureCursor.FindField("OBS")).ToString
                                        End If
                                        'If pFeatureCursor.FindField("NORMA") = -1 Then
                                        'v_norma = " "
                                        'Else
                                        '   v_norma = pFeature.Value(pFeatureCursor.FindField("NORMA")).ToString
                                        'End If
                                        If pFeatureCursor.FindField("ARCHIVO") = -1 Then
                                            v_archivo = " "
                                        Else
                                            v_archivo = pFeature.Value(pFeatureCursor.FindField("ARCHIVO")).ToString
                                            v_archivo = UCase(v_archivo)
                                        End If

                                        If pFeatureCursor.FindField("FEC_ING") = -1 Then
                                            v_fechaing = " "
                                        Else
                                            v_fechaing = pFeature.Value(pFeatureCursor.FindField("FEC_ING")).ToString
                                        End If

                                        If pFeatureCursor.FindField("ENTIDAD") = -1 Then
                                            v_entidad = " "
                                        Else
                                            v_entidad = pFeature.Value(pFeatureCursor.FindField("ENTIDAD")).ToString
                                        End If

                                        If pFeatureCursor.FindField("USO") = -1 Then
                                            v_uso = " "
                                        Else
                                            v_uso = pFeature.Value(pFeatureCursor.FindField("USO")).ToString
                                        End If

                                        'If pFeatureCursor.FindField("ESTADO") = -1 Then
                                        If pFeatureCursor.FindField("EST_GRAF") = -1 Then
                                            v_estado = " "
                                        Else
                                            ' v_estado = pFeature.Value(pFeatureCursor.FindField("ESTADO")).ToString
                                            v_estado = pFeature.Value(pFeatureCursor.FindField("EST_GRAF")).ToString
                                        End If

                                        If pFeatureCursor.FindField("LEYENDA") = -1 Then
                                            v_leyenda = " "
                                        Else
                                            ' v_estado = pFeature.Value(pFeatureCursor.FindField("ESTADO")).ToString
                                            v_leyenda = pFeature.Value(pFeatureCursor.FindField("LEYENDA")).ToString
                                        End If
                                    End If
                                    pMap = pMxDoc.FocusMap
                                    pFeatureLayer = pMap.Layer(0)
                                    pOutFeatureClass = pFeatureLayer.FeatureClass

                                    pMap.DeleteLayer(pFeatureLayer)

                                    If v_tipoproceso = "INGRESAR" Then
                                        cls_catastro.CreaFeature(pOutFeatureClass, pFeature.Shape)

                                    ElseIf v_tipoproceso = "MODIFICAR" Then
                                        cls_catastro.CreaFeature(pOutFeatureClass, pFeature.Shape)

                                        If i = cadena Then
                                            If contador = 1 Then
                                                pQueryFilter = New QueryFilter
                                                pQueryFilter.WhereClause = "ARCHIVO = '" & nm_archivo & "'"

                                                pFeatureCursorpout = pOutFeatureClass.Update(pQueryFilter, True)
                                                pFeaturepout = pFeatureCursorpout.NextFeature
                                                pFeatureTable = pOutFeatureClass
                                                cls_catastro.Eliminafesture(pFeatureTable, pQueryFilter)
                                            End If
                                        Else
                                            If contador = 1 Then
                                                pQueryFilter = New QueryFilter
                                                pQueryFilter.WhereClause = "ARCHIVO = '" & nm_archivo & "'"

                                                pFeatureCursorpout = pOutFeatureClass.Update(pQueryFilter, True)
                                                pFeaturepout = pFeatureCursorpout.NextFeature
                                                pFeatureTable = pOutFeatureClass

                                                cls_catastro.Eliminafesture(pFeatureTable, pQueryFilter)

                                            End If
                                        End If
                                    End If

                                    '********************************
                                    If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                        ''insertando datos al total de reservas
                                        pQueryFilter = New QueryFilter
                                        If tipo_areas = "ReseUTM" Then
                                            'pQueryFilter.WhereClause = "CODIGO = ''"
                                            pQueryFilter.WhereClause = "CODIGO IS NULL"
                                        ElseIf tipo_areas = "ReseGeo" Then
                                            pQueryFilter.WhereClause = "CODIGO IS NULL"
                                        End If

                                        pFeatureCursorpout = pOutFeatureClass.Update(pQueryFilter, True)
                                        pFeaturepout = pFeatureCursorpout.NextFeature
                                        Do While Not pFeaturepout Is Nothing
                                            If tipo_catanominero = "AREA RESERVADA" Then
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("NM_RESE")) = v_nmrese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("CODIGO")) = v_cdrese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("NOMBRE")) = v_nombre
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("HAS")) = v_area
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("TP_RESE")) = v_tprese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("CATEGORI")) = v_categori
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("CLASE")) = v_clase
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ZONA")) = v_zona
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ZONEX")) = v_zonex
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("OBS")) = v_obs
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("NORMA")) = v_norma
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ARCHIVO")) = v_archivo
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("FEC_ING")) = v_fechaing
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ENTIDAD")) = v_entidad
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("USO")) = v_uso
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                'pFeaturepout.Value(pFeatureCursorpout.FindField("ESTADO")) = v_estado
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("EST_GRAF")) = v_estado
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("LEYENDA")) = v_leyenda
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout = pFeatureCursorpout.NextFeature
                                                'pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                'pFeaturepout = pFeatureCursorpout.NextFeature
                                            ElseIf tipo_catanominero = "ZONA URBANA" Then
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("NM_URBA")) = v_nmrese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("CODIGO")) = v_cdrese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("NOMBRE")) = v_nombre
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("HAS")) = v_area
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("TP_URBA")) = v_tprese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("CATEGORI")) = v_categori
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ORDENANZA")) = v_clase
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                'pFeaturepout.Value(pFeatureCursorpout.FindField("ZONA")) = v_zona
                                                'pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ZONEX")) = v_zonex
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("OBS")) = v_obs
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                'pFeaturepout.Value(pFeatureCursorpout.FindField("NORMA")) = v_norma
                                                'pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ARCHIVO")) = v_archivo
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("FEC_ING")) = v_fechaing
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ENTIDAD")) = v_entidad
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("USO")) = v_uso
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("EST_GRAF")) = v_estado
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("LEYENDA")) = v_leyenda
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout = pFeatureCursorpout.NextFeature
                                            End If
                                        Loop
                                    End If
                                    pFeature = pFeatureCursor.NextFeature
                                Loop
                            Else
                                nm_archivo = UCase(nm_archivo)
                                pQueryFilter = New QueryFilter
                                pQueryFilter.WhereClause = "ARCHIVO = '" & nm_archivo & "'"
                                pFeatureCursorpout = pOutFeatureClass.Update(pQueryFilter, True)
                                pFeaturepout = pFeatureCursorpout.NextFeature
                                pFeatureTable = pOutFeatureClass
                                cls_catastro.Eliminafesture(pFeatureTable, pQueryFilter)
                            End If
                        Next i
                    End If
                Loop Until line Is Nothing
                archivo.Close()
            End Using

            pMxDoc.ActiveView.Refresh()
            MsgBox("Se Actualiz� Correctamente su informaci�n al GEODATABASE..., Verificar ", MsgBoxStyle.Information, "GEOCATMIN")
            btnvermapa.Enabled = True

        Catch err As Exception
            MsgBox("Error al insertar registro al Geeodatabase -- Comunicarse con OSI", MsgBoxStyle.Critical, "Observaci�n")
        Finally
        End Try
    End Sub


    Private Sub cbotipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbotipo.SelectedIndexChanged
        If Me.cbotipo.SelectedIndex = 0 Then
            MsgBox("Seleccione el tipo de caso a actualizar ..", MsgBoxStyle.Information, "[BBDGEOCATMIN]")
            Me.cbotipo.Focus()
            Exit Sub
        Else
            btnActualizar.Enabled = True
            btnActualizar.Visible = True
        End If
        v_tipoproceso = Me.cbotipo.Text
    End Sub

    Private Sub cboarea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboarea.SelectedIndexChanged
        If Me.cboarea.SelectedIndex = 0 Then
            MsgBox("Seleccione el tipo de caso a actualizar ..", MsgBoxStyle.Information, "[BBDGEOCATMIN]")
            Me.cbotipo.Focus()
            Exit Sub
        Else
            cbotipo.Enabled = True
        End If
        tipo_catanominero = Me.cboarea.Text
    End Sub

    Private Sub btnvermapa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnvermapa.Click
        If tipo_catanominero = "AREA RESERVADA" Then
            'cls_Catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_AREA_RESERVADA_18_1", m_application, "1", False)
            'cls_Catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_RES_RESERVADA_GEOPSAD56", m_application, "1", False)
            'cls_Catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_AREA_RESERVADA_17_1", m_application, "1", False)
            'cls_Catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_AREA_RESERVADA_19_1", m_application, "1", False)
            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ARE_AREA_RESERVADA_18", m_Application, "1", False)
            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ARE_AREA_RESERVADA_17", m_Application, "1", False)
            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ARE_AREA_RESERVADA_19", m_Application, "1", False)
            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada56, m_Application, "1", False)
        ElseIf tipo_catanominero = "ZONA URBANA" Then
            'cls_Catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZON_ZONA_URBANA_18_1", m_application, "1", False)
            'cls_Catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZUR_ZONA_URBANA_1", m_application, "1", False)
            'cls_Catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZON_ZONA_URBANA_17_1", m_application, "1", False)
            'cls_Catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZON_ZONA_URBANA_19_1", m_application, "1", False)
            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_18", m_Application, "1", False)
            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_17", m_Application, "1", False)
            cls_Catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_19", m_Application, "1", False)
            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana56, m_Application, "1", False)
        End If
        Me.Close()
    End Sub
    Private Sub FT_Cargar_Botones_EVA(ByVal lostrNombreDM As String)
        ' Dim clsWURBINA As New cls_wurbina
        ' clsWURBINA.FT_Botones_EVA("Catastro", m_Application)
    End Sub

    Private Sub rbt_NoVisualiza_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbt_NoVisualiza.CheckedChanged
        sele_denu = False
    End Sub

    Private Sub rbt_Visualiza_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbt_Visualiza.CheckedChanged
        sele_denu = True
    End Sub

    Private Sub txtEste_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEste.TextChanged

    End Sub

    Private Sub dgdDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgdDetalle.Click

    End Sub

    Private Sub clbLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clbLayer.SelectedIndexChanged

    End Sub

    Private Sub Cbo_Libdenu_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.txtConsulta.Text = ""
    End Sub

    Private Sub gbxDatosUbicacion_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gbxDatosUbicacion.Enter

    End Sub

    Private Sub Cbo_libdenu_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_libdenu.SelectedIndexChanged
        If Cbo_libdenu.Text = "C�digo DM" Then
            Tipo_lib = "Individual"
            If tipo_seleccion = "OP_19" Then
                btneliminar.Enabled = False
                btneliminar.Visible = False
                btnlimpiar.Enabled = False
                btnlimpiar.Visible = False
                lst_listadm.Enabled = False
                lst_listadm.Visible = False
                btnBuscar.Enabled = True
                Me.lbl_lista.Visible = False : Me.lbl_lista.Enabled = False
                lbl_Previo.Visible = True
                lstCoordenada.Visible = True

            End If


        ElseIf Cbo_libdenu.Text = "Masivo" Then
            If tipo_seleccion = "OP_19" Then
                btneliminar.Enabled = True
                btneliminar.Visible = True
                btnlimpiar.Enabled = True
                btnlimpiar.Visible = True
                lst_listadm.Enabled = True
                lst_listadm.Visible = True
                btnBuscar.Enabled = True
                btnBuscar.Visible = True
                Me.txtRadio.Visible = False : Me.txtRadio.Enabled = False
                Me.cboZona.Visible = False : Me.cboZona.Enabled = False
                lstCoordenada.Enabled = False : lstCoordenada.Visible = False
                Me.lbl_lista.Visible = True : Me.lbl_lista.Enabled = True
                lbl_Previo.Visible = False

            Else
                Tipo_lib = "Masivo"
                btnvermapa.Enabled = False
                btnActualizar.Enabled = False
                btnvermapa.Visible = False
                btnActualizar.Visible = False
                img_DM1.Visible = False
                cboarea.Visible = False
                cbotipo.Visible = False
                txtArea.Visible = False
                lblArea.Visible = False

                lbl_nombre1.Visible = False
                lbl_nombre2.Visible = False
                Me.cboZona.Location = New System.Drawing.Point(392, 113)
                Me.lblZona.Location = New System.Drawing.Point(392, 98)
                'Me.lblDato.Visible = False : Me.lblDato.Enabled = False
                Me.lblDato.Visible = True : Me.lblDato.Enabled = True
                Me.lblConsultar.Visible = False : Me.lblConsultar.Enabled = False
                ' Me.txtConsulta.Visible = False : Me.txtConsulta.Enabled = False
                Me.txtConsulta.Visible = True : Me.txtConsulta.Enabled = True
                Me.cboConsulta.Visible = False : Me.cboConsulta.Enabled = False
                'Me.txtEste.Visible = False : Me.txtEste.Enabled = False
                'Me.txtNorte.Visible = False : Me.txtNorte.Enabled = False
                Me.btnLimpia.Visible = False : Me.btnLimpia.Visible = False
                Me.btnElimina.Visible = False : Me.btnElimina.Enabled = False
                Me.btnGenera_Poligono.Visible = False : Me.btnGenera_Poligono.Enabled = False
                'Me.lblZona.Location = New System.Drawing.Point(392, 103)
                Me.txtRadio.Visible = True : Me.txtRadio.Enabled = True
                Me.cboZona.Visible = True : Me.cboZona.Enabled = True
                Me.lbl_Previo.Visible = True : Me.lbl_Previo.Enabled = True

                Me.lblRadio.Visible = True : Me.lblRadio.Enabled = True
                Me.lblZona.Visible = True : Me.lblZona.Enabled = True
                ' Me.lbt_dato1.Visible = True : Me.lbt_dato1.Enabled = True
                ' Me.lbt_dato2.Visible = True : Me.lbt_dato2.Enabled = True
                ' Me.lbt_dato1.Text = "Este:" : Me.lbt_dato2.Text = "Norte:"
                Me.btnBuscar.Visible = False : Me.btnBuscar.Enabled = False
                'cboConsulta.Items.Clear()
                clbLayer.Enabled = True
                cboConsulta.Enabled = False
                'txtConsulta.Enabled = False
                cboConsulta.Visible = False

                btnBuscar.Enabled = True
                'Txtdato5.Visible = False
                'Txtdato6.Visible = False
                btnagregar.Visible = False
                txtRadio.Enabled = True
                'txtdato1.Visible = True : txtdato1.Enabled = True
                'txtdato2.Visible = True : txtdato2.Enabled = True
                'txtdato3.Visible = False : txtdato3.Enabled = False
                'txtdato4.Visible = False : txtdato4.Enabled = False
                lstCoordenada.Visible = False
                'End If

                'If tipo_seleccion = "OP_19" Then
                '    cboZona.Visible = True
                '    txtRadio.Visible = True
                'Else
                cboZona.Enabled = True
                txtRadio.Enabled = True
            End If


            btnBuscar.Enabled = True

            btnBuscar.Enabled = True
            btnBuscar.Visible = True
            btnGraficar.Enabled = False
            btnReporte.Enabled = False
            'tipo_seleccion = "OP_16"
            Me.btnGraficar.Enabled = True
            Me.btnGraficar.Visible = True
            img_DM.ImageLocation = glo_pathPNG & "\b_carta.png"

        End If
    End Sub

    Private Sub cboSistema_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboSistema.KeyPress


    End Sub

    Private Sub cboSistema_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSistema.SelectedIndexChanged

        btnBuscar.Enabled = True

        txtConsulta.Enabled = True
        '   cboSistema.SelectedIndex = 0
        v_sistema = cboSistema.SelectedItem.ToString
        If v_sistema = "PSAD-56" Then
            V_Sistemasele = "01"
            carpeta_datum = "PSAD_56"

            glo_PathGDB = glo_Path & "bdgeocatmin.mdb"
            Datum_PSAD_17 = pSpatialReferenceEnvelope.CreateProjectedCoordinateSystem(esriSRProjCSType.esriSRProjCS_PSADUTM_17S)
            Datum_PSAD_18 = pSpatialReferenceEnvelope.CreateProjectedCoordinateSystem(esriSRProjCSType.esriSRProjCS_PSADUTM_18S)
            Datum_PSAD_19 = pSpatialReferenceEnvelope.CreateProjectedCoordinateSystem(esriSRProjCSType.esriSRProjCS_PSADUTM_19S)

            gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO"
            'gstrFC_Departamento = "GPO_DEP_DEPARTAMENTO_CAT"
            gstrFC_Departamento_Z = "GPO_DEP_DEPARTAMENTO_"

            ' gstrFC_Provincia = "GPO_PRO_PROVINCIA_CAT"
            gstrFC_Provincia_Z = "GPO_PRO_PROVINCIA_"
            gstrFC_Distrito_Z = "GPO_DIS_DISTRITO_"
            'gstrFC_Distrito = "GPO_DIS_DISTRITO_CAT"
            gstrFC_Rios = "GLI_RIO_RIOS"
            gstrFC_Carretera = "GLI_VIA_VIAS"
            gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO"
            gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
            gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL"
            gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_"
            gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_"
            gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G56"
            gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G56"
            gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE"
            gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_"
            gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K"
            gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K"
            gstrFC_LHojas = "GPO_HOJ_HOJAS"
            gstrFC_Carta = "GPO_HOJ_HOJAS"

        ElseIf v_sistema = "WGS-84" Then
            carpeta_datum = "WGS_84"
            V_Sistemasele = "02"
            glo_PathGDB = glo_Path & "bdgeocatmin_84.mdb"
            Datum_PSAD_17 = pSpatialReferenceEnvelope.CreateProjectedCoordinateSystem(esriSRProjCSType.esriSRProjCS_WGS1984UTM_17S)
            Datum_PSAD_18 = pSpatialReferenceEnvelope.CreateProjectedCoordinateSystem(esriSRProjCSType.esriSRProjCS_WGS1984UTM_18S)
            Datum_PSAD_19 = pSpatialReferenceEnvelope.CreateProjectedCoordinateSystem(esriSRProjCSType.esriSRProjCS_WGS1984UTM_19S)

            gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
            gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
            gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
            gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
            gstrFC_Rios = "GLI_RIO_RIOS_18"
            gstrFC_Carretera = "GLI_VIA_VIAS_18"
            gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
            '  gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
            'gstrFC_Catastro_Minero = "CQUI0543.GPO_CMI_CATASTRO_MINERO_WGS_"
            gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
            gstrFC_Cuadricula = "GPO_CRE_CUADRICULA_REGIONAL_18"
            'gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
            'gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
            gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
            gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
            gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
            gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
            gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
            gstrFC_Frontera_Z = "GLI_FRO_FRONTERA_WGS_"
            gstrFC_Frontera_10 = "GLI_FRO_FRONTERA_10K_WGS18"
            gstrFC_Frontera_25 = "GLI_FRO_FRONTERA_25K_WGS18"
            gstrFC_LHojas = "GPO_HOJ_HOJAS_"
            gstrFC_Carta = "GPO_HOJ_HOJAS_"
            'ElseIf v_sistema = "WGS84_1" Then
            '    glo_PathGDB = glo_Path & "bdgeocatmin_84.mdb"
            '    Datum_PSAD_17 = pSpatialReferenceEnvelope.CreateProjectedCoordinateSystem(esriSRProjCSType.esriSRProjCS_WGS1984UTM_17S)
            '    Datum_PSAD_18 = pSpatialReferenceEnvelope.CreateProjectedCoordinateSystem(esriSRProjCSType.esriSRProjCS_WGS1984UTM_18S)
            '    Datum_PSAD_19 = pSpatialReferenceEnvelope.CreateProjectedCoordinateSystem(esriSRProjCSType.esriSRProjCS_WGS1984UTM_19S)

            '    gstrFC_CDistrito = "GPO_CDI_CAPITAL_DISTRITO_18"
            '    gstrFC_Departamento_WGS = "GPO_DEP_DEPARTAMENTO_WGS_"
            '    gstrFC_Provincia_WGS = "GPO_PRO_PROVINCIA_WGS_"
            '    gstrFC_Distrito_WGS = "GPO_DIS_DISTRITO_WGS_"
            '    gstrFC_Rios = "GLI_RIO_RIOS_18"
            '    gstrFC_Carretera = "GLI_VIA_VIAS_18"
            '    gstrFC_CPoblado = "GPT_CPO_CENTRO_POBLADO_18"
            '    '  gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
            '    gstrFC_Catastro_Minero = "CQUI0543.GPO_CMI_CATASTRO_MINERO_WGS_"
            '    'gstrFC_Cuadricula = ""
            '    gstrFC_AReservada = "GPO_ARE_AREA_RESERVADA_WGS_"
            '    gstrFC_ZUrbana = "GPO_ZUR_ZONA_URBANA_WGS_"
            '    gstrFC_ZUrbana56 = "GPO_ZUR_ZONA_URBANA_G84"
            '    gstrFC_AReservada56 = "GPO_ARE_AREA_RESERVADA_G84"
            '    gstrFC_ZTraslape = "GPO_ZTR_ZONA_TRASLAPE_18"
            '    gstrFC_Frontera_WGS = "GLI_FRO_FRONTERA_18"
            '    gstrFC_LHojas = "GPO_HOJ_HOJAS_"
            '    gstrFC_Carta = "GPO_HOJ_HOJAS_"
        Else
            MsgBox("No Selecciono el Datum", MsgBoxStyle.Critical, "Observaci�n...")
            Exit Sub
        End If
        Me.txtConsulta.Focus()
    End Sub

    Private Sub lbl_sistema_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_sistema.Click

    End Sub

    Private Sub yMax_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub xMax_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pMap = pMxDoc.FocusMap
        Dim cls_catastro As New cls_DM_1
        cls_catastro.Borra_Todo_Feature("", m_Application)

        Dim pQueryFilter As IQueryFilter
        Dim pFeatureCursor As IFeatureCursor
        Dim pFeature As IFeature
        Dim pFeatureCursorpout As IFeatureCursor
        Dim pFeaturepout As IFeature

        Dim v_nmrese As String = ""
        Dim v_cdrese As String = ""
        Dim v_nombre As String = ""
        Dim v_area As Double = 0.0
        Dim v_tprese As String = ""
        Dim v_categori As String = ""
        Dim v_clase As String = ""
        Dim v_titular As String = ""
        Dim v_zona As String = ""
        Dim v_zonex As String = ""
        Dim v_obs As String = ""
        Dim v_norma As String = ""
        Dim v_archivo As String = ""
        Dim v_fechaing As String = ""
        Dim v_entidad As String = ""
        Dim v_uso As String = ""
        Dim v_estado As String = ""
        Dim canti_reg As Integer
        Dim lostrObervacion As String = ""
        Dim cadena As String
        Dim nm_archivo As String
        Dim cls_consulta As New cls_DM_1
        Dim fecha As String
        Dim pFeatureTable As ITable
        Dim contador As Integer
        Dim MyDate As Date
        Dim v_leyenda As String = ""
        MyDate = Now

        Dim oSW As New StreamWriter(glo_pathTMP & "\prueba.txt")
        Dim Linea As String = ""

        fecha = RellenarComodin(MyDate.Day, 2, "0") & RellenarComodin(MyDate.Month, 2, "0") & RellenarComodin(MyDate.Year, 2, "0")
        Using archivo1 As IO.StreamReader = New IO.StreamReader("C:\" & fecha & ".txt")
            Dim line As String
            Do
                line = archivo1.ReadLine
                If (line <> "") Then
                    cadena = Microsoft.VisualBasic.Left(line, 1)
                    If (cadena = "s") Then  'solo caso featureclass
                        nm_archivo = Microsoft.VisualBasic.Right(line, Len(line) - 2)
                    End If

                    If line = "17" Or line = "18" Or line = "19" Then
                        oSW.WriteLine(nm_archivo & line)
                        oSW.Flush()
                    End If
                End If
            Loop Until line Is Nothing
            archivo1.Close()
        End Using
        oSW.Close()
        v_tipoproceso = "INGRESAR"
        Try
            nm_archivo = UCase(nm_archivo)
            Using archivo As IO.StreamReader = New IO.StreamReader(glo_pathTMP & "\prueba.txt")
                Dim line As String
                Do
                    line = archivo.ReadLine
                    If (line <> "") Then
                        cadena = Microsoft.VisualBasic.Right(line, 2)
                        nm_archivo = Microsoft.VisualBasic.Left(line, Len(line) - 2)
                        nm_archivo = UCase(nm_archivo)

                        For i As Integer = 17 To 19
                            If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Or v_tipoproceso = "ELIMINAR" Then
                                If i = cadena Then
                                    v_zona_dm = i
                                    contador = 1
                                    tipo_areas = "ReseUTM"

                                    If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                        'cls_consulta.Add_capa_catnomin(nm_archivo, m_Application, fecha, ruta_shputm & fecha)
                                        cls_consulta.Add_capa_catnomin(nm_archivo, m_Application, fecha, ruta_shp)
                                    End If
                                    If tipo_catanominero = "AREA RESERVADA" Then
                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_AREA_RESERVADA_" & cadena, m_Application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & cadena, m_Application, "1", False)
                                        ' cls_catastro.PT_CargarFeatureClass_SDE("FLAT1065.GPO_ARE_AREA_RESERVADA_17", m_Application, "1", False)
                                    ElseIf tipo_catanominero = "ZONA URBANA" Then
                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZON_ZONA_URBANA_" & cadena, m_application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & cadena, m_Application, "1", False)
                                    End If
                                ElseIf i = 20 Then
                                    contador = 1
                                    tipo_areas = "ReseGeo"
                                    If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                        cls_consulta.Add_capa_catnomin(nm_archivo & "g", m_Application, fecha, ruta_shp)
                                    End If

                                    If tipo_catanominero = "AREA RESERVADA" Then
                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_AREA_RESERVADA_G56", m_Application, "1", False)
                                        'cls_catastro.PT_CargarFeatureClass_SDE("FLAT1065.GPO_ARE_AREA_RESERVADA_G56", m_Application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & cadena, m_Application, "1", False)

                                    ElseIf tipo_catanominero = "ZONA URBANA" Then

                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZUR_ZONA_URBANA_G56", m_application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE("GIS.GPO_ZUR_ZONA_URBANA_G56", m_Application, "1", False)
                                    End If
                                ElseIf i = 21 Then
                                    contador = 1
                                    tipo_areas = "ReseGeo"
                                    If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                        cls_consulta.Add_capa_catnomin(nm_archivo & "w", m_Application, fecha, ruta_shp)
                                    End If
                                    If tipo_catanominero = "AREA RESERVADA" Then
                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_RESERVADA_G84", m_Application, "1", False)

                                        cls_catastro.PT_CargarFeatureClass_SDE("GPO_ARE_AREA_RESERVADA_G84", m_Application, "1", False)
                                    ElseIf tipo_catanominero = "ZONA URBANA" Then
                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZUR_ZONA_URBANA_G84", m_application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE("GPO_ZUR_ZONA_URBANA_G84", m_Application, "1", False)
                                    End If
                                Else
                                    contador = 1
                                    tipo_areas = "ReseUTM"
                                    If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                        cls_consulta.Add_capa_catnomin(nm_archivo & i, m_Application, fecha, ruta_shp)
                                    End If
                                    If tipo_catanominero = "AREA RESERVADA" Then
                                        'cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ARE_AREA_RESERVADA_" & i, m_Application, "1", False)
                                        ' cls_catastro.PT_CargarFeatureClass_SDE("FLAT1065.GPO_ARE_AREA_RESERVADA_" & i, m_Application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_AReservada & i, m_Application, "1", False)
                                    ElseIf tipo_catanominero = "ZONA URBANA" Then
                                        ' cls_catastro.PT_CargarFeatureClass_SDE("CQUI0543.GPO_ZUR_ZONA_URBANA_" & i, m_application, "1", False)
                                        cls_catastro.PT_CargarFeatureClass_SDE(gstrFC_ZUrbana & i, m_Application, "1", False)
                                    End If
                                End If
                                'pMap = pMxDoc.FocusMap
                                'pFeatureLayer = pMap.Layer(0)
                                'pOutFeatureClass = pFeatureLayer.FeatureClass
                                'pMap.DeleteLayer(pFeatureLayer)
                            End If
                            If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                'cls_consulta.Add_capa_catnomin(nm_archivo, m_application, fecha)
                                'pQueryFilter = New QueryFilter
                                'pQueryFilter.WhereClause = "ARCHIVO = '" & nm_archivo & "'"
                                'pFeatureCursorpout = pOutFeatureClass.Update(pQueryFilter, True)
                                'pFeaturepout = pFeatureCursorpout.NextFeature
                                'pFeatureTable = pOutFeatureClass
                                nm_archivo = UCase(nm_archivo)
                                If pFeatureLayeras_rese.FeatureClass.FeatureCount(Nothing) = 0 Then
                                    MsgBox("No hay registros en la capa ", MsgBoxStyle.Critical, "Observaci�n...")
                                    Exit Sub
                                Else
                                    canti_reg = pFeatureLayeras_rese.FeatureClass.FeatureCount(Nothing)
                                End If
                                pQueryFilter = New QueryFilter
                                pFeatureCursor = pInFeatureClass.Search(pQueryFilter, True)

                                pFeature = pFeatureCursor.NextFeature
                                Do While Not pFeature Is Nothing
                                    contador = contador + 1
                                    If tipo_catanominero = "AREA RESERVADA" Then
                                        If pFeatureCursor.FindField("CODIGO") = -1 Then
                                            v_cdrese = " "
                                        Else
                                            v_cdrese = pFeature.Value(pFeatureCursor.FindField("CODIGO")).ToString
                                        End If
                                        If pFeatureCursor.FindField("NM_RESE") = -1 Then
                                            v_nmrese = " "
                                        Else
                                            v_nmrese = pFeature.Value(pFeatureCursor.FindField("NM_RESE")).ToString
                                        End If
                                        If pFeatureCursor.FindField("NOMBRE") = -1 Then
                                            v_nombre = " "
                                        Else
                                            v_nombre = pFeature.Value(pFeatureCursor.FindField("NOMBRE")).ToString
                                        End If
                                        If pFeatureCursor.FindField("HAS") = -1 Then
                                            v_area = 0.0
                                        Else
                                            v_area = pFeature.Value(pFeatureCursor.FindField("HAS")).ToString
                                        End If
                                        If pFeatureCursor.FindField("TP_RESE") = -1 Then
                                            v_tprese = " "
                                        Else
                                            v_tprese = pFeature.Value(pFeatureCursor.FindField("TP_RESE")).ToString
                                        End If
                                        If pFeatureCursor.FindField("CATEGORI") = -1 Then
                                            v_categori = " "
                                        Else
                                            v_categori = pFeature.Value(pFeatureCursor.FindField("CATEGORI")).ToString
                                        End If
                                        If pFeatureCursor.FindField("CLASE") = -1 Then
                                            v_clase = " "
                                        Else
                                            v_clase = pFeature.Value(pFeatureCursor.FindField("CLASE")).ToString
                                        End If

                                        If pFeatureCursor.FindField("TITULAR") = -1 Then
                                            v_titular = " "
                                        Else
                                            v_titular = pFeature.Value(pFeatureCursor.FindField("TITULAR")).ToString
                                        End If
                                        If pFeatureCursor.FindField("ZONA") = -1 Then
                                            v_zona = " "
                                        Else
                                            v_zona = pFeature.Value(pFeatureCursor.FindField("ZONA")).ToString
                                        End If
                                        If pFeatureCursor.FindField("ZONEX") = -1 Then
                                            v_zonex = " "
                                        Else
                                            v_zonex = pFeature.Value(pFeatureCursor.FindField("ZONEX")).ToString
                                        End If

                                        If pFeatureCursor.FindField("OBS") = -1 Then
                                            v_obs = " "
                                        Else
                                            v_obs = pFeature.Value(pFeatureCursor.FindField("OBS")).ToString
                                        End If
                                        If pFeatureCursor.FindField("NORMA") = -1 Then
                                            v_norma = " "
                                        Else
                                            v_norma = pFeature.Value(pFeatureCursor.FindField("NORMA")).ToString
                                        End If
                                        If pFeatureCursor.FindField("ARCHIVO") = -1 Then
                                            v_archivo = " "
                                        Else
                                            v_archivo = pFeature.Value(pFeatureCursor.FindField("ARCHIVO")).ToString
                                            v_archivo = UCase(v_archivo)
                                        End If

                                        If pFeatureCursor.FindField("FEC_ING") = -1 Then
                                            v_fechaing = " "
                                        Else
                                            v_fechaing = pFeature.Value(pFeatureCursor.FindField("FEC_ING")).ToString
                                        End If

                                        If pFeatureCursor.FindField("ENTIDAD") = -1 Then
                                            v_entidad = " "
                                        Else
                                            v_entidad = pFeature.Value(pFeatureCursor.FindField("ENTIDAD")).ToString
                                        End If

                                        If pFeatureCursor.FindField("USO") = -1 Then
                                            v_uso = " "
                                        Else
                                            v_uso = pFeature.Value(pFeatureCursor.FindField("USO")).ToString
                                        End If

                                        If pFeatureCursor.FindField("EST_GRAF") = -1 Then
                                            v_estado = " "
                                        Else
                                            v_estado = pFeature.Value(pFeatureCursor.FindField("EST_GRAF")).ToString
                                        End If

                                        If pFeatureCursor.FindField("LEYENDA") = -1 Then
                                            v_leyenda = " "
                                        Else
                                            ' v_estado = pFeature.Value(pFeatureCursor.FindField("ESTADO")).ToString
                                            v_leyenda = pFeature.Value(pFeatureCursor.FindField("LEYENDA")).ToString
                                        End If

                                    ElseIf tipo_catanominero = "ZONA URBANA" Then

                                        If pFeatureCursor.FindField("CODIGO") = -1 Then
                                            v_cdrese = " "
                                        Else
                                            v_cdrese = pFeature.Value(pFeatureCursor.FindField("CODIGO")).ToString
                                        End If
                                        If pFeatureCursor.FindField("NM_URBA") = -1 Then
                                            v_nmrese = " "
                                        Else
                                            v_nmrese = pFeature.Value(pFeatureCursor.FindField("NM_URBA")).ToString
                                        End If
                                        If pFeatureCursor.FindField("NOMBRE") = -1 Then
                                            v_nombre = " "
                                        Else
                                            v_nombre = pFeature.Value(pFeatureCursor.FindField("NOMBRE")).ToString
                                        End If
                                        If pFeatureCursor.FindField("HAS") = -1 Then
                                            v_area = 0.0
                                        Else
                                            v_area = pFeature.Value(pFeatureCursor.FindField("HAS")).ToString
                                        End If
                                        If pFeatureCursor.FindField("TP_URBA") = -1 Then
                                            v_tprese = " "
                                        Else
                                            v_tprese = pFeature.Value(pFeatureCursor.FindField("TP_URBA")).ToString
                                        End If
                                        If pFeatureCursor.FindField("CATEGORI") = -1 Then
                                            v_categori = " "
                                        Else
                                            v_categori = pFeature.Value(pFeatureCursor.FindField("CATEGORI")).ToString
                                        End If
                                        If pFeatureCursor.FindField("ORDENANZA") = -1 Then
                                            v_clase = " "
                                        Else
                                            v_clase = pFeature.Value(pFeatureCursor.FindField("ORDENANZA")).ToString
                                        End If

                                        'If pFeatureCursor.FindField("TITULAR") = -1 Then
                                        'v_titular = " "
                                        'Else
                                        '   v_titular = pFeature.Value(pFeatureCursor.FindField("TITULAR")).ToString
                                        'End If
                                        If pFeatureCursor.FindField("ZONA") = -1 Then
                                            v_zona = " "
                                        Else
                                            v_zona = pFeature.Value(pFeatureCursor.FindField("ZONA")).ToString
                                        End If
                                        If pFeatureCursor.FindField("ZONEX") = -1 Then
                                            v_zonex = " "
                                        Else
                                            v_zonex = pFeature.Value(pFeatureCursor.FindField("ZONEX")).ToString
                                        End If

                                        If pFeatureCursor.FindField("OBS") = -1 Then
                                            v_obs = " "
                                        Else
                                            v_obs = pFeature.Value(pFeatureCursor.FindField("OBS")).ToString
                                        End If
                                        'If pFeatureCursor.FindField("NORMA") = -1 Then
                                        'v_norma = " "
                                        'Else
                                        '   v_norma = pFeature.Value(pFeatureCursor.FindField("NORMA")).ToString
                                        'End If
                                        If pFeatureCursor.FindField("ARCHIVO") = -1 Then
                                            v_archivo = " "
                                        Else
                                            v_archivo = pFeature.Value(pFeatureCursor.FindField("ARCHIVO")).ToString
                                            v_archivo = UCase(v_archivo)
                                        End If

                                        If pFeatureCursor.FindField("FEC_ING") = -1 Then
                                            v_fechaing = " "
                                        Else
                                            v_fechaing = pFeature.Value(pFeatureCursor.FindField("FEC_ING")).ToString
                                        End If

                                        If pFeatureCursor.FindField("ENTIDAD") = -1 Then
                                            v_entidad = " "
                                        Else
                                            v_entidad = pFeature.Value(pFeatureCursor.FindField("ENTIDAD")).ToString
                                        End If

                                        If pFeatureCursor.FindField("USO") = -1 Then
                                            v_uso = " "
                                        Else
                                            v_uso = pFeature.Value(pFeatureCursor.FindField("USO")).ToString
                                        End If

                                        'If pFeatureCursor.FindField("ESTADO") = -1 Then
                                        If pFeatureCursor.FindField("EST_GRAF") = -1 Then
                                            v_estado = " "
                                        Else
                                            ' v_estado = pFeature.Value(pFeatureCursor.FindField("ESTADO")).ToString
                                            v_estado = pFeature.Value(pFeatureCursor.FindField("EST_GRAF")).ToString
                                        End If

                                        If pFeatureCursor.FindField("LEYENDA") = -1 Then
                                            v_leyenda = " "
                                        Else
                                            ' v_estado = pFeature.Value(pFeatureCursor.FindField("ESTADO")).ToString
                                            v_leyenda = pFeature.Value(pFeatureCursor.FindField("LEYENDA")).ToString
                                        End If
                                    End If
                                    pMap = pMxDoc.FocusMap
                                    pFeatureLayer = pMap.Layer(0)
                                    pOutFeatureClass = pFeatureLayer.FeatureClass
                                    pMap.DeleteLayer(pFeatureLayer)

                                    If v_tipoproceso = "INGRESAR" Then
                                        cls_catastro.CreaFeature(pOutFeatureClass, pFeature.Shape)


                                    ElseIf v_tipoproceso = "MODIFICAR" Then
                                        cls_catastro.CreaFeature(pOutFeatureClass, pFeature.Shape)

                                        If i = cadena Then
                                            If contador = 1 Then
                                                pQueryFilter = New QueryFilter
                                                pQueryFilter.WhereClause = "ARCHIVO = '" & nm_archivo & "'"

                                                pFeatureCursorpout = pOutFeatureClass.Update(pQueryFilter, True)
                                                pFeaturepout = pFeatureCursorpout.NextFeature
                                                pFeatureTable = pOutFeatureClass
                                                cls_catastro.Eliminafesture(pFeatureTable, pQueryFilter)
                                            End If
                                        Else
                                            If contador = 1 Then
                                                pQueryFilter = New QueryFilter
                                                pQueryFilter.WhereClause = "ARCHIVO = '" & nm_archivo & "'"

                                                pFeatureCursorpout = pOutFeatureClass.Update(pQueryFilter, True)
                                                pFeaturepout = pFeatureCursorpout.NextFeature
                                                pFeatureTable = pOutFeatureClass

                                                cls_catastro.Eliminafesture(pFeatureTable, pQueryFilter)

                                            End If
                                        End If
                                    End If

                                    '********************************
                                    If v_tipoproceso = "INGRESAR" Or v_tipoproceso = "MODIFICAR" Then
                                        ''insertando datos al total de reservas
                                        pQueryFilter = New QueryFilter
                                        If tipo_areas = "ReseUTM" Then
                                            'pQueryFilter.WhereClause = "CODIGO = ''"
                                            pQueryFilter.WhereClause = "CODIGO IS NULL"
                                        ElseIf tipo_areas = "ReseGeo" Then
                                            pQueryFilter.WhereClause = "CODIGO IS NULL"
                                        End If

                                        pFeatureCursorpout = pOutFeatureClass.Update(pQueryFilter, True)
                                        pFeaturepout = pFeatureCursorpout.NextFeature
                                        Do While Not pFeaturepout Is Nothing
                                            If tipo_catanominero = "AREA RESERVADA" Then
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("NM_RESE")) = v_nmrese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("CODIGO")) = v_cdrese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("NOMBRE")) = v_nombre
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("HAS")) = v_area
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("TP_RESE")) = v_tprese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("CATEGORI")) = v_categori
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("CLASE")) = v_clase
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ZONA")) = v_zona
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ZONEX")) = v_zonex
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("OBS")) = v_obs
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("NORMA")) = v_norma
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ARCHIVO")) = v_archivo
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("FEC_ING")) = v_fechaing
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ENTIDAD")) = v_entidad
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("USO")) = v_uso
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                'pFeaturepout.Value(pFeatureCursorpout.FindField("ESTADO")) = v_estado
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("EST_GRAF")) = v_estado
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("LEYENDA")) = v_leyenda
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout = pFeatureCursorpout.NextFeature
                                                'pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                'pFeaturepout = pFeatureCursorpout.NextFeature
                                            ElseIf tipo_catanominero = "ZONA URBANA" Then
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("NM_URBA")) = v_nmrese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("CODIGO")) = v_cdrese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("NOMBRE")) = v_nombre
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("HAS")) = v_area
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("TP_URBA")) = v_tprese
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("CATEGORI")) = v_categori
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ORDENANZA")) = v_clase
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                'pFeaturepout.Value(pFeatureCursorpout.FindField("ZONA")) = v_zona
                                                'pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ZONEX")) = v_zonex
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("OBS")) = v_obs
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                'pFeaturepout.Value(pFeatureCursorpout.FindField("NORMA")) = v_norma
                                                'pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ARCHIVO")) = v_archivo
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("FEC_ING")) = v_fechaing
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("ENTIDAD")) = v_entidad
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("USO")) = v_uso
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("EST_GRAF")) = v_estado
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout.Value(pFeatureCursorpout.FindField("LEYENDA")) = v_leyenda
                                                pFeatureCursorpout.UpdateFeature(pFeaturepout)
                                                pFeaturepout = pFeatureCursorpout.NextFeature
                                            End If
                                        Loop
                                    End If
                                    pFeature = pFeatureCursor.NextFeature
                                Loop
                            Else
                                nm_archivo = UCase(nm_archivo)
                                pQueryFilter = New QueryFilter
                                pQueryFilter.WhereClause = "ARCHIVO = '" & nm_archivo & "'"
                                pFeatureCursorpout = pOutFeatureClass.Update(pQueryFilter, True)
                                pFeaturepout = pFeatureCursorpout.NextFeature
                                pFeatureTable = pOutFeatureClass
                                cls_catastro.Eliminafesture(pFeatureTable, pQueryFilter)
                            End If
                        Next i
                    End If
                Loop Until line Is Nothing
                archivo.Close()
            End Using

            pMxDoc.ActiveView.Refresh()
            MsgBox("Se Actualiz� Correctamente su informaci�n al GEODATABASE..., Verificar ", MsgBoxStyle.Information, "GEOCATMIN")
            btnvermapa.Enabled = True

        Catch err As Exception
            MsgBox("Error al insertar registro al Geeodatabase -- Comunicarse con OSI", MsgBoxStyle.Critical, "Observaci�n")
        Finally
        End Try
    End Sub

    Function f_CalCular_Area(ByVal p_lodtbDetalle As DataTable) As String
        ReDim coordenada_DM(p_lodtbDetalle.Rows.Count + 2)
        For j = 0 To p_lodtbDetalle.Rows.Count - 1
            coordenada_DM(j).v = j + 1
            coordenada_DM(j).x = Format(Math.Round(p_lodtbDetalle.Rows(j).Item("CD_COREST")), "###,###.00")
            coordenada_DM(j).y = Format(Math.Round(p_lodtbDetalle.Rows(j).Item("CD_CORNOR")), "###,###.00")
        Next j
        'Calcular Area
        coordenada_DM(j).x = coordenada_DM(0).x : coordenada_DM(j).y = coordenada_DM(0).y
        Dim d0, d1, dr As Double
        d0 = 0 : d1 = 0 : dr = 0
        For h = 0 To j
            If coordenada_DM(h).x <> 0 Or coordenada_DM(h).y <> 0 Then
                d0 = d0 + coordenada_DM(h).x * coordenada_DM(h + 1).y
                d1 = d1 + coordenada_DM(h).y * coordenada_DM(h + 1).x
            Else
                Exit For
            End If
        Next h
        dr = Format(Math.Round(Math.Abs((d0 - d1) / 2) / 10000, 4))
        Return dr
    End Function

    Private Sub txtdato1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtdato1.TextChanged

    End Sub

    Private Sub cboAnio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAnio.SelectedIndexChanged

        'fecha_h = cboAnio.SelectedItem.ToString

        If Me.cboAnio.SelectedIndex = 0 Then
            'MsgBox("Seleccione A�o :", MsgBoxStyle.Critical, "Observaci�n...")
            'Exit Sub
        Else
            'fecha_h = Me.cboAnio.Text
            fecha_h = cboAnio.SelectedItem.ToString
            Me.txtConsulta.Focus()
        End If


        'Dim lodtbDetalle As New DataTable
        'Dim CLS_DM_1 As New cls_DM_1
        'Dim CLS_DM_2 As New cls_DM_2
        'If lo_Flag_DH = 0 Then Exit Sub


        'Dim loStrShapefile As String = "Poligono_" & DateTime.Now.Ticks.ToString()
        'CLS_DM_1.Create_Shapefile_DM_Prueba(glo_pathTMP, loStrShapefile, v_zona_dm, "Poligono")
        ''  CLS_DM_2.AddFieldDM_SHAPE(loStrShapefile)
        'Try
        '    lodtbDetalle = cls_Oracle.F_SEL_LISTA_DM_HISTORICO(txtConsulta.Text, cboAnio.SelectedValue.ToString)
        '    If lodtbDetalle.Rows.Count = 0 Then
        '        MsgBox(".. No Existe Regsitros, ", MsgBoxStyle.Information, "BDGEOCATMIN")
        '        DialogResult = Windows.Forms.DialogResult.OK : Exit Sub
        '    End If
        '    Dim lodtvOrdena_x As New DataView(lodtbDetalle, Nothing, "CD_COREST ASC", DataViewRowState.CurrentRows)
        '    xMin.Text = lodtvOrdena_x.Item(0).Row("CD_COREST")
        '    xMax.Text = lodtvOrdena_x.Item(lodtvOrdena_x.Count - 1).Row("CD_COREST")
        '    Dim lodtvOrdena_y As New DataView(lodtbDetalle, Nothing, "CD_CORNOR ASC", DataViewRowState.CurrentRows)
        '    yMin.Text = lodtvOrdena_y.Item(0).Row("CD_CORNOR")
        '    yMax.Text = lodtvOrdena_y.Item(lodtvOrdena_y.Count - 1).Row("CD_CORNOR")
        '    dgdDetalle.DataSource = lodtbDetalle
        '    CLS_DM_1.Borra_Todo_Feature("", m_Application)
        '    arch_cata = "union"
        '    CLS_DM_1.Load_FC_GDB("gpt_Vertice_DM", "", m_Application, True)
        '    CLS_DM_1.Delete_Rows_FC_GDB("gpt_Vertice_DM")
        '    CLS_DM_2.Genera_Polygono_DH(loStrShapefile, lodtbDetalle, 18, m_Application, True)
        '    cls_Catastro.ShowLabel_DM("gpt_Vertice_DM", m_Application)
        '    cls_Catastro.Color_Poligono_Simple(m_Application, "Poligono")
        '    cls_Catastro.Genera_Imagen_DM("Vista_Ubigeo", "VistaPrevia")
        '    img_DM.ImageLocation = glo_pathTMP & "\Vista_Ubigeo.jpg"
        '    cls_Catastro.Quitar_Layer("Poligono", m_Application)
        '    CLS_DM_1.Quitar_Layer("gpt_Vertice_DM", m_Application)
        '    lstCoordenada.Items.Add(" Vert. " & "        Este  " & "     " & "        Norte  ")
        '    lstCoordenada.Items.Add("--------------------------------------------")
        '    For i As Integer = 0 To lodtbDetalle.Rows.Count - 1
        '        lstCoordenada.Items.Add(" " & RellenarComodin(i + 1, 3, "0") & "   " & _
        '        Format(Math.Round(lodtbDetalle.Rows(i).Item("CD_COREST"), 3), "###,###.#0") & _
        '        "   " & Format(Math.Round(lodtbDetalle.Rows(i).Item("CD_CORNOR"), 3), "###,###.#0") & "")
        '    Next
        '    txtArea.Text = f_CalCular_Area(lodtbDetalle)
        'Catch ex As Exception
        'End Try
        'dgdDetalle.Enabled = False
        'txtArea.Enabled = True
        'cboZona.Enabled = True
        'txtRadio.Enabled = True
    End Sub

    Private Sub btn_cargarcuadriculas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_cargarcuadriculas.Click
        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Cuadricula, m_Application, "1", False)

        Dim pathcuadricula As String
        Dim pfeaturelayer As IFeatureLayer
        Dim pfields As IFields
        Dim pfeatureclass As IFeatureClass
        Dim VALOR_DEP_REG As String
        Dim V_ZONA_REG As String
        Dim pfeaturecursor As IFeatureCursor
        Dim pfeature As IFeature
        Dim afound As Boolean = False
        'pfeaturelayer = pMap.Layer(0)

        For A As Integer = 0 To pMap.LayerCount - 1
            If pMap.Layer(A).Name = "Cuadricula Regional" Then
                pfeaturelayer = pMap.Layer(A)
                afound = True
                Exit For
            End If
        Next A
        If Not afound Then
            MsgBox("No se encuentra la capa de Cuadricula regional, intentar de nuevo")
            Exit Sub
        End If

        pfeatureclass = pfeaturelayer.FeatureClass
        pfields = pfeatureclass.Fields
        pfeaturecursor = pfeatureclass.Search(Nothing, False)
        pfeature = pfeaturecursor.NextFeature
        Try
            Do Until pfeature Is Nothing
                VALOR_DEP_REG = pfeature.Value(pfeaturecursor.FindField("NM_DEPA"))
                If VALOR_DEP_REG = "MADRE DE DIOS" Then
                    VALOR_DEP_REG = "madrededios"
                ElseIf VALOR_DEP_REG = "SAN MARTIN" Then
                    VALOR_DEP_REG = "sanmartin"
                ElseIf VALOR_DEP_REG = "LIMA METROPOLITANA" Then
                    VALOR_DEP_REG = "limametropolitana"
                ElseIf VALOR_DEP_REG = "LA LIBERTAD" Then
                    VALOR_DEP_REG = "lalibertad"
                End If
                V_ZONA_REG = pfeature.Value(pfeaturecursor.FindField("ZONA"))
                pathcuadricula = VALOR_DEP_REG + V_ZONA_REG
                cls_Catastro.Add_ShapeFile_tmp1(pathcuadricula, m_Application)
                'cls_DM_1.AddFeatureShapeFile(loStrShapefile, m_Application)
                pfeature = pfeaturecursor.NextFeature
            Loop
            cls_Catastro.Quitar_Layer("Cuadricula Regional", m_Application)
            pMxDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, pLayer, Nothing)
            pMxDoc.UpdateContents()
        Catch ex As Exception
        End Try
        img_DM.ImageLocation = glo_pathPNG & "\evaluacion.png"
        glo_pathTMP = "C:\BDGEOCATMIN\Temporal"
        Me.Close()
    End Sub

    Private Sub btn_procesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_procesar.Click

        Dim v_fecsup As New DataTable

        Select Case Me.cbo_tipo.SelectedValue

            Case "OP_22"    'Demarcacion INEI

                Dim RetVal
                glo_Path_EXE = "U:\DATOS\SHAPE\regiones\data2013_CM\input\"
                RetVal = Shell(glo_Path_EXE & "dema2013.bat", 1)
                MsgBox("Una vez que termine de cerrar la ventana PROCESANDO," & vbNewLine & "Pulsar el bot�n: Cargar cuadr�culas", MsgBoxStyle.Information, "Aviso")
                Me.btn_cargarcuadriculas.Enabled = True
                'Exit Sub
                'Frm_Eval_segun_codigo.ShowDialog()
                'SetWindowLong(Frm_Eval_segun_codigo.Handle.ToInt32(), GWL_HWNDPARENT, m_Application.hWnd)

            Case "OP_23" 'Superposicion DM
                'fecsup = Format(dtpFecha1.Value)
                fecsup = Microsoft.VisualBasic.Left(Format(dtpFecha1.Value), 10)
                v_fecsup = cls_Oracle.FT_fecsup(fecsup)
                dgdResultado.DataSource = v_fecsup

                btnexp_excel.Enabled = True
                'Exit Sub

            Case "OP_24" 'DM Simultaneos
                cls_planos.CambiaADataView(m_Application)
                cls_Catastro.Borra_Todo_Feature("", m_Application)
                cls_Catastro.Limpiar_Texto_Pantalla(m_Application)

                Dim v_petsimul As New DataTable
                Dim v_gruposimul As New DataTable
                Dim v_pesird As New DataTable
                Dim v_dmsim As New DataTable
                Dim v_dmsim_1 As New DataTable
                Dim v_dmsim_2 As New DataTable
                Dim v_dmsimzo As New DataTable
                Dim v_fcnum As String = "0"
                Dim v_numdm As String = "0"
                Dim tipoex As String = ""
                Dim gstrFC_Catastro_Minero_S As String = ""
                Dim gstrFC_Catastro_Minero_RD As String = ""
                Dim fecdatum As Date = #4/7/2016#
                'Dim date2 As Date = #12/16/2012 12:00:52 AM#

                btnGraficar.Enabled = True
                btnGraficar.Visible = True
                tipo_opcion = ""
                v_codigo = ""
                pMxDoc = m_Application.Document
                pMap = pMxDoc.FocusMap
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                cls_eval.Eliminadataframe(caso_consulta)
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                pMxDoc.UpdateContents()

                fecsimul = Microsoft.VisualBasic.Left(Format(dtpFecha1.Value), 10)

                If DateTime.Parse(fecsimul) < fecdatum Then
                    v_sistema = "PSAD-56"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                    gstrFC_Catastro_Minero_S = "CEGO0891.GPO_CATASTRO_MINERO_S_"
                    gstrFC_Catastro_Minero_RD = "CEGO0891.GPO_CATASTRO_MINERO_S_RD_"
                    gstrFC_Cuadriculas = "GPO_CUA_CUADRICULAS_"
                    gstrFC_Cuadriculas_S = "CEGO0891.GPO_CUA_CUADRICULAS_S_"
                Else
                    v_sistema = "WGS-84"
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Catastro_Minero_S = "CEGO0891.GPO_CATASTRO_MINERO_SW_"
                    gstrFC_Catastro_Minero_RD = "CEGO0891.GPO_CATASTRO_MINERO_SW_RD_"
                    gstrFC_Cuadriculas = "GPO_CUA_CUADRICULAS_WGS_"
                    gstrFC_Cuadriculas_S = "CEGO0891.GPO_CUA_CUADRICULAS_SW_"
                End If

                v_fcnum = cls_Oracle.FT_FCNUM(fecsimul)

                If Val(v_fcnum) = 1 Then            '****************************************************************************************************************************


                    For i As Integer = 17 To 19     '*********************************************************************************************  
                        If i = 17 Then
                            v_zona_dm = "17"
                        ElseIf i = 18 Then
                            v_zona_dm = "18"
                        ElseIf i = 19 Then
                            v_zona_dm = "19"
                        End If
                        tipoex = "3"     ' obtener todos los DM seg�n fecha de la LD
                        Dim v_dmsimzo3 As New DataTable
                        v_dmsimzo3 = cls_Oracle.FT_dmsimzo(fecsimul, v_zona_dm, tipoex)              ' obtiene relacion de DM simultaneos por zona      
                        Dim codigo_eval3 As String
                        If v_dmsimzo3.Rows.Count > 0 Then
                            For contador As Integer = 0 To v_dmsimzo3.Rows.Count - 1
                                codigo_eval3 = v_dmsimzo3.Rows(contador).Item("CODIGOU")
                                If contador = 0 Then
                                    lista_codigo_simu = "CODIGOU =  '" & codigo_eval3 & "'"
                                ElseIf contador > 0 Then
                                    lista_codigo_simu = lista_codigo_simu & " OR " & "CODIGOU =  '" & codigo_eval3 & "'"
                                End If
                            Next contador
                            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero & v_zona_dm, m_Application, "1", False)
                            cls_eval.consultacapaDM("", "Simultaneo_i", "Catastro")
                            lista_codigo_simu = ""
                            'Almacenando la data consultada de la maestra a la capa temporal 
                            pMap = pMxDoc.FocusMap
                            pFeatureLayer = pMap.Layer(0)
                            pInFeatureClass = pFeatureLayer.FeatureClass
                            pMap.DeleteLayer(pFeatureLayer)
                            pMap = pMxDoc.FocusMap
                            Dim cuenta_dm_ti As Integer
                            Dim contador1 As Integer
                            Dim v_codigou As String = ""
                            Dim v_concesion As String = ""
                            Dim v_titconces As String = ""
                            Dim v_hectagis As String = ""
                            Dim v_tipoex As String = ""
                            Dim v_fecdenu As String = ""
                            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero_S & v_zona_dm, m_Application, "1", False)  'psad 56
                            pMap = pMxDoc.FocusMap
                            pFeatureLayer = pMap.Layer(0)
                            pOutFeatureClass = pFeatureLayer.FeatureClass
                            pMap.DeleteLayer(pFeatureLayer)
                            pFeatureCursor = pInFeatureClass.Search(pQueryFilter_si, True)
                            cuenta_dm_ti = pInFeatureClass.FeatureCount(pQueryFilter_si)
                            pFeature = pFeatureCursor.NextFeature
                            Do Until pFeature Is Nothing
                                contador1 = contador1 + 1
                                If pFeatureCursor.FindField("CODIGOU") = -1 Then
                                    v_codigou = " "
                                Else
                                    v_codigou = pFeature.Value(pFeatureCursor.FindField("CODIGOU")).ToString
                                End If
                                If pFeatureCursor.FindField("CONCESION") = -1 Then
                                    v_concesion = " "
                                Else
                                    v_concesion = pFeature.Value(pFeatureCursor.FindField("CONCESION")).ToString
                                End If
                                If pFeatureCursor.FindField("TIT_CONCES") = -1 Then
                                    v_titconces = " "
                                Else
                                    v_titconces = pFeature.Value(pFeatureCursor.FindField("TIT_CONCES")).ToString
                                End If
                                If pFeatureCursor.FindField("HECTAGIS") = -1 Then
                                    v_hectagis = " "
                                Else
                                    v_hectagis = pFeature.Value(pFeatureCursor.FindField("HECTAGIS")).ToString
                                End If
                                If pFeatureCursor.FindField("TIPO_EX") = -1 Then
                                    v_tipoex = " "
                                Else
                                    v_tipoex = pFeature.Value(pFeatureCursor.FindField("TIPO_EX")).ToString
                                End If
                                If pFeatureCursor.FindField("FEC_DENU") = -1 Then
                                    v_fecdenu = " "
                                Else
                                    v_fecdenu = pFeature.Value(pFeatureCursor.FindField("FEC_DENU")).ToString
                                End If
                                'Esta parte crea el poligono nuevo
                                Dim pFeaturepout As IFeature
                                pFeaturepout = pOutFeatureClass.CreateFeature
                                pFeaturepout.Shape = pFeature.Shape
                                'Aumentando los campos que iran al registro nuevo
                                pFeaturepout.Value(pOutFeatureClass.FindField("CODIGOU")) = v_codigou
                                pFeaturepout.Value(pOutFeatureClass.FindField("CONCESION")) = v_concesion
                                pFeaturepout.Value(pOutFeatureClass.FindField("TIT_CONCES")) = v_titconces
                                pFeaturepout.Value(pOutFeatureClass.FindField("HECTAGIS")) = v_hectagis
                                pFeaturepout.Value(pOutFeatureClass.FindField("TIPO_EX")) = v_tipoex
                                pFeaturepout.Value(pOutFeatureClass.FindField("FEC_DENU")) = v_fecdenu
                                pFeaturepout.Store()  'Grabando
                                pFeature = pFeatureCursor.NextFeature  'Siguiente registro del total
                            Loop
                            pMxDoc.ActiveView.Refresh()
                        End If
                    Next i

                    For i As Integer = 17 To 19
                        If i = 17 Then
                            v_zona_dm = "17"
                        ElseIf i = 18 Then
                            v_zona_dm = "18"
                        ElseIf i = 19 Then
                            v_zona_dm = "19"
                        End If
                        tipoex = "4"     ' obtener todas las Cuadriculas Simultaneas seg�n fecha de la LD
                        Dim v_dmsimzo4 As New DataTable
                        v_dmsimzo4 = cls_Oracle.FT_cuadsimzo(fecsimul, v_zona_dm, tipoex)         '      >>>>      obtiene relacion de Cuadriculas simultaneos por zona
                        Dim cod_cdcuad As String
                        If v_dmsimzo4.Rows.Count > 0 Then
                            For contador As Integer = 0 To v_dmsimzo4.Rows.Count - 1
                                cod_cdcuad = v_dmsimzo4.Rows(contador).Item("CD_CUAD")
                                If contador = 0 Then
                                    lista_cod_cdcuad_simu = "CD_CUAD =  '" & cod_cdcuad & "'"
                                ElseIf contador > 0 Then
                                    lista_cod_cdcuad_simu = lista_cod_cdcuad_simu & " OR " & "CD_CUAD =  '" & cod_cdcuad & "'"
                                End If
                            Next contador
                            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Cuadriculas & v_zona_dm, m_Application, "1", False)
                            cls_eval.consultacapaDM("", "Cuadrisi", "Cuadriculas")
                            lista_cod_cdcuad_simu = ""
                            'Almacenando la data consultada de la maestra a la capa temporal 
                            pMap = pMxDoc.FocusMap
                            pFeatureLayer = pMap.Layer(0)
                            pInFeatureClass = pFeatureLayer.FeatureClass
                            pMap.DeleteLayer(pFeatureLayer)
                            pMap = pMxDoc.FocusMap
                            Dim cuenta_cuad_si As Integer
                            Dim contador1 As Integer
                            Dim v_cdcuad As String = ""
                            Dim v_zona As String = ""
                            'Dim v_titconces As String = ""
                            'Dim v_hectagis As String = ""
                            'Dim v_tipoex As String = ""
                            'Dim v_fecdenu As String = ""
                            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Cuadriculas_S & v_zona_dm, m_Application, "1", False)
                            pMap = pMxDoc.FocusMap
                            pFeatureLayer = pMap.Layer(0)
                            pOutFeatureClass = pFeatureLayer.FeatureClass
                            pMap.DeleteLayer(pFeatureLayer)
                            pFeatureCursor = pInFeatureClass.Search(pQueryFilter_si, True)
                            cuenta_cuad_si = pInFeatureClass.FeatureCount(pQueryFilter_si)
                            pFeature = pFeatureCursor.NextFeature
                            Do Until pFeature Is Nothing
                                contador1 = contador1 + 1
                                If pFeatureCursor.FindField("CD_CUAD") = -1 Then
                                    v_cdcuad = " "
                                Else
                                    v_cdcuad = pFeature.Value(pFeatureCursor.FindField("CD_CUAD")).ToString
                                End If
                                If pFeatureCursor.FindField("ZONA") = -1 Then
                                    v_zona = " "
                                Else
                                    v_zona = pFeature.Value(pFeatureCursor.FindField("ZONA")).ToString
                                End If

                                'Esta parte crea el poligono nuevo
                                Dim pFeaturepout As IFeature
                                pFeaturepout = pOutFeatureClass.CreateFeature
                                pFeaturepout.Shape = pFeature.Shape
                                'Aumentando los campos que iran al registro nuevo
                                pFeaturepout.Value(pOutFeatureClass.FindField("CD_CUAD")) = v_cdcuad
                                pFeaturepout.Value(pOutFeatureClass.FindField("ZONA")) = v_zona
                                pFeaturepout.Store()  'Grabando
                                pFeature = pFeatureCursor.NextFeature  'Siguiente registro del total
                            Loop
                            pMxDoc.ActiveView.Refresh()
                        End If
                    Next i
                    '***************

                    v_dmsim_1 = cls_Oracle.FT_cuad_simul_ld(fecsimul)     'GIS     cuadriculas simultaneas de los DMs
                    v_dmsim_2 = cls_Oracle.FT_grupo_dmsimul_ld(fecsimul)   'Grupos de DM Simultaneos

                    v_numdm = cls_Oracle.FT_ELIMINA_DM_GDB(fecsimul)     'GIS

                    For i As Integer = 17 To 19
                        If i = 17 Then
                            v_zona_dm = "17"
                        ElseIf i = 18 Then
                            v_zona_dm = "18"
                        ElseIf i = 19 Then
                            v_zona_dm = "19"
                        End If
                        tipoex = "1"
                        v_dmsimzo = cls_Oracle.FT_dmsimzo(fecsimul, v_zona_dm, tipoex)              ' obtiene relacion de DM simultaneos por zona
                        If v_dmsimzo.Rows.Count > 0 Then
                            Dim codigo_eval As String = ""
                            Select Case v_dmsimzo.Rows.Count
                                Case 0
                                    MsgBox("No Existe DM Simultaneos en esta zona con la fecha consultada en la Base de Datos", MsgBoxStyle.Information, "Observaci�n...")
                                    Exit Sub
                                Case Else
                                    If v_dmsimzo.Rows.Count > 0 Then
                                        For contador As Integer = 0 To v_dmsimzo.Rows.Count - 1
                                            codigo_eval = v_dmsimzo.Rows(contador).Item("CODIGOU")
                                            If contador = 0 Then
                                                lista_codigo_sim = "CODIGOU =  '" & codigo_eval & "'"
                                            ElseIf contador > 0 Then
                                                lista_codigo_sim = lista_codigo_sim & " OR " & "CODIGOU =  '" & codigo_eval & "'"
                                            End If
                                        Next contador
                                    End If
                                    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero & v_zona_dm, m_Application, "1", False)
                                    cls_eval.consultacapaDM("", "Simultaneot", "Catastro")
                                    lista_codigo_sim = ""
                            End Select

                            'Almacenando la data consultada de la maestra a la capa temporal 
                            '--------------------------------------------------------------------
                            pMap = pMxDoc.FocusMap
                            pFeatureLayer = pMap.Layer(0)
                            pInFeatureClass = pFeatureLayer.FeatureClass
                            pMap.DeleteLayer(pFeatureLayer)
                            pMap = pMxDoc.FocusMap
                            Dim cuenta_dm_ti As Integer
                            Dim contador1 As Integer
                            Dim v_codigou As String = ""
                            Dim v_concesion As String = ""
                            Dim v_titconces As String = ""
                            Dim v_hectagis As String = ""
                            Dim v_tipoex As String = ""
                            Dim v_fecdenu As String = ""
                            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero_S & v_zona_dm, m_Application, "1", False)
                            pMap = pMxDoc.FocusMap
                            pFeatureLayer = pMap.Layer(0)
                            pOutFeatureClass = pFeatureLayer.FeatureClass
                            pMap.DeleteLayer(pFeatureLayer)
                            pFeatureCursor = pInFeatureClass.Search(pQueryFilter_si, True)
                            cuenta_dm_ti = pInFeatureClass.FeatureCount(pQueryFilter_si)
                            pFeature = pFeatureCursor.NextFeature
                            Do Until pFeature Is Nothing
                                contador1 = contador1 + 1
                                If pFeatureCursor.FindField("CODIGOU") = -1 Then
                                    v_codigou = " "
                                Else
                                    v_codigou = pFeature.Value(pFeatureCursor.FindField("CODIGOU")).ToString
                                End If
                                If pFeatureCursor.FindField("CONCESION") = -1 Then
                                    v_concesion = " "
                                Else
                                    v_concesion = pFeature.Value(pFeatureCursor.FindField("CONCESION")).ToString
                                End If
                                If pFeatureCursor.FindField("TIT_CONCES") = -1 Then
                                    v_titconces = " "
                                Else
                                    v_titconces = pFeature.Value(pFeatureCursor.FindField("TIT_CONCES")).ToString
                                End If
                                If pFeatureCursor.FindField("HECTAGIS") = -1 Then
                                    v_hectagis = " "
                                Else
                                    v_hectagis = pFeature.Value(pFeatureCursor.FindField("HECTAGIS")).ToString
                                End If
                                If pFeatureCursor.FindField("TIPO_EX") = -1 Then
                                    v_tipoex = " "
                                Else
                                    v_tipoex = pFeature.Value(pFeatureCursor.FindField("TIPO_EX")).ToString
                                End If
                                If pFeatureCursor.FindField("FEC_DENU") = -1 Then
                                    v_fecdenu = " "
                                Else
                                    v_fecdenu = pFeature.Value(pFeatureCursor.FindField("FEC_DENU")).ToString
                                End If
                                'Esta parte crea el poligono nuevo
                                Dim pFeaturepout As IFeature
                                pFeaturepout = pOutFeatureClass.CreateFeature
                                pFeaturepout.Shape = pFeature.Shape
                                'Aumentando los campos que iran al registro nuevo
                                pFeaturepout.Value(pOutFeatureClass.FindField("CODIGOU")) = v_codigou
                                pFeaturepout.Value(pOutFeatureClass.FindField("CONCESION")) = v_concesion
                                pFeaturepout.Value(pOutFeatureClass.FindField("TIT_CONCES")) = v_titconces
                                pFeaturepout.Value(pOutFeatureClass.FindField("HECTAGIS")) = v_hectagis
                                pFeaturepout.Value(pOutFeatureClass.FindField("TIPO_EX")) = v_tipoex
                                pFeaturepout.Value(pOutFeatureClass.FindField("FEC_DENU")) = v_fecdenu
                                pFeaturepout.Store()  'Grabando
                                pFeature = pFeatureCursor.NextFeature  'Siguiente registro del total
                            Loop
                            pMxDoc.ActiveView.Refresh()
                        End If
                        '-------------------------------


                        'tipoex = "4"     ' obtener todas las Cuadriculas Simultaneas seg�n fecha de la LD
                        'Dim v_dmsimzo4a As New DataTable
                        'v_dmsimzo4a = cls_Oracle.FT_dmsimzo(fecsimul, v_zona_dm, tipoex)         '      >>>>      obtiene relacion de Cuadriculas simultaneos por zona
                        'Dim cod_cdcuad As String
                        'If v_dmsimzo4a.Rows.Count > 0 Then
                        '    For contador As Integer = 0 To v_dmsimzo4a.Rows.Count - 1
                        '        cod_cdcuad = v_dmsimzo4a.Rows(contador).Item("CD_CUAD")
                        '        If contador = 0 Then
                        '            lista_cod_cdcuad_simu = "CD_CUAD =  '" & cod_cdcuad & "'"
                        '        ElseIf contador > 0 Then
                        '            lista_cod_cdcuad_simu = lista_cod_cdcuad_simu & " OR " & "CD_CUAD =  '" & cod_cdcuad & "'"
                        '        End If
                        '    Next contador
                        '    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Cuadriculas & v_zona_dm, m_Application, "1", False)
                        '    cls_eval.consultacapaDM("", "Cuadrisi", "Cuadriculas")
                        '    lista_cod_cdcuad_simu = ""
                        '    'Almacenando la data consultada de la maestra a la capa temporal 
                        '    pMap = pMxDoc.FocusMap
                        '    pFeatureLayer = pMap.Layer(0)
                        '    pInFeatureClass = pFeatureLayer.FeatureClass
                        '    pMap.DeleteLayer(pFeatureLayer)
                        '    pMap = pMxDoc.FocusMap
                        '    Dim cuenta_cuad_si As Integer
                        '    Dim contador1 As Integer
                        '    Dim v_cdcuad As String = ""
                        '    Dim v_zona As String = ""
                        '    'Dim v_titconces As String = ""
                        '    'Dim v_hectagis As String = ""
                        '    'Dim v_tipoex As String = ""
                        '    'Dim v_fecdenu As String = ""
                        '    cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Cuadriculas_S & v_zona_dm, m_Application, "1", False)
                        '    pMap = pMxDoc.FocusMap
                        '    pFeatureLayer = pMap.Layer(0)
                        '    pOutFeatureClass = pFeatureLayer.FeatureClass
                        '    pMap.DeleteLayer(pFeatureLayer)
                        '    pFeatureCursor = pInFeatureClass.Search(pQueryFilter_si, True)
                        '    cuenta_cuad_si = pInFeatureClass.FeatureCount(pQueryFilter_si)
                        '    pFeature = pFeatureCursor.NextFeature
                        '    Do Until pFeature Is Nothing
                        '        contador1 = contador1 + 1
                        '        If pFeatureCursor.FindField("CD_CUAD") = -1 Then
                        '            v_cdcuad = " "
                        '        Else
                        '            v_cdcuad = pFeature.Value(pFeatureCursor.FindField("CD_CUAD")).ToString
                        '        End If
                        '        If pFeatureCursor.FindField("ZONA") = -1 Then
                        '            v_zona = " "
                        '        Else
                        '            v_zona = pFeature.Value(pFeatureCursor.FindField("ZONA")).ToString
                        '        End If

                        '        'Esta parte crea el poligono nuevo
                        '        Dim pFeaturepout As IFeature
                        '        pFeaturepout = pOutFeatureClass.CreateFeature
                        '        pFeaturepout.Shape = pFeature.Shape
                        '        'Aumentando los campos que iran al registro nuevo
                        '        pFeaturepout.Value(pOutFeatureClass.FindField("CD_CUAD")) = v_cdcuad
                        '        pFeaturepout.Value(pOutFeatureClass.FindField("ZONA")) = v_zona
                        '        pFeaturepout.Store()  'Grabando
                        '        pFeature = pFeatureCursor.NextFeature  'Siguiente registro del total
                        '    Loop
                        '    pMxDoc.ActiveView.Refresh()
                        'End If


                        '--------------------------
                        tipoex = "2"
                        Dim v_dmsimzo2 As New DataTable
                        v_dmsimzo2 = cls_Oracle.FT_dmsimzo(fecsimul, v_zona_dm, tipoex)              ' obtiene relacion de DM simultaneos RD por zona  >>> EN WGS84 ES CERO (VACIO)
                        Dim codigo_eval2 As String
                        If v_dmsimzo2.Rows.Count > 0 Then
                            For contador As Integer = 0 To v_dmsimzo2.Rows.Count - 1
                                codigo_eval2 = v_dmsimzo2.Rows(contador).Item("CODIGOU")
                                If contador = 0 Then
                                    lista_codigo_sim_rd = "CODIGOU =  '" & codigo_eval2 & "'"
                                ElseIf contador > 0 Then
                                    lista_codigo_sim_rd = lista_codigo_sim_rd & " OR " & "CODIGOU =  '" & codigo_eval2 & "'"
                                End If
                            Next contador
                            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero & v_zona_dm, m_Application, "1", False)
                            cls_eval.consultacapaDM("", "Simultaneo_rd", "Catastro")
                            lista_codigo_sim_rd = ""
                            'Almacenando la data consultada de la maestra a la capa temporal 
                            pMap = pMxDoc.FocusMap
                            pFeatureLayer = pMap.Layer(0)
                            pInFeatureClass = pFeatureLayer.FeatureClass
                            pMap.DeleteLayer(pFeatureLayer)
                            pMap = pMxDoc.FocusMap
                            Dim cuenta_dm_ti As Integer
                            Dim contador1 As Integer
                            Dim v_codigou As String = ""
                            Dim v_concesion As String = ""
                            Dim v_titconces As String = ""
                            Dim v_hectagis As String = ""
                            Dim v_tipoex As String = ""
                            Dim v_fecdenu As String = ""
                            cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero_RD & v_zona_dm, m_Application, "1", False)
                            pMap = pMxDoc.FocusMap
                            pFeatureLayer = pMap.Layer(0)
                            pOutFeatureClass = pFeatureLayer.FeatureClass
                            pMap.DeleteLayer(pFeatureLayer)
                            pFeatureCursor = pInFeatureClass.Search(pQueryFilter_si, True)
                            cuenta_dm_ti = pInFeatureClass.FeatureCount(pQueryFilter_si)
                            pFeature = pFeatureCursor.NextFeature
                            Do Until pFeature Is Nothing
                                contador1 = contador1 + 1
                                If pFeatureCursor.FindField("CODIGOU") = -1 Then
                                    v_codigou = " "
                                Else
                                    v_codigou = pFeature.Value(pFeatureCursor.FindField("CODIGOU")).ToString
                                End If
                                If pFeatureCursor.FindField("CONCESION") = -1 Then
                                    v_concesion = " "
                                Else
                                    v_concesion = pFeature.Value(pFeatureCursor.FindField("CONCESION")).ToString
                                End If
                                If pFeatureCursor.FindField("TIT_CONCES") = -1 Then
                                    v_titconces = " "
                                Else
                                    v_titconces = pFeature.Value(pFeatureCursor.FindField("TIT_CONCES")).ToString
                                End If
                                If pFeatureCursor.FindField("HECTAGIS") = -1 Then
                                    v_hectagis = " "
                                Else
                                    v_hectagis = pFeature.Value(pFeatureCursor.FindField("HECTAGIS")).ToString
                                End If
                                If pFeatureCursor.FindField("TIPO_EX") = -1 Then
                                    v_tipoex = " "
                                Else
                                    v_tipoex = pFeature.Value(pFeatureCursor.FindField("TIPO_EX")).ToString
                                End If
                                If pFeatureCursor.FindField("FEC_DENU") = -1 Then
                                    v_fecdenu = " "
                                Else
                                    v_fecdenu = pFeature.Value(pFeatureCursor.FindField("FEC_DENU")).ToString
                                End If
                                'Esta parte crea el poligono nuevo
                                Dim pFeaturepout As IFeature
                                pFeaturepout = pOutFeatureClass.CreateFeature
                                pFeaturepout.Shape = pFeature.Shape
                                'Aumentando los campos que iran al registro nuevo
                                pFeaturepout.Value(pOutFeatureClass.FindField("CODIGOU")) = v_codigou
                                pFeaturepout.Value(pOutFeatureClass.FindField("CONCESION")) = v_concesion
                                pFeaturepout.Value(pOutFeatureClass.FindField("TIT_CONCES")) = v_titconces
                                pFeaturepout.Value(pOutFeatureClass.FindField("HECTAGIS")) = v_hectagis
                                pFeaturepout.Value(pOutFeatureClass.FindField("TIPO_EX")) = v_tipoex
                                pFeaturepout.Value(pOutFeatureClass.FindField("FEC_DENU")) = v_fecdenu
                                pFeaturepout.Store()  'Grabando
                                pFeature = pFeatureCursor.NextFeature  'Siguiente registro del total
                            Loop
                            pMxDoc.ActiveView.Refresh()
                        End If
                    Next i           '**********************************************************************************************

                    v_pesird = cls_Oracle.FT_pesird()      'consolidado de grupo de redenuncios    >>>>  EN WGS84 ES CERO (VACIO)
                ElseIf Val(v_fcnum) = 2 Then
                    MsgBox("Ingrese una fecha valida de Libre Denunciabilidad")
                    Exit Sub
                End If      '*****************************************************************************************************************************************************

                v_gruposimul = cls_Oracle.FT_gruposimul(fecsimul)   'grupos y subgrupos     GIS

                v_petsimul = cls_Oracle.FT_petsimul(fecsimul)     'consolidado de grupo y subgrupos   >>>  AGREGAR PARAMETRO DE FECHA

                If v_petsimul.Rows.Count = 0 Then
                    MsgBox("No Existe ning�n Item con esta consulta ", MsgBoxStyle.Information, "[BDGEOCIENT�FICA]")
                    '   Me.txtConsulta.Text = ""
                    '  Me.btnGraficar.Enabled = False
                    Exit Sub
                End If

                Dim v_bdsimul As String = "0"

                v_bdsimul = cls_Oracle.FT_verifica_bdsimultaneos(fecsimul)

                If Val(v_bdsimul) = 1 Then

                    '****************************************************************************************** BLOQUE PARA GRABAR EN LA BASE DE DATOS *******************************
                    esc_sim = "1"

                    'Dim cls_catastro As New cls_DM_1
                    'Dim cls_Eval As New Cls_evaluacion
                    Dim cls_prueba As New cls_Prueba
                    Dim lodtbLeyenda As New DataTable

                    Dim v_grusim As New DataTable
                    Dim v_grusimf As New DataTable
                    Dim v_coorsim As New DataTable
                    Dim v_hojasim As New DataTable
                    Dim num As Integer = 0

                    Dim vnum_dm As String = ""
                    Dim grusim As String = ""
                    Dim grusimf As String = ""
                    Dim p_shapefile As String = ""
                    Dim pForm As New Frm_Eval_segun_codigo
                    'fecha_archi = DateTime.Now.Ticks.ToString()
                    Dim loStrShapefile As String = "Catastro" & DateTime.Now.Ticks.ToString()
                    Dim tipo As String = ""
                    For i As Integer = 0 To v_petsimul.Rows.Count - 1
                        fecha_archi = DateTime.Now.Ticks.ToString()

                        grusim = v_petsimul.Rows(i).Item("GRUPO").ToString 'p_ListBox.Items(i).SubItems(0).Text
                        vnum_dm = v_petsimul.Rows(i).Item("Num_DM").ToString 'p_ListBox.Items(i).SubItems(2).Text
                        v_zona_dm = v_petsimul.Rows(i).Item("ZONA").ToString ' p_ListBox.Items(i).SubItems(3).Text
                        grusimf = v_petsimul.Rows(i).Item("GRUPOF").ToString 'p_ListBox.Items(i).SubItems(0).Text
                        sgrupo_sim = v_petsimul.Rows(i).Item("SUB_GRUPO").ToString 'p_ListBox.Items(i).SubItems(0).Text
                        'cd_cuad_sim = v_petsimul.Rows(i).Item("CUADRICULA").ToString 'p_ListBox.Items(i).SubItems(0).Text      

                        tipo = "1"
                        v_grusimf = cls_Oracle.FT_cuasim(grusimf, tipo)      ' obtengo las cuadriculas simultaneas

                        Dim codigo_eval As String = ""
                        Dim hectagis_sim As String = ""
                        Dim cuad_eval As String = ""
                        Dim v_cd_cuad As String = ""
                        num_cuasim = v_grusimf.Rows.Count     'obtengo n�mero de cuadriculas simultaneas
                        '----------------------------------------------------------
                        Dim RetornoTabla1, RetornoTabla2, RetornoTabla3, RetornoTabla4, RetornoTabla5 As String
                        Dim tabla As String = ""
                        Dim coord_x As Single = 0
                        Dim coord_y As Single = 0
                        Dim v_numver As Integer = 0
                        Dim v_hoja As String = ""
                        Dim v_dema As String = ""

                        tabla = "1"    'inserta en  SG_D_SIMULTANEOS2@gamma            
                        'RetornoTabla1 = cls_Oracle.FT_INS_TABLASIMUL(tabla, fecsimul, grusim, sgrupo_sim, vnum_dm, num_cuasim, v_numver, coord_x, coord_y, codigo_eval, v_zona_dm, v_hoja, v_dema)
                        'RetornoTabla1 = cls_Oracle.FT_SIMULTANEO_EVALGIS(tabla, fecsimul, grusim, sgrupo_sim, vnum_dm, num_cuasim, v_numver, coord_x, coord_y, codigo_eval, v_zona_dm, v_hoja, v_dema)
                        RetornoTabla1 = cls_Oracle.P_INS_SIMULTANEOS(fecsimul, grusim, sgrupo_sim, vnum_dm, num_cuasim)
                        'MsgBox("----3.-----:  " & RetornoTabla1)

                        lista_codigo_sim = ""
                        lista_dm_sim = ""
                        lista_cuad_sim = ""

                        If v_grusimf.Rows.Count > 0 Then
                            For contador As Integer = 0 To v_grusimf.Rows.Count - 1
                                cuad_eval = v_grusimf.Rows(contador).Item("CD_CUAD")
                                If contador = 0 Then
                                    lista_cuad_sim = "CD_CUAD =  '" & cuad_eval & "'"
                                ElseIf contador > 0 Then
                                    lista_cuad_sim = lista_cuad_sim & " OR " & "CD_CUAD =  '" & cuad_eval & "'"
                                End If
                            Next contador
                        End If
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Cuadriculas_S & v_zona_dm, m_Application, "2", False)            ' gstrFC_Cuadriculas
                        cls_eval.consultacapaDM("", "Cuadric", "Cuadricula_sim")    ' >>>>  "Cuadriculas"
                        cls_Catastro.Expor_Tema("Cuadri_sim", sele_denu, m_Application)

                        Dim datum_origen As String = ""
                        datum_origen = v_sistema

                        v_grusim = cls_Oracle.FT_grusim(grusimf, tipo, datum_origen)       ' Obtengo los DMs simultaneos y sus campos del Feature class
                        If v_grusim.Rows.Count > 0 Then
                            For contador As Integer = 0 To v_grusim.Rows.Count - 1
                                codigo_eval = v_grusim.Rows(contador).Item("CODIGOU")
                                hectagis_sim = v_grusim.Rows(contador).Item("HECTAGIS")
                                dm_sim = codigo_eval
                                If contador = 0 Then
                                    hectagis_min = hectagis_sim
                                    dm_sim = codigo_eval
                                    lista_codigo_sim = "CODIGOU =  '" & codigo_eval & "'"
                                    'dmsim = "CG_CODIGO =  '" & codigo_eval & "'"
                                ElseIf contador > 0 Then
                                    If hectagis_sim < hectagis_min Then
                                        hectagis_min = hectagis_sim
                                        dm_sim = codigo_eval
                                    End If
                                    lista_codigo_sim = lista_codigo_sim & " OR " & "CODIGOU =  '" & codigo_eval & "'"
                                End If
                                '------------------------------------------------------------------------------------
                                tabla = "3"       'inserta en SG_D_DMXGRSIMUL2@gamma
                                'RetornoTabla3 = cls_Oracle.FT_SIMULTANEO_EVALGIS(tabla, fecsimul, grusim, sgrupo_sim, vnum_dm, num_cuasim, v_numver, coord_x, coord_y, codigo_eval, v_zona_dm, v_hoja, v_dema)
                                'RetornoTabla3 = cls_Oracle.FT_SIMULTANEO_EVALGIS(tabla, fecsimul, grusim, sgrupo_sim, "", "", "", "", "", codigo_eval, v_zona_dm, "", "")
                                RetornoTabla3 = cls_Oracle.FT_INS_DMXGRSIMUL(fecsimul, grusim, sgrupo_sim, codigo_eval, v_zona_dm)
                                '------------------------------------------------------------------------------------
                                'MsgBox("----3.-----:  " & RetornoTabla3)
                            Next contador
                        End If
                        tipo = "3"
                        v_coorsim = cls_Oracle.FT_coorsim(dm_sim)      'se obtiene coordenadas del DM simultaneo
                        v_hojasim = cls_Oracle.FT_hojasim(dm_sim, tipo)
                        If v_hojasim.Rows.Count > 0 Then
                            For contador As Integer = 0 To v_hojasim.Rows.Count - 1
                                v_carta_dm = v_hojasim.Rows(contador).Item("CD_HOJA")
                                v_hoja = v_carta_dm
                                '------------------------------------------------------------------------------------  
                                tabla = "4"         'inserta en  SG_D_CARTAXDERESIMUL2@gamma
                                'RetornoTabla4 = cls_Oracle.FT_SIMULTANEO_EVALGIS(tabla, fecsimul, grusim, sgrupo_sim, vnum_dm, num_cuasim, v_numver, coord_x, coord_y, codigo_eval, v_zona_dm, v_hoja, v_dema)
                                RetornoTabla4 = cls_Oracle.FT_INS_CARTAXDERESIMUL(fecsimul, grusim, sgrupo_sim, v_hoja, codigo_eval)
                                '------------------------------------------------------------------------------------
                                'MsgBox("----3.-----:  " & RetornoTabla4)
                            Next contador
                        End If

                        tipo = "4"
                        v_hojasim = cls_Oracle.FT_hojasim(dm_sim, tipo)
                        If v_hojasim.Rows.Count > 0 Then
                            For contador As Integer = 0 To v_hojasim.Rows.Count - 1
                                v_demagis = v_hojasim.Rows(contador).Item("DE_CODDEM")
                                v_dema = v_demagis
                                '------------------------------------------------------------------------------------
                                tabla = "5"         'inserta en  SG_D_DEMAXDERESIMUL2@gamma   
                                'RetornoTabla5 = cls_Oracle.FT_SIMULTANEO_EVALGIS(tabla, fecsimul, grusim, sgrupo_sim, vnum_dm, num_cuasim, v_numver, coord_x, coord_y, codigo_eval, v_zona_dm, v_hoja, v_dema)
                                RetornoTabla5 = cls_Oracle.FT_INS_DEMAXDERESIMUL(fecsimul, grusim, sgrupo_sim, v_dema, codigo_eval)
                                '------------------------------------------------------------------------------------
                                'MsgBox("----3.-----:  " & RetornoTabla5)
                            Next contador
                        End If

                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero & v_zona_dm, m_Application, "2", True)
                        cls_eval.consultacapaDM("", "Simultaneot", "Catastro")
                        cls_Catastro.Expor_Tema("Cata_sim", sele_denu, m_Application)
                        lista_codigo_sim = ""

                        cls_Catastro.Borra_Todo_Feature("", m_Application)
                        cls_Catastro.Limpiar_Texto_Pantalla(m_Application)

                        p_shapefile = "Cata_sim" & fecha_archi & ""
                        cls_Catastro.Add_ShapeFile_tmp(p_shapefile, m_Application)
                        cls_eval.agregacampotema_tpm(p_shapefile, "Cata_sim")
                        cls_Catastro.Add_ShapeFile(p_shapefile, m_Application)
                        cls_Catastro.Zoom_to_Layer("Catastro_sim")

                        p_shapefile = "Cuadri_sim" & fecha_archi & ""
                        cls_Catastro.Add_ShapeFile_tmp(p_shapefile, m_Application)
                        cls_eval.agregacampotema_tpm(p_shapefile, "Cuadri_sim")
                        cls_Catastro.Add_ShapeFile(p_shapefile, m_Application)
                        cls_Catastro.Update_Value_DM_cuad(m_Application, "Cuadricula_sim", cd_cuad_sim)

                        'If i = 93 Then
                        '    Stop
                        'End If

                        If Val(num_cuasim) = 1 Then
                            p_shapefile = "Cuadri_sim" & fecha_archi & ""
                            cls_eval.consultacapaDM_sim("", "Cuadri", "Cuadricula_sim")
                            cls_Catastro.ClearLayerSelection(pFeatureLayer)
                            cls_eval.consultacapaDM("", "Cuadri", "Cuadricula_sim")
                            cls_Catastro.rotulatexto_dm_poligono_sim(m_Application, "Cuadricula_sim")
                            cls_Catastro.ClearLayerSelection(pFeatureLayer)
                        Else
                            p_shapefile = "Cuadri_sim" & fecha_archi & ""
                            'cls_Eval.Geoprocesamiento_temas_sim("dissolve", pApp, "Cuadri_sim")
                            cls_eval.Geoprocesamiento_temas_sim("dissolve", m_Application, p_shapefile)
                            pMxDoc.ActiveView.Refresh()
                            p_shapefile = "Cuadri_dsim" & fecha_archi & ""
                            cls_Catastro.Add_ShapeFile_tmp(p_shapefile, m_Application)
                            cls_eval.agregacampotema_tpm(p_shapefile, "Cuadri_dsim")
                            cls_Catastro.Add_ShapeFile(p_shapefile, m_Application)
                            cls_Catastro.Update_Value_DM_cuad1(m_Application, "Cuadricula_dsim", "")
                            cls_Catastro.Quitar_Layer("Cuadricula_sim", m_Application)

                            'cls_catastro.Quitar_Layer("Cuadri_dsim", m_application)
                            'cls_catastro.Quitar_Layer(p_shapefile, m_application)
                            cls_eval.consultacapaDM_sim("", "Cuadrid", "Cuadricula_dsim")
                            cls_Catastro.ClearLayerSelection(pFeatureLayer)
                            cls_eval.consultacapaDM("", "Cuadrid", "Cuadricula_dsim")
                            cls_Catastro.rotulatexto_dm_poligono_sim(m_Application, "Cuadricula_dsim")
                            cls_Catastro.ClearLayerSelection(pFeatureLayer)
                        End If

                        cls_Catastro.rotulatexto_dm("Catastro_sim", m_Application)
                        If Val(num_cuasim) = "1" Then
                            cls_Catastro.rotulatexto_dm("Cuadricula_sim", m_Application)
                        Else
                            cls_Catastro.rotulatexto_dm("Cuadricula_dsim", m_Application)
                        End If

                        For w As Integer = 0 To dt_coordenada.Rows.Count - 1
                            v_numver = dt_coordenada.Rows(w).Item("num")
                            coord_x = dt_coordenada.Rows(w).Item("x")
                            coord_y = dt_coordenada.Rows(w).Item("y")
                            '---------------------------------------------
                            tabla = "2"       'inserta en  SG_D_SIMULCOOR2@gamma       
                            'RetornoTabla2 = cls_Oracle.FT_SIMULTANEO_EVALGIS(tabla, fecsimul, grusim, sgrupo_sim, vnum_dm, num_cuasim, v_numver, coord_x, coord_y, codigo_eval, v_zona_dm, v_hoja, v_dema)
                            RetornoTabla2 = cls_Oracle.FT_INS_SIMULCOOR(fecsimul, grusim, sgrupo_sim, v_numver, coord_x, coord_y)
                            '---------------------------------------------
                            'MsgBox("----3.-----:  " & RetornoTabla2)
                        Next w

                        'tabla = "4"
                        'lostrRetorno = cls_Oracle.FT_SIMULTANEO_EVALGIS(tabla, fecsimul, grusim, sgrupo_sim, vnum_dm, num_cuasim, v_numver, coord_x, coord_y, codigo_eval, v_zona_dm, v_carta_dm, v_dema)
                        'tabla = "5"
                        'lostrRetorno = cls_Oracle.FT_SIMULTANEO_EVALGIS(tabla, fecsimul, grusim, sgrupo_sim, vnum_dm, num_cuasim, v_numver, coord_x, coord_y, codigo_eval, v_zona_dm, v_carta_dm, v_demagis)

                        v_demagis = ""
                        v_carta_dm = ""
                        v_dema = ""
                        v_hoja = ""
                    Next i
                    '************************************************************************************************

                End If

                '***  se inserta codigo para exportar tabla de la BD  ***
                Dim v_dmxgrupo As New DataTable
                v_dmxgrupo = cls_Oracle.FT_DMXGRSIMUL(fecsimul)

                Try
                    Kill(glo_pathSIM & "*.*")
                Catch ex As Exception
                End Try

                cls_Catastro.creandotabla_Rep_DMSimul("DMSimul")

                Dim pTable As ITable
                Dim pWorkspaceFactory1 As IWorkspaceFactory
                pWorkspaceFactory1 = New ShapefileWorkspaceFactory
                Dim pFWS As IFeatureWorkspace
                Dim pTWS As ITableName
                Dim carpeta As IWorkspace

                carpeta = pWorkspaceFactory1.OpenFromFile(glo_pathSIM, 0)
                pFWS = carpeta
                pTable = pFWS.OpenTable("Reporte_DMSimul" & fecha_archi_sim)

                Dim cuenta_reg As Integer = 0
                Dim ptableCursor As ICursor
                Dim pfields3 As Fields
                Dim pRow As IRow

                Dim V_SI_FECSIM As String
                Dim V_SI_GRUPO As String
                Dim V_SI_AREAGR As String
                Dim V_CG_CODIGO As String
                Dim V_DS_ZONA As String
                Dim V_DS_FECING As String

                If v_dmxgrupo.Rows.Count >= 1 Then
                    For contador1 As Integer = 0 To v_dmxgrupo.Rows.Count - 1
                        cuenta_reg = cuenta_reg + 1
                        pRow = pTable.CreateRow
                        pfields3 = pTable.Fields
                        V_SI_FECSIM = v_dmxgrupo.Rows(contador1).Item("SI_FECSIM")
                        V_SI_GRUPO = v_dmxgrupo.Rows(contador1).Item("SI_GRUPO")
                        V_SI_AREAGR = v_dmxgrupo.Rows(contador1).Item("SI_AREAGR")
                        V_CG_CODIGO = v_dmxgrupo.Rows(contador1).Item("CG_CODIGO")
                        V_DS_ZONA = v_dmxgrupo.Rows(contador1).Item("DS_ZONA")
                        V_DS_FECING = v_dmxgrupo.Rows(contador1).Item("DS_FECING")

                        ptableCursor = pTable.Search(Nothing, False)

                        pRow.Value(pfields3.FindField("SI_FECSIM")) = V_SI_FECSIM
                        pRow.Value(pfields3.FindField("SI_GRUPO")) = V_SI_GRUPO
                        pRow.Value(pfields3.FindField("SI_AREAGR")) = V_SI_AREAGR
                        pRow.Value(pfields3.FindField("CG_CODIGO")) = V_CG_CODIGO
                        pRow.Value(pfields3.FindField("DS_ZONA")) = V_DS_ZONA
                        pRow.Value(pfields3.FindField("DS_FECING")) = V_DS_FECING

                        pRow.Store()
                    Next contador1
                Else
                    MsgBox("No Existe Registros en la Tabla Reporte_DMSimul", MsgBoxStyle.Information, "Observaci�n...")
                    'Exit Sub
                End If

                'pMxDoc.UpdateContents()    'para probar
                'pFeatureSelection.Clear()  'para probar 

                Me.btnGraficar.Enabled = True
                Me.dgdDetalle.DataSource = v_petsimul
                'PT_Inicializar_Grilla_Simultaneoc()     ' ojo   esto debe estar comentado
                PT_Agregar_Funciones_Simultaneoc()
                Me.chkEstado.Checked = False : Me.chkEstado.Checked = True
                Me.dgdDetalle.AllowUpdate = True

                btn_procesar.Visible = False
                btn_procesar.Enabled = False

            Case "OP_25" 'PMA

                If v_sistema = "PSAD-56" Then
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_"
                    gstrFC_Provincia = "GPO_PRO_PROVINCIA_"
                ElseIf v_sistema = "WGS-84" Then
                    gstrFC_Catastro_Minero = "GPO_CMI_CATASTRO_MINERO_WGS_"
                    gstrFC_Provincia = "GPO_PRO_PROVINCIA_WGS_"
                End If

                tipo_opcion = ""
                v_codigo = ""
                pMxDoc = m_Application.Document
                pMap = pMxDoc.FocusMap
                caso_consulta = "CATASTRO MINERO"
                If pMap.Name <> "CATASTRO MINERO" Then
                    cls_planos.buscaadataframe(caso_consulta, False)
                    If valida = False Then
                        pMap.Name = "CATASTRO MINERO"
                        pMxDoc.UpdateContents()
                    End If
                    cls_eval.ActivaDataframe_Opcion(caso_consulta, m_Application)
                    pMxDoc.UpdateContents()
                End If
                cls_eval.Eliminadataframe(caso_consulta)
                cls_planos.buscaadataframe("CATASTRO MINERO", False)
                If valida = False Then
                    pMap.Name = "CATASTRO MINERO"
                End If
                cls_Catastro.Borra_Todo_Feature("", m_Application)      'se agrego
                cls_Catastro.Limpiar_Texto_Pantalla(m_Application)      'se agrego
                pMxDoc.UpdateContents()
                Dim v_petpma As New DataTable
                Dim datum_origen As String = ""
                'fecsup = Date.Today
                datum_origen = v_sistema
                'v_petpma = cls_Oracle.FT_petpma()
                v_petpma = cls_Oracle.FT_petpma(datum_origen)
                dgdResultado.DataSource = v_petpma
                btnexp_excel.Enabled = True
                Dim codigo_eval As String = ""
                Dim codigo_prov As String = ""
                Dim codigo_provd As String = ""
                'Dim subprov As String = Left(codigo_prov, 4)
                Select Case v_petpma.Rows.Count
                    Case 0
                        MsgBox("No Existe Petitorios PMA ubicados fuera de la Demarcaci�n Calificada", MsgBoxStyle.Information, "Observaci�n...")
                        Exit Sub
                    Case Else
                        'If v_petpma.Rows.Count > 0 Then
                        '    For contador As Integer = 0 To v_petpma.Rows.Count - 1
                        '        codigo_eval = v_petpma.Rows(contador).Item("CG_CODIGO")
                        '        codigo_prov = v_petpma.Rows(contador).Item("CODDEM_G")
                        '        If contador = 0 Then
                        '            lista_codigo_pma = "CODIGOU =  '" & codigo_eval & "'"
                        '            lista_codigo_provg = "CD_PROV =  '" & codigo_prov & "'"
                        '        ElseIf contador > 0 Then
                        '            lista_codigo_pma = lista_codigo_pma & " OR " & "CODIGOU =  '" & codigo_eval & "'"
                        '            lista_codigo_provg = lista_codigo_provg & " OR " & "CD_PROV =  '" & codigo_prov & "'"
                        '        End If
                        '    Next contador
                        'End If

                        'If v_petpma.Rows.Count > 0 Then
                        '    For contador As Integer = 0 To v_petpma.Rows.Count - 1
                        '        codigo_prov = v_petpma.Rows(contador).Item("CODDEM")
                        '        'codigo_prov = Left(codigo_prov, 4)
                        '        If contador = 0 Then
                        '            lista_codigo_provd = "CD_PROV =  '" & codigo_prov & "'"
                        '        ElseIf contador > 0 Then
                        '            lista_codigo_provd = lista_codigo_provd & " OR " & "CD_PROV =  '" & codigo_prov & "'"
                        '        End If
                        '    Next contador
                        'End If

                        If v_petpma.Rows.Count > 0 Then
                            For contador As Integer = 0 To v_petpma.Rows.Count - 1
                                codigo_eval = v_petpma.Rows(contador).Item("CG_CODIGO")
                                codigo_prov = v_petpma.Rows(contador).Item("CODDEM_G")
                                codigo_provd = v_petpma.Rows(contador).Item("CODDEM")
                                If contador = 0 Then
                                    lista_codigo_pma = "CODIGOU =  '" & codigo_eval & "'"
                                    lista_codigo_provg = "CD_PROV =  '" & codigo_prov & "'"
                                    lista_codigo_provd = "CD_PROV =  '" & codigo_provd & "'"
                                ElseIf contador > 0 Then
                                    lista_codigo_pma = lista_codigo_pma & " OR " & "CODIGOU =  '" & codigo_eval & "'"
                                    lista_codigo_provg = lista_codigo_provg & " OR " & "CD_PROV =  '" & codigo_prov & "'"
                                    lista_codigo_provd = lista_codigo_provd & " OR " & "CD_PROV =  '" & codigo_provd & "'"
                                End If
                            Next contador
                        End If

                        cls_Catastro.Quitar_Layer("Catastro", m_Application)
                        cls_Catastro.Quitar_Layer("Provincia", m_Application)
                        pMxDoc.ActivatedView.Refresh()
                        lista_codigo_prov = lista_codigo_provd & " OR " & lista_codigo_provg
                        v_zona_dm = "18"
                        'cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia_Z & v_zona_dm, m_Application, "1", False)
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Provincia & v_zona_dm, m_Application, "1", False)
                        cls_Catastro.ShowLabel_DM2("Provincia", m_Application)
                        cls_eval.consultacapaDM("", "Provincia", "Provincia")
                        cls_Catastro.ZOOM_SELECTED("Provincia", m_Application)
                        cls_Catastro.Color_Poligono_Simple(m_Application, "Provincia")
                        lista_codigo_prov = ""
                        cls_Catastro.PT_CargarFeatureClass_SDE(gstrFC_Catastro_Minero & v_zona_dm, m_Application, "1", True)  'psad 56
                        cls_Catastro.DefinitionExpression(lista_codigo_pma, m_Application, "Catastro")
                        'cls_eval.consultacapaDM("", "PMA", "Catastro")
                        cls_Catastro.Color_Poligono_Simple_2(m_Application, "Catastro")
                        lista_codigo_pma = ""
                End Select
                Exit Sub
        End Select
    End Sub

    Private Sub bnneliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btneliminar.Click
        Dim valor_sele As String
        valor_sele = lst_listadm.Text
        If valor_sele <> "" Then
            Me.lst_listadm.Items.RemoveAt(lst_listadm.SelectedIndex)
        End If


    End Sub

    Private Sub btnlimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnlimpiar.Click
        lst_listadm.Items.Clear()

    End Sub


    Public Sub ExportarDatosExcel(ByVal DataGridView1 As DataGridView, ByVal titulo As String)
        Dim m_Excel As New Excel.Application
        m_Excel.Cursor = Excel.XlMousePointer.xlWait
        m_Excel.Visible = True
        Dim objLibroExcel As Excel.Workbook = m_Excel.Workbooks.Add
        Dim objHojaExcel As Excel.Worksheet = objLibroExcel.Worksheets(1)
        With objHojaExcel
            .Visible = Excel.XlSheetVisibility.xlSheetVisible
            .Activate()
            'Encabezado.
            .Range("A1:L1").Merge()
            .Range("A1:L1").Value = "INGEMMET"
            .Range("A1:L1").Font.Bold = True
            .Range("A1:L1").Font.Size = 16
            'Texto despues del encabezado.
            .Range("A2:L2").Merge()
            .Range("A2:L2").Value = titulo
            .Range("A2:L2").Font.Bold = True
            .Range("A2:L2").Font.Size = 10
            'Nombres
            'Estilo a titulos de la tabla.
            .Range("A6:P6").Font.Bold = True
            'Establecer tipo de letra al rango determinado.
            .Range("A1:P37").Font.Name = "Tahoma"
            'Los datos se registran a partir de la columna A, fila 4.
            Const primeraLetra As Char = "A"
            Const primerNumero As Short = 6
            Dim Letra As Char, UltimaLetra As Char
            Dim Numero As Integer, UltimoNumero As Integer
            Dim cod_letra As Byte = Asc(primeraLetra) - 1
            Dim sepDec As String = Application.CurrentCulture.NumberFormat.NumberDecimalSeparator
            Dim sepMil As String = Application.CurrentCulture.NumberFormat.NumberGroupSeparator
            Dim strColumna As String = ""
            Dim LetraIzq As String = ""
            Dim cod_LetraIzq As Byte = Asc(primeraLetra) - 1
            Letra = primeraLetra
            Numero = primerNumero
            Dim objCelda As Excel.Range
            For Each c As DataGridViewColumn In DataGridView1.Columns
                If c.Visible Then
                    : If Letra = "Z" Then
                        Letra = primeraLetra
                        cod_letra = Asc(primeraLetra)
                        cod_LetraIzq += 1
                        LetraIzq = Chr(cod_LetraIzq)
                    Else
                        cod_letra += 1
                        Letra = Chr(cod_letra)
                    End If
                    strColumna = LetraIzq + Letra + Numero.ToString
                    objCelda = .Range(strColumna, Type.Missing)
                    objCelda.Value = c.HeaderText
                    objCelda.EntireColumn.Font.Size = 10
                    'Establece un formato a los numeros por Default.
                    'objCelda.EntireColumn.NumberFormat = c.DefaultCellStyle.Format
                    If c.ValueType Is GetType(Decimal) OrElse c.ValueType Is GetType(Double) Then
                        objCelda.EntireColumn.NumberFormat = "#" + sepMil + "0" + sepDec + "00"
                    End If
                End If
            Next
            Dim objRangoEncab As Excel.Range = .Range(primeraLetra + Numero.ToString, LetraIzq + Letra + Numero.ToString)
            objRangoEncab.BorderAround(1, Excel.XlBorderWeight.xlMedium)
            UltimaLetra = Letra
            Dim UltimaLetraIzq As String = LetraIzq
            'Cargar Datos del DataGridView.
            Dim i As Integer = Numero + 1
            For Each reg As DataGridViewRow In DataGridView1.Rows
                LetraIzq = ""
                cod_LetraIzq = Asc(primeraLetra) - 1
                Letra = primeraLetra
                cod_letra = Asc(primeraLetra) - 1
                For Each c As DataGridViewColumn In DataGridView1.Columns
                    If c.Visible Then
                        If Letra = "Z" Then
                            Letra = primeraLetra
                            cod_letra = Asc(primeraLetra)
                            cod_LetraIzq += 1
                            LetraIzq = Chr(cod_LetraIzq)
                        Else
                            cod_letra += 1
                            Letra = Chr(cod_letra)
                        End If
                        strColumna = LetraIzq + Letra
                        'Aqui se realiza la carga de datos.
                        .Cells(i, strColumna) = IIf(IsDBNull(reg.ToString), "", "'" & reg.Cells(c.Index).Value)
                        'Establece las propiedades de los datos del DataGridView por Default.
                        '.Cells(i, strColumna) = IIf(IsDBNull(reg.(c.DataPropertyName)), c.DefaultCellStyle.NullValue, reg(c.DataPropertyName))
                        '.Range(strColumna + i, strColumna + i).In()
                    End If
                Next
                Dim objRangoReg As Excel.Range = .Range(primeraLetra + i.ToString, strColumna + i.ToString)
                objRangoReg.Rows.BorderAround()
                objRangoReg.Select()
                i += 1
            Next
            UltimoNumero = i
            'Dibujar las l�neas de las columnas.
            LetraIzq = ""
            cod_LetraIzq = Asc("A")
            cod_letra = Asc(primeraLetra)
            Letra = primeraLetra
            For Each c As DataGridViewColumn In DataGridView1.Columns
                If c.Visible Then
                    objCelda = .Range(LetraIzq + Letra + primerNumero.ToString, LetraIzq + Letra + (UltimoNumero - 1).ToString)
                    objCelda.BorderAround()
                    If Letra = "Z" Then
                        Letra = primeraLetra
                        cod_letra = Asc(primeraLetra)
                        LetraIzq = Chr(cod_LetraIzq)
                        cod_LetraIzq += 1
                    Else
                        cod_letra += 1
                        Letra = Chr(cod_letra)
                    End If
                End If
            Next
            'Dibujar el border exterior grueso de la tabla.
            Dim objRango As Excel.Range = .Range(primeraLetra + primerNumero.ToString, UltimaLetraIzq + UltimaLetra + (UltimoNumero - 1).ToString)
            objRango.Select()
            objRango.Columns.AutoFit()
            objRango.Columns.BorderAround(1, Excel.XlBorderWeight.xlMedium)
        End With
        m_Excel.Cursor = Excel.XlMousePointer.xlDefault
    End Sub

    Private Sub btnexp_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexp_excel.Click
        Select Case Me.cbo_tipo.SelectedValue

            Case "OP_23"    'Superposicion DM

                ExportarDatosExcel(dgdResultado, "REPORTE DE PETITORIOS SUPERPUESTOS INGRESADOS EN LA FECHA")
                Me.dtpFecha1.Text = ""

            Case "OP_25" 'Superposicion DM

                ExportarDatosExcel(dgdResultado, "REPORTE DE PETITORIOS PMA UBICADOS FUERA DE DEMARCACION CALIFICADA")

        End Select
    End Sub

    Private Sub txtExiste_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtExiste.TextChanged

    End Sub

    Private Sub cboSistema_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSistema.SelectionChangeCommitted

    End Sub

    Private Sub txtConsulta_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConsulta.TextChanged

    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub
End Class