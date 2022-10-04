Imports ESRI.ArcGIS.GISClient
Imports ESRI.ArcGIS.Display
Imports System.Runtime.InteropServices
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Output
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Editor
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.CartoUI
Imports ESRI.ArcGIS.GeoDatabaseUI
Imports stdole
Imports PORTAL_Clases

Public Class Cls_planos
    Structure Punto_DM
        Dim v As Integer
        Dim x As Double
        Dim y As Double
    End Structure
    Public cls_Catastro As New cls_DM_1
    Public lodtbReporte As New DataTable
    Private cls_oracle As New cls_Oracle
    Public p_App As IApplication
    'Public m_Application As IApplication
    Public formulario As New Frm_formatoplanos
    Private cls_eval As New Cls_evaluacion
    ' Private cls_Planos As New Cls_planos

    'Verifica si el tema de Dm esta en la vista
    '*******************************************
    Public Sub Generaplanoreporte(ByVal s_tipo_plano As String)
        Try
            Dim rutaPlantilla As String = ""
            If s_tipo_plano = "Reporte previo" Then
                rutaPlantilla = glo_pathMXT & "\Plantilla_listadodm1.mxt"
            ElseIf s_tipo_plano = "Plano reporte A4" Then

            ElseIf s_tipo_plano = "Plano catastral" Then
                ' rutaPlantilla = glo_pathMXT & "\Plantilla_evd.mxt"
                If v_sistema = "PSAD-56" Then
                    rutaPlantilla = glo_pathMXT & "\Plantilla_evd_56.mxd"
                ElseIf v_sistema = "WGS-84" Then
                    rutaPlantilla = glo_pathMXT & "\Plantilla_evd_84.mxD"
                End If
               
            ElseIf s_tipo_plano = "Plano Libredenu" Then
                rutaPlantilla = glo_pathMXT & "\Plantilla_libredenu.mxt"
            ElseIf s_tipo_plano = "Plano Area superpuesta" Then
                rutaPlantilla = glo_pathMXT & "\Plantilla_sup" & conta_hoja_sup & ".mxt"
            ElseIf s_tipo_plano = "Plano Area Restringida" Then
                rutaPlantilla = glo_pathMXT & "\Plantilla_area_restringida.mxd"
            ElseIf s_tipo_plano = "Plano Acumulacion" Then
                ' rutaPlantilla = glo_pathMXT & "\ Plantilla_Acumula" & conta_hoja_sup & ".mxt"
                rutaPlantilla = glo_pathMXT & "\Plantilla_Acumulacion" & conta_hoja_sup & ".mxt"
            ElseIf s_tipo_plano = "Plano Demarca" Then

                ' rutaPlantilla = glo_pathMXT & "\plantilla_demarca.mxt"

                If v_sistema = "PSAD-56" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_demarca_56.mxd"
                ElseIf v_sistema = "WGS-84" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_demarca_84.mxD"
                End If

            ElseIf s_tipo_plano = "Plano Geologico" Then
            rutaPlantilla = glo_pathMXT & "\plantilla_Geologico.mxd"
            ElseIf s_tipo_plano = "Plano Cuadricula" Then
            rutaPlantilla = glo_pathMXT & "\plantilla_cuadricula.mxt"
            ElseIf s_tipo_plano = "Plano carta" Then
            If v_sistema = "PSAD-56" Then


                rutaPlantilla = glo_pathMXT & "\plantilla_cartaign_56.mxD"
            ElseIf v_sistema = "WGS-84" Then
                rutaPlantilla = glo_pathMXT & "\plantilla_cartaign_84.mxD"
            End If

            ElseIf s_tipo_plano = "Plano Simultaneo" Then
            rutaPlantilla = glo_pathMXT & "\plantilla_sim.mxd"
            ElseIf s_tipo_plano = "Plano Venta" Then
            If sele_plano = "Formato A4" Then
                'rutaPlantilla = glo_pathMXT & "\plantilla_pv_A4.mxt"
                ' rutaPlantilla = glo_pathMXT & "\plantilla_pv_A4_c.mxd"
                ' rutaPlantilla = glo_pathMXT & "\plantilla_pv_A4_m.mxt"
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_A4_m.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_A4_m_situacion.mxd"

                End If
                ' rutaPlantilla = glo_pathMXT & "\plantilla_pv_A4_m_situacion.mxd"
            ElseIf sele_plano = "Formato A3" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_A3_m.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_A3_m_situacion.mxd"
                End If
            ElseIf sele_plano = "Formato A2" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_A2.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_A2_m_situacion.mxd"
                End If

            ElseIf sele_plano = "Formato A0" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_A0.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_A0_m_situacion.mxd"

                End If
            End If

            ElseIf s_tipo_plano = "Plano_variado" Then
            If sele_plano = "Plano A4 Vertical" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_VA4.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_VA4_situacion.mxd"
                End If
            ElseIf sele_plano = "Plano A4 Horizontal" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                        'rutaPlantilla = glo_pathMXT & "\plantilla_pv_HA4.mxt"
                        rutaPlantilla = glo_pathMXT & "\plantilla_PV_HA4.mxd"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_HA4_situacion.mxd"
                End If

            ElseIf sele_plano = "Plano A3 Vertical" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_VA3.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_VA3_situacion.mxd"
                End If

            ElseIf sele_plano = "Plano A3 Horizontal" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_HA3.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_HA3_situacion.mxd"
                End If

            ElseIf sele_plano = "Plano A2 Vertical" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_VA2.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_VA2_situacion.mxd"
                End If

            ElseIf sele_plano = "Plano A2 Horizontal" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_HA2.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_HA2_situacion.mxd"
                End If

            ElseIf sele_plano = "Plano A1 Vertical" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_VA1.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_VA1_situacion.mxd"
                End If

            ElseIf sele_plano = "Plano A1 Horizontal" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_HA1.mxt"

                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_HA1_situacion.mxd"
                End If

            ElseIf sele_plano = "Plano A0 Vertical" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_VA0.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_VA0_situacion.mxd"
                End If

            ElseIf sele_plano = "Plano A0 Horizontal" Then
                If seleccion_plano_si = "Plano Catastral Publico" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_HA0.mxt"
                ElseIf seleccion_plano_si = "Plano Catastral Situacion DM" Then
                    rutaPlantilla = glo_pathMXT & "\plantilla_pv_HA0_situacion.mxd"
                End If
            End If
            End If

            '   pMxDoc = p_app.Document
            Dim rutalayout As IGxFile
            rutalayout = New GxMap
            rutalayout.Path = rutaPlantilla
            Dim pGxPageLayout As IGxMapPageLayout
            pGxPageLayout = rutalayout
            '  Dim pPageLayout As IPageLayout2

            pPageLayout = pGxPageLayout.PageLayout

            'documento = ThisDocument
            pPageLayout.ReplaceMaps(pMxDoc.Maps)
            pMxDoc.PageLayout = pPageLayout

            ' pMapFrame1.MapScale = escalaf  'define otra vez escala al documento
            pMxDoc.UpdateContents()
            '     pPageLayout = pMxDoc.PageLayout
        Catch ex As Exception
            '  MsgBox("error en GENERAR PLANTILLA DE PLANOS", MsgBoxStyle.Critical, "Observaci�n..")
        End Try
    End Sub

    Public Function WriteBlobFileContentsToTableRowField(ByVal string_Filename As System.String, ByVal table As ESRI.ArcGIS.Geodatabase.ITable) As System.Boolean

        Try
            Dim int32_BlobFieldIndex As System.Int32
            'Read the Blob (binary) stream from a file.
            Dim memoryBlobStream As ESRI.ArcGIS.esriSystem.IMemoryBlobStream = New ESRI.ArcGIS.esriSystem.MemoryBlobStreamClass
            memoryBlobStream.LoadFromFile(string_Filename)

            'Add a new row to the table
            Dim row As ESRI.ArcGIS.Geodatabase.IRow = table.CreateRow

            'Get the Blob Field from the Table.
            Dim fields As ESRI.ArcGIS.Geodatabase.IFields = row.Fields
            Dim field As ESRI.ArcGIS.Geodatabase.IField = fields.Field(int32_BlobFieldIndex)

            'Test to ensure the index number specified is a Blob Field
            If field.Type = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeBlob Then

                'Write the Blob (binary) stream to the Blob Field and save
                row.Value(int32_BlobFieldIndex) = CType(memoryBlobStream, System.Object) ' Explicit Cast
                row.Store()

                'success
                Return True

            Else

                'unsuccessful
                Return False

            End If
        Catch ex As System.Exception

            'unsuccessful with error, interrogate more
            Return False

        End Try

    End Function


    Public Sub CambiaADataView(ByVal p_App As IApplication)
        pMxDoc = p_App.Document
        Dim pActiveView As IActiveView
        Dim pUID As New UID
        pActiveView = pMxDoc.ActiveView
        Dim pCmdItem As ICommandItem
        If TypeOf pActiveView Is IPageLayout Then
            pUID.Value = "esriArcMapUI.GeographicViewCommand"
            pCmdItem = p_App.Document.CommandBars.Find(pUID)
            pCmdItem.Execute()
        End If
    End Sub

    Public Sub CambiaALayout(ByVal p_App As IApplication)
        'Cambia de vista a layout
        'pMxDoc = p_App.Document
        Dim pCmdItem As ICommandItem
        Dim pActiveView As IActiveView
        Dim pUID As New UID
        pActiveView = pMxDoc.ActiveView
        If Not TypeOf pActiveView Is IPageLayout Then
            pUID.Value = "esriArcMapUI.LayoutViewCommand"
            pCmdItem = p_App.Document.CommandBars.Find(pUID)
            'pCmdItem = pMxDoc.CommandBars.Find(pUID)
            'pCmdItem = pMxDoc.CommandBars.Find(pUID)
            pCmdItem.Execute()
        End If
    End Sub

    Public Sub mueveposiciondataframe(ByVal nombre_dataframe As String, ByVal p_app As IApplication)
        pMxDoc = p_app.Document
        Dim cls_Catastro As New cls_DM_1

        Dim mapas As IMaps
        mapas = pMxDoc.Maps
        If mapas.Count > 1 Then
            Dim pEnumLayers As IEnumLayer
            Dim pLayer As ILayer
            Try
                Dim i As Integer
                Dim k As Integer
                For i = 0 To mapas.Count - 1
                    pMap = pMxDoc.Maps.Item(0)
                    If pMap.Name <> nombre_dataframe Then
                        'Esta parte para hacer no visible a las capas de los demas dataframe
                        If pMap.LayerCount > 0 Then
                            pEnumLayers = pMap.Layers
                            pLayer = pEnumLayers.Next
                            Do Until pLayer Is Nothing
                                pLayer.Visible = False
                                pLayer = pEnumLayers.Next
                            Loop
                        End If
                        pMxDoc.Maps.Remove(pMap)
                        pMxDoc.Maps.Add(pMap)
                        pMxDoc.CurrentContentsView.Refresh(Nothing)
                        pMxDoc.UpdateContents()
                    Else
                        activadataframecatastro("CATASTRO MINERO")
                        For k = 1 To mapas.Count - 1
                            'pMap = pMxDoc.FocusMap
                            pMap = pMxDoc.Maps.Item(k)
                            'Set mapa = documento.Maps.Item(0)
                            If pMap.Name <> nombre_dataframe Then
                                'Esta parte para hacer no visible a las capas
                                If pMap.LayerCount > 0 Then
                                    pEnumLayers = pMap.Layers
                                    pLayer = pEnumLayers.Next
                                    Do Until pLayer Is Nothing
                                        pLayer.Visible = False
                                        pLayer = pEnumLayers.Next
                                    Loop
                                    'cls_Catastro.Limpiar_Texto_Pantalla(p_app)
                                End If

                                pMxDoc.UpdateContents()

                            Else
                            End If
                        Next k

                        pMxDoc.UpdateContents()
                    End If
                Next i
                pMxDoc.UpdateContents()

            Catch ex As Exception
                MsgBox("Error en Obtener Posici�n del Dataframe")
            End Try
        End If

    End Sub

    Public Sub mueveposiciondataframe_siev(ByVal nombre_dataframe As String, ByVal p_app As IApplication)
        pMxDoc = p_app.Document
        Dim cls_Catastro As New cls_DM_1

        Dim mapas As IMaps
        mapas = pMxDoc.Maps
        If mapas.Count > 1 Then
            Dim pEnumLayers As IEnumLayer
            Dim pLayer As ILayer
            Try
                Dim i As Integer
                Dim k As Integer
                For i = 0 To mapas.Count - 1
                    pMap = pMxDoc.Maps.Item(0)
                    If pMap.Name <> nombre_dataframe Then
                        'Esta parte para hacer no visible a las capas de los demas dataframe
                        If pMap.LayerCount > 0 Then
                            pEnumLayers = pMap.Layers
                            pLayer = pEnumLayers.Next
                            Do Until pLayer Is Nothing
                                pLayer.Visible = False
                                pLayer = pEnumLayers.Next
                            Loop
                        End If
                        pMxDoc.Maps.Remove(pMap)
                        pMxDoc.Maps.Add(pMap)
                        pMxDoc.CurrentContentsView.Refresh(Nothing)
                        pMxDoc.UpdateContents()
                    Else
                        activadataframecatastro("CATASTRO MINERO")
                        'activadataframecatastro("DM SIMULTANEO")
                        For k = 1 To mapas.Count - 1
                            'pMap = pMxDoc.FocusMap
                            pMap = pMxDoc.Maps.Item(k)
                            'Set mapa = documento.Maps.Item(0)
                            If pMap.Name <> nombre_dataframe Then
                                'Esta parte para hacer no visible a las capas
                                If pMap.LayerCount > 0 Then
                                    pEnumLayers = pMap.Layers
                                    pLayer = pEnumLayers.Next
                                    Do Until pLayer Is Nothing
                                        pLayer.Visible = False
                                        pLayer = pEnumLayers.Next
                                    Loop
                                    'cls_Catastro.Limpiar_Texto_Pantalla(p_app)
                                End If

                                pMxDoc.UpdateContents()

                            Else
                            End If
                        Next k

                        pMxDoc.UpdateContents()
                    End If
                Next i
                pMxDoc.UpdateContents()

            Catch ex As Exception
                MsgBox("Error en Obtener Posici�n del Dataframe")
            End Try
        End If

    End Sub

    Public Sub activadataframecatastro(ByVal nombre_dataframe As String)
        'Activando el datraframe Catastro Minero
        Dim mapas As IMaps
        mapas = pMxDoc.Maps
        Dim i As Integer
        For i = 0 To mapas.Count - 1
            'Set mapa = mapas.Item(i)
            pMap = mapas.Item(0)
            'Set documento.activeView = mapa
            If pMap.Name = nombre_dataframe Then
                pMxDoc.ActiveView = pMap
                'mapa.MapScale = escalaf
                pMxDoc.UpdateContents()
                Exit Sub
            End If
        Next i
    End Sub

    Public Sub prendecapas()
        pMap = pMxDoc.FocusMap
        Dim m_pEnumLayers As IEnumLayer
        Dim m_pLayer As ILayer
        Dim k As Integer
        For k = 0 To pMap.LayerCount - 1
            m_pEnumLayers = pMap.Layers
            m_pLayer = m_pEnumLayers.Next
            Do Until m_pLayer Is Nothing
                If m_pLayer.Name = "acceditario17" Or m_pLayer.Name = "acceditario18" Or m_pLayer.Name = "acceditario19" Then
                    m_pLayer.Visible = False
                    m_pLayer = m_pEnumLayers.Next
                Else
                    m_pLayer.Visible = True
                    m_pLayer = m_pEnumLayers.Next
                End If
            Loop
        Next k
        pMxDoc.UpdateContents()
    End Sub


    Public Sub CambiardeLayoutaView()
        Dim pActiveView As IActiveView
        Dim pUID As New UID
        pActiveView = pMxDoc.ActiveView
        If Not TypeOf pActiveView Is IPageLayout Then ' Si ActiveView es DataView
            pUID.Value = "esriArcMapUI.LayoutViewCommand"
            pMxDoc.CommandBars.Find(pUID).Execute()
        End If
    End Sub
    Public Sub Generaplanos()

        Dim rutaPlantilla As String = ""
        Try
            If sele_plano = "Formato A4" Then
                rutaPlantilla = glo_pathMXT & "\plantilla_pv_A4.mxt"
            ElseIf sele_plano = "Formato A3" Then
                rutaPlantilla = glo_pathMXT & "\plantilla_pv_A3.mxt"
            ElseIf sele_plano = "Formato A2" Then
                rutaPlantilla = glo_pathMXT & "\plantilla_pv_A2.mxt"
            ElseIf sele_plano = "Formato A0" Then
                rutaPlantilla = glo_pathMXT & "\plantilla_pv_A0.mxt"
            End If
            Dim rutalayout As IGxFile
            rutalayout = New GxMap
            rutalayout.Path = rutaPlantilla
            Dim pGxPageLayout As IGxMapPageLayout
            pGxPageLayout = rutalayout
            Dim pPageLayout As IPageLayout
            pPageLayout = pGxPageLayout.PageLayout
            pPageLayout.ReplaceMaps(pMxDoc.Maps)
            pMxDoc.PageLayout = pPageLayout
            pMapFrame1.MapScale = escalaf  'define otra vez escala al documento
            pMxDoc.UpdateContents()
        Catch ex As Exception
            MsgBox("No se tiene acceso a la Unidad U ...", MsgBoxStyle.Information, "[BDGEOCATMIN]")
        End Try
    End Sub

    Public Sub escogevista()
        Dim cls_eval As New Cls_evaluacion
        'Dim pos As Integer
        Dim pMaps As IMaps2
        'Dim NroMaps As Integer
        pMaps = pMxDoc.Maps
        pMap = pMxDoc.FocusMap
        Dim mensaje_escala As String
        If pMap.Name <> "CATASTRO MINERO" Then
            cls_eval.activadataframe("CATASTRO MINERO")
        End If

        mensaje_escala = InputBox("LA ESCALA A GENERAR EN EL PLANO ES", "Ingresando escala", "100000")
        If mensaje_escala = "" Then
            escala_s = mensaje_escala
        Else
            escala_s = mensaje_escala
        End If
        pMxDoc.ActiveView.Refresh()
        pMxDoc.UpdateContents()
        sele_plano_sup = escala_s
        escalaf = escala_s
    End Sub

    Public Sub creacionmedidasgrillas(ByVal nombre_dataframe As String)
        Dim cls_planos As New Cls_planos

        pMap = pMxDoc.FocusMap
        Dim gridIntervalo As Integer

        'creacion medidasgrid
        pMeasuredGrid = New MeasuredGrid
        m_pMapGrid = pMeasuredGrid
        'Asignando propiedades

        pMeasuredGrid.FixedOrigin = True
        pMeasuredGrid.Units = pMap.MapUnits

        'If nombre_dataframe = "CARTA IGN" Then
        'pMeasuredGrid.XIntervalSize = 4000  'intervalo este
        'pMeasuredGrid.YIntervalSize = 4000 'intervalo norte
        'Else

        If escalaf < 50000 Then
            gridIntervalo = "1000"
        ElseIf ((escalaf >= 50000) And (escalaf < 100000)) Then
            gridIntervalo = "2000"
        ElseIf ((escalaf >= 100000) And (escalaf <= 150000)) Then
            gridIntervalo = "4000"
        ElseIf ((escalaf > 150000) And (escalaf <= 200000)) Then
            gridIntervalo = "5000"
        ElseIf ((escalaf > 200000) And (escalaf <= 300000)) Then
            gridIntervalo = "8000"
        ElseIf ((escalaf > 300000) And (escalaf <= 400000)) Then
            gridIntervalo = "20000"
        Else
            gridIntervalo = "50000"
        End If

        pMeasuredGrid.XIntervalSize = gridIntervalo  'intervalo este
        pMeasuredGrid.YIntervalSize = gridIntervalo 'intervalo norte

        Dim pProjectedGrid As IProjectedGrid
        Dim RgbColor As IRgbColor
        RgbColor = New RgbColor

        pProjectedGrid = pMeasuredGrid
        pProjectedGrid.SpatialReference = pMap.SpatialReference

        Dim pLineSymbol As ISimpleLineSymbol
        pLineSymbol = New SimpleLineSymbol
        pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid
        pLineSymbol.Width = 0.00001

        m_pMapGrid.LineSymbol = pLineSymbol
        RgbColor.RGB = RGB(235, 235, 235)
        m_pMapGrid.Name = "Coordenadas"
        m_pMapGrid.LineSymbol.Color = RgbColor

        'Tick Properties
        'm_pMapGrid.TickLength = 5
        m_pMapGrid.TickLength = 2
        pLineSymbol = New SimpleLineSymbol
        pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid
        pLineSymbol.Width = 0.0001
        m_pMapGrid.TickLineSymbol = pLineSymbol

        'TickMarkSymbol
        Dim pStyleGallery As IStyleGallery
        pStyleGallery = pMxDoc.StyleGallery
        Dim pEnumStyleGall As IEnumStyleGalleryItem
        Dim pStyleItem As IStyleGalleryItem
        Dim pMarkerSym As IMarkerSymbol
        'Set pEnumStyleGall = pStyleGallery.Items("Marker Symbols", "ESRI.style", "")
        pEnumStyleGall = pStyleGallery.Items("Marker Symbols", "esri.style", "")
        pEnumStyleGall.Reset()
        pStyleItem = pEnumStyleGall.Next
        pMarkerSym = pStyleItem.Item
        pMarkerSym.Size = 1.0#
        m_pMapGrid.TickMarkSymbol = pMarkerSym
        'm_pMapGrid.TickMarkSymbol.Color = RgbColor
        'Tick, SubTick, Grid Visibility

        m_pMapGrid.SetTickVisibility(True, True, True, True)
        m_pMapGrid.SetSubTickVisibility(True, True, True, True)
        m_pMapGrid.SetLabelVisibility(True, True, True, True)
        m_pMapGrid.Visible = True
        'm_pMapGrid.TickLineSymbol.Color = RgbColor
        'Label Format

        cls_planos.CreateFormattedGridLabel()
        cls_planos.CreateSimpleMapGridBorder()

        'Add MapGrid to Layout and refresh

        Dim pGraphicsContainer As IGraphicsContainer
        Dim pMapFrame As IMapFrame
        Dim pActiveView As IActiveView
        Dim pMapGrids As IMapGrids
        pGraphicsContainer = pMxDoc.PageLayout
        pMapFrame = pGraphicsContainer.FindFrame(pMap)
        pMapGrids = pMapFrame
        pMapGrids.AddMapGrid(m_pMapGrid)
        pActiveView = pMxDoc.PageLayout
        pActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, Nothing, Nothing)
        'End If

    End Sub
    Public Sub CreateFormattedGridLabel()
        Dim pFormattedGridLabel As IFormattedGridLabel
        pFormattedGridLabel = New FormattedGridLabel
        'Set IGridLabel Properties
        Dim pGridLabel As IGridLabel
        pGridLabel = pFormattedGridLabel
        Dim pFont As IFontDisp
        pFont = New StdFont
        pFont.Name = "tahoma"
        'pFont.Size = 3  'Tamano del texto del rotulado de coordenadas
        pFont.Size = 4.3

        pGridLabel.Font = pFont
        'pGridLabel.Color = BuildRGB(0, 0, 0)
        pGridLabel.LabelOffset = 2

        'Dim RgbColor As IRgbColor
        'RgbColor = New RgbColor
        'RgbColor.RGB = RGB(255, 0, 0)

        'Specify Vertical Labels
        pGridLabel.LabelAlignment(esriGridAxisEnum.esriGridAxisLeft) = False
        pGridLabel.LabelAlignment(esriGridAxisEnum.esriGridAxisRight) = False

        'Set IFormattedGridLabel properties
        Dim pNumericFormat As INumericFormat
        pNumericFormat = New NumericFormat
        pNumericFormat.AlignmentOption = esriNumericAlignmentEnum.esriAlignRight
        pNumericFormat.RoundingOption = esriRoundingOptionEnum.esriRoundNumberOfDecimals
        pNumericFormat.RoundingValue = 0
        pNumericFormat.ShowPlusSign = False
        pNumericFormat.UseSeparator = False
        pNumericFormat.ZeroPad = True
        pFormattedGridLabel.Format = pNumericFormat
        'Make this the mapgrid's label
        m_pMapGrid.LabelFormat = pGridLabel
    End Sub

    Public Sub CreateSimpleMapGridBorder()
        Dim pSimpleMapGridBorder As ISimpleMapGridBorder
        pSimpleMapGridBorder = New SimpleMapGridBorder
        Dim myColor As IColor
        Dim pLineSymbol As ISimpleLineSymbol
        pLineSymbol = New SimpleLineSymbol
        pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid
        myColor = New RgbColor
        'myColor.RGB = RGB(225, 225, 225)
        'myColor.RGB = RGB(225, 225, 225)
        'pLineSymbol.Color = myColor
        pLineSymbol.Width = 0.1
        pSimpleMapGridBorder.LineSymbol = pLineSymbol
        m_pMapGrid.Border = pSimpleMapGridBorder

    End Sub

    Public Sub generaplanos_arearestringida(ByVal p_app As IApplication)
        Dim cls_eval As New Cls_evaluacion
        '  cls_eval.ver_DM_segunresoluciones(m_application)
        Dim cls_planos As New Cls_planos
        Dim cls_dm_2 As New cls_DM_2
        Dim cls_catastro As New cls_DM_1
        cls_planos.buscaadataframe("CATASTRO MINERO", False)
        cls_planos.asignaescaladataframe("CATASTRO MINERO")
        cls_planos.mueveposiciondataframe("CATASTRO MINERO", p_app)
        cls_planos.prendecapas()
        cls_catastro.Zoom_to_Layer("Zona Reservada")
        'cls_catastro.Zoom_to_Layer("Zona Urbana")      ' PARA PROBAR SE COMENTO

        cls_eval.asigna_escaladataframe("CATASTRO MINERO")
        'Exit Sub
        escala_plano_eval = escala_s
        caso_consulta = "CATASTRO MINERO"
        cls_planos.asigna_escaladaplanolayout("CATASTRO MINERO", p_app)
        '  Exit Sub
        escalaf = escala_plano_eval
        cls_planos.Generaplanoreporte("Plano Area Restringida")
        'escalaf = pMap.MapScale

        cls_planos.creacionmedidasgrillas("CATASTRO MINERO")

        'cls_planos.CambiaADataView(p_app)   ' se movio para probar

        cls_planos.AgregarTextosLayout("Area Restringida", "")

        ''caso_consulta = "DEMARCACION POLITICA"
        'cls_planos.agrega_logofoliacion_plano(m_application, "DEMARCACION POLITICA")
        ''   cls_planos.Genera_Leyenda_capa("Leyenda", p_app)
        cls_planos.CambiaADataView(p_app)
        cls_eval.adicionadataframe("PLANO UBICACION")
        cls_eval.temasubicacion("PLANO UBICACION", p_app)
        caso_consulta = "CATASTRO MINERO"
        cls_eval.ActivaDataframe_Opcion(caso_consulta, p_app)
        cls_planos.asigna_escaladaplanolayout("CATASTRO MINERO", p_app)
        cls_planos.CambiaALayout(p_app)
        pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentDefault

        Dim pagina As IPageLayout
        pagina = pMxDoc.PageLayout
        pagina.ZoomToWhole()
        pMxDoc.UpdateContents()
        pMxDoc.ActivatedView.Refresh()

    End Sub

    Public Sub generaplanosdemarcacion(ByVal p_app As IApplication)

        Dim cls_planos As New Cls_planos
        Dim cls_dm_2 As New cls_DM_2
        Dim cls_catastro As New cls_DM_1
        cls_planos.buscaadataframe("DEMARCACION POLITICA", False)
        ' If valida = False Then
        'Exit Sub
        'End If

        cls_planos.asignaescaladataframe("DEMARCACION POLITICA")
        cls_planos.mueveposiciondataframe("DEMARCACION POLITICA", p_app)
        cls_planos.prendecapas()
        cls_catastro.Zoom_to_Layer("Catastro")
        cls_eval.asigna_escaladataframe("DEMARCACION POLITICA")
        caso_consulta = "DEMARCACION POLITICA"
        cls_planos.asigna_escaladaplanolayout("DEMARCACION POLITICA", p_app)
        escalaf = escala_plano_dema
        cls_planos.Generaplanoreporte("Plano Demarca")
        cls_planos.creacionmedidasgrillas("DEMARCACION POLITICA")
        cls_planos.AgregarTextosLayout("Demarca", "")
        'caso_consulta = "DEMARCACION POLITICA"
        cls_planos.agrega_logofoliacion_plano(p_app, "DEMARCACION POLITICA")
        '   cls_planos.Genera_Leyenda_capa("Leyenda", p_app)
        cls_planos.CambiaADataView(p_app)
        cls_eval.adicionadataframe("MAPA UBICACION")
        cls_eval.temasubicacion("MAPA UBICACION", p_app)
        caso_consulta = "DEMARCACION POLITICA"
        cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
        cls_planos.asigna_escaladaplanolayout("DEMARCACION POLITICA", p_app)
        cls_planos.CambiaALayout(p_app)
        pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentDefault
        Dim pagina As IPageLayout
        pagina = pMxDoc.PageLayout
        pagina.ZoomToWhole()
        pMxDoc.UpdateContents()
        pMxDoc.ActiveView.Refresh()
    End Sub

    Public Sub generaplanosimultaneos(ByVal p_app As IApplication)
        Dim cls_planos As New Cls_planos
        Dim cls_dm_2 As New cls_DM_2
        Dim cls_catastro As New cls_DM_1
        cls_planos.buscaadataframe("CATASTRO MINERO", False)
        cls_planos.asignaescaladataframe("CATASTRO MINERO")
        '''cls_planos.mueveposiciondataframe("DEMARCACION POLITICA", p_app)
        '''cls_planos.prendecapas()
        cls_catastro.Actualizar_DM(v_zona_dm)  'dm
        cls_catastro.Zoom_to_Layer("Catastro_sim")
        'cls_catastro.Color_Poligono_Simple_2(m_application, "DM_simultaneos")
        If Val(num_cuasim) = "1" Then
            cls_catastro.Color_Poligono_Simple_2(m_application, "Cuadricula_sim")
        Else
            cls_catastro.Color_Poligono_Simple_2(m_application, "Cuadricula_dsim")
        End If
        cls_catastro.Color_Poligono_Simple_3(m_application, "Catastro_sim")
        pMxDoc.UpdateContents()
        'cls_catastro.DefinitionExpression_2(lista_dm_sim, p_app, "DM_simultaneos")   ' NEW
        cls_eval.asigna_escaladataframe("CATASTRO MINERO")
        cls_planos.asigna_escaladaplanolayout("CATASTRO MINERO", p_app)
        'escalaf = escala_plano_simul
        escalaf = escala_plano_eval
        '''cls_planos.Generaplanoreporte("Plano Demarca")
        cls_planos.Generaplanoreporte("Plano Simultaneo")
       
        LEERESULTADOSEVAL_SIM("Plano Simultaneo")
        cls_planos.creacionmedidasgrillas("CATASTRO MINERO")
        ''caso_consulta = "DEMARCACION POLITICA"
        '''cls_planos.agrega_logofoliacion_plano(p_app, "DEMARCACION POLITICA")
        '   cls_planos.Genera_Leyenda_capa("Leyenda", p_app)
        '''cls_planos.CambiaADataView(p_app)
        '''cls_eval.adicionadataframe("MAPA UBICACION")
        '''cls_eval.temasubicacion("MAPA UBICACION", p_app)
        '''caso_consulta = "DEMARCACION POLITICA"
        '''cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
        'cls_planos.asigna_escaladaplanolayout("Dm_simultaneos", p_app)
        cls_planos.asigna_escaladaplanolayout("CATASTRO MINERO", p_app)
        cls_planos.CambiaALayout(p_app)
        pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentDefault
        Dim pagina As IPageLayout
        pagina = pMxDoc.PageLayout
        pagina.ZoomToWhole()
        pMxDoc.UpdateContents()
        esc_sim = ""
    End Sub

    Public Sub generaplanosimultaneoev(ByVal p_app As IApplication)
        Dim cls_planos As New Cls_planos
        Dim cls_dm_2 As New cls_DM_2
        Dim cls_catastro As New cls_DM_1
        cls_planos.buscaadataframe("DM SIMULTANEO", False)
        caso_consulta = "CATASTRO MINERO"
        cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
        cls_planos.asignaescaladataframe_sim("CATASTRO MINERO")
        escala_plano_eval = pMap.MapScale
        caso_consulta = "DM SIMULTANEO"
        cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
        cls_planos.mueveposiciondataframe_siev("DM SIMULTANEO", p_app)
        cls_planos.prendecapas()
        'cls_catastro.Actualizar_DM(v_zona_dm)  'dm
        cls_catastro.Zoom_to_Layer("Catastro_sim")
        'cls_catastro.Color_Poligono_Simple_2(m_application, "DM_simultaneos")
        If Val(num_cuasim) = "1" Then
            cls_catastro.Color_Poligono_Simple_2(m_application, "Cuadricula_sim")
        Else
            cls_catastro.Color_Poligono_Simple_2(m_application, "Cuadricula_dsim")
        End If
        cls_catastro.Color_Poligono_Simple_3(m_application, "Catastro_sim")
        pMxDoc.UpdateContents()
        'cls_catastro.DefinitionExpression_2(lista_dm_sim, p_app, "DM_simultaneos")   ' NEW
        cls_eval.asigna_escaladataframe("DM SIMULTANEO")
        'cls_planos.asigna_escaladaplanolayout("DM SIMULTANEO", p_app)
        escalaf = escala_plano_simul
        '''cls_planos.Generaplanoreporte("Plano Demarca")
        cls_planos.Generaplanoreporte("Plano Simultaneo")

        LEERESULTADOSEVAL_SIM("Plano Simultaneo")
        cls_planos.creacionmedidasgrillas("DM SIMULTANEO")
        cls_planos.CambiaADataView(p_app)
        'cls_eval.adicionadataframe("DM SIMULTANEO")
        'cls_eval.temasubicacion("DM SIMULTANEO", p_app)
        caso_consulta = "DM SIMULTANEO"
        ' cls_eval.adicionadataframe("DM SIMULTANEO")
        cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)

        cls_planos.asigna_escaladaplanolayout("DM SIMULTANEO", p_app)

        cls_planos.CambiaALayout(p_app)
        pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentDefault
        Dim pagina As IPageLayout
        pagina = pMxDoc.PageLayout
        pagina.ZoomToWhole()
        pMxDoc.UpdateContents()
        esc_sim = ""
    End Sub

    Public Sub genera_plano_Geologico(ByVal p_app As IApplication)
        Try
            Dim cls_planos As New Cls_planos
            Dim cls_dm_2 As New cls_DM_2
            Dim cls_catastro As New cls_DM_1
            Dim tema As IFeatureLayer
            Dim afound As Boolean

            cls_planos.buscaadataframe("MAPA GEOLOGICO", False)
            If valida = False Then
                Exit Sub
            End If

            cls_planos.asignaescaladataframe("MAPA GEOLOGICO")
            cls_planos.mueveposiciondataframe("MAPA GEOLOGICO", p_app)
            cls_planos.prendecapas()
            Dim capa As String
            Dim archivo As String
            Dim aliasname As String
            Dim leyen As String
            ' If colecciones_cd_goelogia_50.Count > 0 Then
            If valida_capa_geologica = 1 Then
                contador_capa_100 = 1
                'If cantidad_capa_geo50 > 0 Then
                If selecciona_capa_geologica = "Capa_Geologica_fran_50" Then
                    leyen = "Geo_ley_fran50"
                    aliasname = "Geologia_fran50"
                    archivo = loStrShapefileGEO_50k
                    capa = "Geo_fran50"
                    'ElseIf colecciones_cd_goelogia_100.Count > 0 Then
                    'ElseIf cantidad_capa_geo100 > 0 Then
                ElseIf selecciona_capa_geologica = "Capa_Geologica_fran_100" Then
                    leyen = "Geo_ley_fran100"
                    aliasname = "Geologia_fran100"
                    archivo = loStrShapefileGEO_100k
                    capa = "Geo_fran100"
                    'Else
                ElseIf selecciona_capa_geologica = "Capa_Geologica_bol" Then
                    leyen = "Geo_ley"
                    aliasname = "Geologia_bol"
                    archivo = loStrShapefileGEO
                    capa = "Geo_bol100"

                End If

                For A As Integer = 0 To pMap.LayerCount - 1
                    'If pMap.Layer(A).Name = "Geo_bol100" Then
                    If pMap.Layer(A).Name = capa Then
                        tema = pMap.Layer(A)
                        tema.Visible = False
                        afound = True
                        Exit For
                    End If
                Next A

                pMxDoc.UpdateContents()

                'cls_catastro.Add_ShapeFile2(loStrShapefileGEO, p_app, "Geologia")
                cls_catastro.Add_ShapeFile2(archivo, p_app, aliasname)
                Dim leyendageo As New DataTable
                'leyendageo = cls_catastro.f_Genera_Leyenda_DM("Geo_ley", p_app)
                'cls_catastro.Poligono_style_GEO("Geo_ley", leyendageo, glo_pathStyle & "\geologia13022013.style", "CODI", "Geolog�a", p_app)
                'cls_catastro.Genera_Cambio_LeyendaTot("Geo_ley", p_app)

                leyendageo = cls_catastro.f_Genera_Leyenda_DM(leyen, p_app)
                ' cls_catastro.Poligono_style_GEO(leyen, leyendageo, glo_pathStyle & "\geologia13022013.style", "CODI", "Geolog�a", p_app)
                cls_catastro.Poligono_style_GEO(leyen, leyendageo, glo_pathStyle & "\geologia.style", "CODI", "Geolog�a", p_app)
                cls_catastro.Genera_Cambio_LeyendaTot(leyen, p_app)

            ElseIf valida_capa_geologica = 2 Then

                'If selecciona_capa_geologica = "Capa_Geologica_fran_50__Capa_Geologica_fran_100" Then
                For t As Integer = 1 To contador_capa_100
                    If t = 1 Then
                        Select Case selecciona_capa_geologica
                            Case "Capa_Geologica_fran_50__Capa_Geologica_fran_100"

                                ' If cantidad_capa_geo50 > 0 Then

                                leyen = "Geo_ley_fran50"
                                aliasname = "Geologia_fran50"
                                archivo = loStrShapefileGEO_50k
                                capa = "Geo_fran50"
                            Case "Capa_Geologica_fran_50__Capa_Geologica_bol"

                                ' If cantidad_capa_geo50 > 0 Then

                                leyen = "Geo_ley_fran50"
                                aliasname = "Geologia_fran50"
                                archivo = loStrShapefileGEO_50k
                                capa = "Geo_fran50"
                            Case "Capa_Geologica_fran_100__Capa_Geologica_bol"

                                ' If cantidad_capa_geo50 > 0 Then

                                leyen = "Geo_ley_fran100"
                                aliasname = "Geologia_fran100"
                                archivo = loStrShapefileGEO_100k
                                capa = "Geo_fran100"

                        End Select

                        'ElseIf colecciones_cd_goelogia_100.Count > 0 Then
                    ElseIf t = 2 Then
                        Select Case selecciona_capa_geologica
                            Case "Capa_Geologica_fran_50__Capa_Geologica_fran_100"
                                leyen = "Geo_ley_fran100"
                                aliasname = "Geologia_fran100"
                                archivo = loStrShapefileGEO_100k
                                capa = "Geo_fran100"
                            Case "Capa_Geologica_fran_50__Capa_Geologica_bol"
                                leyen = "Geo_ley"
                                aliasname = "Geologia_bol"
                                archivo = loStrShapefileGEO
                                capa = "Geo_bol100"
                            Case "Capa_Geologica_fran_100__Capa_Geologica_bol"
                                leyen = "Geo_ley"
                                aliasname = "Geologia_bol"
                                archivo = loStrShapefileGEO
                                capa = "Geo_bol100"

                        End Select

                    End If

                    For A As Integer = 0 To pMap.LayerCount - 1
                        'If pMap.Layer(A).Name = "Geo_bol100" Then
                        If pMap.Layer(A).Name = capa Then
                            tema = pMap.Layer(A)
                            tema.Visible = False
                            afound = True
                            Exit For
                        End If
                    Next A

                    pMxDoc.UpdateContents()

                    'cls_catastro.Add_ShapeFile2(loStrShapefileGEO, p_app, "Geologia")
                    cls_catastro.Add_ShapeFile2(archivo, p_app, aliasname)
                    Dim leyendageo As New DataTable
                    'leyendageo = cls_catastro.f_Genera_Leyenda_DM("Geo_ley", p_app)
                    'cls_catastro.Poligono_style_GEO("Geo_ley", leyendageo, glo_pathStyle & "\geologia13022013.style", "CODI", "Geolog�a", p_app)
                    'cls_catastro.Genera_Cambio_LeyendaTot("Geo_ley", p_app)

                    leyendageo = cls_catastro.f_Genera_Leyenda_DM(leyen, p_app)
                    ' cls_catastro.Poligono_style_GEO(leyen, leyendageo, glo_pathStyle & "\geologia13022013.style", "CODI", "Geolog�a", p_app)
                    cls_catastro.Poligono_style_GEO(leyen, leyendageo, glo_pathStyle & "\geologia.style", "CODI", "Geolog�a", p_app)
                    cls_catastro.Genera_Cambio_LeyendaTot(leyen, p_app)

                Next t
                'End If

            ElseIf valida_capa_geologica = 3 Then  'Si se encuentra superpuesto a las 3 capas 

                For t As Integer = 1 To contador_capa_100
                    If t = 1 Then
                        leyen = "Geo_ley_fran50"
                        aliasname = "Geologia_fran50"
                        archivo = loStrShapefileGEO_50k

                    ElseIf t = 2 Then
                        leyen = "Geo_ley_fran100"
                        aliasname = "Geologia_fran100"
                        archivo = loStrShapefileGEO_100k
                        capa = "Geo_fran100"
                    ElseIf t = 3 Then

                        leyen = "Geo_ley"
                        aliasname = "Geologia_bol"
                        archivo = loStrShapefileGEO
                        capa = "Geo_bol100"
                    End If

                    For A As Integer = 0 To pMap.LayerCount - 1
                        'If pMap.Layer(A).Name = "Geo_bol100" Then
                        If pMap.Layer(A).Name = capa Then
                            tema = pMap.Layer(A)
                            tema.Visible = False
                            afound = True
                            Exit For
                        End If
                    Next A

                    pMxDoc.UpdateContents()


                    'cls_catastro.Add_ShapeFile2(loStrShapefileGEO, p_app, "Geologia")
                    cls_catastro.Add_ShapeFile2(archivo, p_app, aliasname)
                    Dim leyendageo As New DataTable
                    'leyendageo = cls_catastro.f_Genera_Leyenda_DM("Geo_ley", p_app)
                    'cls_catastro.Poligono_style_GEO("Geo_ley", leyendageo, glo_pathStyle & "\geologia13022013.style", "CODI", "Geolog�a", p_app)
                    'cls_catastro.Genera_Cambio_LeyendaTot("Geo_ley", p_app)

                    leyendageo = cls_catastro.f_Genera_Leyenda_DM(leyen, p_app)
                    '    cls_catastro.Poligono_style_GEO(leyen, leyendageo, glo_pathStyle & "\geologia13022013.style", "CODI", "Geolog�a", p_app)
                    cls_catastro.Poligono_style_GEO(leyen, leyendageo, glo_pathStyle & "\geologia.style", "CODI", "Geolog�a", p_app)
                    cls_catastro.Genera_Cambio_LeyendaTot(leyen, p_app)

                Next t
            End If


            'Aplicada leyenda en la tabla para generar estilos
            '   Dim leyendageo As New DataTable
            'Dim nmcapa As String

            'For A As Integer = 0 To pMap.LayerCount - 1
            '    Dim leyendageo As New DataTable
            '    If pMap.Layer(A).Name <> "Catastro" Then
            '        tema = pMap.Layer(A)
            '        'tema.Visible = True
            '        nmcapa = tema.Name



            '        leyendageo = cls_catastro.f_Genera_Leyenda_DM(nmcapa, p_app)
            '        ''Va en categoria Geolog�a por que en el estilo esta sino iria vacio
            '        '  cls_catastro.Poligono_style_GEO(nmcapa, leyendageo, glo_pathStyle & "\geologia13022013.style", "CODI", "Geolog�a", p_app)
            '        cls_catastro.Poligono_style_GEO(nmcapa, leyendageo, glo_pathStyle & "\geologia.style", "CODI", "Geolog�a", p_app)

            '        ' cls_catastro.Genera_Cambio_LeyendaTot(nmcapa, p_app)
            '        cls_catastro.ShowLabel_DM(nmcapa, p_app)
            '        cls_catastro.Genera_Cambio_LeyendaTot(nmcapa, p_app)
            '    End If
            'Next A


            cls_catastro.Zoom_to_Layer("Catastro")

            '  Exit Sub

            cls_eval.asigna_escaladataframe("MAPA GEOLOGICO")

            cls_planos.asigna_escaladaplanolayout("MAPA GEOLOGICO", p_app)
            escalaf = escala_plano_geolo
            cls_planos.Generaplanoreporte("Plano Geologico")
            cls_planos.creacionmedidasgrillas("MAPA GEOLOGICO")
            cls_planos.AgregarTextosLayout("Geologico", "")


            '  cls_planos.agrega_logofoliacion_plano(p_app, "MAPA GEOLOGICO")
            cls_planos.CambiaADataView(p_app)
            cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
            cls_planos.Genera_Leyenda_capa("Leyenda", p_app)
            'cls_planos.CambiaADataView(p_app)

            'If valida_capa_geologica = 1 Then
            If contador_capa_100 = 1 Then
                pMap.DeleteLayer(pMap.Layer(0))  'Eliminando la capa de ayuda para generar leyenda de superposicion Geo-DM
                'ElseIf valida_capa_geologica = 2 Then
            ElseIf contador_capa_100 = 2 Then

                pMap.DeleteLayer(pMap.Layer(0))  'Eliminando la capa de ayuda para generar leyenda de superposicion Geo-DM
                pMap.DeleteLayer(pMap.Layer(0))  'Eliminando la capa de ayuda para generar leyenda de superposicion Geo-DM
                'ElseIf valida_capa_geologica = 3 Then
            ElseIf contador_capa_100 = 3 Then
                pMap.DeleteLayer(pMap.Layer(0))  'Eliminando la capa de ayuda para generar leyenda de superposicion Geo-DM
                pMap.DeleteLayer(pMap.Layer(0))
                pMap.DeleteLayer(pMap.Layer(0))
            End If
            'Haciendo visislbe la otra capa

            For A As Integer = 0 To pMap.LayerCount - 1
                'If pMap.Layer(A).Name = capa Then
                tema = pMap.Layer(A)
                tema.Visible = True
                'afound = True
                'Exit For
                'End If
            Next A


            'For A As Integer = 0 To pMap.LayerCount - 1
            '    If pMap.Layer(A).Name = "Geo_bol100" Then
            '        tema = pMap.Layer(A)
            '        tema.Visible = True
            '        afound = True
            '        Exit For
            '    End If
            'Next A

            'cls_eval.adicionadataframe("MAPA UBICACION")
            'cls_eval.temasubicacion("MAPA UBICACION", p_app)
            'caso_consulta = "DEMARCACION POLITICA"
            'cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
            'cls_planos.asigna_escaladaplanolayout("DEMARCACION POLITICA", p_app)

            caso_consulta = "MAPA GEOLOGICO"
            cls_eval.ActivaDataframe_Opcion("MAPA GEOLOGICO", p_app)

            cls_planos.CambiaALayout(p_app)
            pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentDefault
            Dim pagina As IPageLayout
            pagina = pMxDoc.PageLayout
            pagina.ZoomToWhole()


            ' pMxDoc.UpdateContents()



            'cls_planos.agrega_logofoliacion_plano(p_app, "CARTA IGN")
            'cls_planos.CambiaADataView(p_app)
            'cls_eval.adicionadataframe("MAPA EMPALME")
            'cls_eval.temasubicacion("MAPA DE EMPALME", p_app)
            'caso_consulta = "CARTA IGN"
            'cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
            'cls_planos.asigna_escaladaplanolayout("CARTA IGN", p_app)
            'cls_planos.CambiaALayout(p_app)
            'pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentDefault
            'Dim pagina As IPageLayout
            'pagina = pMxDoc.PageLayout
            'pagina.ZoomToWhole()
        Catch
            MsgBox("Error en procedimiento Plano Geologico", MsgBoxStyle.Information, "SIGCATMIN")
        End Try




    End Sub



    Public Sub generaplanoscarta(ByVal p_app As IApplication)
        Dim cls_planos As New Cls_planos
        Dim cls_dm_2 As New cls_DM_2
        Dim cls_catastro As New cls_DM_1
        cls_planos.buscaadataframe("CARTA IGN", False)
        If valida = False Then
            Exit Sub
        End If

        cls_planos.asignaescaladataframe("CARTA IGN")
        cls_planos.mueveposiciondataframe("CARTA IGN", p_app)
        cls_planos.prendecapas()
        cls_catastro.Zoom_to_Layer("Catastro")
        cls_eval.asigna_escaladataframe("CARTA IGN")
        cls_planos.asigna_escaladaplanolayout("CARTA IGN", p_app)
        escalaf = escala_plano_carta
        'cls_planos.Defineproyeccion_UTM()
        cls_catastro.Actualizar_DM(v_zona_dm)
        cls_planos.Generaplanoreporte("Plano carta")
        cls_planos.creacionmedidasgrillas("CARTA IGN")
        'caso_consulta = "CARTA IGN"
        cls_planos.AgregarTextosLayout("Carta", "")
        cls_planos.agrega_logofoliacion_plano(p_app, "CARTA IGN")
        cls_planos.CambiaADataView(p_app)
        cls_eval.adicionadataframe("MAPA EMPALME")
        cls_eval.temasubicacion("MAPA DE EMPALME", p_app)
        caso_consulta = "CARTA IGN"
        cls_eval.activadataframe_opcion(caso_consulta, m_application)
        cls_planos.asigna_escaladaplanolayout("CARTA IGN", p_app)
        cls_planos.CambiaALayout(p_app)
        pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentDefault
        Dim pagina As IPageLayout
        pagina = pMxDoc.PageLayout
        pagina.ZoomToWhole()


    End Sub
    Public Sub removedataframeadicional(ByVal nombre_dataframe As String)
        'Activando el datraframe adicional
        Dim mapas As IMaps
        mapas = pMxDoc.Maps
        pMap = pMxDoc.FocusMap
        Dim i As Integer
        For i = 0 To mapas.Count - 1
            pMap = mapas.Item(i)
            pMxDoc.UpdateContents()
            If nombre_dataframe = "CARTA IGN" Then
                If pMap.Name = "MAPA DE EMPALME" Then
                    mapas.RemoveAt(i)
                    pMxDoc.UpdateContents()
                    Exit Sub
                End If
            ElseIf nombre_dataframe = "DEMARCACION POLITICA" Then
                If pMap.Name = "MAPA UBICACION" Then
                    mapas.RemoveAt(i)
                    pMxDoc.UpdateContents()
                    Exit Sub
                End If
            ElseIf nombre_dataframe = "CATASTRO MINERO" Then
                If pMap.Name = "PLANO UBICACION" Then
                    mapas.RemoveAt(i)
                    pMxDoc.UpdateContents()
                    Exit Sub
                End If
            End If


        Next i
        pMxDoc.UpdateContents()
    End Sub

    Public Sub asignaescaladataframe_sim(ByVal nombre_dataframe As String)
        'Activando el datraframe Catastro Minero
        Dim mapas As IMaps
        mapas = pMxDoc.Maps
        Dim i As Integer
        For i = 0 To mapas.Count - 1
            pMap = mapas.Item(i)
            'pMap = mapas.Item(0)
            'Set documento.activeView = mapa
            If pMap.Name = nombre_dataframe Then
                pMap.MapScale = 1.0
                pMxDoc.UpdateContents()
                Exit Sub
            End If
        Next i
    End Sub

    Public Sub asignaescaladataframe(ByVal nombre_dataframe As String)
        'Activando el datraframe Catastro Minero
        Dim mapas As IMaps
        mapas = pMxDoc.Maps
        Dim i As Integer
        For i = 0 To mapas.Count - 1
            pMap = mapas.Item(i)
            'pMap = mapas.Item(0)
            'Set documento.activeView = mapa
            If pMap.Name <> nombre_dataframe Then
                If pMap.Name = "MAPA DE EMPALME" Then
                ElseIf pMap.Name = "MAPA UBICACION" Then
                Else
                    pMap.MapScale = 0.5
                End If
                pMxDoc.UpdateContents()
                Exit Sub
            End If
        Next i
    End Sub
    Public Sub buscaadataframe(ByVal nombre_dataframe As String, ByVal valida1 As Boolean)
        'Activando el datraframe Catastro Minero
        Dim mapas As IMaps
        mapas = pMxDoc.Maps
        pMap = pMxDoc.FocusMap
        Dim i As Integer
        For i = 0 To mapas.Count - 1
            pMap = mapas.Item(i)
            If pMap.Name = nombre_dataframe Then
                valida1 = True
                valida = valida1
                Exit Sub
            Else
                valida = False

            End If
        Next i
    End Sub

    Public Sub cambiacolor_frame(ByVal SELE_CASO As String)
        Dim pFillSymbol As IFillSymbol
        Dim myColor As IColor
        Dim pActiveView As IActiveView
        'PROGRAMA PARA CAMBIAR COLOR AL FRAMA - VISTA PREVIA DEL DM
        '*************************************************************
        pGraphicsContainer = pMxDoc.PageLayout
        'Frame asociado al focus map
        pMapFrame = pGraphicsContainer.FindFrame(pMxDoc.FocusMap)
        If pMapFrame Is Nothing Then
            Exit Sub
        End If
        'Aplicando simbologia al frame

        pSymbolBackground = pMapFrame.Background
        If pSymbolBackground Is Nothing Then
            pSymbolBackground = New SymbolBackground
        End If
        pFillSymbol = New SimpleFillSymbol
        myColor = New RgbColor

        If SELE_CASO = "PORCOLOR" Then
            myColor.RGB = RGB(225, 225, 225)
        ElseIf SELE_CASO = "NORMAL" Then
            myColor.RGB = RGB(255, 255, 255)
        ElseIf SELE_CASO = "Mar" Then
            myColor.RGB = RGB(214, 255, 252)
        Else
            myColor.RGB = RGB(255, 255, 255)
        End If
        pFillSymbol.Color = myColor
        pSymbolBackground.FillSymbol = pFillSymbol
        pMapFrame.Background = pSymbolBackground
        'Refresca el frame
        pActiveView = pMxDoc.FocusMap
        pActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, Nothing, Nothing)
        pMxDoc.UpdateContents()

    End Sub

    Public Sub Genera_Plano_Areasup_dmxAreaRestringida(ByVal pApp As IApplication, ByVal p_ListBox As Object)

        '  If v_cantiprioritarios > 0 Then

        v_nmrese_sup = ""

        ' For w As Integer = 0 To p_ListBox.BindingContext(p_ListBox.DataSource, p_ListBox.DataMember).Count - 1
        'v_nmrese_sup = p_ListBox.Item(w, "NM_RESE").ToString


        'Next w

        For w As Integer = 0 To p_ListBox.BindingContext(p_ListBox.DataSource, p_ListBox.DataMember).Count - 1

            If p_ListBox.Item(w, "SELEC") = True Then
                v_nmrese_sup = p_ListBox.Item(w, "NM_RESE").ToString
            End If


        Next


        Dim Cls_evaluacion As New Cls_evaluacion
        ' Dim RetVal
        'RetVal = Shell(glo_Path & "\estilos\Barra_Proceso.exe", 1)
        Dim cls_eval As New Cls_evaluacion
        Dim cls_planos As New Cls_planos
        Dim cls_catastro As New cls_DM_1
        Dim pForm1 As New Frm_formatoplanos
        pForm1.p_App = p_App
        '  Dim afound As Boolean = False
        Dim pFeatLayer As IFeatureLayer

        pForm1.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
        ' cls_catastro.Verifica_areasuper_dmxArearestringida("Areainter_" & v_codigo & fecha_archi1, m_application)
        '  cls_catastro.Verifica_areasuper_dmxArearestringida("AreaRese_super", m_application)


        For A As Integer = 0 To pMap.LayerCount - 1
            If pMap.Layer(A).Name = "AreaRese_super" Then
                pFeatLayer = pMap.Layer(A)
                '   pGeoFeatureL = pFeatLayer 
                v_existe_sup = True
                Exit For
            End If
        Next A
        If Not v_existe_sup Then
            v_existe_sup = False
        End If


        'Dim formulario As New Frm_formatoplanos
        cls_planos.asignaescaladataframe("CATASTRO MINERO")  'COMENTO
        cls_planos.mueveposiciondataframe("CATASTRO MINERO", pApp)  'COMENTO
        cls_planos.prendecapas() 'SE COEMTNO
        cls_catastro.Zoom_to_Layer("AreaRese_super")
        'cls_catastro.Zoom_to_Layer("Areadispo_" & v_codigo)

        pMxDoc.UpdateContents()

        Dim escala_sup As Long
        escala_sup = pMap.MapScale
        escalaf = escala_sup
        cls_eval.asigna_escaladataframe("CATASTRO MINERO")
        ' escala_sup = InputBox("Ingrese la escala del Plano Area Superpuesta", "BGGEOCATMIN", "100000")
        'If escala_sup = 0 Then
        'MsgBox("No ingreso Escala, se asumir� escala 1:100000", MsgBoxStyle.Critical, "BDGEOCATMIN")
        'End If
        'pMap.MapScale = escala_sup
        pMxDoc.UpdateContents()
        'v_existe_sup = False
        If v_existe_sup = True Then
            ' fecha_archi_sup_t = DateTime.Now.Ticks.ToString
            'cls_catastro.creandotabla_Areasup()
            s_tipo_plano = "Plano DMxArea_Restringida"
            Dim colec As String
            If colecciones_planos.Count > 0 Then
                For contador As Integer = 1 To colecciones_planos.Count
                    colec = colecciones_planos.Item(contador)
                    pForm1.lstformatoplanos.Items.Add(colec)
                Next contador
                'pForm1.Show()
            Else
                'formulario.lstformatoplanos.Items.Clear()

                pForm1.lstformatoplanos.Items.Clear()
                fecha_archi_sup_t = DateTime.Now.Ticks.ToString
                cls_catastro.creandotabla_Areasup()

                conta_hoja_sup = 1
                conta_reg = 0
                'cls_planos.Generaplanoreporte("Plano Area superpuesta")
                colecciones_planos.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                'formulario.lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                pForm1.Refresh()
                cls_planos.Obtieneposicion_Plano_1(pForm1.lstformatoplanos)
                'pForm1.ShowDialog()
            End If
            'Else
            '    pForm1.lstformatoplanos.Items.Clear()
            '    'cls_formulario.lstformatoplanos.ClearSelected()
            '    conta_hoja_sup = 1
            '    conta_reg = 0
            '    'v_codigo_x = ""
            '    'nombre_dm_x = ""
            '    'areaf = 0
            '    'escalaf = pMap.MapScale
            '    cls_planos.Generaplanoreporte("Plano Area superpuesta")
            '    pForm1.lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
            '    colecciones_planos.Add("Plano Area superpuesta N�" & conta_hoja_sup)
            '    pForm1.Refresh()
            '    cls_planos.Obtieneposicion_Plano_1(pForm1.lstformatoplanos)
            '    'pForm1.ShowDialog()
            'End If

        Else

            pForm1.lstformatoplanos.Items.Clear()
            'cls_formulario.lstformatoplanos.ClearSelected()
            conta_hoja_sup = 1
            conta_reg = 0
            'v_codigo_x = ""
            'nombre_dm_x = ""
            'areaf = 0
            'escalaf = pMap.MapScale
            cls_planos.Generaplanoreporte("Plano Area superpuesta")
            pForm1.lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
            colecciones_planos.Add("Plano Area superpuesta N�" & conta_hoja_sup)
            pForm1.Refresh()
            cls_planos.Obtieneposicion_Plano_1(pForm1.lstformatoplanos)


        End If




        Cls_evaluacion.cierra_ejecutable()
        pForm1.ShowDialog()
        v_calculAreaint = True

        ' Else
        'MsgBox("El DM Evaluado no presenta D.M. Prioritarios para generar el Plano de Areas Superpuestas", MsgBoxStyle.Information, "[SIGCATMIN]")
        'Exit Sub
        '   End If
        ' Next w

    End Sub


    Public Sub Genera_Plano_Area_Disponible(ByVal pApp As IApplication)

        If v_cantiprioritarios > 0 Then
            Dim Cls_evaluacion As New Cls_evaluacion
            Dim RetVal
            RetVal = Shell(glo_Path & "\estilos\Barra_Proceso.exe", 1)
            Dim cls_eval As New Cls_evaluacion
            Dim cls_planos As New Cls_planos
            Dim cls_catastro As New cls_DM_1
            Dim pForm1 As New Frm_formatoplanos
            pForm1.p_App = p_App
            pForm1.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
            cls_catastro.Verifica_areasuperpuesta("Areainter_" & v_codigo & fecha_archi1, m_application)

            'Dim formulario As New Frm_formatoplanos
            cls_planos.asignaescaladataframe("CATASTRO MINERO")  'COMENTO
            cls_planos.mueveposiciondataframe("CATASTRO MINERO", pApp)  'COMENTO
            cls_planos.prendecapas() 'SE COEMTNO
            cls_catastro.Zoom_to_Layer("Areainter_" & v_codigo)
            cls_catastro.Zoom_to_Layer("Areadispo_" & v_codigo)
            'cls_catastro.Zoom_to_Layer("Catastro")                
            pMxDoc.UpdateContents()
            'pMap.MapScale = escala_plano_eval
            Dim escala_sup As Long
            escala_sup = pMap.MapScale
            escalaf = escala_sup
            cls_eval.asigna_escaladataframe("CATASTRO MINERO")
            ' escala_sup = InputBox("Ingrese la escala del Plano Area Superpuesta", "BGGEOCATMIN", "100000")
            'If escala_sup = 0 Then
            'MsgBox("No ingreso Escala, se asumir� escala 1:100000", MsgBoxStyle.Critical, "BDGEOCATMIN")
            'End If
            'pMap.MapScale = escala_sup
            pMxDoc.UpdateContents()
            'v_existe_sup = False
            If v_existe_sup = True Then
                Dim colec As String
                If colecciones_planos.Count > 0 Then
                    For contador As Integer = 1 To colecciones_planos.Count
                        colec = colecciones_planos.Item(contador)
                        pForm1.lstformatoplanos.Items.Add(colec)
                    Next contador
                    'pForm1.Show()
                Else
                    'formulario.lstformatoplanos.Items.Clear()
                    pForm1.lstformatoplanos.Items.Clear()
                    conta_hoja_sup = 1
                    conta_reg = 0
                    'cls_planos.Generaplanoreporte("Plano Area superpuesta")
                    colecciones_planos.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                    'formulario.lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                    pForm1.Refresh()
                    cls_planos.Obtieneposicion_Plano_1(pForm1.lstformatoplanos)
                    'pForm1.ShowDialog()
                End If
            Else
                pForm1.lstformatoplanos.Items.Clear()
                'cls_formulario.lstformatoplanos.ClearSelected()
                conta_hoja_sup = 1
                conta_reg = 0
                'v_codigo_x = ""
                'nombre_dm_x = ""
                'areaf = 0
                'escalaf = pMap.MapScale
                cls_planos.Generaplanoreporte("Plano Area superpuesta")
                pForm1.lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                colecciones_planos.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                pForm1.Refresh()
                cls_planos.Obtieneposicion_Plano_1(pForm1.lstformatoplanos)
                'pForm1.ShowDialog()
            End If
            Cls_evaluacion.cierra_ejecutable()
            pForm1.ShowDialog()
            v_calculAreaint = True
        Else
            MsgBox("El DM Evaluado no presenta D.M. Prioritarios para generar el Plano de Areas Superpuestas", MsgBoxStyle.Information, "[BDGEOCATMIN]")
            Exit Sub
        End If
    End Sub



    Public Sub Genera_Plano_Acumulacion(ByVal pApp As IApplication)

        Dim Cls_evaluacion As New Cls_evaluacion
        s_tipo_plano = "Plano Acumulacion"
        Dim cls_eval As New Cls_evaluacion
        Dim cls_planos As New Cls_planos
        Dim cls_catastro As New cls_DM_1
        Dim pForm1 As New Frm_formatoplanos
        pForm1.p_App = p_App
        pForm1.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
        'cls_catastro.Verifica_areasuperpuesta("Areainter_" & v_codigo & fecha_archi1, m_application)


        cls_planos.asignaescaladataframe("CATASTRO MINERO")  'COMENTO
        cls_planos.mueveposiciondataframe("CATASTRO MINERO", pApp)  'COMENTO
        'cls_planos.prendecapas() 'SE COEMTNO
        'cls_catastro.Zoom_to_Layer("Areainter_" & v_codigo)
        'cls_catastro.Zoom_to_Layer("Areadispo_" & v_codigo)
        'cls_catastro.Zoom_to_Layer("Catastro")
        pMxDoc.UpdateContents()
        'pMap.MapScale = escala_plano_eval
        Dim escala_sup As Long
        escala_sup = pMap.MapScale
        escalaf = escala_sup
        cls_eval.asigna_escaladataframe("CATASTRO MINERO")

        pMxDoc.UpdateContents()
        'v_existe_sup = False
        If v_existe_sup = True Then
            Dim colec As String
            If colecciones_planos.Count > 0 Then
                For contador As Integer = 1 To colecciones_planos.Count
                    colec = colecciones_planos.Item(contador)
                    pForm1.lstformatoplanos.Items.Add(colec)
                Next contador
                'pForm1.Show()
            Else
                'formulario.lstformatoplanos.Items.Clear()
                pForm1.lstformatoplanos.Items.Clear()
                conta_hoja_sup = 1
                conta_reg = 0
                'cls_planos.Generaplanoreporte("Plano Area superpuesta")
                colecciones_planos.Add("Plano Acumulacion N�" & conta_hoja_sup)
                'formulario.lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                pForm1.Refresh()
                cls_planos.Obtieneposicion_Plano_1(pForm1.lstformatoplanos)
                'pForm1.ShowDialog()
            End If
        Else
            pForm1.lstformatoplanos.Items.Clear()
            'cls_formulario.lstformatoplanos.ClearSelected()
            conta_hoja_sup = 1
            conta_reg = 0

            cls_planos.Generaplanoreporte("Plano Acumulacion")

            pForm1.lstformatoplanos.Items.Add("Plano Acumulacion N�" & conta_hoja_sup)
            colecciones_planos.Add("Plano Acumulacion N�" & conta_hoja_sup)

            pForm1.Refresh()

            cls_planos.Obtieneposicion_Plano_1(pForm1.lstformatoplanos)
            'pForm1.ShowDialog()
        End If
        Cls_evaluacion.cierra_ejecutable()
        pForm1.ShowDialog()
        v_calculAreaint = True


    End Sub


    Public Sub Obtieneposicion_Plano_2xx()
        'On Error GoTo EH
        Dim cls_planos As New Cls_planos
        cls_planos.Generaplanoreporte("Plano Area superpuesta")
        Dim area_cal As Double = 0
        posi_y_m = 17.4
        posi_y1_m = 16.8
        Dim t1 As Long
        'Cabecera del plano  fijo

        For t1 = 1 To 4
            pTextElement3 = New TextElement
            pEnv = New Envelope
            pPoint3 = New Point
            If t1 = 1 Then
                pTextElement3.Text = "CODIGO : " & v_codigo & "HOJA N�" & conta_hoja_sup
            ElseIf t1 = 2 Then
                pTextElement3.Text = "NOMBRE : " & v_nombre_dm
            ElseIf t1 = 3 Then
                pTextElement3.Text = "HECTAREAS : " & area_cal
            ElseIf t1 = 4 Then
                pTextElement3.Text = "AREA DISPONIBLE : " & "SS"
            End If
            pPoint3.X = 14.1
            pPoint3.Y = posi_y1_m - 0.6
            pPoint3.X = 18.7
            pPoint3.Y = posi_y_m
            pEnv.UpperRight = pPoint3
            pElement3 = pTextElement3
            pElement3.Geometry = pEnv

            posi_y1_m = posi_y1_m - 0.5
            posi_y_m = posi_y_m - 0.5
        Next t1

        'Segunda parte

        posi_y1 = posi_y1_m - 0.4
        posi_y = posi_y1_m - 0.4
    End Sub

    Public Sub Obtieneposicion_Plano_2()
        Try
            'On Error GoTo EH
            Dim cls_planos As New Cls_planos
            cls_planos.Generaplanoreporte("Plano Area superpuesta")
            Dim area_cal As Double = 0
            posi_y_m = 17.4
            posi_y1_m = 16.8
            Dim t1 As Long
            'Cabecera del plano  fijo

            For t1 = 1 To 5
                pTextElement3 = New TextElement
                pEnv = New Envelope
                pPoint3 = New Point
                If t1 = 1 Then
                    pTextElement3.Text = "#" & conta_hoja_sup
                ElseIf t1 = 2 Then
                    pTextElement3.Text = "CODIGO : " & v_codigo
                ElseIf t1 = 3 Then
                    pTextElement3.Text = "NOMBRE : " & v_nombre_dm
                ElseIf t1 = 4 Then
                    pTextElement3.Text = "HECTAREAS : " & area_cal
                ElseIf t1 = 5 Then
                    pTextElement3.Text = "AREA DISPONIBLE : " & "SS"
                End If
                'Posici�n dinamica de los textos
                If t1 = 1 Then  ' contador fijo de la hoja
                    pPoint3.X = 22.1
                    pPoint3.Y = 17.8
                    pPoint3.X = 26.7
                    pPoint3.Y = 18.4
                    pEnv.UpperRight = pPoint3
                    pElement3 = pTextElement3
                    pElement3.Geometry = pEnv
                    posi_y_m = posi_y_m + 0.6
                    posi_y1_m = posi_y1_m + 0.6
                Else
                    pPoint3.X = 14.1
                    pPoint3.Y = posi_y1_m - 0.6
                    pPoint3.X = 18.7
                    pPoint3.Y = posi_y_m
                    pEnv.UpperRight = pPoint3
                    pElement3 = pTextElement3
                    pElement3.Geometry = pEnv
                End If
                posi_y1_m = posi_y1_m - 0.3
                posi_y_m = posi_y_m - 0.3
            Next t1
            posi_y1 = posi_y1_m
            posi_y = posi_y1_m
        Catch ex As Exception
            MsgBox("Error al pasar a la siguiente hoja del calculo de Areas Disponibles...", MsgBoxStyle.Critical, "GIS")

        End Try
    End Sub

    Public Sub INSERTAAREASUPERPUESTA_PLANO()
        'PROGRAMA PARA INSERTAR TEXTOS AL LAYOUT SEGUN POSICI�N - PLANO AREA SUPERPUESTA
        Dim cls_planos As New Cls_planos
        'Dim pFeatureCursor As IFeatureCursor
        Dim pFeatLayer As IFeatureLayer = Nothing
        'Dim fclas_tema As IFeatureClass
        'Dim pPageLayout As IPageLayout
        Dim pActiveView As IActiveView
        pMap = pMxDoc.FocusMap
        Dim posi_y2_m As Double
        Dim posi_y2 As Double
        Dim pRow As IRow
        colecciones_AreaSup.Clear()

        Dim area_cal As Double = 0
        Dim area_dispon As Double = 0
        Dim pPoint1 As IPoint
        Dim myColor As IRgbColor
        Dim pFont As IFontDisp
        Dim afound As Boolean = False
        Dim v_contador_x As String
        If s_tipo_plano = "Plano_areasuperpuesta" Then
            For A As Integer = 0 To pMap.LayerCount - 1
                If pMap.Layer(A).Name = "Areainter_" & v_codigo Then
                    pFeatLayer = pMap.Layer(A)
                    afound = True
                    Exit For
                End If
            Next A
        ElseIf s_tipo_plano = "Plano DMxArea_Restringida" Then
            For A As Integer = 0 To pMap.LayerCount - 1
                If pMap.Layer(A).Name = "AreaRese_super" Then
                    pFeatLayer = pMap.Layer(A)
                    afound = True
                    Exit For
                End If
            Next A
        ElseIf s_tipo_plano = "Plano Acumulacion" Then

            For A As Integer = 0 To pMap.LayerCount - 1
                If pMap.Layer(A).Name = "Catastro" Then
                    pFeatLayer = pMap.Layer(A)
                    afound = True
                    Exit For
                End If
            Next A
        End If
        If Not afound Then
            'MsgBox("No se encuentra el tema de Area Superpuesta en la vista", MsgBoxStyle.Critical, "Observaci�n...")
            ' Exit Sub
        End If

        conta_hoja_s = conta_hoja_sup

        'Llamando a la tabla de coordenadas

        Dim pTable As ITable
        Dim pWorkspaceFactory1 As IWorkspaceFactory
        pWorkspaceFactory1 = New ShapefileWorkspaceFactory
        Dim pFWS As IFeatureWorkspace
        'pFWS = pWorkspaceFactory.OpenFromFile("D:\BDGEOCATMIN\Temporal", 0)
        pFWS = pWorkspaceFactory1.OpenFromFile(glo_pathTMP, 0)
        'pTable = pFWS.OpenTable("Areainter_" & v_codigo & "_t")
        pTable = pFWS.OpenTable("Areainter_" & v_codigo & "_" & fecha_archi_sup_t & "_t")


        Dim ptableCursor As ICursor
        Dim pfields3 As Fields
        pfields3 = pTable.Fields
        Dim pQueryFilter As IQueryFilter
        pfields3 = pTable.Fields
        pQueryFilter = New QueryFilter
        pQueryFilter.WhereClause = "HOJA = '" & conta_hoja_s & "'"
        ptableCursor = pTable.Search(pQueryFilter, True)
        pRow = ptableCursor.NextRow
        Dim v_codigo_t As String = ""
        Dim conta_contador As Long
        Dim v_contador_t As String = ""

        Do Until pRow Is Nothing
            v_codigo_x = pRow.Value(pfields3.FindField("CODIGO"))
            v_contador_x = pRow.Value(pfields3.FindField("CONTADOR"))
            'MsgBox(v_codigo_x, MsgBoxStyle.Critical, v_contador_x)
            'MsgBox(v_codigo_x & "---" & v_contador_x, MsgBoxStyle.Critical, v_codigo_t & "-*-" & v_contador_t)
            conta_contador = v_contador_x.Length
            If v_codigo_x <> v_codigo_t Or v_contador_x <> v_contador_t Then
                'If v_codigo_x <> v_codigo_t Then
                'colecciones_AreaSup.Add(v_codigo_x & v_contador_x & conta_contador)
                colecciones_AreaSup.Add(v_codigo_x & "_" & v_contador_x)
                v_codigo_t = v_codigo_x
                v_contador_t = v_contador_x

            End If

            pRow = ptableCursor.NextRow
        Loop
        'Define layout
        pPageLayout = pMxDoc.PageLayout
        pPageLayout.Page.Units = esriUnits.esriCentimeters
        pActiveView = pPageLayout
        pGraphicsContainer = pPageLayout
        posi_y_t = 17.4
        posi_y1_t = 16.8
        Dim t As Long

        'Cabecera del plano  fijo

        For t = 1 To 8          ' SE CAMBIA A 8, PARA AGREGAR DATUM DEL PLANO

            pTextElement3 = New TextElement
            pEnv = New Envelope
            pPoint3 = New Point

            If t = 1 Then
                'pTextElement3.Text = "AREA DISPONIBLE : " & area_dispon
                Dim MyDate As Date
                MyDate = Now
                Dim fecha_p As String
                fecha_p = RellenarComodin(MyDate.Year, 2, "0") & "/" & RellenarComodin(MyDate.Month, 2, "0") & "/" & MyDate.Day
                pTextElement3.Text = "FECHA : " & fecha_p
            ElseIf t = 2 Then
                'pTextElement3.Text = "AREA DISPONIBLE : " & area_dispon
                pTextElement3.Text = "ESCALA  1:" & escalaf

            ElseIf t = 3 Then
                pTextElement3.Text = "#" & conta_hoja_sup
            ElseIf t = 4 Then
                pTextElement3.Text = "CODIGO : " & v_codigo
            ElseIf t = 5 Then
                pTextElement3.Text = "NOMBRE : " & v_nombre_dm
            ElseIf t = 6 Then
                'pTextElement3.Text = "HECTAREAS : " & area_cal
                If s_tipo_plano = "Plano_areasuperpuesta" Then
                    pTextElement3.Text = "HECTAREAS (Inicial) : " & FormatNumber(v_area_eval, 4) & " Ha."
                Else
                    pTextElement3.Text = "NUMERO DE HECTAREAS : " & FormatNumber(v_area_eval, 4) & " Ha."
                End If

            ElseIf t = 7 Then
                'pTextElement3.Text = "AREA DISPONIBLE : " & area_dispon

                If s_tipo_plano = "Plano_areasuperpuesta" Then
                    pTextElement3.Text = "AREA DISPONIBLE : " & FormatNumber(v_area_dispo, 4) & " Ha."
                ElseIf s_tipo_plano = "Plano DMxArea_Restringida" Then
                    pTextElement3.Text = " "
                Else
                    pTextElement3.Text = " "

                End If

            ElseIf t = 8 Then
                pTextElement3.Text = "DATUM :  " & v_sistema

            End If
            'Posici�n dinamica de los textos

            If t = 1 Then  ' 
                pPoint3.X = 10.1
                pPoint3.Y = 18.4
                pPoint3.X = 11.5
                pPoint3.Y = 17.8
                pEnv.UpperRight = pPoint3
                pElement3 = pTextElement3
                pElement3.Geometry = pEnv

            ElseIf t = 2 Then  '
                pPoint3.X = 14.1
                pPoint3.Y = 18.4
                pPoint3.X = 15.1
                pPoint3.Y = 17.8
                pEnv.UpperRight = pPoint3
                pElement3 = pTextElement3
                pElement3.Geometry = pEnv


            ElseIf t = 3 Then  ' contador fijo de la hoja
                pPoint3.X = 22.1
                pPoint3.Y = 17.8
                pPoint3.X = 26.7
                pPoint3.Y = 18.4
                pEnv.UpperRight = pPoint3
                pElement3 = pTextElement3
                pElement3.Geometry = pEnv
                posi_y_t = posi_y_t + 0.6
                posi_y1_t = posi_y1_t + 0.6

            ElseIf t = 8 Then

                pPoint3.X = 5.3
                pPoint3.Y = 2.1   '2.23
                pPoint3.X = 9.0
                pPoint3.Y = 1.3    '1.43
                pEnv.UpperRight = pPoint3
                pElement3 = pTextElement3
                pElement3.Geometry = pEnv
                'posi_y_t = posi_y_t + 0.6
                'posi_y1_t = posi_y1_t + 0.6

            Else  ' datos generales del dm
                pPoint3.X = 14.1
                pPoint3.Y = posi_y1_t - 0.6
                pPoint3.X = 18.7
                pPoint3.Y = posi_y_t
                pEnv.UpperRight = pPoint3
                pElement3 = pTextElement3
                pElement3.Geometry = pEnv
            End If

            'Simbolo del texto

            pTxtSym3 = New TextSymbol
            'fuente del texto
            pFont = New StdFont
            pFont.Name = "Tahoma"
            pFont.Size = 8
            pFont.Bold = False
            pTxtSym3.Font = pFont
            myColor = New RgbColor
            If t = 1 Or t = 2 Or t = 8 Then
                myColor.RGB = RGB(0, 0, 255)
            Else
                myColor.RGB = RGB(197, 0, 255)
            End If

            pTxtSym3.Color = myColor

            'Propiedades del Simbolo

            pTxtSym3.Angle = 0
            pTxtSym3.RightToLeft = False
            pTxtSym3.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
            pTxtSym3.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
            If t = 1 Or t = 2 Then
                pTxtSym3.CharacterSpacing = 16
            Else
                pTxtSym3.CharacterSpacing = 20
            End If

            pTxtSym3.Case = esriTextCase.esriTCNormal
            pTextElement3.Symbol = pTxtSym3
            'If corta_nplano = conta_hoja_s Then
            ' Si es la hoja a�adir
            pGraphicsContainer.AddElement(pTextElement3, 1)
            'End If

            'Refrescar solo los graficos del PageLayout
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            If t >= 3 Then
                posi_y1_t = posi_y1_t - 0.4
                posi_y_t = posi_y_t - 0.4
            End If
        Next t

        'posi_y = 15.2
        'posi_y1 = 14.6
        posi_y = 15.8
        posi_y1 = 15.2

        'Obteniendo datos de las areas superpuestas
        'Dim valor As Long
        Dim valor1 As Long
        Dim v_este As Double
        Dim v_norte As Double
        Dim v_vertice As Integer
        Dim v_areaf1 As Double
        Dim v_contador As String
        'valor = pFeatLayer.FeatureClass.FeatureCount(Nothing)
        valor1 = 0

        Dim v_codigo_s As String = ""
        Dim k As Integer
        Dim v_codigo_s1 As String = ""
        Dim v_codigo_s2 As String = ""
        Dim v_canti_tabla As Long
        'valor_posi = 0
        For k = 1 To colecciones_AreaSup.Count
            'conta_reg = conta_reg + 1
            v_codigo_s = colecciones_AreaSup.Item(k)
            v_contador_x = Mid(v_codigo_s, InStr(v_codigo_s, "_") + 1)
            'Microsoft.VisualBasic.Right(v_codigo_s, 1)
            v_codigo_s = Mid(v_codigo_s, 1, InStr(v_codigo_s, "_") - 1)
            'Microsoft.VisualBasic.Left(v_codigo_s, v_codigo_s.Length - 1)
            'comienza tu segundo bucle, segun la hoja en la tabla
            pfields3 = pTable.Fields
            pQueryFilter = New QueryFilter
            'pQueryFilter.WhereClause = "HOJA = '" & conta_hoja_s & "' AND CODIGO =  '" & v_codigo_s & "'"
            pQueryFilter.WhereClause = "HOJA = '" & conta_hoja_s & "' AND CODIGO =  '" & v_codigo_s & "' AND CONTADOR =  '" & v_contador_x & "'"
            'MsgBox(pQueryFilter.WhereClause)
            ptableCursor = pTable.Search(pQueryFilter, True)
            v_canti_tabla = pTable.RowCount(pQueryFilter)
            pRow = ptableCursor.NextRow
            Dim k1 As Integer
            v_codigo_s2 = ""
            'posi_y1 = posi_y1 - 0.4
            'posi_y = posi_y - 0.4
            valor1 = 0
            'If ((posi_y > 1.6) Or (posi_y1 > 1.6)) Then

            Do Until pRow Is Nothing
                valor1 = valor1 + 1
                v_codigo_x = pRow.Value(pfields3.FindField("CODIGO"))
                nombre_dm_x = pRow.Value(pfields3.FindField("NOMBRE"))
                'areaf = pRow.Value(pfields3.FindField("HOJA"))
                v_vertice = pRow.Value(pfields3.FindField("VERTICE"))
                v_este = pRow.Value(pfields3.FindField("ESTE"))
                v_norte = pRow.Value(pfields3.FindField("NORTE"))
                v_areaf1 = pRow.Value(pfields3.FindField("AREA"))
                v_contador = pRow.Value(pfields3.FindField("CONTADOR"))
                posi_y1 = posi_y1 - 0.3
                posi_y = posi_y - 0.3
                If v_codigo_x <> v_codigo_s2 Then                    '
                    posi_y2_m = posi_y1
                    posi_y2 = posi_y
                    For k1 = 1 To 5
                        pTextElement1 = New TextElement
                        pEnv = New Envelope
                        pPoint1 = New Point
                        If k1 = 1 Then
                            If s_tipo_plano = "Plano DMxArea_Restringida" Then
                                pTextElement1.Text = " "
                            Else
                                pTextElement1.Text = "CODIGO :" & v_codigo_s
                            End If

                        ElseIf k1 = 2 Then
                            If s_tipo_plano = "Plano DMxArea_Restringida" Then
                                pTextElement1.Text = " "
                            Else
                                pTextElement1.Text = "NOMBRE :" & nombre_dm_x
                            End If

                        ElseIf k1 = 3 Then
                                If s_tipo_plano = "Plano_areasuperpuesta" Then
                                    pTextElement1.Text = "COORDENADAS DEL AREA SUPERPUESTA"
                                ElseIf s_tipo_plano = "Plano DMxArea_Restringida" Then
                                pTextElement1.Text = "COORDENADAS UTM DEL AREA SUPERPUEST A" & vbNewLine & v_nmrese_sup

                                Else
                                    pTextElement1.Text = "COORDENADAS DE LA ACUMULACION"
                                End If

                        ElseIf k1 = 4 Then
                                pTextElement1.Text = "    VERT.          NORTE            ESTE"
                        ElseIf k1 = 5 Then
                                pTextElement1.Text = "   ---------------------------------------------"
                        End If
                        If ((k1 = 1) Or (k1 = 2)) Then
                            pPoint1.X = 15.6
                            pPoint1.Y = posi_y2_m - 0.6
                            pPoint1.X = 20.2
                            pPoint1.Y = posi_y2
                        ElseIf ((k1 = 3) Or (k1 = 4)) Then
                            pPoint1.X = 15.4
                            pPoint1.Y = posi_y2_m - 0.6
                            pPoint1.X = 19.3
                            pPoint1.Y = posi_y2
                        ElseIf (k1 = 5) Then
                            pPoint1.X = 15.4
                            pPoint1.Y = posi_y2_m - 0.6
                            pPoint1.X = 19.3
                            pPoint1.Y = posi_y2
                            'posi_y1 = posi_y1 - 1.2
                            'posi_y = posi_y - 1.2
                        End If
                        pEnv.UpperRight = pPoint1
                        pElement1 = pTextElement1
                        pElement1.Geometry = pEnv
                        'Simbolo del texto
                        pTxtSym1 = New TextSymbol
                        pFont = New StdFont
                        pFont.Name = "Tahoma"
                        pFont.Size = 8
                        pFont.Bold = False
                        pTxtSym1.Font = pFont
                        myColor = New RgbColor

                        If ((k1 = 1) Or (k1 = 2)) Then
                            myColor.RGB = RGB(0, 0, 255)
                        ElseIf k1 = 3 Then
                            myColor.RGB = RGB(255, 0, 0)
                        ElseIf k1 = 4 Then
                            myColor.RGB = RGB(0, 0, 0)
                        ElseIf k1 = 5 Then
                            myColor.RGB = RGB(0, 0, 0)
                        End If
                        pTxtSym1.Color = myColor
                        'Propiedades del Simbolo

                        pTxtSym1.Angle = 0
                        pTxtSym1.RightToLeft = False
                        pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                        pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                        pTxtSym1.CharacterSpacing = 20
                        pTxtSym1.Case = esriTextCase.esriTCNormal
                        pTextElement1.Symbol = pTxtSym1
                        pGraphicsContainer.AddElement(pTextElement1, 1)
                        'Refrescar solo los graficos del PageLayout
                        pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
                        'posi_y2_m = posi_y2_m - 0.4
                        'posi_y2 = posi_y2 - 0.4
                        posi_y2_m = posi_y2_m - 0.32
                        posi_y2 = posi_y2 - 0.32
                    Next k1
                    v_codigo_s2 = v_codigo_x
                    posi_y1 = posi_y2_m
                    posi_y = posi_y2
                End If
                'Else

                'Posici�n del texto antes de empezar a roular coordenadas
                'posi_y = 15.8
                'posi_y1 = 15.2
                'posi_y1 = posi_y1 - 0.3
                'posi_y = posi_y - 0.3

                pTextElement2 = New TextElement
                pEnv = New Envelope
                pPoint2 = New Point

                If v_vertice > 0 Then
                    pTextElement2.Text = "     " & v_vertice & "           " & FormatNumber(v_norte, 2) & "       " & FormatNumber(v_este, 2)  ' Texto de salida
                ElseIf v_vertice = 0 Then
                    If nombre_dm_x = "Constantexxxyyy" Then
                        pTextElement2.Text = "---------------------------------------------"
                    ElseIf nombre_dm_x = "Areaxxxyyy" Then
                        pTextElement2.Text = "     AREA UTM :" & FormatNumber(v_areaf1, 4) & " Ha."
                    End If
                End If
                pPoint2.X = 15.4
                pPoint2.Y = posi_y1 - 0.6
                pPoint2.X = 19.3
                pPoint2.Y = posi_y
                pEnv.UpperRight = pPoint2
                pElement2 = pTextElement2
                pElement2.Geometry = pEnv

                'Simbolo del texto

                pTxtSym2 = New TextSymbol
                pFont = New StdFont
                pFont.Name = "Tahoma"
                pFont.Size = 8
                pFont.Bold = False
                pTxtSym2.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym2.Color = myColor

                'Propiedades del Simbolo

                pTxtSym2.Angle = 0
                pTxtSym2.RightToLeft = False
                pTxtSym2.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym2.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym2.CharacterSpacing = 20
                pTxtSym2.Case = esriTextCase.esriTCNormal
                pTextElement2.Symbol = pTxtSym2
                'If corta_nplano = conta_hoja_s Then
                pGraphicsContainer.AddElement(pTextElement2, 1)

                posi_y1 = posi_y1 - 0.18
                posi_y = posi_y - 0.18
                pRow = ptableCursor.NextRow
            Loop
            'Else
            'posi_y = 15.8
            'posi_y1 = 15.2
            'posi_y1 = posi_y1 - 0.3
            'posi_y = posi_y - 0.3
            'End If

        Next k
    End Sub

    Public Sub Insertatexto_planovarios()
        'On Error GoTo EH
        'PROGRAMA PARA INSERTAR TEXTOS CUANDO SE PASA A OTRA HOJA SEGUN VARIABLE

        Dim cls_planos As New Cls_planos
        cls_planos.Generaplanoreporte("Plano Area superpuesta")
        Dim area_cal As Double = 0
        Dim area_dispon As Double = 0
        Dim pActiveView As IActiveView
        pActiveView = pPageLayout

        'Dim pTxtSym As IFormattedTextSymbol
        'Dim contatexto As Integer
        Dim myColor As IRgbColor
        'Dim pFont1 As IFontDisp
        Dim pFont As IFontDisp
        'Dim pActiveView As IActiveView

        'Genera otra vez rotulado en la otra hoja
        '*******************************************

        posi_y_m = 17.4
        posi_y1_m = 16.8
        Dim t1 As Long
        'Cabecera del plano  fijo

        For t1 = 1 To 5
            pTextElement3 = New TextElement
            pEnv = New Envelope
            pPoint3 = New Point
            If t1 = 1 Then
                pTextElement3.Text = "#" & conta_hoja_sup
            ElseIf t1 = 2 Then
                pTextElement3.Text = "CODIGO : " & v_codigo
            ElseIf t1 = 3 Then
                pTextElement3.Text = "NOMBRE : " & v_nombre_dm
            ElseIf t1 = 4 Then
                pTextElement3.Text = "HECTAREAS : " & FormatNumber(area_cal, 4)
            ElseIf t1 = 5 Then
                pTextElement3.Text = "AREA DISPONIBLE : " & FormatNumber(area_dispon, 4)
            End If

            'Posici�n dinamica de los textos
            If t1 = 1 Then  ' contador fijo de la hoja
                pPoint3.X = 22.1
                pPoint3.Y = 17.8
                pPoint3.X = 26.7
                pPoint3.Y = 18.4
                pEnv.UpperRight = pPoint3
                pElement3 = pTextElement3
                pElement3.Geometry = pEnv
                posi_y_m = posi_y_m + 0.6
                posi_y1_m = posi_y1_m + 0.6
            Else
                pPoint3.X = 14.1
                pPoint3.Y = posi_y1_m - 0.6
                pPoint3.X = 18.7
                pPoint3.Y = posi_y_m
                pEnv.UpperRight = pPoint3
                pElement3 = pTextElement3
                pElement3.Geometry = pEnv
            End If

            'Simbolo del texto

            pTxtSym3 = New TextSymbol
            pFont = New StdFont
            pFont.Name = "Tahoma"
            pFont.Size = 8
            pFont.Bold = False
            pTxtSym3.Font = pFont
            myColor = New RgbColor
            myColor.RGB = RGB(197, 0, 255)
            pTxtSym3.Color = myColor

            'Propiedades del Simbolo

            pTxtSym3.Angle = 0
            pTxtSym3.RightToLeft = False
            pTxtSym3.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
            pTxtSym3.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
            pTxtSym3.CharacterSpacing = 20
            pTxtSym3.Case = esriTextCase.esriTCNormal
            pTextElement3.Symbol = pTxtSym3
            If corta_nplano = conta_hoja_s Then
                pGraphicsContainer.AddElement(pTextElement3, 1)
            End If

            'Refrescar solo los graficos del PageLayout
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            posi_y1_m = posi_y1_m - 0.5
            posi_y_m = posi_y_m - 0.5

        Next t1
        'Posici�n final del texto para seguir continuando con el bucle de coordenadas
        posi_y1 = posi_y1_m + 0.2
        posi_y = posi_y1_m + 0.2

    End Sub
    Public Sub Obtieneposicion_Plano_1_1(ByVal lstformatoplanos As Windows.Forms.ListBox)

        'Programa para obtener posici�n del texto en el layout

        'Dim formulario As New Frm_formatoplanos

        Dim cls_planos As New Cls_planos
        Dim pFeatureCursor As IFeatureCursor
        Dim pFeatLayer As IFeatureLayer = Nothing
        Dim fclas_tema As IFeatureClass
        Dim este1 As Double
        Dim norte1 As Double

        'Dim coordenada_DM(300) As Punto_DM
        Dim pActiveView As IActiveView
        Dim posi_y2_m As Double
        Dim posi_y2 As Double
        Dim area_cal As Double = 0
        Dim area_dispon As Double = 0
        Dim campo1 As String
        Dim campo2 As String
        Dim campo3 As String
        'Dim areaf As Double
        Dim pPoint1 As IPoint
        pMap = pMxDoc.FocusMap
        Dim areaf1 As Double
        Dim pt As IPoint
        Dim l As IPolygon
        Dim pFont As IFontDisp
        Dim afound As Boolean = False
        Dim h, j As Integer
        For A As Integer = 0 To pMap.LayerCount - 1
            If pMap.Layer(A).Name = "Areainter_" & v_codigo Then
                pFeatLayer = pMap.Layer(A)
                afound = True
                Exit For
            End If
        Next A
        If Not afound Then
            'MsgBox("No se encuentra el Layer")
            Exit Sub
        End If

        pPageLayout = pMxDoc.PageLayout
        pPageLayout.Page.Units = esriUnits.esriCentimeters
        pActiveView = pPageLayout
        pGraphicsContainer = pPageLayout

        'Agregando la tabla

        Dim pTable As ITable
        Dim pWorkspaceFactory1 As IWorkspaceFactory
        pWorkspaceFactory1 = New ShapefileWorkspaceFactory
        Dim pFWS As IFeatureWorkspace
        pFWS = pWorkspaceFactory1.OpenFromFile(glo_pathTMP, 0)
        pTable = pFWS.OpenTable("Areainter_" & v_codigo & "_t")
        Dim ptableCursor As ICursor
        Dim pfields3 As Fields
        pfields3 = pTable.Fields
        ptableCursor = pTable.Search(Nothing, False)
        Dim pRow As IRow

        '*******Acabo de agregar tabla

        posi_y_t = 17.4
        posi_y1_t = 16.8
        Dim t As Long
        'Cabecera del plano  fijo

        For t = 1 To 5
            pTextElement3 = New TextElement
            pEnv = New Envelope
            pPoint3 = New Point
            If t = 1 Then
                pTextElement3.Text = "#" & conta_hoja_sup
            ElseIf t = 2 Then
                pTextElement3.Text = "CODIGO : " & v_codigo & ""
            ElseIf t = 3 Then
                pTextElement3.Text = "NOMBRE : " & v_nombre_dm
            ElseIf t = 4 Then
                pTextElement3.Text = "HECTAREAS : " & "A"
            ElseIf t = 5 Then
                pTextElement3.Text = "AREA DISPONIBLE : " & area_dispon
            End If

            'Posici�n dinamica de los textos

            If t = 1 Then  ' contador fijo de la hoja
                pPoint3.X = 22.1
                pPoint3.Y = 17.8
                pPoint3.X = 26.7
                pPoint3.Y = 18.4
                pEnv.UpperRight = pPoint3
                pElement3 = pTextElement3
                pElement3.Geometry = pEnv
                posi_y_t = posi_y_t + 0.6
                posi_y1_t = posi_y1_t + 0.6

            Else  ' datos generales del dm

                pPoint3.X = 14.1
                pPoint3.Y = posi_y1_t - 0.6
                pPoint3.X = 18.7
                pPoint3.Y = posi_y_t
                pEnv.UpperRight = pPoint3
                pElement3 = pTextElement3
                pElement3.Geometry = pEnv
            End If

            'Simbolo del texto

            pTxtSym3 = New TextSymbol
            'fuente del texto

            pFont = New StdFont
            pFont.Name = "Tahoma"
            pFont.Size = 8
            pFont.Bold = False
            pTxtSym3.Font = pFont
            'myColor = New RgbColor
            'myColor.RGB = RGB(197, 0, 255)

            'pTxtSym3.Color = myColor

            'Propiedades del Simbolo

            'pTxtSym3.Angle = 0
            'pTxtSym3.RightToLeft = False
            'pTxtSym3.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
            'pTxtSym3.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
            'pTxtSym3.CharacterSpacing = 20
            'pTxtSym3.Case = esriTextCase.esriTCNormal
            'pTextElement3.Symbol = pTxtSym3
            'If corta_nplano = conta_hoja_s Then
            ' Si es la hoja a�adir
            '   pGraphicsContainer.AddElement(pTextElement3, 1)
            'End If

            'Refrescar solo los graficos del PageLayout

            'pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

            posi_y1_t = posi_y1_t - 0.5
            posi_y_t = posi_y_t - 0.5
        Next t

        'Buscando los campos del tema
        fclas_tema = pFeatLayer.FeatureClass
        pFields = fclas_tema.Fields
        campo1 = pFields.FindField("CODIGOU_1")
        pFields = fclas_tema.Fields
        campo2 = pFields.FindField("CONCESIO_1")
        campo3 = pFields.FindField("AREA_FINAL")
        pFeatureCursor = pFeatLayer.Search(Nothing, False)
        fclas_tema = pFeatLayer.FeatureClass

        'posi_y = 15.2
        'posi_y1 = 14.6
        posi_y = 15.8
        posi_y1 = 15.2
        Dim valor As Long
        Dim valor1 As Long
        valor = pFeatLayer.FeatureClass.FeatureCount(Nothing)
        valor1 = 0
        pFeature = pFeatureCursor.NextFeature

        Dim conta_t As Long
        Dim coordenada_DM(300) As Punto_DM
        Do Until pFeature Is Nothing
            'Dim coordenada_DM(300) As Punto_DM
            conta_t = conta_t + 1
            valor1 = valor1 + 1
            l = pFeature.Shape
            ptcol = l
            ReDim coordenada_DM(ptcol.PointCount)
            For i As Integer = 0 To ptcol.PointCount - 1
                coordenada_DM(i).x = 0
                coordenada_DM(i).y = 0
            Next

            v_codigo_x = pFeature.Value(campo1)
            nombre_dm_x = pFeature.Value(campo2)
            areaf1 = pFeature.Value(campo3)
            Dim k As Long
            For k = 1 To 5
                pTextElement1 = New TextElement
                pEnv = New Envelope
                pPoint1 = New Point
                If conta_t = 1 Then  ' Si es posici�n inicial  - (1)
                    If k = 1 Then
                        pTextElement1.Text = "CODIGO :" & v_codigo_x
                    ElseIf k = 2 Then
                        pTextElement1.Text = "NOMBRE :" & nombre_dm_x
                    ElseIf k = 3 Then
                        pTextElement1.Text = "COORDENADAS DEL AREA SUPERPUESTA"
                    ElseIf k = 4 Then
                        pTextElement1.Text = "    VERT.          NORTE            ESTE"
                    ElseIf k = 5 Then
                        pTextElement1.Text = "   ---------------------------------------------"
                    End If
                Else 'Si es posici�n 2 a mas
                    If k = 1 Then
                        pTextElement1.Text = "CODIGO :" & v_codigo_x
                    ElseIf k = 2 Then
                        pTextElement1.Text = "NOMBRE : " & nombre_dm_x
                    ElseIf k = 3 Then
                        pTextElement1.Text = "COORDENADAS DEL AREA SUPERPUESTA"
                    ElseIf k = 4 Then
                        pTextElement1.Text = "    VERT.            NORTE              ESTE"
                    ElseIf k = 5 Then
                        pTextElement1.Text = "   ---------------------------------------------"
                    End If
                End If

                If conta_t = 1 Then  ' Si es posici�n inicial  - (1)
                    If ((k = 1) Or (k = 2)) Then
                        pPoint1.X = 15.6
                        pPoint1.Y = posi_y1 - 0.6
                        pPoint1.X = 20.2
                        pPoint1.Y = posi_y
                    ElseIf ((k = 3) Or (k = 4)) Then
                        pPoint1.X = 15.4
                        pPoint1.Y = posi_y1 - 0.6
                        pPoint1.X = 19.3
                        pPoint1.Y = posi_y
                    ElseIf (k = 5) Then
                        pPoint1.X = 15.4
                        pPoint1.Y = posi_y1 - 0.6
                        pPoint1.X = 19.3
                        pPoint1.Y = posi_y
                        posi_y1 = posi_y1 - 1.2
                        posi_y = posi_y - 1.2
                    End If

                Else

                    If ((k = 1) Or (k = 2)) Then
                        pPoint1.X = 15.6
                        pPoint1.Y = posi_y1 - 0.6
                        pPoint1.X = 20.2
                        pPoint1.Y = posi_y
                    ElseIf ((k = 3) Or (k = 4)) Then
                        pPoint1.X = 15.4
                        pPoint1.Y = posi_y1 - 0.6
                        pPoint1.X = 19.3
                        pPoint1.Y = posi_y
                    ElseIf (k = 5) Then
                        pPoint1.X = 15.4
                        pPoint1.Y = posi_y1 - 0.6
                        pPoint1.X = 19.3
                        pPoint1.Y = posi_y
                        'posi_y1 = posi_y1 - 1.2

                        'posi_y = posi_y - 1.2

                    End If
                End If
                pEnv.UpperRight = pPoint1
                pElement1 = pTextElement1
                pElement1.Geometry = pEnv
                'Simbolo del texto

                pTxtSym1 = New TextSymbol
                pFont = New StdFont
                pFont.Name = "Tahoma"
                pFont.Size = 8

                posi_y1 = posi_y1 - 0.4
                posi_y = posi_y - 0.4

                '*****************************

                If ((posi_y < 1.6) Or (posi_y1 < 1.6)) Then  'Para saber si pasa de la hoja
                    conta_hoja_sup = conta_hoja_sup + 1
                    If conta_hoja_sup > 50 Then
                        MsgBox("Este caso tiene para generar mas de 50 planos, comunicar a OSI", MsgBoxStyle.Critical, "BDGEOCATMIN")

                        Exit Sub
                    Else
                        lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                        colecciones_planos.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                        cls_planos.Obtieneposicion_Plano_2()
                    End If
                End If

            Next k

            'Posici�n del texto antes de empezar a roular coordenadas
            If conta_t = 1 Then  ' Para 1er punto
                posi_y1 = posi_y1 + 1.5
                posi_y = posi_y + 1.5
            Else  'Para n puntos
                posi_y1 = posi_y1
                posi_y = posi_y
            End If
            posi_y1 = posi_y1

            posi_y = posi_y

            'comienza tu segundo bucle, el que recorre los puntos de cada polilinea

            conta_vert = 0
            j_vert = 0

            For j = 0 To ptcol.PointCount - 2
                conta_vert = conta_vert + 1
                'MsgBox(conta_vert)
                pt = ptcol.Point(j)
                If conta_vert = 1 Then  ' Para 1er punto
                    posi_y1 = posi_y1 - 0.2
                    'posi_y1 = posi_y1 - 0.18
                    posi_y = posi_y - 0.2
                Else  'Para n puntos
                    posi_y1 = posi_y1
                    posi_y = posi_y
                End If
                este1 = pt.X
                norte1 = pt.Y

                'Redonde de coordenadas
                norte1 = Format(Math.Round(norte1, 3), "###,###.00")
                este1 = Format(Math.Round(este1, 3), "###,###.00")
                'este1 = FormatNumber(este1, 2)
                'norte1 = FormatNumber(norte1, 2)

                coordenada_DM(j).v = j + 1
                coordenada_DM(j).x = este1
                coordenada_DM(j).y = norte1

                pRow = pTable.CreateRow
                pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                pRow.Value(pfields3.FindField("NOMBRE")) = nombre_dm_x
                pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                pRow.Value(pfields3.FindField("VERTICE")) = j + 1
                pRow.Value(pfields3.FindField("ESTE")) = FormatNumber(este1, 2)
                pRow.Value(pfields3.FindField("NORTE")) = FormatNumber(norte1, 2)
                pRow.Value(pfields3.FindField("AREA")) = areaf1
                pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                pRow.Store()
                pTextElement2 = New TextElement
                pEnv = New Envelope
                pPoint2 = New Point
                'pTextElement2.Text = "     " & j + 1 & "           " & FormatNumber(norte1, 2) & "       " & FormatNumber(este1, 2)

                pTextElement2.Text = "     " & j + 1 & "           " & norte1 & "       " & este1
                pPoint2.X = 15.4
                pPoint2.Y = posi_y1 - 0.6
                pPoint2.X = 19.3
                pPoint2.Y = posi_y
                pEnv.UpperRight = pPoint2
                pElement2 = pTextElement2
                pElement2.Geometry = pEnv
                'posi_y1 = posi_y1 - 0.4
                'posi_y = posi_y - 0.4
                posi_y1 = posi_y1 - 0.35
                posi_y = posi_y - 0.35
                If ((posi_y < 1.6) Or (posi_y1 < 1.6)) Then
                    conta_hoja_sup = conta_hoja_sup + 1
                    If conta_hoja_sup > 50 Then
                        'formulario.Show()
                        Exit Sub
                    Else
                        j_vert = j + 1
                        lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                        colecciones_planos.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                        cls_planos.Obtieneposicion_Plano_2()
                    End If
                End If
            Next j ' ----- fin de bucle interno

            'Calcular Area seg�n coordenadas redondeadas

            coordenada_DM(j).x = coordenada_DM(0).x
            coordenada_DM(j).y = coordenada_DM(0).y
            Dim d0, d1, dr As Double
            d0 = 0 : d1 = 0 : dr = 0
            For h = 0 To j - 1  ' UBound(coordenada_DM) - 1
                If coordenada_DM(h).x <> 0 Or coordenada_DM(h).y <> 0 Then
                    d0 = d0 + coordenada_DM(h).x * coordenada_DM(h + 1).y
                    d1 = d1 + coordenada_DM(h).y * coordenada_DM(h + 1).x
                Else
                    Exit For
                End If
            Next h
            dr = Math.Round(Math.Abs((d0 - d1) / 2) / 10000, 4)  'termino
            'dr = Math.Abs((d0 - d1) / 2) / 10000  'termino
            areaf1 = Format(Math.Round(dr, 4), "###,###.0000")
            'MsgBox(areaf1, MsgBoxStyle.Exclamation, "1")
            'coordenada_DM(h).x.ce()

            'areaf1 = FormatNumber(areaf1, 4)
            posi_y1 = posi_y1 - 0.35
            posi_y = posi_y - 0.35
            'texto de Area Disponible

            posi_y2_m = posi_y1
            posi_y2 = posi_y
            Dim t1 As Long
            'Cabecera del plano  fijo
            For t1 = 1 To 3
                pTextElement2 = New TextElement
                pEnv = New Envelope
                pPoint2 = New Point
                If t1 = 1 Then
                    pTextElement2.Text = "---------------------------------------------"
                    pRow = pTable.CreateRow
                    pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                    pRow.Value(pfields3.FindField("NOMBRE")) = "Constantexxxyyy"
                    pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                    pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                    pRow.Store()
                ElseIf t1 = 2 Then
                    pTextElement2.Text = "AREA UTM :" & areaf & " Ha."
                    pRow = pTable.CreateRow
                    pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                    pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                    pRow.Value(pfields3.FindField("NOMBRE")) = "Areaxxxyyy"
                    pRow.Value(pfields3.FindField("AREA")) = areaf1
                    pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                    pRow.Store()

                ElseIf t1 = 3 Then
                    pTextElement2.Text = "---------------------------------------------"
                    pRow = pTable.CreateRow
                    pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                    pRow.Value(pfields3.FindField("NOMBRE")) = "Constantexxxyyy"
                    pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                    pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                    pRow.Store()
                End If

                'Posici�n dinamica de los textos

                pPoint2.X = 15.4
                pPoint2.Y = posi_y2_m - 0.6
                pPoint2.X = 19.3
                pPoint2.Y = posi_y2
                pEnv.UpperRight = pPoint2
                pElement2 = pTextElement2
                pElement2.Geometry = pEnv
                posi_y2_m = posi_y2_m - 0.5
                posi_y2 = posi_y2 - 0.5

            Next t1

            'Posici�n final del texto para seguir continuando con el bucle de coordenadas
            posi_y2_m = posi_y2_m + 0.2
            posi_y2 = posi_y2 + 0.2
            posi_y1 = posi_y2_m
            posi_y = posi_y2
            pFeature = pFeatureCursor.NextFeature

        Loop
        lstformatoplanos.Items.Add("Plano Area superpuesta N�1")
        lstformatoplanos.Sorted = True



    End Sub

    Public Sub Obtieneposicion_Plano_1(ByVal lstformatoplanos As Windows.Forms.ListBox)
        Try
            'Programa para obtener posici�n del texto en el layout


            Dim cuenta_valores As Integer
            Dim cuenta_valores1 As Integer = 0


            Dim cls_planos As New Cls_planos
            Dim pFeatureCursor As IFeatureCursor
            Dim pFeatLayer As IFeatureLayer = Nothing
            Dim fclas_tema As IFeatureClass
            Dim este1 As Double
            Dim norte1 As Double

            'Dim coordenada_DM(300) As Punto_DM
            Dim pActiveView As IActiveView
            Dim posi_y2_m As Double
            Dim posi_y2 As Double
            Dim area_cal As Double = 0
            Dim area_dispon As Double = 0
            Dim campo1 As String
            Dim campo2 As String
            Dim campo3 As String
            'Dim areaf As Double
            Dim pPoint1 As IPoint
            pMap = pMxDoc.FocusMap
            Dim areaf1 As Double
            Dim pt As IPoint
            Dim l As IPolygon
            Dim pFont As IFontDisp
            Dim afound As Boolean = False
            Dim h, j As Integer
            For A As Integer = 0 To pMap.LayerCount - 1
                If s_tipo_plano = "Plano Acumulacion" Then  'Para plano Acumulacion
                    If pMap.Layer(A).Name = "Catastro" Then
                        pFeatLayer = pMap.Layer(A)
                        afound = True
                        Exit For

                    End If
                ElseIf s_tipo_plano = "Plano DMxArea_Restringida" Then
                    If pMap.Layer(A).Name = "AreaRese_super" Then
                        pFeatLayer = pMap.Layer(A)
                        afound = True
                        Exit For

                    End If
                Else  'Para plano Areas Superpuestas
                    If pMap.Layer(A).Name = "Areainter_" & v_codigo Then
                        pFeatLayer = pMap.Layer(A)
                        afound = True
                        Exit For
                    End If

                End If
            Next A
            If Not afound Then
                'MsgBox("No se encuentra el Layer")
                Exit Sub
            End If


            '        Dim cls_Catastro As New cls_DM_1
            'If s_tipo_plano = "Plano Acumulacion" Then
            '    fecha_archi_sup_t = DateTime.Now.Ticks.ToString
            '    cls_Catastro.creandotabla_Areasup()
            'End If

            pPageLayout = pMxDoc.PageLayout
            pPageLayout.Page.Units = esriUnits.esriCentimeters
            pActiveView = pPageLayout
            pGraphicsContainer = pPageLayout


            ''Creando el xml para areas superpuestas

            'Dim dt As New DataTable
            'dt = dgdDetalle.DataSource
            'Dim glodstControl1 As New DataSet
            'glodstControl1.Tables.Add(dt.Copy)

            'glodstControl1.WriteXml(glo_Path & "\arestringida.xml")


            'Agregando la tabla que se genero previamente

            Dim pTable_sup As ITable
            Dim pWorkspaceFactory1 As IWorkspaceFactory
            pWorkspaceFactory1 = New ShapefileWorkspaceFactory
            Dim pFWS As IFeatureWorkspace
            '& "\
            'pFWS = pWorkspaceFactory1.OpenFromFile(glo_pathTMP & "\", 0)
            pFWS = pWorkspaceFactory1.OpenFromFile(glo_pathTMP, 0)
            'pTable = pFWS.OpenTable("Areainter_" & v_codigo & "_t")
            pTable_sup = pFWS.OpenTable("Areainter_" & v_codigo & "_" & fecha_archi_sup_t & "_t")

            Dim ptableCursor As ICursor
            Dim pfields3 As Fields
            pfields3 = pTable_sup.Fields
            ' ptableCursor = pTable_sup.Search(Nothing, False)
            Dim pRow As IRow
            'posi_y_t = 17.4
            'posi_y1_t = 16.8
            Dim t As Long

            'Buscando los campos del tema
            fclas_tema = pFeatLayer.FeatureClass
            pFields = fclas_tema.Fields

            'buscando los campos de la capa segun el tipo de supersicion
            'tipo dm superpuesto a areas restringias

            If s_tipo_plano = "Plano DMxArea_Restringida" Then


                campo1 = pFields.FindField("CODIGOU")
                ' pFields = fclas_tema.Fields
                campo2 = pFields.FindField("CONCESION")
                campo3 = pFields.FindField("HECTAGIS")
                pFeatureCursor = pFeatLayer.Search(Nothing, False)
                fclas_tema = pFeatLayer.FeatureClass

            Else  'PARA DM PRIORITARIOS U OTROS
                campo1 = pFields.FindField("CODIGOU_1")
                ' pFields = fclas_tema.Fields
                campo2 = pFields.FindField("CONCESIO_1")
                campo3 = pFields.FindField("AREA_FINAL")
                pFeatureCursor = pFeatLayer.Search(Nothing, False)
                fclas_tema = pFeatLayer.FeatureClass
            End If

            'posi_y = 11.8
            'posi_y1 = 11.2

            Dim valor As Long
            Dim valor1 As Long
            valor = pFeatLayer.FeatureClass.FeatureCount(Nothing)
            valor1 = 0
            pFeature = pFeatureCursor.NextFeature

            Dim conta_t As Long
            Dim coordenada_DM(300) As Punto_DM
            '    pRow = ptableCursor.NextRow
            'pFeature = pFeatureCursor.NextFeature
            Do Until pFeature Is Nothing


                'Dim coordenada_DM(300) As Punto_DM
                conta_t = conta_t + 1
                valor1 = valor1 + 1
                l = pFeature.Shape
                ptcol = l
                ReDim coordenada_DM(ptcol.PointCount)
                'For i As Integer = 0 To ptcol.PointCount - 1
                'coordenada_DM(i).x = 0
                'coordenada_DM(i).y = 0
                'Next
                cuenta_valores = ptcol.PointCount + 7  'contador de registros
                cuenta_valores1 = cuenta_valores1 + cuenta_valores

                If s_tipo_plano <> "Plano Acumulacion" Then

                    v_codigo_x = pFeature.Value(campo1)
                    nombre_dm_x = pFeature.Value(campo2)
                    areaf1 = pFeature.Value(campo3)
                End If
                'Next k
                If cuenta_valores1 <= 33 Then


                    'comienza tu segundo bucle, el que recorre los puntos de cada polilinea
                    conta_vert = 0
                    j_vert = 0
                    ' pRow = ptableCursor.NextRow
                    For j = 0 To ptcol.PointCount - 2
                        conta_vert = conta_vert + 1
                        'MsgBox(conta_vert)
                        pt = ptcol.Point(j)
                        'If conta_vert = 1 Then  ' Para 1er punto
                        '    posi_y1 = posi_y1 - 0.2
                        '    'posi_y1 = posi_y1 - 0.18
                        '    posi_y = posi_y - 0.2
                        'Else  'Para n puntos
                        '    posi_y1 = posi_y1
                        '    posi_y = posi_y
                        'End If
                        este1 = pt.X
                        norte1 = pt.Y

                        'Redonde de coordenadas
                        norte1 = Format(Math.Round(norte1, 3), "###,###.00")
                        este1 = Format(Math.Round(este1, 3), "###,###.00")
                        'este1 = FormatNumber(este1, 2)
                        'norte1 = FormatNumber(norte1, 2)

                        coordenada_DM(j).v = j + 1
                        coordenada_DM(j).x = este1
                        coordenada_DM(j).y = norte1


                        ' Try
                        ' Dim int32_BlobFieldIndex As System.Int32
                        'Read the Blob (binary) stream from a file.
                        '  Dim memoryBlobStream As ESRI.ArcGIS.esriSystem.IMemoryBlobStream = New ESRI.ArcGIS.esriSystem.MemoryBlobStreamClass
                        ' memoryBlobStream.LoadFromFile("C:\BDGEOCATMIN\Temporal\Areainter_" & v_codigo & "_" & fecha_archi_sup_t & ".dbf")

                        'Add a new row to the table
                        ' Dim row As ESRI.ArcGIS.Geodatabase.IRow = pTable_sup.CreateRow
                        '  pRow = pTable_sup.CreateRow
                        'Get the Blob Field from the Table.
                        'Dim fields As ESRI.ArcGIS.Geodatabase.IFields = pRow.Fields
                        'Dim field As ESRI.ArcGIS.Geodatabase.IField = fields.Field(int32_BlobFieldIndex)

                        'Test to ensure the index number specified is a Blob Field
                        'If field.Type = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeBlob Then

                        'Write the Blob (binary) stream to the Blob Field and save
                        ' pRow.Value(int32_BlobFieldIndex) = CType(memoryBlobStream, System.Object) ' Explicit Cast
                        ' pRow.Store()
                        '


                        'Else



                        'End If
                        'Catch ex As System.Exception

                        'unsuccessful with error, interrogate more
                        'Return False
                        'MsgBox("1")
                        ' End Try



                        '    WriteBlobFileContentsToTableRowField("Areainter_" & v_codigo & "_" & fecha_archi_sup_t & "_t", pTable_sup)
                        pRow = pTable_sup.CreateRow

                        pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                        pRow.Value(pfields3.FindField("NOMBRE")) = nombre_dm_x
                        pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                        pRow.Value(pfields3.FindField("VERTICE")) = j + 1
                        pRow.Value(pfields3.FindField("ESTE")) = FormatNumber(este1, 2)
                        pRow.Value(pfields3.FindField("NORTE")) = FormatNumber(norte1, 2)
                        pRow.Value(pfields3.FindField("AREA")) = areaf1
                        pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                        pRow.Store()
                        '          pRow = ptableCursor.NextRow
                        'pTextElement2 = New TextElement
                        'pEnv = New Envelope
                        'pPoint2 = New Point



                    Next j ' ----- fin de bucle interno

                    'Calcular Area seg�n coordenadas redondeadas

                    coordenada_DM(j).x = coordenada_DM(0).x
                    coordenada_DM(j).y = coordenada_DM(0).y
                    Dim d0, d1, dr As Double
                    d0 = 0 : d1 = 0 : dr = 0
                    For h = 0 To j - 1  ' UBound(coordenada_DM) - 1
                        If coordenada_DM(h).x <> 0 Or coordenada_DM(h).y <> 0 Then
                            d0 = d0 + coordenada_DM(h).x * coordenada_DM(h + 1).y
                            d1 = d1 + coordenada_DM(h).y * coordenada_DM(h + 1).x
                        Else
                            Exit For
                        End If
                    Next h
                    dr = Math.Round(Math.Abs((d0 - d1) / 2) / 10000, 4)  'termino
                    'dr = Math.Abs((d0 - d1) / 2) / 10000  'termino
                    areaf1 = Format(Math.Round(dr, 4), "###,###.0000")
                    'MsgBox(areaf1, MsgBoxStyle.Exclamation, "1")
                    'coordenada_DM(h).x.ce()

                    'posi_y1 = posi_y1 - 0.35
                    'posi_y = posi_y - 0.35
                    'texto de Area Disponible

                    'posi_y2_m = posi_y1
                    'posi_y2 = posi_y
                    Dim t1 As Long
                    'Cabecera del plano  fijo
                    '  pRow = ptableCursor.NextRow
                    For t1 = 1 To 3
                        'pTextElement2 = New TextElement
                        'pEnv = New Envelope
                        'pPoint2 = New Point
                        pRow = pTable_sup.CreateRow
                        If t1 = 1 Then
                            'pTextElement2.Text = "---------------------------------------------"
                            ' pRow = pTable_sup.CreateRow
                            pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                            pRow.Value(pfields3.FindField("NOMBRE")) = "Constantexxxyyy"
                            pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                            pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                            ' pRow.Store()
                            '            pRow = ptableCursor.NextRow
                        ElseIf t1 = 2 Then
                            'pTextElement2.Text = "AREA UTM :" & areaf & " Ha."
                            ' pRow = pTable_sup.CreateRow
                            pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                            pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                            pRow.Value(pfields3.FindField("NOMBRE")) = "Areaxxxyyy"
                            pRow.Value(pfields3.FindField("AREA")) = areaf1
                            pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                            '  pRow.Store()
                            '           pRow = ptableCursor.NextRow
                        ElseIf t1 = 3 Then
                            ' pTextElement2.Text = "---------------------------------------------"
                            'pRow = pTable_sup.CreateRow
                            pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                            pRow.Value(pfields3.FindField("NOMBRE")) = "Constantexxxyyy"
                            pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                            pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                            'pRow.Store()
                            '          pRow = ptableCursor.NextRow
                        End If

                        'Posici�n dinamica de los textos

                        'pPoint2.X = 15.4
                        'pPoint2.Y = posi_y2_m - 0.6
                        'pPoint2.X = 19.3
                        'pPoint2.Y = posi_y2
                        'pEnv.UpperRight = pPoint2
                        'pElement2 = pTextElement2
                        'pElement2.Geometry = pEnv
                        'posi_y2_m = posi_y2_m - 0.5
                        'posi_y2 = posi_y2 - 0.5
                        pRow.Store()
                    Next t1

                ElseIf cuenta_valores1 > 33 Then
                    cuenta_valores = 0
                    cuenta_valores1 = 0
                    cuenta_valores = ptcol.PointCount + 7  'contador de registros
                    cuenta_valores1 = cuenta_valores1 + cuenta_valores
                    conta_hoja_sup = conta_hoja_sup + 1
                    If conta_hoja_sup > 100 Then
                        'formulario.Show()
                        Exit Sub
                    Else
                        'lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                        'colecciones_planos.Add("Plano Area superpuesta N�" & conta_hoja_sup)

                        If s_tipo_plano = "Plano Acumulacion" Then

                            lstformatoplanos.Items.Add("Plano Acumulacion N�" & conta_hoja_sup)
                            colecciones_planos.Add("Plano Acumulacion N�" & conta_hoja_sup)
                        Else

                            lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
                            colecciones_planos.Add("Plano Area superpuesta N�" & conta_hoja_sup)

                        End If

                        'cls_planos.Obtieneposicion_Plano_2()
                        'comienza tu segundo bucle, el que recorre los puntos de cada polilinea

                        conta_vert = 0
                        j_vert = 0
                        '     pRow = ptableCursor.NextRow
                        For j = 0 To ptcol.PointCount - 2
                            conta_vert = conta_vert + 1
                            ''MsgBox(conta_vert)
                            pt = ptcol.Point(j)

                            este1 = pt.X
                            norte1 = pt.Y

                            'Redonde de coordenadas
                            norte1 = Format(Math.Round(norte1, 3), "###,###.00")
                            este1 = Format(Math.Round(este1, 3), "###,###.00")
                            'este1 = FormatNumber(este1, 2)
                            'norte1 = FormatNumber(norte1, 2)

                            coordenada_DM(j).v = j + 1
                            coordenada_DM(j).x = este1
                            coordenada_DM(j).y = norte1

                            pRow = pTable_sup.CreateRow
                            pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                            pRow.Value(pfields3.FindField("NOMBRE")) = nombre_dm_x
                            pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                            pRow.Value(pfields3.FindField("VERTICE")) = j + 1
                            pRow.Value(pfields3.FindField("ESTE")) = FormatNumber(este1, 2)
                            pRow.Value(pfields3.FindField("NORTE")) = FormatNumber(norte1, 2)
                            pRow.Value(pfields3.FindField("AREA")) = areaf1
                            pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                            pRow.Store()
                            '        pRow = ptableCursor.NextRow

                        Next j ' ----- fin de bucle interno


                        'Calcular Area seg�n coordenadas redondeadas

                        coordenada_DM(j).x = coordenada_DM(0).x
                        coordenada_DM(j).y = coordenada_DM(0).y
                        Dim d0, d1, dr As Double
                        d0 = 0 : d1 = 0 : dr = 0
                        For h = 0 To j - 1  ' UBound(coordenada_DM) - 1
                            If coordenada_DM(h).x <> 0 Or coordenada_DM(h).y <> 0 Then
                                d0 = d0 + coordenada_DM(h).x * coordenada_DM(h + 1).y
                                d1 = d1 + coordenada_DM(h).y * coordenada_DM(h + 1).x
                            Else
                                Exit For
                            End If
                        Next h
                        dr = Math.Round(Math.Abs((d0 - d1) / 2) / 10000, 4)  'termino
                        'dr = Math.Abs((d0 - d1) / 2) / 10000  'termino
                        areaf1 = Format(Math.Round(dr, 4), "###,###.0000")
                        'MsgBox(areaf1, MsgBoxStyle.Exclamation, "1")
                        'coordenada_DM(h).x.ce()


                        'posi_y1 = posi_y1 - 0.35
                        'posi_y = posi_y - 0.35
                        'texto de Area Disponible

                        'posi_y2_m = posi_y1
                        'posi_y2 = posi_y
                        Dim t1 As Long
                        'Cabecera del plano  fijo
                        'pRow = ptableCursor.NextRow
                        For t1 = 1 To 3
                            'pTextElement2 = New TextElement
                            'pEnv = New Envelope
                            'pPoint2 = New Point
                            pRow = pTable_sup.CreateRow
                            If t1 = 1 Then
                                ' pTextElement2.Text = "---------------------------------------------"
                                ' pRow = pTable_sup.CreateRow
                                pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                                pRow.Value(pfields3.FindField("NOMBRE")) = "Constantexxxyyy"
                                pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                                pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                                'pRow.Store()
                                '    pRow = ptableCursor.NextRow
                            ElseIf t1 = 2 Then
                                'pTextElement2.Text = "AREA UTM :" & areaf & " Ha."
                                'pRow = pTable_sup.CreateRow
                                pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                                pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                                pRow.Value(pfields3.FindField("NOMBRE")) = "Areaxxxyyy"
                                pRow.Value(pfields3.FindField("AREA")) = areaf1
                                pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                                'pRow.Store()
                                '       pRow = ptableCursor.NextRow

                            ElseIf t1 = 3 Then
                                'pTextElement2.Text = "---------------------------------------------"
                                'pRow = pTable_sup.CreateRowBuffer
                                pRow.Value(pfields3.FindField("CODIGO")) = v_codigo_x
                                pRow.Value(pfields3.FindField("NOMBRE")) = "Constantexxxyyy"
                                pRow.Value(pfields3.FindField("HOJA")) = conta_hoja_sup
                                pRow.Value(pfields3.FindField("CONTADOR")) = conta_t
                                'pRow.Store()
                                'pRow = ptableCursor.NextRow
                            End If

                            'Posici�n dinamica de los textos

                            'pPoint2.X = 15.4
                            'pPoint2.Y = posi_y2_m - 0.6
                            'pPoint2.X = 19.3
                            'pPoint2.Y = posi_y2
                            'pEnv.UpperRight = pPoint2
                            'pElement2 = pTextElement2
                            'pElement2.Geometry = pEnv
                            'posi_y2_m = posi_y2_m - 0.5
                            'posi_y2 = posi_y2 - 0.5
                            pRow.Store()
                        Next t1
                    End If

                    'cuenta_valores_2 = 0
                    'cuenta_valores_3 = 0
                End If

                'Posici�n final del texto para seguir continuando con el bucle de coordenadas
                'posi_y2_m = posi_y2_m + 0.2
                'posi_y2 = posi_y2 + 0.2
                'posi_y1 = posi_y2_m
                'posi_y = posi_y2

                pFeature = pFeatureCursor.NextFeature

            Loop

            If s_tipo_plano = "Plano Acumulacion" Then
                lstformatoplanos.Items.Add("Plano Acumulacion N�1")
            Else
                lstformatoplanos.Items.Add("Plano Area superpuesta N�1")
            End If

            lstformatoplanos.Sorted = True

        Catch ex As Exception
            MsgBox("Error en el calculo de Areas Disponibles...", MsgBoxStyle.Critical, "GIS")

        End Try


    End Sub

    Public Sub asigna_escaladaplanolayout(ByVal nombre_dataframe As String, ByVal p_app As IApplication)

        'pMxDoc = p_App.Document
        pGC = pMxDoc.PageLayout
        'Dim pMapFrame1 As IMapFrame
        pMapFrame1 = pGC.FindFrame(pMxDoc.FocusMap)
        If caso_consulta = "TIPO CARTA IGN" Or caso_consulta = "CARTA IGN" Then
            'pMapFrame1.MapScale = escala_plano_carta
            pMapFrame1.MapScale = 100000
        ElseIf caso_consulta = "TIPO DEMARCACION POLITICA" Or caso_consulta = "DEMARCACION POLITICA" Then
            'pMapFrame1.MapScale = escala_plano_dema
            pMapFrame1.MapScale = 250000
        ElseIf caso_consulta = "CATASTRO MINERO" Then
            If esc_sim = "" Then
                pMapFrame1.MapScale = escala_plano_eval
            ElseIf esc_sim = "1" Then
                pMapFrame1.MapScale = escala_plano_eval * 4
            End If
        ElseIf caso_consulta = "DM_simultaneos" Then
            If esc_sim = "" Then
                pMapFrame1.MapScale = escala_plano_eval
            ElseIf esc_sim = "1" Then
                pMapFrame1.MapScale = escala_plano_eval * 2
            End If
        ElseIf caso_consulta = "DM SIMULTANEO" Then
            If esc_sim = "" Then
                pMapFrame1.MapScale = escala_plano_eval
            ElseIf esc_sim = "1" Then
                pMapFrame1.MapScale = escala_plano_simul * 4   ' se cambio de 2 a 4
            End If
        ElseIf caso_consulta = "Plano Venta" Then
            pMapFrame1.MapScale = escalaf
        ElseIf caso_consulta = "MAPA GEOLOGICO" Then
            pMapFrame1.MapScale = escala_plano_geolo
        End If

        pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentScale
        pMxDoc.UpdateContents()

    End Sub

    Public Sub Generaplanoreporte1(ByVal p_app As IApplication)
        ''Public Sub ChangeMapFrameScale(ByVal newScale As Double)
        'Dim pLayout As IPageLayout
        ''pLayout = pMxDoc.PageLayout
        'Dim pActiveView As IActiveView
        'Dim pLayoutGraphicsContainer As IGraphicsContainer
        'Dim pMapFrame As IMapFrame
        'Dim pMxDoc As IMxDocument

        ''Get a reference to the layout's graphics container
        'pMxDoc = p_app.Document
        'pLayout = pMxDoc.PageLayout
        'pLayoutGraphicsContainer = pMxDoc.PageLayout

        ''Find the map frame associated with the focus map
        ''pMapFrame = pLayoutGraphicsContainer.FindFrame(pMxDoc.FocusMap)
        'pMapFrame = pLayoutGraphicsContainer.FindFrame(pMxDoc.Maps.Item(0))

        ''If pMapFrame Is Nothing Then Exit Sub

        ''set the scale
        ''pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentScale
        'pMapFrame.MapScale = 100000
        'pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentScale
        ''Refresh the map frame
        'pActiveView = pMxDoc.PageLayout
        'pActiveView.Refresh()
        ''MsgBox(pMap.MapScale)


        'Dim pApp As IMxApplication
        Dim pDoc As IMxDocument
        Dim pMap As IMap
        Dim pLayout As IPageLayout
        Dim pGraphContainer As IGraphicsContainer
        Dim pElement As IElement

        'pApp = 
        pDoc = p_app.Document
        pMap = pDoc.FocusMap
        pLayout = pDoc.PageLayout
        pGraphContainer = pMxDoc.PageLayout
        'While Not pElement Is Nothing
        pGraphContainer.Reset()
        pElement = pGraphContainer.Next
        If TypeOf pElement Is IMapFrame Then
            Dim pMapFrame As IMapFrame
            pMapFrame = pElement
            pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentScale
            pMapFrame.MapScale = 100000

        End If
        'pElement = pGraphContainer.Next
        'End While

        pDoc.ActiveView.Refresh()
    End Sub

    'Public Sub genera_plano_libredenu(ByVal p_app As IApplication, ByVal tipo_plano As String)
    Public Sub genera_planoevaluacion(ByVal p_app As IApplication, ByVal tipo_plano As String)
        'Try
        '    caso_consulta = "DM SIMULTANEO"
        '    cls_eval.Eliminadataframe_sim(caso_consulta)
        '    If valida = True Then
        '        escala_plano_eval = 100000
        '        valida = False
        '    End If
        'Catch ex As Exception

        'End Try

        'Dim cls_planos As New Cls_planos
        'op_11 = Evaluacion, Op_12 = Simulacion
        'Pregunta primero si existe capa catastro para hacer plano evaluacion

        'Dim afound As Boolean = False
        'For A As Integer = 0 To pMap.LayerCount - 1
        '    If pMap.Layer(A).Name = "Catastro" Then
        '        afound = True
        '        Exit For
        '    End If
        'Next A
        'If Not afound Then
        '    MsgBox("No se encuentra capa Catastro para generar Plano Evaluaci�n...", MsgBoxStyle.Critical, "Observaci�n...")
        '    Exit Sub
        'End If


        'tipo_seleccion = "OP_11"
        If ((tipo_seleccion = "OP_11") Or (tipo_seleccion = "OP_12") Or (tipo_seleccion = "OP_16") Or (tipo_seleccion = "OP_20")) Then  'Evaluacion y Simulacion

            buscaadataframe("CATASTRO MINERO", False)
            If valida = False Then
                Exit Sub
            End If

            'Pregunta primero si existe capa catastro para hacer plano evaluacion

            Dim afound As Boolean = False
            For A As Integer = 0 To pMap.LayerCount - 1
                If pMap.Layer(A).Name = "Catastro" Then
                    afound = True
                    Exit For
                End If
            Next A
            If Not afound Then
                MsgBox("No se encuentra capa Catastro para generar Plano Evaluaci�n...", MsgBoxStyle.Critical, "Observaci�n...")
                Exit Sub
            End If


            v_posi_pr = False
            v_posi_po = False
            v_posi_si = False
            v_posi_ex = False
            Cuenta_an = 0
            Cuenta_po = 0
            Cuenta_si = 0
            Cuenta_ex = 0
            Dim cls_catastro As New cls_DM_1

            If tipo_plano = "Evaluacion" Then
                s_tipo_plano = "Plano_Evaluacion"
                cls_catastro.Zoom_to_Layer("Catastro")
                escalaf = escala_plano_eval
                mueveposiciondataframe("CATASTRO MINERO", p_app)
                prendecapas()
            ElseIf tipo_plano = "Libredenu" Then
                s_tipo_plano = "Plano_libredenu"

                'escalaf = escala_plano_eval
                mueveposiciondataframe("CATASTRO MINERO", p_app)
                prendecapas()
            End If

            '  Dim cls_catastro As New cls_DM_1
            If tipo_plano = "Libredenu" Then
                Generaplanoreporte("Plano Libredenu")
            ElseIf tipo_plano = "Evaluacion" Then
                Generaplanoreporte("Plano catastral")
            End If

            '  cls_catastro.Zoom_to_Layer("Catastro")
            creacionmedidasgrillas("CATASTRO MINERO")
            If tipo_plano = "Evaluacion" Then
                LEERESULTADOSEVAL("Plano_Evaluacion")
                AgregarTextosLayout("Evaluacion", "evaluacion")
            ElseIf tipo_plano = "Libredenu" Then
                LEERESULTADOSEVAL("Plano_libredenu")
                AgregarTextosLayout("Libredenu", "Libredenu")
            End If


            'AgregarTextosLayout("Libredenu", "Libredenu")
            GENERALISTADODM_PLANOEV(p_app)
            'GENERALISTADODM_PLANOEV_ant()
            agrega_logofoliacion_plano(p_app, "CATASTRO MINERO")
            caso_consulta = "CATASTRO MINERO"
            escala_plano_eval = escalaf
            'cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
            asigna_escaladaplanolayout("CATASTRO MINERO", p_app)
            cls_catastro.Zoom_to_Layer("Catastro")
            CambiaALayout(p_app)
            s_tipo_plano = ""
            pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentDefault
            Dim pagina As IPageLayout
            pagina = pMxDoc.PageLayout
            pagina.ZoomToWhole()
        Else
            MsgBox("No es el tipo de plano para esta consulta", MsgBoxStyle.Critical, "BDGEOCATMIN")
        End If


    End Sub


    Public Sub genera_planoevaluacion_ant(ByVal p_app As IApplication)
        'Dim cls_planos As New Cls_planos
        'op_11 = Evaluacion, Op_12 = Simulacion
        tipo_seleccion = "OP_11"
        If ((tipo_seleccion = "OP_11") Or (tipo_seleccion = "OP_12")) Then  'Evaluacion y Simulacion

            buscaadataframe("CATASTRO MINERO", False)
            If valida = False Then
                Exit Sub
            End If
            s_tipo_plano = "Plano_Evaluacion"
            v_posi_pr = False
            v_posi_po = False
            v_posi_si = False
            v_posi_ex = False
            Cuenta_an = 0
            Cuenta_po = 0
            Cuenta_si = 0
            Cuenta_ex = 0


            'Dim cls_planos As New Cls_planos
            Dim cls_catastro As New cls_DM_1
            'asignaescaladataframe("CATASTRO MINERO")
            mueveposiciondataframe("CATASTRO MINERO", p_app)  'se comento hoy
            prendecapas()  'se comento hoy
            'cls_catastro.Zoom_to_Layer("Catastro")
            'asigna_escaladaplanolayout("CATASTRO MINERO", p_app)

            escalaf = escala_plano_eval  'se comento hoy
            escalaf = pMap.MapScale  ' se aumento hoy


            Generaplanoreporte("Plano catastral")
            cls_catastro.Zoom_to_Layer("Catastro")
            creacionmedidasgrillas("CATASTRO MINERO")
            LEERESULTADOSEVAL("Plano_Evaluacion")
            AgregarTextosLayout("Evaluacion", "evaluacion")
            GENERALISTADODM_PLANOEV(p_app)
            agrega_logofoliacion_plano(p_app, "CATASTRO MINERO")
            caso_consulta = "CATASTRO MINERO"

            'cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
            asigna_escaladaplanolayout("CATASTRO MINERO", p_app)
            'CambiaALayout(p_app)
            s_tipo_plano = ""
            pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentDefault
            Dim pagina As IPageLayout
            pagina = pMxDoc.PageLayout
            pagina.ZoomToWhole()
        Else
            MsgBox("No genero la opci�n de Evaluaci�n de DM", MsgBoxStyle.Critical, "BDGEOCATMIN")
        End If


    End Sub
    Public Sub AgregarTextosLayout(ByVal sele_reporte As String, ByVal caso_simula As String)
        'PROGRAMA PARA AGREGAR TEXTOS EN LAYOUT
        '*******************************************
        Dim MyDate As Date
        MyDate = Now
        Dim fecha As String
        fecha = RellenarComodin(MyDate.Year, 2, "0") & "/" & RellenarComodin(MyDate.Month, 2, "0") & "/" & MyDate.Day

        Dim posi_y2 As Double
        Dim posi_y3 As Double

        Dim pPageLayout As IPageLayout
        Dim pActiveView As IActiveView
        Dim pGraphicsContainer As IGraphicsContainer
        Dim pElement As IElement
        Dim pTextElement As ITextElement
        Dim pPoint As IPoint
        Dim pTxtSym As IFormattedTextSymbol
        Dim contatexto As Integer
        Dim myColor As IRgbColor
        Dim pFont As IFontDisp

        pPageLayout = pMxDoc.PageLayout
        pPageLayout.Page.Units = esriUnits.esriCentimeters
        pActiveView = pPageLayout
        pGraphicsContainer = pPageLayout

        Dim posi_x As String
        Dim posi_x1 As String
        Dim conta_x As Long
        'Para obtener fecha

        'Dim fecha_plano As Date
        'fecha_plano = Date
        Dim posi_y As Double
        Dim posi_y1 As Double
        Dim pEnv As IEnvelope
        posi_y = posi_y_priori_CNM
        posi_y1 = posi_y_priori1_CNM
        Dim Nombre_urba As String = ""
        Dim Nombre_rese As String = ""

        'Colocando textos fijos al plano

        If sele_reporte = "Evaluacion" Then
            For contatexto = 1 To 16
                'Guardar el texto
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "Carta : " & v_carta_dm
                    pPoint.X = 8.1
                    pPoint.Y = 18.3
                    pPoint.X = 9.2
                    pPoint.Y = 17.8
                ElseIf contatexto = 2 Then
                    pTextElement.Text = "Fecha : " & fecha
                    pPoint.X = 13.8
                    pPoint.Y = 18.3
                    pPoint.X = 14.8
                    pPoint.Y = 17.8
                ElseIf contatexto = 3 Then
                    pTextElement.Text = "CODIGO DEL DM : " & v_codigo
                    pPoint.X = 14.1
                    pPoint.Y = 16.5
                    pPoint.X = 18.2
                    pPoint.Y = 17.3
                ElseIf contatexto = 4 Then
                    pTextElement.Text = "NOMBRE DEL DM : " & v_nombre_dm
                    pPoint.X = 14.1
                    pPoint.Y = 15.9
                    pPoint.X = 18.2
                    pPoint.Y = 16.7
                ElseIf contatexto = 5 Then
                    pTextElement.Text = "HECTAREA : " & " " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 14.1
                    pPoint.Y = 15.4
                    pPoint.X = 18.2
                    pPoint.Y = 16.1
                ElseIf contatexto = 6 Then
                    If Cuenta_an = 0 Then
                        pTextElement.Text = "DERECHOS PRIORITARIOS :  No Presenta DM Prioritarios"
                    Else
                        pTextElement.Text = "DERECHOS PRIORITARIOS : " & "(" & Cuenta_an & ")"
                    End If
                    pPoint.X = 15.7
                    pPoint.Y = 14.7
                    pPoint.X = 18.2
                    pPoint.Y = 15.5
                ElseIf contatexto = 7 Then
                    If v_tipo_exp = "RD" Then
                        If Cuenta_rd = 0 Then
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad "
                        Else
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad " & "(" & Cuenta_rd & ")"
                        End If
                        pPoint.X = 14.1
                        pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.X = 18.2
                        pPoint.Y = conta_rd_posi_y - 0.2
                    Else
                        pPoint.X = 14.1
                        'pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.Y = 14.7
                        pPoint.X = 18.2
                        'pPoint.Y = conta_rd_posi_y - 0.2
                        pPoint.Y = 15.5

                    End If
                ElseIf contatexto = 8 Then
                    If Cuenta_po = 0 Then
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA : No Presenta "
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES :  No Presenta DM Posteriores"
                        End If
                    Else
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA: " & "(" & Cuenta_rd & ")"
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES : " & "(" & Cuenta_po & ")"
                        End If
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = conta_po_posi_y1 - 0.2
                    pPoint.X = 18.2
                    pPoint.Y = conta_po_posi_y - 0.2

                ElseIf contatexto = 9 Then
                    If Cuenta_si = 0 Then
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  No Presenta DM Simultaneos"
                    Else
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  " & "(" & Cuenta_si & ")"
                    End If
                    pPoint.X = 14.1
                    'pPoint.Y = conta_si_posi_y1 - 0.15
                    pPoint.Y = conta_si_posi_y1 - 0.1
                    pPoint.X = 18.2
                    'pPoint.Y = conta_si_posi_y - 0.15
                    pPoint.Y = conta_si_posi_y - 0.1

                ElseIf contatexto = 10 Then
                    If Cuenta_ex = 0 Then
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  No Presenta DM Extinguidos"
                    Else
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  " & "(" & Cuenta_ex & ")"
                    End If

                    pPoint.X = 14.1
                    'pPoint.Y = conta_ex_posi_y1 - 0.15
                    pPoint.Y = conta_ex_posi_y1 - 0.01
                    pPoint.X = 18.2
                    pPoint.Y = conta_ex_posi_y - 0.01

                ElseIf contatexto = 11 Then

                    pTextElement.Text = "CATASTRO NO MINERO"


                    'pPoint.Y = posi_y1 - 0.4
                    ''pPoint.Y = posi_y1 - 0.6
                    pPoint.Y = posi_y1 - 0.4
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 0.4


                ElseIf contatexto = 12 Then
                    If lista_urba = "" Then
                        pTextElement.Text = "Zonas Urbanas :" & " No se encuentra superpuesto a un Area urbana"
                    Else
                        conta_x = lista_urba.Length
                        If conta_x > 65 Then
                            posi_x = Mid(lista_urba, 1, 65)
                            posi_x1 = Right(lista_urba, (lista_urba.Length - 65))
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & lista_urba
                        End If
                    End If
                    pPoint.X = 14.1
                    ''pPoint.Y = posi_y1 - 1
                    pPoint.Y = posi_y1 - 0.8
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 0.8


                ElseIf contatexto = 13 Then
                    If lista_rese = "" Then
                        pTextElement.Text = "Zonas Reservadas :" & "  No se encuentra superpuesto a un Area de Reserva"
                    Else
                        conta_x = lista_rese.Length
                        If conta_x > 650 Then
                            posi_x = Mid(lista_rese, 1, 65)
                            posi_x1 = Right(lista_rese, (lista_rese.Length - 65))
                            pTextElement.Text = "Zonas Reservadas : " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Reservadas : " & "   " & lista_rese
                        End If
                    End If
                    pPoint.X = 14.1
                    ''pPoint.Y = posi_y1 - 1.4
                    pPoint.Y = posi_y1 - 1.3
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1.3
                ElseIf contatexto = 14 Then

                    pTextElement.Text = "L�mites fronterizos (Fuente IGN): Distancia de la l�nea de frontera de " & distancia_fron & " (Km.)"

                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 1.8
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1.8

                ElseIf contatexto = 15 Then

                    pTextElement.Text = "                                LISTADO DE DERECHOS MINEROS"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.2
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.2

                ElseIf contatexto = 16 Then
                    ' pTextElement.Text = "N�   NOMBRE                         CODIGO      ZONA   TE      TP   PUBL   INCOR   SUST"
                    pTextElement.Text = "N�      NOMBRE                                    CODIGO              TE    TP   INCOR  SUST"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.6
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.6
                    posi_y1_list = posi_y1 - 2.9
                    posi_y2_list = posi_y - 2.9

                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"

                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then

                    pFont.Size = 8
                    'pFont.Size = 10

                ElseIf ((contatexto = 1) Or (contatexto = 2)) Then
                    pFont.Size = 7
                Else
                    'pFont.Size = 5.5
                    pFont.Size = 6.0     'cambio 2015
                    'pFont.Size = 8
                End If

                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    myColor.RGB = RGB(197, 0, 255)
                ElseIf ((contatexto = 1) Or (contatexto = 2) Or (contatexto = 6) Or (contatexto = 7) Or (contatexto = 8) Or (contatexto = 9) Or (contatexto = 10)) Then
                    myColor.RGB = RGB(71, 61, 255)
                ElseIf contatexto = 11 Then
                    myColor.RGB = RGB(230, 0, 0)
                ElseIf ((contatexto = 12) Or (contatexto = 13) Or (contatexto = 14) Or (contatexto = 15)) Then
                    myColor.RGB = RGB(0, 0, 0)
                ElseIf (contatexto = 16) Then
                    myColor.RGB = RGB(71, 61, 255)
                End If
                pTxtSym.Color = myColor

                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                If contatexto = 1 Or contatexto = 2 Or contatexto = 3 Or contatexto = 4 Or contatexto = 5 Or contatexto = 6 Then
                    pGraphicsContainer.AddElement(pTextElement, 1)
                ElseIf contatexto = 7 Then
                    If v_tipo_exp = "RD" Then
                        If v_posi_rd = False Or Cuenta_rd > 0 Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                ElseIf contatexto = 8 Then
                    If v_posi_po = False Or Cuenta_po > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 9 Then
                    If v_posi_si = False Or Cuenta_si > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 10 Then
                    If v_posi_ex = False Or Cuenta_ex > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                Else
                    If contador_hojas < 1 Then
                        If v_posi_pr = False And v_posi_po = False And v_posi_si = False And v_posi_ex = False Or v_posi_rd Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                End If
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto

        ElseIf sele_reporte = "Libredenu" Then
            For contatexto = 1 To 16
                'Guardar el texto
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "Carta : " & v_carta_dm
                    pPoint.X = 8.1
                    pPoint.Y = 18.3
                    pPoint.X = 9.2
                    pPoint.Y = 17.8
                ElseIf contatexto = 2 Then
                    pTextElement.Text = "Escala : " & escalaf_lib
                    pPoint.X = 10.9
                    pPoint.Y = 18.3
                    pPoint.X = 11.5
                    pPoint.Y = 17.8
                ElseIf contatexto = 3 Then
                    pTextElement.Text = "Fecha : " & fecha
                    pPoint.X = 13.8
                    pPoint.Y = 18.3
                    pPoint.X = 14.8
                    pPoint.Y = 17.8
                ElseIf contatexto = 4 Then
                    pTextElement.Text = "CODIGO DEL DM : " & v_codigo
                    pPoint.X = 14.1
                    pPoint.Y = 16.5
                    pPoint.X = 18.2
                    pPoint.Y = 17.3
                ElseIf contatexto = 5 Then
                    pTextElement.Text = "NOMBRE DEL DM : " & v_nombre
                    pPoint.X = 14.1
                    pPoint.Y = 15.9
                    pPoint.X = 18.2
                    pPoint.Y = 16.7
                ElseIf contatexto = 6 Then
                    pTextElement.Text = "HECTAREA : " & " " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 14.1
                    pPoint.Y = 15.4
                    pPoint.X = 18.2
                    pPoint.Y = 16.1
                ElseIf contatexto = 7 Then
                    If Cuenta_an = 0 Then
                        pTextElement.Text = "DERECHOS PRIORITARIOS :  No Presenta DM Prioritarios"
                    Else
                        pTextElement.Text = "DERECHOS PRIORITARIOS : " & "(" & Cuenta_an & ")"
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = 14.8
                    pPoint.X = 18.2
                    pPoint.Y = 15.5


                ElseIf contatexto = 8 Then
                    If v_tipo_exp = "RD" Then
                        If Cuenta_rd = 0 Then
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad "
                        Else
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad " & "(" & Cuenta_rd & ")"
                        End If
                        pPoint.X = 14.1
                        pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.X = 18.2
                        pPoint.Y = conta_rd_posi_y - 0.2
                    Else
                        pPoint.X = 14.1
                        'pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.Y = 14.7
                        pPoint.X = 18.2
                        'pPoint.Y = conta_rd_posi_y - 0.2
                        pPoint.Y = 15.5

                    End If
                ElseIf contatexto = 9 Then
                    If Cuenta_po = 0 Then
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA : No Presenta "
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES :  No Presenta DM Posteriores"
                        End If
                    Else
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA: " & "(" & Cuenta_rd & ")"
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES : " & "(" & Cuenta_po & ")"
                        End If
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = conta_po_posi_y1 - 0.2
                    pPoint.X = 18.2
                    pPoint.Y = conta_po_posi_y - 0.2

                ElseIf contatexto = 10 Then
                    If Cuenta_si = 0 Then
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  No Presenta DM Simultaneos"
                    Else
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  " & "(" & Cuenta_si & ")"
                    End If
                    pPoint.X = 14.1
                    'pPoint.Y = conta_si_posi_y1 - 0.15
                    pPoint.Y = conta_si_posi_y1 - 0.1
                    pPoint.X = 18.2
                    'pPoint.Y = conta_si_posi_y - 0.15
                    pPoint.Y = conta_si_posi_y - 0.1

                ElseIf contatexto = 11 Then
                    If Cuenta_ex = 0 Then
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  No Presenta DM Extinguidos"
                    Else
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  " & "(" & Cuenta_ex & ")"
                    End If

                    pPoint.X = 14.1
                    'pPoint.Y = conta_ex_posi_y1 - 0.15
                    pPoint.Y = conta_ex_posi_y1 - 0.01
                    pPoint.X = 18.2
                    pPoint.Y = conta_ex_posi_y - 0.01

                ElseIf contatexto = 12 Then

                    pTextElement.Text = "OTROS DATOS DE EVALUACION"

                    'pPoint.Y = posi_y1 - 0.4
                    ''pPoint.Y = posi_y1 - 0.6
                    pPoint.Y = posi_y1 - 0.4
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 0.4


                ElseIf contatexto = 13 Then
                    If lista_urba = "" Then
                        pTextElement.Text = "Zonas Urbanas :" & " No se encuentra superpuesto a un Area urbana"
                    Else
                        conta_x = lista_urba.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_urba, 1, 60)
                            posi_x1 = Right(lista_urba, (lista_urba.Length - 60))
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & lista_urba
                        End If
                    End If
                    pPoint.X = 14.1
                    ''pPoint.Y = posi_y1 - 1
                    pPoint.Y = posi_y1 - 0.8
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 0.8


                ElseIf contatexto = 14 Then
                    If lista_rese = "" Then
                        pTextElement.Text = "Zonas Reservadas :" & " No se encuentra superpuesto a un Area de Reserva"
                    Else
                        conta_x = lista_rese.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_rese, 1, 60)
                            posi_x1 = Right(lista_rese, (lista_rese.Length - 60))
                            pTextElement.Text = "Zonas Reservadas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Reservadas :      " & "  " & lista_rese
                        End If
                    End If
                    pPoint.X = 14.1
                    ''pPoint.Y = posi_y1 - 1.4
                    pPoint.Y = posi_y1 - 1.2
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1.2

                ElseIf contatexto = 15 Then

                    pTextElement.Text = "                                LISTADO DE DERECHOS MINEROS"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.2
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.2

                ElseIf contatexto = 16 Then
                    pTextElement.Text = "N�   NOMBRE                         CODIGO      ZONA   TE      TP   PUBL   INCOR   SUST"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.6
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.6
                    posi_y1_list = posi_y1 - 2.9
                    posi_y2_list = posi_y - 2.9

                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'If ((contatexto = 9) Or (contatexto = 10) Or (contatexto = 11)) Then
                If ((contatexto = 4) Or (contatexto = 5) Or (contatexto = 6)) Then
                    'pFont.Size = 8
                    pFont.Size = 8

                ElseIf ((contatexto = 1) Or (contatexto = 2) Or (contatexto = 3)) Then
                    pFont.Size = 7
                Else
                    'pFont.Size = 9
                    pFont.Size = 6    'cambio 2015
                    'pFont.Size = 5.5
                End If

                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                If ((contatexto = 4) Or (contatexto = 5) Or (contatexto = 6)) Then
                    myColor.RGB = RGB(197, 0, 255)
                ElseIf ((contatexto = 1) Or (contatexto = 2) Or (contatexto = 3) Or (contatexto = 7) Or (contatexto = 8) Or (contatexto = 9) Or (contatexto = 10) Or (contatexto = 11) Or (contatexto = 12)) Then
                    myColor.RGB = RGB(71, 61, 255)
                ElseIf contatexto = 12 Then
                    myColor.RGB = RGB(230, 0, 0)
                ElseIf ((contatexto = 13) Or (contatexto = 14) Or (contatexto = 15)) Then
                    myColor.RGB = RGB(0, 0, 0)
                ElseIf (contatexto = 16) Then
                    myColor.RGB = RGB(71, 61, 255)
                End If
                pTxtSym.Color = myColor

                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                If contatexto = 1 Or contatexto = 2 Or contatexto = 3 Or contatexto = 4 Or contatexto = 5 Or contatexto = 6 Or contatexto = 7 Then
                    pGraphicsContainer.AddElement(pTextElement, 1)
                ElseIf contatexto = 8 Then
                    If v_tipo_exp = "RD" Then
                        If v_posi_rd = False Or Cuenta_rd > 0 Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                ElseIf contatexto = 9 Then
                    If v_posi_po = False Or Cuenta_po > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 10 Then
                    If v_posi_si = False Or Cuenta_si > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 11 Then
                    If v_posi_ex = False Or Cuenta_ex > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                Else
                    If contador_hojas < 1 Then
                        If v_posi_pr = False And v_posi_po = False And v_posi_si = False And v_posi_ex = False Or v_posi_rd Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                End If
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Geologico" Then
            For contatexto = 1 To 9
                'Guardar el texto
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then

                    pTextElement.Text = v_codigo
                    pPoint.X = 18.9
                    pPoint.Y = 16.4
                    ' pPoint.X = 11.7
                    ' pPoint.Y = 5.34

                ElseIf contatexto = 2 Then

                    conta_x = v_nombre_dm.Length
                    If conta_x >= 60 Then
                        posi_x = Mid(v_nombre_dm, 1, 60)
                        posi_x1 = Right(v_nombre_dm, (v_nombre_dm.Length - 60))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = v_nombre_dm
                    End If
                    ' pPoint.X = 11.7    '17.1
                    ' pPoint.Y = 4.68     '9.1

                    pPoint.X = 20.1  '17.1
                    pPoint.Y = 15.37     '9.1
                ElseIf contatexto = 3 Then
                    pTextElement.Text = FormatNumber(v_area_eval, 4) & " Ha."

                    pPoint.X = 18.9    '17.1
                    'pPoint.Y = 8.1
                    pPoint.Y = 14.25     '9.1
                    'pPoint.X = 16.75    '21.7
                    'pPoint.Y = 8.9
                    'pPoint.Y = 4.37     '9.9
                ElseIf contatexto = 4 Then

                    conta_x = lista_cd_cartas.Length
                    If conta_x > 50 Then
                        posi_x = Mid(lista_cd_cartas, 1, 50)
                        'posi_x1 = Right(lista_nmhojas_ign, (lista_nmhojas_ign.Length - 60))
                        posi_x1 = Right(lista_cd_cartas, (lista_cd_cartas.Length - 50))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        'PTextElement.Text = "Nombre de la Carta : " & "  " & lista_nmhojas_ign
                        pTextElement.Text = "                   " & lista_cd_cartas
                    End If

                    pPoint.X = 19.1  '17.9
                    'pPoint.Y = 6.9
                    pPoint.Y = 13.18    '7.6
                    ''pPoint.X = 21.7
                    'pPoint.Y = 7.7
                    ''pPoint.Y = 8.4


                ElseIf contatexto = 5 Then
                    'pTextElement.Text = lista_prov
                    conta_x = lista_nmhojas_ign.Length
                    If conta_x > 50 Then
                        posi_x = Mid(lista_nmhojas_ign, 1, 50)
                        posi_x1 = Right(lista_nmhojas_ign, (lista_nmhojas_ign.Length - 50))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = lista_nmhojas_ign
                    End If
                    pPoint.X = 21.1
                    'pPoint.Y = 6.3
                    pPoint.Y = 12.08
                    ''pPoint.X = 21.7
                    'pPoint.Y = 7.1
                    ''pPoint.Y = 7.6
                ElseIf contatexto = 6 Then
                    'pTextElement.Text = lista_depa
                    conta_x = lista_Cd_boletin.Length
                    If conta_x > 48 Then
                        posi_x = Mid(lista_Cd_boletin, 1, 48)
                        posi_x1 = Right(lista_Cd_boletin, (lista_Cd_boletin.Length - 48))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                        pPoint.X = 20.15
                        'pPoint.Y = 5.7
                        pPoint.Y = 2.7
                    Else
                        pTextElement.Text = lista_Cd_boletin
                        pPoint.X = 20.3
                        pPoint.Y = 2.5

                    End If
                ElseIf contatexto = 7 Then
                    pTextElement.Text = DateTime.Today
                    pPoint.X = 20.7
                    '  pPoint.X = 15.5
                    pPoint.Y = 7.1
                    ' pPoint.Y = 21.9
                ElseIf contatexto = 8 Then
                    pTextElement.Text = "1: " & escala_plano_geolo
                    pPoint.X = 22.7
                    '  pPoint.X = 15.5
                    pPoint.Y = 7.1
                    ' pPoint.Y = 21.9
                ElseIf contatexto = 9 Then
                    pTextElement.Text = v_sistema
                    pPoint.X = 20.8
                    '  pPoint.X = 15.5
                    pPoint.Y = 6.4
                    ' pPoint.Y = 21.9

                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv

                'Simbolo del texto
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                If contatexto = 7 Or contatexto = 8 Then
                    pFont.Size = 4
                Else

                    pFont.Size = 7
                End If
                'Else

                'pFont.Size = 7
                'End If
                pFont.Bold = False
                pTxtSym.Font = pFont
                'Propiedades del Simbolo
                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False

                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Cuadricula" Then
            Dim canti_cuadri As Integer
            canti_cuadri = v_area_eval / 100

            For contatexto = 1 To 8
                'Guardar el texto
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "Carta : " & v_carta_dm
                    pPoint.X = 8.1
                    pPoint.Y = 18.5
                    pPoint.X = 9.2
                    pPoint.Y = 18.0
                ElseIf contatexto = 2 Then
                    pTextElement.Text = "Fecha : " & fecha
                    pPoint.X = 13.8
                    pPoint.Y = 18.5
                    pPoint.X = 14.8
                    pPoint.Y = 18.0
                ElseIf contatexto = 3 Then
                    pTextElement.Text = "CODIGO DEL DM : " & v_codigo
                    pPoint.X = 14.4
                    pPoint.Y = 16.5
                    pPoint.X = 18.5
                    pPoint.Y = 17.3
                ElseIf contatexto = 4 Then
                    pTextElement.Text = "NOMBRE DEL DM : " & v_nombre_dm
                    pPoint.X = 14.4
                    pPoint.Y = 15.9
                    pPoint.X = 18.5
                    pPoint.Y = 16.7
                ElseIf contatexto = 5 Then
                    pTextElement.Text = "HECTAREA : " & " " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 14.4
                    pPoint.Y = 15.4
                    pPoint.X = 18.5
                    pPoint.Y = 16.1
                ElseIf contatexto = 6 Then
                    pTextElement.Text = "* EL PRESENTE PETITORIO HA SIDO FORMULADO POR : " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 17.8
                    pPoint.Y = 11.2
                    pPoint.X = 18.8
                    pPoint.Y = 10.8

                ElseIf contatexto = 7 Then
                    pTextElement.Text = "* ESTA CONFORMADO POR  " & canti_cuadri & " CUADRICULAS DE 100.00 Ha."
                    pPoint.X = 17.8
                    pPoint.Y = 10.6
                    pPoint.X = 18.8
                    pPoint.Y = 10.2
                ElseIf contatexto = 8 Then
                    pTextElement.Text = "* LAS CUADRICULAS SON  " & nombre_datos
                    pPoint.X = 17.8
                    pPoint.Y = 10.0
                    pPoint.X = 18.8
                    pPoint.Y = 9.6
                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTxtSym = New TextSymbol
                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"

                pFont.Size = 6
                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    myColor.RGB = RGB(197, 0, 255)
                ElseIf ((contatexto = 1) Or (contatexto = 2)) Then
                    myColor.RGB = RGB(71, 61, 255)
                ElseIf ((contatexto = 6) Or (contatexto = 7) Or (contatexto = 8)) Then
                    myColor.RGB = RGB(0, 0, 255)

                End If
                pTxtSym.Color = myColor
                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto

        ElseIf sele_reporte = "Carta" Then
            Dim posi_y_c As Double
            Dim posi_y_c1 As Double
            posi_y_c = 12.35
            posi_y_c1 = 12.03

            'Guardar el texto
            For contatexto = 1 To 13
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then

                    'pTextElement.Text = "DERECHO MINERO:  " & v_nombre_dm
                    conta_x = v_nombre_dm.Length
                    'If conta_x > 45 Then
                    If conta_x > 60 Then
                        posi_x = Mid(v_nombre_dm, 1, 60)
                        posi_x1 = Right(v_nombre_dm, (v_nombre_dm.Length - 60))
                        pTextElement.Text = "DERECHO MINERO:  " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "DERECHO MINERO:  " & v_nombre_dm
                    End If
                    pPoint.X = 19.1
                    'pPoint.Y = 12.353
                    pPoint.Y = posi_y_c
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                    'pPoint.Y = 12.003

                ElseIf contatexto = 2 Then
                    conta_x = lista_nmhojas_ign.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_nmhojas_ign, 1, 60)
                        'posi_x1 = Right(lista_nmhojas_ign, (lista_nmhojas_ign.Length - 60))
                        posi_x1 = Right(lista_nmhojas_ign, (lista_nmhojas_ign.Length - 60))
                        pTextElement.Text = "Nombre de la Carta : " & "  " & posi_x & vbNewLine & posi_x1
                    Else
                        'PTextElement.Text = "Nombre de la Carta : " & "  " & lista_nmhojas_ign
                        pTextElement.Text = "Nombre de la Carta : " & "  " & lista_nmhojas_ign
                    End If

                    pPoint.X = 19.1
                    'pPoint.Y = 11.984
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 11.634
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 3 Then
                    'pTextElement.Text = "Numero de la Carta :  " & carta_v
                    conta_x = lista_cd_cartas.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_cd_cartas, 1, 60)
                        posi_x1 = Right(lista_cd_cartas, (lista_cd_cartas.Length - 60))
                        pTextElement.Text = "N�mero de la Carta : " & "  " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "N�mero de la Carta : " & "  " & lista_cd_cartas
                    End If
                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 4 Then
                    pTextElement.Text = "Escala                  :" & "    " & "1/100 000"
                    pPoint.X = 19.1
                    'pPoint.Y = 11.341
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 10.991

                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 5 Then
                    'pTextElement.Text = "Zona                    :" & "   " & "ZONA"
                    pTextElement.Text = "Zona                    : " & "   " & v_zona_dm
                    pPoint.X = 19.1
                    'pPoint.Y = 11.02
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 10.68
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 6 Then
                    pTextElement.Text = "UBICACION"
                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 7 Then
                    'pTextElement.Text = "Distrito                :   " & lista_dist
                    If caso_consulta1 = "TIPO CARTA IGN" Then
                        conta_x = lista_dist_carta.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_dist_carta, 1, 45)
                            posi_x1 = Right(lista_dist_carta, (lista_dist_carta.Length - 60))
                            pTextElement.Text = "DISTRITOS (S)       : " & "   " & posi_x & vbNewLine & posi_x1
                            posi_y_c = posi_y_c + 0.3
                            posi_y_c1 = posi_y_c1 - 0.3
                        Else
                            pTextElement.Text = "DISTRITOS (S)       : " & "   " & lista_dist_carta
                        End If
                    Else
                        ' pTextElement.Text = "DISTRITOS (S)       : " & "   " & lista_dist

                        conta_x = lista_dist.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_dist, 1, 45)
                            posi_x1 = Right(lista_dist, (lista_dist.Length - 60))
                            pTextElement.Text = "DISTRITOS (S)       : " & "   " & posi_x & vbNewLine & posi_x1
                            posi_y_c = posi_y_c + 0.3
                            posi_y_c1 = posi_y_c1 - 0.3
                        Else
                            pTextElement.Text = "DISTRITOS (S)       : " & "   " & lista_dist
                        End If

                    End If

                        pPoint.X = 19.1
                        pPoint.X = 19.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.3
                        posi_y_c1 = posi_y_c1 - 0.3

                    ElseIf contatexto = 8 Then
                        '   conta_x = lista_prov.Length
                        '  If conta_x > 60 Then
                        ' posi_x = Mid(lista_prov, 1, 60)
                        'posi_x1 = Right(lista_prov, (lista_prov.Length - 60))
                    If caso_consulta1 = "TIPO CARTA IGN" Then
                        conta_x = lista_prov_carta.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_prov_carta, 1, 60)
                            posi_x1 = Right(lista_prov_carta, (lista_prov_carta.Length - 60))
                            pTextElement.Text = "PROVINCIA (S)       : " & "   " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "PROVINCIA (S)       : " & "   " & lista_prov_carta
                        End If

                    Else
                        'pTextElement.Text = "PROVINCIA (S)       : " & "   " & lista_prov

                        conta_x = lista_prov.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_prov, 1, 60)
                            posi_x1 = Right(lista_prov, (lista_prov.Length - 60))
                            pTextElement.Text = "PROVINCIA (S)       : " & "   " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "PROVINCIA (S)       : " & "   " & lista_prov
                        End If


                    End If

                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                    ElseIf contatexto = 9 Then
                      
                    If caso_consulta1 = "TIPO CARTA IGN" Then
                        conta_x = lista_depa_carta.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_depa_carta, 1, 60)
                            posi_x1 = Right(lista_depa_carta, (lista_depa_carta.Length - 60))
                            pTextElement.Text = "DEPARTAMENTO (S) : " & "   " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "DEPARTAMENTO (S) : " & "   " & lista_depa_carta
                        End If

                    Else
                        conta_x = lista_depa.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_depa, 1, 60)
                            posi_x1 = Right(lista_depa, (lista_depa.Length - 60))
                            pTextElement.Text = "DEPARTAMENTO (S) : " & "   " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "DEPARTAMENTO (S) : " & "   " & lista_depa
                        End If
                    End If

                        pPoint.X = 19.1
                        pPoint.X = 19.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.4
                        posi_y_c1 = posi_y_c1 - 0.4

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "OBSERVACIONES"
                        pPoint.X = 19.1
                        'pPoint.Y = 9.13
                        'pEnv.LowerLeft = pPoint
                        pPoint.X = 19.1
                        'pPoint.Y = 8.9
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = "No existen Observaciones"
                        pPoint.X = 19.1

                        pPoint.X = 19.1
                        'pPoint.Y = 2.1
                        pPoint.Y = posi_y_c - 0.3
                        pPoint.Y = posi_y_c1 - 0.3

                        posi_y_c = posi_y_c - 0.3
                        posi_y_c1 = posi_y_c1 - 0.3
                        posi_y_m = posi_y_c
                        posi_y1_m = posi_y_c1

                        'ElseIf contatexto = 11 Then
                        '    pTextElement.Text = "ELIPSOIDE...............................................INTERNACIONAL"
                        '    pPoint.X = 1.958
                        '    pPoint.Y = 1.48
                        '    'pEnv.LowerLeft = pPoint
                        '    pPoint.X = 1.958
                        '    pPoint.Y = 1.48

                        'ElseIf contatexto = 12 Then
                        '    pTextElement.Text = "PROYECCION.................................TRANSVERSA DE MERCATOR"
                        '    pPoint.X = 1.958
                        '    pPoint.Y = 1.289
                        '    'pEnv.LowerLeft = pPoint
                        '    pPoint.X = 1.958
                        '    pPoint.Y = 1.289

                        'ElseIf contatexto = 13 Then
                        '    pTextElement.Text = "DATUM VERTICAL..............................................NIVEL MEDIO DEL MAR"
                        '    pPoint.X = 10.3
                        '    pPoint.Y = 1.48
                        '    'pEnv.LowerLeft = pPoint
                        '    pPoint.X = 10.3
                        '    pPoint.Y = 1.48

                        'ElseIf contatexto = 14 Then
                        '    pTextElement.Text = "DATUM HORIZONTAL.........DATUM PROVISIONAL SUDAMERICANO DE 1956"
                        '    pPoint.X = 10.3
                        '    pPoint.Y = 1.289
                        '    'pEnv.LowerLeft = pPoint
                        '    pPoint.X = 10.3
                        '    pPoint.Y = 1.289
                    ElseIf contatexto = 12 Then
                        pTextElement.Text = fecha
                        pPoint.X = 16.0
                        pPoint.Y = 2.9
                        pPoint.X = 19.7
                        pPoint.Y = 2.1
                    ElseIf contatexto = 13 Then
                        'pTextElement.Text = escala_plano_carta
                        'pPoint.X = 17.3
                        pPoint.X = 19.4
                        pPoint.Y = 2.9
                        'pPoint.X = 21.1
                        pPoint.X = 21.9
                        pPoint.Y = 2.1
                    End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                'Simbolo del texto
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'color del texto
                'Dim pColor As IRgbColor
                myColor = New RgbColor

                'If contatexto = 1 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'ElseIf ((contatexto > 10) And (contatexto < 15)) Then
                'pFont.Size = 5.5
                'ElseIf contatexto = 6 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'ElseIf contatexto = 10 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'Else
                'pFont.Size = 8
                'End If

                If contatexto = 1 Then
                    pFont.Size = 8
                    myColor.RGB = RGB(0, 0, 250)
                ElseIf ((contatexto > 10) And (contatexto < 15)) Then
                    pFont.Size = 5.5
                ElseIf contatexto = 6 Then
                    pFont.Size = 9
                    myColor.RGB = RGB(0, 0, 250)
                ElseIf contatexto = 10 Then
                    pFont.Size = 9
                    myColor.RGB = RGB(0, 0, 250)
                    'ElseIf contatexto = 11 Or contatexto = 12 Or contatexto = 13 Or contatexto = 14 Then
                    'pFont.Size = 3
                    'myColor.RGB = RGB(0, 0, 250)
                Else
                    pFont.Size = 6
                End If


                pFont.Bold = False
                pTxtSym.Font = pFont
                pTxtSym.Color = myColor

                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Plano_variado" Then
            If tipo_opcion = "1" Then
                lista_nmhojas = v_nombre
                lista_cartas = v_codigo
            End If
            Dim posi_y_c As Double
            Dim posi_y_c1 As Double

            If sele_plano1 = "Plano A4 Vertical" Then
                posi_y_c = 27.18
                posi_y_c1 = 27.08

            ElseIf sele_plano1 = "Plano A4 Horizontal" Then
                posi_y_c = 11.8
                posi_y_c1 = 11.5


                ' posi_y_c = 12.2
                ' posi_y_c1 = 11.9
            ElseIf sele_plano1 = "Plano A3 Vertical" Then
                posi_y_c = 37.65
                posi_y_c1 = 37.55
                'posi_y_c = 40.45
                'posi_y_c1 = 40.35
            ElseIf sele_plano1 = "Plano A3 Horizontal" Then
                posi_y_c = 17.05
                posi_y_c1 = 16.75

            ElseIf sele_plano1 = "Plano A2 Vertical" Then
                posi_y_c = 54.7
                posi_y_c1 = 54.5

            ElseIf sele_plano1 = "Plano A2 Horizontal" Then
                posi_y_c = 24.0
                posi_y_c1 = 23.9

            ElseIf sele_plano1 = "Plano A1 Vertical" Then
                posi_y_c = 74.8
                posi_y_c1 = 74.3
                'posi_y_c = 81.0
                'posi_y_c1 = 80.5

            ElseIf sele_plano1 = "Plano A1 Horizontal" Then
                posi_y_c = 33.95
                posi_y_c1 = 33.65
            ElseIf sele_plano1 = "Plano A0 Vertical" Then
                posi_y_c = 108.4
                posi_y_c1 = 107.4

            ElseIf sele_plano1 = "Plano A0 Horizontal" Then
                posi_y_c = 48.65
                posi_y_c1 = 47.65
            End If

            If sele_plano1 = "Plano A4 Horizontal" Then
                For contatexto = 1 To 15
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then      'exploracion
                        pTextElement.Text = v_explo1
                        '    pPoint.X = 24.92
                        '   pPoint.X = 26.0

                        pPoint.X = 25.0   '25.7
                        pPoint.X = 26.0   '26.7

                        pPoint.Y = 10.0
                        pPoint.Y = 12.0

                        'pPoint.Y = posi_y_c
                        'pPoint.Y = posi_y_c1


                        ' posi_y_c = posi_y_c + 0.6
                        ' posi_y_c1 = posi_y_c1 + 0.6


                        'posi_y_c = posi_y_c + 2  '0.55   '0.9
                        'posi_y_c1 = posi_y_c1 + 2  '0.55  '0.9

                    ElseIf contatexto = 2 Then      'explotacion
                        pTextElement.Text = v_expta1

                        pPoint.X = 25.0   '25.5
                        pPoint.X = 26.0   '26.6
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        'posi_y_c = posi_y_c - 1.1
                        'posi_y_c1 = posi_y_c1 - 1.1

                        pPoint.Y = 9.0
                        pPoint.Y = 11.0

                        'posi_y_c = posi_y_c - 2.8   '1.5
                        'posi_y_c1 = posi_y_c1 - 2.8  '1.5


                    ElseIf contatexto = 3 Then      'exploracion - explotacion
                        ' pTextElement.Text = v_explo_v_expta1
                        pTextElement.Text = "       "
                        pPoint.X = 24.92
                        pPoint.X = 26.0
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c + 0.1
                        posi_y_c1 = posi_y_c1 + 0.1

                    ElseIf contatexto = 4 Then      'cierre
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 37.02
                        pPoint.X = 38.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c + 1.5
                        posi_y_c1 = posi_y_c1 + 1.5

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 22.5  '22.5
                        pPoint.X = 23.1  '23.1
                        ' pPoint.Y = 2.75
                        'pPoint.Y = 2.55
                        pPoint.Y = 2.75   '2.5
                        pPoint.Y = 2.55   '2.3
                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 22.7   '22.8
                        pPoint.X = 23.3   '23.4
                        'pPoint.Y = 2.35
                        'pPoint.Y = 2.15

                        pPoint.Y = 2.35  '2.1
                        pPoint.Y = 2.15   '1.9

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 22.8   '22.8
                        pPoint.X = 23.4   '23.4
                        pPoint.Y = 1.95   '1.7
                        pPoint.Y = 1.75   '1.5

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 23.0   '22.8
                        pPoint.X = 23.6   '23.4
                        'pPoint.Y = 1.5
                        'pPoint.Y = 1.3
                        pPoint.Y = 1.45   '1.25
                        pPoint.Y = 1.25   '1.05
                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 25.4   '25.4
                        pPoint.X = 26.0   '26.0
                        ' pPoint.Y = 2.35
                        ' pPoint.Y = 2.15

                        pPoint.Y = 2.3   '2.1
                        pPoint.Y = 2.15   '1.95

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 25.4   '25.4
                        pPoint.X = 26.0   '26.0
                        'pPoint.Y = 1.5
                        'pPoint.Y = 1.3

                        pPoint.Y = 1.45   '1.25
                        pPoint.Y = 1.25   '1.05

                    ElseIf contatexto = 11 Then     'escala
                        pTextElement.Text = escalaf
                        'pPoint.X = 24.1
                        'pPoint.X = 24.8
                        'pPoint.Y = 11.35
                        'pPoint.Y = 13.15


                        pPoint.X = 24.1
                        pPoint.X = 24.8
                        pPoint.Y = 11.15   '11.55
                        pPoint.Y = 13.15   '13.55

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        'pPoint.X = 26.57
                        'pPoint.X = 27.55
                        'pPoint.Y = 6.6
                        'pPoint.Y = 8.4


                        pPoint.X = 26.5   '27.07
                        pPoint.X = 27.5   '28.05
                        pPoint.Y = 6.35    '6.9
                        pPoint.Y = 8.35   '8.9

                    ElseIf contatexto = 13 Then
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        'pPoint.X = 26.57
                        'pPoint.X = 27.55
                        'pPoint.Y = 7.3
                        'pPoint.Y = 9.1



                        pPoint.X = 26.5   '26.97
                        pPoint.X = 27.5   '27.95
                        pPoint.Y = 8.2   '7.9
                        pPoint.Y = 9.2  '9.7

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        'pPoint.X = 25.4
                        'pPoint.X = 26.4
                        'pPoint.Y = 6.35


                        pPoint.X = 26.4
                        pPoint.X = 27.4
                        pPoint.Y = 7.15
                        pPoint.Y = 8.15
                        'pPoint.Y = 7.35

                    ElseIf contatexto = 15 Then  'SISTEMA
                        pTextElement.Text = v_sistema
                        'pPoint.X = 25.4
                        'pPoint.X = 26.4
                        'pPoint.Y = 6.35


                        pPoint.X = 24.4   '26.04
                        pPoint.X = 26.4   '28.04
                        pPoint.Y = 2.75   '2.55
                        pPoint.Y = 2.55   '1.55
                        'pPoint.Y = 7.35

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 4.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 6

                    Else
                        pFont.Size = 5.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A4 Vertical" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 11.35
                        pPoint.X = 12.35
                        pPoint.Y = 27.0
                        pPoint.Y = 26.9

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 11.35
                        pPoint.X = 12.35
                        pPoint.Y = 26.45
                        pPoint.Y = 26.35

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 11.35
                        pPoint.X = 12.35
                        pPoint.Y = 25.95
                        pPoint.Y = 25.85

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 44.4
                        pPoint.X = 45.4
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.41
                        posi_y_c1 = posi_y_c1 - 0.41

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 14.05
                        pPoint.X = 15.05
                        pPoint.Y = 23.27
                        pPoint.Y = 23.17

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 14.4
                        pPoint.X = 15.5
                        pPoint.Y = 22.87
                        pPoint.Y = 22.77

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 14.4
                        pPoint.X = 15.5
                        pPoint.Y = 22.5
                        pPoint.Y = 22.4

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 14.4
                        pPoint.X = 15.5
                        pPoint.Y = 22.0
                        pPoint.Y = 21.9

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 17.5
                        pPoint.X = 18.1
                        pPoint.Y = 22.87
                        pPoint.Y = 22.77

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 17.5
                        pPoint.X = 18.1
                        pPoint.Y = 22.0
                        pPoint.Y = 21.9

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 14.8
                        pPoint.X = 15.4
                        pPoint.Y = 28.15
                        pPoint.Y = 28.05

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 19.03
                        pPoint.X = 19.63
                        pPoint.Y = 25.05
                        pPoint.Y = 26.85

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 19.03
                        pPoint.X = 19.63
                        pPoint.Y = 24.45
                        pPoint.Y = 26.25

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 17.7
                        pPoint.X = 18.7
                        pPoint.Y = 24.15
                        pPoint.Y = 25.15

                    End If

                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor

                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 4.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 6

                    Else
                        pFont.Size = 5.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A3 Horizontal" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 35.5
                        pPoint.X = 36.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 35.5
                        pPoint.X = 36.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 35.5
                        pPoint.X = 36.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1

                        pPoint.X = 48.8
                        pPoint.X = 49.8
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9


                    ElseIf contatexto = 5 Then      'HOJA
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 31.25
                        pPoint.X = 32.25
                        pPoint.Y = 3.6
                        pPoint.Y = 3.35

                    ElseIf contatexto = 6 Then      'CODIGO
                        pTextElement.Text = lista_cartas
                        pPoint.X = 31.75
                        pPoint.X = 32.75
                        pPoint.Y = 3.0
                        pPoint.Y = 2.78

                    ElseIf contatexto = 7 Then      'TAMA�O
                        pTextElement.Text = sele_plano2
                        pPoint.X = 31.75
                        pPoint.X = 32.75
                        pPoint.Y = 2.4
                        pPoint.Y = 2.15

                    ElseIf contatexto = 8 Then      'FECHA
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 31.75
                        pPoint.X = 32.75
                        pPoint.Y = 1.8
                        pPoint.Y = 1.55

                    ElseIf contatexto = 9 Then      'ZONA
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 35.95
                        pPoint.X = 36.55
                        pPoint.Y = 3.0
                        pPoint.Y = 2.78
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 35.95
                        pPoint.X = 36.55
                        pPoint.Y = 1.8
                        pPoint.Y = 1.55

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 35.35
                        pPoint.X = 35.05
                        pPoint.Y = 19.35
                        pPoint.Y = 18.35

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 37.07
                        pPoint.X = 38.67
                        pPoint.Y = 10.85
                        pPoint.Y = 12.65

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 37.07
                        pPoint.X = 38.67
                        pPoint.Y = 9.9
                        pPoint.Y = 11.7

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 35.05
                        pPoint.X = 36.55
                        pPoint.Y = 8.375
                        pPoint.Y = 10.175

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 5.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 7.5

                    Else
                        pFont.Size = 6.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A3 Vertical" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 17.07
                        pPoint.X = 18.07
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.6
                        posi_y_c1 = posi_y_c1 - 0.6

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 17.07
                        pPoint.X = 18.07
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.65
                        posi_y_c1 = posi_y_c1 - 0.65

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 17.07
                        pPoint.X = 18.07
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.65
                        posi_y_c1 = posi_y_c1 - 0.65

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 37.92
                        pPoint.X = 38.92
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9

                    ElseIf contatexto = 5 Then      'HOJA
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 21.8
                        pPoint.X = 22.8
                        pPoint.Y = 32.85
                        pPoint.Y = 32.65

                    ElseIf contatexto = 6 Then      'CODIGO
                        pTextElement.Text = lista_cartas
                        pPoint.X = 22.1
                        pPoint.X = 23.1
                        pPoint.Y = 32.35
                        pPoint.Y = 32.15

                    ElseIf contatexto = 7 Then      'TAMA�O
                        pTextElement.Text = sele_plano2
                        pPoint.X = 22.1
                        pPoint.X = 23.1
                        pPoint.Y = 31.85
                        pPoint.Y = 31.65

                    ElseIf contatexto = 8 Then      'FECHA
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 22.1
                        pPoint.X = 23.1
                        pPoint.Y = 32.35
                        pPoint.Y = 31.15

                    ElseIf contatexto = 9 Then      'ZONA
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 25.2
                        pPoint.X = 26.2
                        pPoint.Y = 32.35
                        pPoint.Y = 32.15
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 25.2
                        pPoint.X = 26.2
                        pPoint.Y = 31.35
                        pPoint.Y = 31.15
                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 22.35
                        pPoint.X = 23.05
                        pPoint.Y = 44.05
                        pPoint.Y = 39.05

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 26.47
                        pPoint.X = 28.07
                        pPoint.Y = 35.8
                        pPoint.Y = 37.6

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 26.47
                        pPoint.X = 28.07
                        pPoint.Y = 34.95
                        pPoint.Y = 36.75

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 25.15
                        pPoint.X = 26.65
                        pPoint.Y = 33.675
                        pPoint.Y = 35.475

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 5.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 7.5
                    Else
                        pFont.Size = 6.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A2 Horizontal" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 51.35
                        pPoint.X = 52.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.15
                        posi_y_c1 = posi_y_c1 - 1.15

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 51.35
                        pPoint.X = 52.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 51.35
                        pPoint.X = 52.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.15
                        posi_y_c1 = posi_y_c1 - 1.15

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 73.25
                        pPoint.X = 74.25
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.4
                        posi_y_c1 = posi_y_c1 - 1.4

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 45.4
                        pPoint.X = 46.4
                        pPoint.Y = 4.9
                        pPoint.Y = 4.5

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 46.3
                        pPoint.X = 47.3
                        pPoint.Y = 4.0
                        pPoint.Y = 3.6

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 46.3
                        pPoint.X = 47.3
                        pPoint.Y = 3.2
                        pPoint.Y = 2.8

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 46.3
                        pPoint.X = 47.3
                        pPoint.Y = 2.3
                        pPoint.Y = 1.9

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 51.3
                        pPoint.X = 52.3
                        pPoint.Y = 4.0
                        pPoint.Y = 3.6

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 51.3
                        pPoint.X = 52.3
                        pPoint.Y = 2.3
                        pPoint.Y = 1.9

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 48.6
                        pPoint.X = 49.6
                        pPoint.Y = 27.05
                        pPoint.Y = 26.05

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 53.87
                        pPoint.X = 55.47
                        pPoint.Y = 16.2
                        pPoint.Y = 18.0

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 53.87
                        pPoint.X = 55.47
                        pPoint.Y = 14.8
                        pPoint.Y = 16.6

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 51.45
                        pPoint.X = 53.0
                        pPoint.Y = 12.6
                        pPoint.Y = 14.4

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 9
                    ElseIf contatexto = 11 Then
                        pFont.Size = 13

                    Else
                        pFont.Size = 10
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A2 Vertical" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 24.7
                        pPoint.X = 25.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 24.7
                        pPoint.X = 25.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 24.7
                        pPoint.X = 25.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 44.7
                        pPoint.X = 45.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 30.3
                        pPoint.X = 31.3
                        pPoint.Y = 48.0
                        pPoint.Y = 46.7

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 46.2
                        pPoint.Y = 45.9

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 45.4
                        pPoint.Y = 45.1

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 44.6
                        pPoint.Y = 44.3

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm

                        pPoint.X = 35.95
                        pPoint.X = 36.95
                        pPoint.Y = 46.2
                        pPoint.Y = 45.9
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 35.95
                        pPoint.X = 36.95
                        pPoint.Y = 44.6
                        pPoint.Y = 44.3
                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 30.9
                        pPoint.X = 31.9
                        pPoint.Y = 56.3
                        pPoint.Y = 55.9

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 38.4
                        pPoint.X = 40.0
                        pPoint.Y = 51.8
                        pPoint.Y = 53.6

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 38.4
                        pPoint.X = 40.0
                        pPoint.Y = 50.6
                        pPoint.Y = 52.4

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 36.2
                        pPoint.X = 37.8
                        pPoint.Y = 48.38
                        pPoint.Y = 50.18

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 9
                    ElseIf contatexto = 11 Then
                        pFont.Size = 13

                    Else
                        pFont.Size = 10
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A1 Horizontal" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 73.05
                        pPoint.X = 74.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.5
                        posi_y_c1 = posi_y_c1 - 1.5

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 73.05
                        pPoint.X = 74.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.5
                        posi_y_c1 = posi_y_c1 - 1.5

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 73.05
                        pPoint.X = 74.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.7
                        posi_y_c1 = posi_y_c1 - 1.7

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 93.05
                        pPoint.X = 94.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.9
                        posi_y_c1 = posi_y_c1 - 1.9

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 64.0
                        pPoint.X = 65.3
                        pPoint.Y = 6.3
                        pPoint.Y = 5.6

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 64.7
                        pPoint.X = 65.9
                        pPoint.Y = 5.1
                        pPoint.Y = 4.4

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 64.7
                        pPoint.X = 65.9
                        pPoint.Y = 3.9
                        pPoint.Y = 3.2

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 64.7
                        pPoint.X = 65.9
                        pPoint.Y = 2.6
                        pPoint.Y = 1.9

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 73.1
                        pPoint.X = 74.1
                        pPoint.Y = 5.1
                        pPoint.Y = 4.4

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 73.1
                        pPoint.X = 74.1
                        pPoint.Y = 2.6
                        pPoint.Y = 1.9

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 70.1
                        pPoint.X = 71.1
                        pPoint.Y = 37.85
                        pPoint.Y = 36.85

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 77.0
                        pPoint.X = 78.6
                        pPoint.Y = 23.7
                        pPoint.Y = 25.5

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 77.0
                        pPoint.X = 78.6
                        pPoint.Y = 21.8
                        pPoint.Y = 23.6

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 73.6
                        pPoint.X = 75.2
                        pPoint.Y = 18.5
                        pPoint.Y = 20.3

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 12.3
                    ElseIf contatexto = 11 Then
                        pFont.Size = 16

                    Else
                        pFont.Size = 15
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A1 Vertical" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 35.8
                        pPoint.X = 36.8
                        pPoint.Y = 76.9
                        pPoint.Y = 76.4

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 35.8
                        pPoint.X = 36.8
                        pPoint.Y = 75.5
                        pPoint.Y = 75.0

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 35.8
                        pPoint.X = 36.8
                        pPoint.Y = 74.0
                        pPoint.Y = 73.5

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 75.8
                        pPoint.X = 76.8
                        pPoint.Y = 72.6
                        pPoint.Y = 72.1

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 42.9
                        pPoint.X = 44.1
                        pPoint.Y = 66.6
                        pPoint.Y = 66.1

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 43.9
                        pPoint.X = 44.9
                        pPoint.Y = 65.5
                        pPoint.Y = 65.0

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2

                        pPoint.X = 43.9
                        pPoint.X = 44.9
                        pPoint.Y = 64.4
                        pPoint.Y = 63.9

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 43.9
                        pPoint.X = 44.9
                        pPoint.Y = 63.2
                        pPoint.Y = 62.7

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zona_dm

                        pPoint.X = 51.5
                        pPoint.X = 52.5
                        pPoint.Y = 65.5
                        pPoint.Y = 65.0
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 51.5
                        pPoint.X = 52.5
                        pPoint.Y = 63.2
                        pPoint.Y = 62.7

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 44.1
                        pPoint.X = 45.1
                        pPoint.Y = 80.9
                        pPoint.Y = 79.9

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 55.1
                        pPoint.X = 56.7
                        pPoint.Y = 74.8
                        pPoint.Y = 76.6

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 55.1
                        pPoint.X = 56.7
                        pPoint.Y = 72.9
                        pPoint.Y = 74.7

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 52.4
                        pPoint.X = 54.0
                        pPoint.Y = 69.9
                        pPoint.Y = 71.7

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 12.3
                    ElseIf contatexto = 11 Then
                        pFont.Size = 16

                    Else
                        pFont.Size = 15
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A0 Horizontal" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 103.75
                        pPoint.X = 104.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 103.75
                        pPoint.X = 104.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 103.75
                        pPoint.X = 104.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 143.75
                        pPoint.X = 144.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 91.7
                        pPoint.X = 92.7
                        pPoint.Y = 15.4
                        pPoint.Y = 9.4

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 92.7
                        pPoint.X = 93.9
                        pPoint.Y = 8.7
                        pPoint.Y = 7.7

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 92.7
                        pPoint.X = 93.9
                        pPoint.Y = 7.1
                        pPoint.Y = 6.1

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 92.7
                        pPoint.X = 93.9
                        pPoint.Y = 5.15
                        pPoint.Y = 4.45

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 103.7
                        pPoint.X = 104.7
                        pPoint.Y = 8.85
                        pPoint.Y = 7.85

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 103.7
                        pPoint.X = 104.7
                        pPoint.Y = 5.15
                        pPoint.Y = 4.45

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 97.25
                        pPoint.X = 98.45
                        pPoint.Y = 53.1
                        pPoint.Y = 52.1

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 109.25
                        pPoint.X = 110.85
                        pPoint.Y = 34.15
                        pPoint.Y = 35.95

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 109.25
                        pPoint.X = 110.85
                        pPoint.Y = 31.6
                        pPoint.Y = 33.4

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 104.75
                        pPoint.X = 106.35
                        pPoint.Y = 27.1
                        pPoint.Y = 28.9

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 17
                    ElseIf contatexto = 11 Then
                        pFont.Size = 22.5
                    Else
                        pFont.Size = 21
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A0 Vertical" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 52.4
                        pPoint.X = 52.6
                        pPoint.Y = 108.95
                        pPoint.Y = 107.95

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 52.4
                        pPoint.X = 52.6
                        pPoint.Y = 106.9
                        pPoint.Y = 105.9

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 52.4
                        pPoint.X = 52.6
                        pPoint.Y = 104.9
                        pPoint.Y = 103.9

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 92.3
                        pPoint.X = 92.5
                        pPoint.Y = 98.8
                        pPoint.Y = 97.8

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 61.2
                        pPoint.X = 62.4
                        pPoint.Y = 95.2
                        pPoint.Y = 94.2

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 62.8
                        pPoint.X = 63.8
                        pPoint.Y = 93.6
                        pPoint.Y = 92.6

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 62.8
                        pPoint.X = 63.8
                        pPoint.Y = 92.0
                        pPoint.Y = 91.0
                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 62.8
                        pPoint.X = 63.8
                        pPoint.Y = 90.25
                        pPoint.Y = 89.25

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 72.3
                        pPoint.X = 74.3
                        pPoint.Y = 93.6
                        pPoint.Y = 92.6
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 72.3
                        pPoint.X = 74.3
                        pPoint.Y = 90.25
                        pPoint.Y = 89.25

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 62.9
                        pPoint.X = 63.9
                        pPoint.Y = 114.1
                        pPoint.Y = 113.1
                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 79.0
                        pPoint.X = 80.6
                        pPoint.Y = 109.3
                        pPoint.Y = 108.3

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 79.0
                        pPoint.X = 80.6
                        pPoint.Y = 106.8
                        pPoint.Y = 105.8

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 74.5
                        pPoint.X = 76.1
                        pPoint.Y = 102.3
                        pPoint.Y = 101.3

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol
                    'fuente del texto
                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto
                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 17
                    ElseIf contatexto = 11 Then
                        pFont.Size = 22.5
                    Else
                        pFont.Size = 21
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            End If
        ElseIf sele_reporte = "Demarca" Then
            For contatexto = 1 To 8
                'Guardar el texto
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    conta_x = v_nombre_dm.Length
                    If conta_x >= 38 Then
                        posi_x = Mid(v_nombre_dm, 1, 38)
                        posi_x1 = Right(v_nombre_dm, (v_nombre_dm.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = v_nombre_dm
                    End If
                    pPoint.X = 17.1
                    'pPoint.Y = 8.1
                    pPoint.Y = 9.1
                    pPoint.X = 21.7
                    'pPoint.Y = 8.9
                    pPoint.Y = 9.9
                ElseIf contatexto = 2 Then
                    pTextElement.Text = v_codigo
                    pPoint.X = 17.9
                    'pPoint.Y = 7.5
                    pPoint.Y = 8.2
                    pPoint.X = 21.7
                    'pPoint.Y = 8.3
                    pPoint.Y = 9.2
                ElseIf contatexto = 3 Then
                    'posi_x = ""
                    'posi_x1 = ""
                    'pTextElement.Text = lista_dist
                    If caso_consulta1 = "TIPO DEMARCACION POLITICA" Then
                        conta_x = lista_dist_dema.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_dist_dema, 1, 38)
                            posi_x1 = Right(lista_dist_dema, (lista_dist_dema.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_dist_dema
                        End If
                    Else
                        conta_x = lista_dist.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_dist, 1, 38)
                            posi_x1 = Right(lista_dist, (lista_dist.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_dist
                        End If
                    End If

                    pPoint.X = 17.9
                    'pPoint.Y = 6.9
                    pPoint.Y = 7.6
                    pPoint.X = 21.7
                    'pPoint.Y = 7.7
                    pPoint.Y = 8.4

                    ElseIf contatexto = 4 Then
                    'pTextElement.Text = lista_prov
                    If caso_consulta1 = "TIPO DEMARCACION POLITICA" Then
                        conta_x = lista_prov_dema.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_prov_dema, 1, 38)
                            posi_x1 = Right(lista_prov_dema, (lista_prov_dema.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_prov_dema
                        End If
                    Else
                        conta_x = lista_prov.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_prov, 1, 38)
                            posi_x1 = Right(lista_prov, (lista_prov.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_prov
                        End If
                    End If

                    pPoint.X = 17.9
                    'pPoint.Y = 6.3
                    pPoint.Y = 6.8
                    pPoint.X = 21.7
                    'pPoint.Y = 7.1
                    pPoint.Y = 7.6
                    ElseIf contatexto = 5 Then
                    'pTextElement.Text = lista_depa
                    If caso_consulta1 = "TIPO DEMARCACION POLITICA" Then
                        conta_x = lista_depa_dema.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_depa_dema, 1, 38)
                            posi_x1 = Right(lista_depa_dema, (lista_depa_dema.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_depa_dema
                        End If
                    Else
                        conta_x = lista_depa.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_depa, 1, 38)
                            posi_x1 = Right(lista_depa, (lista_depa.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_depa
                        End If
                    End If

                    pPoint.X = 17.9
                    'pPoint.Y = 5.7
                    pPoint.Y = 5.7
                    pPoint.X = 21.7
                    'pPoint.Y = 6.5
                    pPoint.Y = 6.7
                    ElseIf contatexto = 6 Then
                        pTextElement.Text = fecha
                        pPoint.X = 16.1
                        pPoint.Y = 2.9
                        pPoint.X = 19.8
                        pPoint.Y = 2.1
                    ElseIf contatexto = 7 Then
                        'pTextElement.Text = escala_plano_dema
                        'pPoint.X = 17.3
                        pPoint.X = 18.3
                        pPoint.Y = 2.9
                        'pPoint.X = 21.1
                        pPoint.X = 22.1
                        pPoint.Y = 2.1

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 5.6
                        pPoint.Y = 2.12
                        pPoint.X = 9.6
                    pPoint.Y = 1.4  '1.32
                    End If

                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv

                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'If contatexto = 6 Then
                    pFont.Size = 7
                    'ElseIf contatexto = 7 Then
                    'pFont.Size = 6
                    'ElseIf contatexto = 8 Then
                    'pFont.Size = 6
                    'Else
                    'pFont.Size = 7
                    'End If
                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    'Propiedades del Simbolo
                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False

                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)
                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto

        ElseIf sele_reporte = "Area Restringida" Then
            For contatexto = 1 To 8
                'Guardar el texto
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point

                If contatexto = 1 Then
                    'Para listar nombre de departamentos
                    conta_x = lista_cadena_depa.Length
                    If conta_x > 38 Then
                        posi_x = Mid(lista_cadena_depa, 1, 38)
                        posi_x1 = Right(lista_cadena_depa, (lista_cadena_depa.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = lista_cadena_depa
                    End If



                    pPoint.X = 16.9
                    pPoint.Y = 10.1
                    pPoint.X = 21.7
                    pPoint.Y = 10.7

                ElseIf contatexto = 2 Then
                    'Para listar nombre de Provincias

                    conta_x = lista_cadena_prov.Length
                    If conta_x > 38 Then
                        posi_x = Mid(lista_cadena_prov, 1, 38)
                        posi_x1 = Right(lista_cadena_prov, (lista_cadena_prov.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = lista_cadena_prov
                    End If


                    'pPoint.X = 17.9
                    ''pPoint.Y = 6.3
                    'pPoint.Y = 6.8
                    'pPoint.X = 21.7
                    ''pPoint.Y = 7.1
                    'pPoint.Y = 7.6

                    pPoint.X = 16.9
                    'pPoint.Y = 6.3
                    pPoint.Y = 9.1
                    pPoint.X = 21.7
                    'pPoint.Y = 7.1
                    pPoint.Y = 9.9
                ElseIf contatexto = 3 Then
                    'Para listar nombre de distritos

                    conta_x = lista_cadena_dist.Length
                    If conta_x > 38 Then
                        posi_x = Mid(lista_cadena_dist, 1, 38)
                        posi_x1 = Right(lista_cadena_dist, (lista_cadena_dist.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = lista_cadena_dist
                    End If



                    'pPoint.X = 17.9
                    ''pPoint.Y = 5.7
                    'pPoint.Y = 5.7
                    'pPoint.X = 21.7
                    ''pPoint.Y = 6.5
                    'pPoint.Y = 6.7



                    pPoint.X = 16.9
                    'pPoint.Y = 5.7
                    pPoint.Y = 8.2
                    pPoint.X = 21.7
                    'pPoint.Y = 6.5
                    pPoint.Y = 9.2

                ElseIf contatexto = 4 Then
                    pTextElement.Text = fecha
                    pPoint.X = 21.9
                    pPoint.Y = 2.6
                    pPoint.X = 24.9
                    pPoint.Y = 1.95
                ElseIf contatexto = 5 Then
                    pTextElement.Text = escalaf
                    'pPoint.X = 17.3
                    pPoint.X = 22.1
                    pPoint.Y = 4.0
                    'pPoint.X = 21.1
                    pPoint.X = 24.9
                    pPoint.Y = 3.0

                ElseIf contatexto = 6 Then
                    pTextElement.Text = v_sistema
                    'pPoint.X = 17.3
                    pPoint.X = 17.5
                    pPoint.Y = 4.0
                    'pPoint.X = 21.1
                    pPoint.X = 21.5
                    pPoint.Y = 3.0


                ElseIf contatexto = 7 Then
                    pTextElement.Text = v_zona_dm & "S"
                    pPoint.X = 17.5
                    pPoint.Y = 3.1
                    pPoint.X = 21.5
                    pPoint.Y = 2.3
                ElseIf contatexto = 8 Then
                    If v_opcion_Rese_sele = "AN" Then
                        'pTextElement.Text = "AREA RESERVADA " & v_nom_rese_sele
                        pTextElement.Text = TIP_RESE_PLANO & "   " & v_nom_rese_sele
                    ElseIf v_opcion_Rese_sele = "ZU" Then
                        pTextElement.Text = "AREA URBANA   " & v_nom_rese_sele
                    End If

                    ' pTextElement.Text = v_zona_dm
                    pPoint.X = 17.8
                    pPoint.Y = 4.5
                    pPoint.X = 21.8
                    pPoint.Y = 3.7


                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv

                'Simbolo del texto
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                If contatexto = 4 Or contatexto = 5 Or contatexto = 6 Or contatexto = 7 Then
                    pFont.Size = 4
                Else
                    pFont.Size = 7
                End If
                ' pFont.Size = 7
                'ElseIf contatexto = 7 Then
                'pFont.Size = 6
                'ElseIf contatexto = 8 Then
                'pFont.Size = 6
                'Else
                'pFont.Size = 7
                'End If
                pFont.Bold = False
                pTxtSym.Font = pFont
                'Propiedades del Simbolo
                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False

                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Plano Venta" Then  'Reporte para plano Venta
            If sele_plano = "Formato A4" Then
                'posi_y1 = 4.15
                'posi_y = 4.85
                'posi_y2 = 4.15
                'posi_y3 = 4.85


                posi_y1 = 4.1
                posi_y = 4.8
                posi_y2 = 4.1
                posi_y3 = 4.8
            ElseIf sele_plano = "Formato A3" Then
                'posi_y1 = 4.9
                'posi_y = 5.6
                'posi_y2 = 4.9
                'posi_y3 = 5.6

                posi_y1 = 4.2
                posi_y = 4.9
                posi_y2 = 4.2
                posi_y3 = 4.9

            ElseIf sele_plano = "Formato A2" Then
                posi_y1 = 3.6
                posi_y = 4.3
                posi_y2 = 3.6
                posi_y3 = 4.3
            ElseIf sele_plano = "Formato A0" Then
                posi_y1 = 3.4
                posi_y = 4.1
                posi_y2 = 4.4
                posi_y3 = 4.1
            End If
            For contatexto = 1 To 9 'contador de textos
                'For contatexto = 1 To 15 'contador de textos
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "HOJA : " & valor_codhoja  ' cabecera hoja

                    If sele_plano = "Formato A4" Then
                        pPoint.X = 15.5
                        pPoint.Y = 25.55
                        pPoint.X = 17.5
                        pPoint.Y = 26.55
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 23.5
                        pPoint.Y = 36.4
                        pPoint.X = 25.5
                        pPoint.Y = 38.4
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 35.5
                        pPoint.Y = 51.0
                        pPoint.X = 36.4
                        pPoint.Y = 53.6
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 71.5
                        pPoint.Y = 106.6
                        pPoint.X = 73.0
                        pPoint.Y = 108.4
                    End If

                ElseIf contatexto = 2 Then
                    ' pTextElement.Text = valor_nmhoja  'nombre hoja
                    pTextElement.Text = valor_nmhoja & "-" & valor_codhoja  ' cabecera hoja
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.2
                        pPoint.Y = posi_y1 - 0.5
                        pPoint.X = 12.9
                        pPoint.Y = posi_y - 0.5
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.7
                        pPoint.Y = posi_y1 - 0.5
                        pPoint.X = 18.7
                        pPoint.Y = posi_y - 0.5
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.5
                        pPoint.Y = posi_y1 + 3.05
                        pPoint.X = 26.6
                        pPoint.Y = posi_y + 3.05
                        posi_y = posi_y - 0.6
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 50.5
                        pPoint.Y = posi_y1 + 10.9
                        pPoint.X = 53.6
                        pPoint.Y = posi_y + 10.9
                        posi_y = posi_y - 0.5
                    End If


                    'If sele_plano = "Formato A4" Then
                    '    pPoint.X = 8.8
                    '    pPoint.Y = posi_y1 - 0.42
                    '    pPoint.X = 13.5
                    '    pPoint.Y = posi_y - 0.42
                    '    posi_y = posi_y - 0.5
                    'ElseIf sele_plano = "Formato A3" Then
                    '    pPoint.X = 17.5
                    '    pPoint.Y = posi_y1 - 0.4
                    '    pPoint.X = 19.5
                    '    pPoint.Y = posi_y - 0.4
                    '    posi_y = posi_y - 0.7
                    'ElseIf sele_plano = "Formato A2" Then
                    '    pPoint.X = 24.5
                    '    pPoint.Y = posi_y1 + 2.8
                    '    pPoint.X = 27.6
                    '    pPoint.Y = posi_y + 2.8
                    '    posi_y = posi_y - 0.5
                    'ElseIf sele_plano = "Formato A0" Then
                    '    pPoint.X = 52.8
                    '    pPoint.Y = posi_y1 + 9.7
                    '    pPoint.X = 55.9
                    '    pPoint.Y = posi_y + 9.7
                    '    posi_y = posi_y - 0.5
                    'End If


                ElseIf contatexto = 3 Then
                    '  pTextElement.Text = valor_codhoja  'codigo hoja
                    pTextElement.Text = escalaf
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.42
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.42
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.4
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.4
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.8
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.8
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 9.7
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 9.7
                        posi_y = posi_y - 0.5
                    End If

                    'ElseIf contatexto = 4 Then
                    '    pTextElement.Text = fecha   'fecha
                    '    If sele_plano = "Formato A4" Then  ' tama�o
                    '        ' pTextElement.Text = "A4"
                    '        pTextElement.Text = escalaf
                    '        pPoint.X = 8.8
                    '        pPoint.Y = posi_y1 - 0.33
                    '        pPoint.X = 13.5
                    '        pPoint.Y = posi_y - 0.33
                    '        posi_y = posi_y - 0.5
                    '    ElseIf sele_plano = "Formato A3" Then
                    '        'pTextElement.Text = "A3"
                    '        pPoint.X = 17.5
                    '        pPoint.Y = posi_y1 - 0.35
                    '        pPoint.X = 19.5
                    '        pPoint.Y = posi_y - 0.35
                    '        posi_y = posi_y - 0.7
                    '    ElseIf sele_plano = "Formato A2" Then
                    '        ' pTextElement.Text = "A2"
                    '        pPoint.X = 24.5
                    '        pPoint.Y = posi_y1 + 2.4
                    '        pPoint.X = 27.6
                    '        pPoint.Y = posi_y + 2.4
                    '        posi_y = posi_y - 0.5
                    '    ElseIf sele_plano = "Formato A0" Then
                    '        ' pTextElement.Text = "A0"
                    '        pPoint.X = 52.8
                    '        pPoint.Y = posi_y1 + 8.35
                    '        pPoint.X = 55.9
                    '        pPoint.Y = posi_y + 8.35
                    '        posi_y = posi_y - 0.5
                    '    End If

                ElseIf contatexto = 4 Then
                    pTextElement.Text = fecha   'fecha
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.29
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.29
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.3
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.3
                        posi_y = posi_y - 0.8
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.05
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.05
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 7.1
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 7.1
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 5 Then
                    '  ElseIf contatexto = 6 Then
                    'pTextElement.Text = "PSAD-56"       'datum
                    pTextElement.Text = v_sistema
                    If sele_plano = "Formato A4" Then
                        ' pPoint.X = 14.5
                        pPoint.X = 13.7
                        pPoint.Y = posi_y2 - 0.5
                        ' pPoint.X = 16.2
                        pPoint.X = 15.4
                        pPoint.Y = posi_y3 - 0.5
                        posi_y3 = posi_y3 - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        ' pPoint.X = 22.0
                        pPoint.X = 21.3
                        pPoint.Y = posi_y2 - 0.5
                        'pPoint.X = 23.5
                        pPoint.X = 22.8
                        pPoint.Y = posi_y3 - 0.5
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 3.1
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 3.1
                        posi_y3 = posi_y3 - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 11.0
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 11.0
                        posi_y3 = posi_y3 - 0.5
                    End If

                    ' ElseIf contatexto = 7 Then
                ElseIf contatexto = 6 Then
                    pTextElement.Text = v_zona_dm   'zona
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 13.7
                        pPoint.Y = posi_y2 - 0.42
                        pPoint.X = 15.4
                        pPoint.Y = posi_y3 - 0.42
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A3" Then
                        'pPoint.X = 22.0
                        pPoint.X = 21.3
                        pPoint.Y = posi_y2 - 0.4
                        ' pPoint.X = 23.5
                        pPoint.X = 22.8
                        pPoint.Y = posi_y3 - 0.4
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 2.7
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 2.7
                        posi_y3 = posi_y3 - 0.53

                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 9.8
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 9.8
                        posi_y3 = posi_y3 - 0.53
                    End If

                    ' ElseIf contatexto = 8 Then
                ElseIf contatexto = 7 Then
                    'pTextElement.Text = valor_zoncat   'zona catastral
                    pTextElement.Text = "GUIDO VALDIVIA PONCE"   'autor

                    If sele_plano = "Formato A4" Then
                        pPoint.X = 13.2
                        pPoint.Y = posi_y2 - 0.33
                        pPoint.X = 15.0
                        pPoint.Y = posi_y3 - 0.33
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 20.4
                        pPoint.Y = posi_y2 - 0.35
                        pPoint.X = 21.8
                        pPoint.Y = posi_y3 - 0.35
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 2.3
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 2.3
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 8.4
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 8.4
                        posi_y3 = posi_y3 - 0.53
                    End If

                    'ElseIf contatexto = 9 Then
                    '    pTextElement.Text = "GUIDO VALDIVIA PONCE"   'autor
                    '    If sele_plano = "Formato A4" Then
                    '        pPoint.X = 13.3
                    '        pPoint.Y = posi_y2 - 0.25
                    '        pPoint.X = 15.0
                    '        pPoint.Y = posi_y3 - 0.25
                    '    ElseIf sele_plano = "Formato A3" Then
                    '        pPoint.X = 20.5
                    '        pPoint.Y = posi_y2 - 0.4
                    '        pPoint.X = 21.9
                    '        pPoint.Y = posi_y3 - 0.4
                    '    ElseIf sele_plano = "Formato A2" Then
                    '        pPoint.X = 29.9
                    '        pPoint.Y = posi_y2 + 2.0
                    '        pPoint.X = 30.9
                    '        pPoint.Y = posi_y3 + 2.0
                    '    ElseIf sele_plano = "Formato A0" Then
                    '        pPoint.X = 61.0
                    '        pPoint.Y = posi_y2 + 7.1
                    '        pPoint.X = 62.3
                    '        pPoint.Y = posi_y3 + 7.1
                    '    End If

                    'ElseIf contatexto = 10 Then  'DM Uso MInero
                    '    If canti_capa_actmin > 0 Then
                    '        pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                    '    Else
                    '        pTextElement.Text = "0"
                    '    End If
                    '    If sele_plano = "Formato A4" Then
                    '        pPoint.X = 10.4
                    '        pPoint.Y = posi_y2 + 1.2
                    '        pPoint.X = 12.3
                    '        pPoint.Y = posi_y3 + 1.2
                    '    ElseIf sele_plano = "Formato A3" Then
                    '        pPoint.X = 16.25
                    '        pPoint.Y = posi_y2 + 1.8
                    '        pPoint.X = 17.65
                    '        pPoint.Y = posi_y3 + 1.8
                    '    ElseIf sele_plano = "Formato A2" Then
                    '        pPoint.X = 23.1
                    '        pPoint.Y = posi_y2 + 4.9
                    '        pPoint.X = 25.1
                    '        pPoint.Y = posi_y3 + 4.9
                    '    ElseIf sele_plano = "Formato A0" Then
                    '        pPoint.X = 49.0
                    '        pPoint.Y = posi_y2 + 13.1
                    '        pPoint.X = 50.3
                    '        pPoint.Y = posi_y3 + 13.1
                    '    End If

                    'ElseIf contatexto = 11 Then  'DM Actividad Minera
                    '    If canti_capa_usomin > 0 Then
                    '        pTextElement.Text = canti_capa_usomin  'contador Capa Uso mi
                    '    Else
                    '        pTextElement.Text = "0"
                    '    End If
                    '    If sele_plano = "Formato A4" Then
                    '        pPoint.X = 10.4
                    '        pPoint.Y = posi_y2 + 0.67
                    '        pPoint.X = 12.3
                    '        pPoint.Y = posi_y3 + 0.67
                    '    ElseIf sele_plano = "Formato A3" Then
                    '        pPoint.X = 16.25
                    '        pPoint.Y = posi_y2 + 1.0
                    '        pPoint.X = 17.65
                    '        pPoint.Y = posi_y3 + 1.0
                    '    ElseIf sele_plano = "Formato A2" Then
                    '        pPoint.X = 23.1
                    '        pPoint.Y = posi_y2 + 3.8
                    '        pPoint.X = 25.1
                    '        pPoint.Y = posi_y3 + 3.8
                    '    ElseIf sele_plano = "Formato A0" Then
                    '        pPoint.X = 49.0
                    '        pPoint.Y = posi_y2 + 10.9
                    '        pPoint.X = 50.3
                    '        pPoint.Y = posi_y3 + 10.9
                    ' End If

                ElseIf contatexto = 8 Then  'Exploracion
                    If v_explo1 > 0 Then
                        pTextElement.Text = v_explo1  'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        '' pPoint.X = 5.9
                        'pPoint.X = 5.9
                        'pPoint.Y = posi_y2 + 1.12
                        'pPoint.X = 8.0
                        ''pPoint.X = 7.9
                        'pPoint.Y = posi_y3 + 1.12
                        'posi_y3 = posi_y3 - 0.45



                        If seleccion_plano_si = "Plano Catastral Situacion DM" Then

                            pPoint.X = 6.92
                            pPoint.Y = posi_y2 + 1.12
                            pPoint.X = 9.0
                            'pPoint.X = 7.9
                            pPoint.Y = posi_y3 + 1.12
                            posi_y3 = posi_y3 - 0.45
                        Else
                            ' pPoint.X = 5.9
                            pPoint.X = 5.9
                            pPoint.Y = posi_y2 + 1.12
                            pPoint.X = 8.0
                            'pPoint.X = 7.9
                            pPoint.Y = posi_y3 + 1.12
                            posi_y3 = posi_y3 - 0.45
                        End If



                    ElseIf sele_plano = "Formato A3" Then
                        If seleccion_plano_si = "Plano Catastral Situacion DM" Then
                            pPoint.X = 9.78
                            pPoint.Y = posi_y2 + 1.68
                            pPoint.X = 11.18
                            pPoint.Y = posi_y3 + 1.68
                        Else
                            pPoint.X = 9.65
                            pPoint.Y = posi_y2 + 1.6
                            pPoint.X = 11.05
                            pPoint.Y = posi_y3 + 1.6
                        End If

                    ElseIf sele_plano = "Formato A2" Then
                        If seleccion_plano_si = "Plano Catastral Situacion DM" Then
                            pPoint.X = 16.3
                            '   pPoint.Y = posi_y2 + 4.7
                            pPoint.Y = posi_y2 + 3.9
                            pPoint.X = 17.3
                            ' pPoint.Y = posi_y3 + 4.7
                            pPoint.Y = posi_y3 + 3.9
                        Else
                            pPoint.X = 14.1
                            pPoint.Y = posi_y2 + 4.7
                            pPoint.X = 16.1
                            pPoint.Y = posi_y3 + 4.7
                        End If

                    ElseIf sele_plano = "Formato A0" Then
                        If seleccion_plano_si = "Plano Catastral Situacion DM" Then
                            pPoint.X = 32.55
                            pPoint.Y = posi_y2 + 11.7
                            pPoint.X = 33.85
                            pPoint.Y = posi_y3 + 11.7
                        Else
                            pPoint.X = 30.85
                            pPoint.Y = posi_y2 + 12.7
                            pPoint.X = 32.15
                            pPoint.Y = posi_y3 + 12.7
                        End If
                    End If

                        'ElseIf contatexto = 13 Then  'Exploracion y explotacion
                        '    If v_explo_v_expta1 > 0 Then
                        '        pTextElement.Text = v_explo_v_expta1  'contador Capa Uso mi
                        '    Else
                        '        pTextElement.Text = "0"
                        '    End If
                        '    If sele_plano = "Formato A4" Then
                        '        pPoint.X = 5.9
                        '        pPoint.Y = posi_y2 + 1.12
                        '        pPoint.X = 8.0
                        '        pPoint.Y = posi_y3 + 1.12
                        '        posi_y3 = posi_y3 - 0.4
                        '    ElseIf sele_plano = "Formato A3" Then
                        '        pPoint.X = 9.75
                        '        pPoint.Y = posi_y2 + 1.0
                        '        pPoint.X = 11.15
                        '        pPoint.Y = posi_y3 + 1.0
                        '    ElseIf sele_plano = "Formato A2" Then
                        '        pPoint.X = 14.1
                        '        pPoint.Y = posi_y2 + 3.9
                        '        pPoint.X = 16.1
                        '        pPoint.Y = posi_y3 + 3.9
                        '    ElseIf sele_plano = "Formato A0" Then
                        '        pPoint.X = 30.85
                        '        pPoint.Y = posi_y2 + 10.9
                        '        pPoint.X = 32.15
                        '        pPoint.Y = posi_y3 + 10.9
                        '    End If

                    ElseIf contatexto = 9 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        If sele_plano = "Formato A4" Then
                            'pPoint.X = 5.9
                            ''pPoint.X = 5.7
                            'pPoint.Y = posi_y2 + 1.12
                            'pPoint.X = 8.0
                            ''pPoint.X = 7.9
                            'pPoint.Y = posi_y3 + 1.12

                        If seleccion_plano_si = "Plano Catastral Situacion DM" Then

                            pPoint.X = 6.92
                            pPoint.Y = posi_y2 + 1.12
                            pPoint.X = 9.0
                            'pPoint.X = 7.9
                            pPoint.Y = posi_y3 + 1.12
                        Else
                            pPoint.X = 5.9
                            'pPoint.X = 5.7
                            pPoint.Y = posi_y2 + 1.12
                            pPoint.X = 8.0
                            'pPoint.X = 7.9
                            pPoint.Y = posi_y3 + 1.12
                        End If

                    ElseIf sele_plano = "Formato A3" Then
                        If seleccion_plano_si = "Plano Catastral Situacion DM" Then
                            pPoint.X = 9.78
                            pPoint.Y = posi_y2 + 1.08
                            pPoint.X = 11.18
                            pPoint.Y = posi_y3 + 1.08
                        Else
                            pPoint.X = 9.65
                            pPoint.Y = posi_y2 + 1
                            pPoint.X = 11.05
                            pPoint.Y = posi_y3 + 1
                        End If

                    ElseIf sele_plano = "Formato A2" Then
                        If seleccion_plano_si = "Plano Catastral Situacion DM" Then
                            pPoint.X = 16.3
                            ' pPoint.Y = posi_y2 + 3.1
                            pPoint.Y = posi_y2 + 3.3
                            pPoint.X = 17.3
                            'pPoint.Y = posi_y3 + 3.1
                            pPoint.Y = posi_y3 + 3.3
                        Else
                            pPoint.X = 14.1
                            pPoint.Y = posi_y2 + 3.1
                            pPoint.X = 16.1
                            pPoint.Y = posi_y3 + 3.1
                        End If

                    ElseIf sele_plano = "Formato A0" Then
                        If seleccion_plano_si = "Plano Catastral Situacion DM" Then
                            ' pPoint.X = 16.1
                            pPoint.X = 32.55
                            pPoint.Y = posi_y2 + 9.52
                            pPoint.X = 33.85
                            pPoint.Y = posi_y3 + 9.52

                        Else
                            ' pPoint.X = 15.1
                            pPoint.X = 30.85
                            pPoint.Y = posi_y2 + 9.4
                            pPoint.X = 32.15
                            pPoint.Y = posi_y3 + 9.4

                        End If

                        'ElseIf contatexto = 15 Then  'CONTADOR A�O DAC
                        '    pTextElement.Text = lostr_Join_Codigos_AREA   'contador Capa Uso mi
                        '    If sele_plano = "Formato A4" Then
                        '        pPoint.X = 9.83
                        '        pPoint.X = 11.53
                        '        pPoint.Y = 3.32
                        '        pPoint.Y = 3.12
                        '    ElseIf sele_plano = "Formato A3" Then
                        '        pPoint.X = 15.05
                        '        pPoint.Y = posi_y2 - 0.3
                        '        pPoint.X = 16.45
                        '        pPoint.Y = posi_y3 - 0.3
                        '    ElseIf sele_plano = "Formato A2" Then
                        '        pPoint.X = 21.1
                        '        pPoint.Y = posi_y2 + 2.2
                        '        pPoint.X = 23.1
                        '        pPoint.Y = posi_y3 + 2.2
                        '    ElseIf sele_plano = "Formato A0" Then
                        '        pPoint.X = 44.25
                        '        pPoint.Y = posi_y2 + 7.57
                        '        pPoint.X = 45.55
                        '        pPoint.Y = posi_y3 + 7.57
                        '    End If

                    End If
                    End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                'Simbolo del texto
                pTxtSym = New TextSymbol
                'fuente del texto

                'Dim pFont As IFontDisp
                pFont = New StdFont
                pFont.Name = "Tahoma"
                If sele_plano = "Formato A4" Then
                    pFont.Size = 4.0
                ElseIf sele_plano = "Formato A3" Then
                    pFont.Size = 6.0
                ElseIf sele_plano = "Formato A2" Then
                    pFont.Size = 8.5
                ElseIf sele_plano = "Formato A0" Then
                    pFont.Size = 14
                End If
                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym.Color = myColor
                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal

                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

            Next contatexto
        End If
    End Sub
    Public Sub AgregarTextosLayout_ANT150512(ByVal sele_reporte As String, ByVal caso_simula As String)
        'PROGRAMA PARA AGREGAR TEXTOS EN LAYOUT
        '*******************************************
        Dim MyDate As Date
        MyDate = Now
        Dim fecha As String
        fecha = RellenarComodin(MyDate.Year, 2, "0") & "/" & RellenarComodin(MyDate.Month, 2, "0") & "/" & MyDate.Day

        Dim posi_y2 As Double
        Dim posi_y3 As Double

        Dim pPageLayout As IPageLayout
        Dim pActiveView As IActiveView
        Dim pGraphicsContainer As IGraphicsContainer
        Dim pElement As IElement
        Dim pTextElement As ITextElement
        Dim pPoint As IPoint
        Dim pTxtSym As IFormattedTextSymbol
        Dim contatexto As Integer
        Dim myColor As IRgbColor
        Dim pFont As IFontDisp

        pPageLayout = pMxDoc.PageLayout
        pPageLayout.Page.Units = esriUnits.esriCentimeters
        pActiveView = pPageLayout
        pGraphicsContainer = pPageLayout

        Dim posi_x As String
        Dim posi_x1 As String
        Dim conta_x As Long
        'Para obtener fecha

        'Dim fecha_plano As Date
        'fecha_plano = Date
        Dim posi_y As Double
        Dim posi_y1 As Double
        Dim pEnv As IEnvelope
        posi_y = posi_y_priori_CNM
        posi_y1 = posi_y_priori1_CNM
        Dim Nombre_urba As String = ""
        Dim Nombre_rese As String = ""

        'Colocando textos fijos al plano

        If sele_reporte = "Evaluacion" Then
            For contatexto = 1 To 16
                'Guardar el texto
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "Carta : " & v_carta_dm
                    pPoint.X = 8.1
                    pPoint.Y = 18.3
                    pPoint.X = 9.2
                    pPoint.Y = 17.8
                ElseIf contatexto = 2 Then
                    pTextElement.Text = "Fecha : " & fecha
                    pPoint.X = 13.8
                    pPoint.Y = 18.3
                    pPoint.X = 14.8
                    pPoint.Y = 17.8
                ElseIf contatexto = 3 Then
                    pTextElement.Text = "CODIGO DEL DM : " & v_codigo
                    pPoint.X = 14.1
                    pPoint.Y = 16.5
                    pPoint.X = 18.2
                    pPoint.Y = 17.3
                ElseIf contatexto = 4 Then
                    pTextElement.Text = "NOMBRE DEL DM : " & v_nombre_dm
                    pPoint.X = 14.1
                    pPoint.Y = 15.9
                    pPoint.X = 18.2
                    pPoint.Y = 16.7
                ElseIf contatexto = 5 Then
                    pTextElement.Text = "HECTAREA : " & " " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 14.1
                    pPoint.Y = 15.4
                    pPoint.X = 18.2
                    pPoint.Y = 16.1
                ElseIf contatexto = 6 Then
                    If Cuenta_an = 0 Then
                        pTextElement.Text = "DERECHOS PRIORITARIOS :  No Presenta DM Prioritarios"
                    Else
                        pTextElement.Text = "DERECHOS PRIORITARIOS : " & "(" & Cuenta_an & ")"
                    End If
                    pPoint.X = 15.7
                    pPoint.Y = 14.7
                    pPoint.X = 18.2
                    pPoint.Y = 15.5
                ElseIf contatexto = 7 Then
                    If v_tipo_exp = "RD" Then
                        If Cuenta_rd = 0 Then
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad "
                        Else
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad " & "(" & Cuenta_rd & ")"
                        End If
                        pPoint.X = 14.1
                        pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.X = 18.2
                        pPoint.Y = conta_rd_posi_y - 0.2
                    Else
                        pPoint.X = 14.1
                        'pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.Y = 14.7
                        pPoint.X = 18.2
                        'pPoint.Y = conta_rd_posi_y - 0.2
                        pPoint.Y = 15.5

                    End If
                ElseIf contatexto = 8 Then
                    If Cuenta_po = 0 Then
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA : No Presenta "
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES :  No Presenta DM Posteriores"
                        End If
                    Else
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA: " & "(" & Cuenta_rd & ")"
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES : " & "(" & Cuenta_po & ")"
                        End If
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = conta_po_posi_y1 - 0.2
                    pPoint.X = 18.2
                    pPoint.Y = conta_po_posi_y - 0.2

                ElseIf contatexto = 9 Then
                    If Cuenta_si = 0 Then
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  No Presenta DM Simultaneos"
                    Else
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  " & "(" & Cuenta_si & ")"
                    End If
                    pPoint.X = 14.1
                    'pPoint.Y = conta_si_posi_y1 - 0.15
                    pPoint.Y = conta_si_posi_y1 - 0.1
                    pPoint.X = 18.2
                    'pPoint.Y = conta_si_posi_y - 0.15
                    pPoint.Y = conta_si_posi_y - 0.1

                ElseIf contatexto = 10 Then
                    If Cuenta_ex = 0 Then
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  No Presenta DM Extinguidos"
                    Else
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  " & "(" & Cuenta_ex & ")"
                    End If

                    pPoint.X = 14.1
                    'pPoint.Y = conta_ex_posi_y1 - 0.15
                    pPoint.Y = conta_ex_posi_y1 - 0.01
                    pPoint.X = 18.2
                    pPoint.Y = conta_ex_posi_y - 0.01

                ElseIf contatexto = 11 Then

                    pTextElement.Text = "CATASTRO NO MINERO"


                    'pPoint.Y = posi_y1 - 0.4
                    ''pPoint.Y = posi_y1 - 0.6
                    pPoint.Y = posi_y1 - 0.4
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 0.4


                ElseIf contatexto = 12 Then
                    If lista_urba = "" Then
                        pTextElement.Text = "Zonas Urbanas :" & " No se encuentra superpuesto a un Area urbana"
                    Else
                        conta_x = lista_urba.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_urba, 1, 60)
                            posi_x1 = Right(lista_urba, (lista_urba.Length - 60))
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & lista_urba
                        End If
                    End If
                    pPoint.X = 14.1
                    ''pPoint.Y = posi_y1 - 1
                    pPoint.Y = posi_y1 - 0.8
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 0.8


                ElseIf contatexto = 13 Then
                    If lista_rese = "" Then
                        pTextElement.Text = "Zonas Reservadas :" & " No se encuentra superpuesto a un Area de Reserva"
                    Else
                        conta_x = lista_urba.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_rese, 1, 60)
                            posi_x1 = Right(lista_urba, (lista_rese.Length - 60))
                            pTextElement.Text = "Zonas Reservadas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Reservadas :      " & "  " & lista_rese
                        End If
                    End If
                    pPoint.X = 14.1
                    ''pPoint.Y = posi_y1 - 1.4
                    pPoint.Y = posi_y1 - 1.2
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1.2
                ElseIf contatexto = 14 Then

                    pTextElement.Text = "L�mites fronterizos (Fuente IGN): Distancia de la l�nea de frontera de " & distancia_fron & " (Km.)"

                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 1.8
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1.8

                ElseIf contatexto = 15 Then

                    pTextElement.Text = "                                LISTADO DE DERECHOS MINEROS"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.2
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.2

                ElseIf contatexto = 16 Then
                    pTextElement.Text = "N�   NOMBRE                         CODIGO      ZONA   TE      TP   PUBL   INCOR   SUST"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.6
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.6
                    posi_y1_list = posi_y1 - 2.9
                    posi_y2_list = posi_y - 2.9

                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"

                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then

                    pFont.Size = 8
                    'pFont.Size = 10
                Else


                    pFont.Size = 5
                    'pFont.Size = 8
                End If

                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    myColor.RGB = RGB(197, 0, 255)
                ElseIf ((contatexto = 1) Or (contatexto = 2) Or (contatexto = 6) Or (contatexto = 7) Or (contatexto = 8) Or (contatexto = 9) Or (contatexto = 10)) Then
                    myColor.RGB = RGB(71, 61, 255)
                ElseIf contatexto = 11 Then
                    myColor.RGB = RGB(230, 0, 0)
                ElseIf ((contatexto = 12) Or (contatexto = 13) Or (contatexto = 14) Or (contatexto = 15)) Then
                    myColor.RGB = RGB(0, 0, 0)
                ElseIf (contatexto = 16) Then
                    myColor.RGB = RGB(71, 61, 255)
                End If
                pTxtSym.Color = myColor

                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                If contatexto = 1 Or contatexto = 2 Or contatexto = 3 Or contatexto = 4 Or contatexto = 5 Or contatexto = 6 Then
                    pGraphicsContainer.AddElement(pTextElement, 1)
                ElseIf contatexto = 7 Then
                    If v_tipo_exp = "RD" Then
                        If v_posi_rd = False Or Cuenta_rd > 0 Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                ElseIf contatexto = 8 Then
                    If v_posi_po = False Or Cuenta_po > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 9 Then
                    If v_posi_si = False Or Cuenta_si > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 10 Then
                    If v_posi_ex = False Or Cuenta_ex > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                Else
                    If contador_hojas < 1 Then
                        If v_posi_pr = False And v_posi_po = False And v_posi_si = False And v_posi_ex = False Or v_posi_rd Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                End If
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Libredenu" Then
            For contatexto = 1 To 15
                'Guardar el texto
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "Carta : " & v_carta_dm
                    pPoint.X = 8.1
                    pPoint.Y = 18.3
                    pPoint.X = 9.2
                    pPoint.Y = 17.8
                ElseIf contatexto = 2 Then
                    pTextElement.Text = "Fecha : " & fecha
                    pPoint.X = 13.8
                    pPoint.Y = 18.3
                    pPoint.X = 14.8
                    pPoint.Y = 17.8
                ElseIf contatexto = 3 Then
                    pTextElement.Text = "CODIGO DEL DM : " & v_codigo
                    pPoint.X = 14.1
                    pPoint.Y = 16.5
                    pPoint.X = 18.2
                    pPoint.Y = 17.3
                ElseIf contatexto = 4 Then
                    pTextElement.Text = "NOMBRE DEL DM : " & v_nombre_dm
                    pPoint.X = 14.1
                    pPoint.Y = 15.9
                    pPoint.X = 18.2
                    pPoint.Y = 16.7
                ElseIf contatexto = 5 Then
                    pTextElement.Text = "HECTAREA : " & " " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 14.1
                    pPoint.Y = 15.4
                    pPoint.X = 18.2
                    pPoint.Y = 16.1
                ElseIf contatexto = 6 Then
                    If Cuenta_an = 0 Then
                        pTextElement.Text = "DERECHOS PRIORITARIOS :  No Presenta DM Prioritarios"
                    Else
                        pTextElement.Text = "DERECHOS PRIORITARIOS : " & "(" & Cuenta_an & ")"
                    End If
                    pPoint.X = 15.7
                    pPoint.Y = 14.7
                    pPoint.X = 18.2
                    pPoint.Y = 15.5
                ElseIf contatexto = 7 Then
                    If v_tipo_exp = "RD" Then
                        If Cuenta_rd = 0 Then
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad "
                        Else
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad " & "(" & Cuenta_rd & ")"
                        End If
                        pPoint.X = 14.1
                        pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.X = 18.2
                        pPoint.Y = conta_rd_posi_y - 0.2
                    Else
                        pPoint.X = 14.1
                        'pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.Y = 14.7
                        pPoint.X = 18.2
                        'pPoint.Y = conta_rd_posi_y - 0.2
                        pPoint.Y = 15.5

                    End If
                ElseIf contatexto = 8 Then
                    If Cuenta_po = 0 Then
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA : No Presenta "
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES :  No Presenta DM Posteriores"
                        End If
                    Else
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA: " & "(" & Cuenta_rd & ")"
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES : " & "(" & Cuenta_po & ")"
                        End If
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = conta_po_posi_y1 - 0.2
                    pPoint.X = 18.2
                    pPoint.Y = conta_po_posi_y - 0.2

                ElseIf contatexto = 9 Then
                    If Cuenta_si = 0 Then
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  No Presenta DM Simultaneos"
                    Else
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  " & "(" & Cuenta_si & ")"
                    End If
                    pPoint.X = 14.1
                    'pPoint.Y = conta_si_posi_y1 - 0.15
                    pPoint.Y = conta_si_posi_y1 - 0.1
                    pPoint.X = 18.2
                    'pPoint.Y = conta_si_posi_y - 0.15
                    pPoint.Y = conta_si_posi_y - 0.1

                ElseIf contatexto = 10 Then
                    If Cuenta_ex = 0 Then
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  No Presenta DM Extinguidos"
                    Else
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  " & "(" & Cuenta_ex & ")"
                    End If

                    pPoint.X = 14.1
                    'pPoint.Y = conta_ex_posi_y1 - 0.15
                    pPoint.Y = conta_ex_posi_y1 - 0.01
                    pPoint.X = 18.2
                    pPoint.Y = conta_ex_posi_y - 0.01

                ElseIf contatexto = 11 Then

                    pTextElement.Text = "OTROS DATOS DE EVALUACION"

                    'pPoint.Y = posi_y1 - 0.4
                    ''pPoint.Y = posi_y1 - 0.6
                    pPoint.Y = posi_y1 - 0.4
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 0.4


                ElseIf contatexto = 12 Then
                    If lista_urba = "" Then
                        pTextElement.Text = "Zonas Urbanas :" & " No se encuentra superpuesto a un Area urbana"
                    Else
                        conta_x = lista_urba.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_urba, 1, 60)
                            posi_x1 = Right(lista_urba, (lista_urba.Length - 60))
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & lista_urba
                        End If
                    End If
                    pPoint.X = 14.1
                    ''pPoint.Y = posi_y1 - 1
                    pPoint.Y = posi_y1 - 0.8
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 0.8


                ElseIf contatexto = 13 Then
                    If lista_rese = "" Then
                        pTextElement.Text = "Zonas Reservadas :" & " No se encuentra superpuesto a un Area de Reserva"
                    Else
                        conta_x = lista_urba.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_rese, 1, 60)
                            posi_x1 = Right(lista_urba, (lista_rese.Length - 60))
                            pTextElement.Text = "Zonas Reservadas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Reservadas :      " & "  " & lista_rese
                        End If
                    End If
                    pPoint.X = 14.1
                    ''pPoint.Y = posi_y1 - 1.4
                    pPoint.Y = posi_y1 - 1.2
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1.2

                ElseIf contatexto = 14 Then

                    pTextElement.Text = "                                LISTADO DE DERECHOS MINEROS"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.2
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.2

                ElseIf contatexto = 15 Then
                    pTextElement.Text = "N�   NOMBRE                         CODIGO      ZONA   TE      TP   PUBL   INCOR   SUST"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.6
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.6
                    posi_y1_list = posi_y1 - 2.9
                    posi_y2_list = posi_y - 2.9

                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'If ((contatexto = 9) Or (contatexto = 10) Or (contatexto = 11)) Then
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    'pFont.Size = 8
                    pFont.Size = 8
                Else
                    'pFont.Size = 9
                    ''pFont.Size = 6
                    pFont.Size = 5
                End If

                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    myColor.RGB = RGB(197, 0, 255)
                ElseIf ((contatexto = 1) Or (contatexto = 2) Or (contatexto = 6) Or (contatexto = 7) Or (contatexto = 8) Or (contatexto = 9) Or (contatexto = 10)) Then
                    myColor.RGB = RGB(71, 61, 255)
                ElseIf contatexto = 11 Then
                    myColor.RGB = RGB(230, 0, 0)
                ElseIf ((contatexto = 12) Or (contatexto = 13) Or (contatexto = 14) Or (contatexto = 15)) Then
                    myColor.RGB = RGB(0, 0, 0)
                ElseIf (contatexto = 16) Then
                    myColor.RGB = RGB(71, 61, 255)
                End If
                pTxtSym.Color = myColor

                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                If contatexto = 1 Or contatexto = 2 Or contatexto = 3 Or contatexto = 4 Or contatexto = 5 Or contatexto = 6 Then
                    pGraphicsContainer.AddElement(pTextElement, 1)
                ElseIf contatexto = 7 Then
                    If v_tipo_exp = "RD" Then
                        If v_posi_rd = False Or Cuenta_rd > 0 Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                ElseIf contatexto = 8 Then
                    If v_posi_po = False Or Cuenta_po > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 9 Then
                    If v_posi_si = False Or Cuenta_si > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 10 Then
                    If v_posi_ex = False Or Cuenta_ex > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                Else
                    If contador_hojas < 1 Then
                        If v_posi_pr = False And v_posi_po = False And v_posi_si = False And v_posi_ex = False Or v_posi_rd Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                End If
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Cuadricula" Then
            Dim canti_cuadri As Integer
            canti_cuadri = v_area_eval / 100

            For contatexto = 1 To 8
                'Guardar el texto
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "Carta : " & v_carta_dm
                    pPoint.X = 8.1
                    pPoint.Y = 18.5
                    pPoint.X = 9.2
                    pPoint.Y = 18.0
                ElseIf contatexto = 2 Then
                    pTextElement.Text = "Fecha : " & fecha
                    pPoint.X = 13.8
                    pPoint.Y = 18.5
                    pPoint.X = 14.8
                    pPoint.Y = 18.0
                ElseIf contatexto = 3 Then
                    pTextElement.Text = "CODIGO DEL DM : " & v_codigo
                    pPoint.X = 14.4
                    pPoint.Y = 16.5
                    pPoint.X = 18.5
                    pPoint.Y = 17.3
                ElseIf contatexto = 4 Then
                    pTextElement.Text = "NOMBRE DEL DM : " & v_nombre_dm
                    pPoint.X = 14.4
                    pPoint.Y = 15.9
                    pPoint.X = 18.5
                    pPoint.Y = 16.7
                ElseIf contatexto = 5 Then
                    pTextElement.Text = "HECTAREA : " & " " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 14.4
                    pPoint.Y = 15.4
                    pPoint.X = 18.5
                    pPoint.Y = 16.1
                ElseIf contatexto = 6 Then
                    pTextElement.Text = "* EL PRESENTE PETITORIO HA SIDO FORMULADO POR : " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 17.8
                    pPoint.Y = 11.2
                    pPoint.X = 18.8
                    pPoint.Y = 10.8

                ElseIf contatexto = 7 Then
                    pTextElement.Text = "* ESTA CONFORMADO POR  " & canti_cuadri & " CUADRICULAS DE 100.00 Ha."
                    pPoint.X = 17.8
                    pPoint.Y = 10.6
                    pPoint.X = 18.8
                    pPoint.Y = 10.2
                ElseIf contatexto = 8 Then
                    pTextElement.Text = "* LAS CUADRICULAS SON  " & nombre_datos
                    pPoint.X = 17.8
                    pPoint.Y = 10.0
                    pPoint.X = 18.8
                    pPoint.Y = 9.6
                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTxtSym = New TextSymbol
                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"

                pFont.Size = 6
                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    myColor.RGB = RGB(197, 0, 255)
                ElseIf ((contatexto = 1) Or (contatexto = 2)) Then
                    myColor.RGB = RGB(71, 61, 255)
                ElseIf ((contatexto = 6) Or (contatexto = 7) Or (contatexto = 8)) Then
                    myColor.RGB = RGB(0, 0, 255)

                End If
                pTxtSym.Color = myColor
                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Carta" Then
            Dim posi_y_c As Double
            Dim posi_y_c1 As Double
            posi_y_c = 12.35
            posi_y_c1 = 12.03

            'Guardar el texto
            For contatexto = 1 To 13
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then

                    'pTextElement.Text = "DERECHO MINERO:  " & v_nombre_dm
                    conta_x = v_nombre_dm.Length
                    'If conta_x > 45 Then
                    If conta_x > 60 Then
                        posi_x = Mid(v_nombre_dm, 1, 60)
                        posi_x1 = Right(v_nombre_dm, (v_nombre_dm.Length - 60))
                        pTextElement.Text = "DERECHO MINERO:  " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "DERECHO MINERO:  " & v_nombre_dm
                    End If
                    pPoint.X = 19.1
                    'pPoint.Y = 12.353
                    pPoint.Y = posi_y_c
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                    'pPoint.Y = 12.003

                ElseIf contatexto = 2 Then
                    conta_x = lista_nmhojas_ign.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_nmhojas_ign, 1, 60)
                        'posi_x1 = Right(lista_nmhojas_ign, (lista_nmhojas_ign.Length - 60))
                        posi_x1 = Right(lista_nmhojas_ign, (lista_nmhojas_ign.Length - 60))
                        pTextElement.Text = "Nombre de la Carta : " & "  " & posi_x & vbNewLine & posi_x1
                    Else
                        'PTextElement.Text = "Nombre de la Carta : " & "  " & lista_nmhojas_ign
                        pTextElement.Text = "Nombre de la Carta : " & "  " & lista_nmhojas_ign
                    End If

                    pPoint.X = 19.1
                    'pPoint.Y = 11.984
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 11.634
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 3 Then
                    'pTextElement.Text = "Numero de la Carta :  " & carta_v
                    conta_x = lista_cd_cartas.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_cd_cartas, 1, 60)
                        posi_x1 = Right(lista_cd_cartas, (lista_cd_cartas.Length - 60))
                        pTextElement.Text = "N�mero de la Carta : " & "  " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "N�mero de la Carta : " & "  " & lista_cd_cartas
                    End If
                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 4 Then
                    pTextElement.Text = "Escala                  :" & "    " & "1/100 000"
                    pPoint.X = 19.1
                    'pPoint.Y = 11.341
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 10.991

                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 5 Then
                    'pTextElement.Text = "Zona                    :" & "   " & "ZONA"
                    pTextElement.Text = "Zona                    : " & "   " & v_zona_dm
                    pPoint.X = 19.1
                    'pPoint.Y = 11.02
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 10.68
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 6 Then
                    pTextElement.Text = "UBICACION"
                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 7 Then
                    'pTextElement.Text = "Distrito                :   " & lista_dist
                    conta_x = lista_dist.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_dist, 1, 45)
                        posi_x1 = Right(lista_dist, (lista_dist.Length - 60))
                        pTextElement.Text = "DISTRITOS (S)       : " & "   " & posi_x & vbNewLine & posi_x1
                        posi_y_c = posi_y_c + 0.3
                        posi_y_c1 = posi_y_c1 - 0.3
                    Else
                        pTextElement.Text = "DISTRITOS (S)       : " & "   " & lista_dist
                    End If

                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 8 Then
                    conta_x = lista_prov.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_prov, 1, 60)
                        posi_x1 = Right(lista_prov, (lista_prov.Length - 60))
                        pTextElement.Text = "PROVINCIA (S)       : " & "   " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "PROVINCIA (S)       : " & "   " & lista_prov
                    End If

                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 9 Then
                    conta_x = lista_depa.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_depa, 1, 60)
                        posi_x1 = Right(lista_depa, (lista_depa.Length - 60))
                        pTextElement.Text = "DEPARTAMENTO (S) : " & "   " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "DEPARTAMENTO (S) : " & "   " & lista_depa
                    End If

                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 10 Then
                    pTextElement.Text = "OBSERVACIONES"
                    pPoint.X = 19.1
                    'pPoint.Y = 9.13
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 8.9
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1

                ElseIf contatexto = 11 Then
                    pTextElement.Text = "No existen Observaciones"
                    pPoint.X = 19.1

                    pPoint.X = 19.1
                    'pPoint.Y = 2.1
                    pPoint.Y = posi_y_c - 0.3
                    pPoint.Y = posi_y_c1 - 0.3

                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3
                    posi_y_m = posi_y_c
                    posi_y1_m = posi_y_c1

                    'ElseIf contatexto = 11 Then
                    '    pTextElement.Text = "ELIPSOIDE...............................................INTERNACIONAL"
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.48
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.48

                    'ElseIf contatexto = 12 Then
                    '    pTextElement.Text = "PROYECCION.................................TRANSVERSA DE MERCATOR"
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.289
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.289

                    'ElseIf contatexto = 13 Then
                    '    pTextElement.Text = "DATUM VERTICAL..............................................NIVEL MEDIO DEL MAR"
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.48
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.48

                    'ElseIf contatexto = 14 Then
                    '    pTextElement.Text = "DATUM HORIZONTAL.........DATUM PROVISIONAL SUDAMERICANO DE 1956"
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.289
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.289
                ElseIf contatexto = 12 Then
                    pTextElement.Text = fecha
                    pPoint.X = 16.0
                    pPoint.Y = 2.9
                    pPoint.X = 19.7
                    pPoint.Y = 2.1
                ElseIf contatexto = 13 Then
                    'pTextElement.Text = escala_plano_carta
                    'pPoint.X = 17.3
                    pPoint.X = 19.4
                    pPoint.Y = 2.9
                    'pPoint.X = 21.1
                    pPoint.X = 21.9
                    pPoint.Y = 2.1
                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                'Simbolo del texto
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'color del texto
                'Dim pColor As IRgbColor
                myColor = New RgbColor

                'If contatexto = 1 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'ElseIf ((contatexto > 10) And (contatexto < 15)) Then
                'pFont.Size = 5.5
                'ElseIf contatexto = 6 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'ElseIf contatexto = 10 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'Else
                'pFont.Size = 8
                'End If

                If contatexto = 1 Then
                    pFont.Size = 8
                    myColor.RGB = RGB(0, 0, 250)
                ElseIf ((contatexto > 10) And (contatexto < 15)) Then
                    pFont.Size = 5.5
                ElseIf contatexto = 6 Then
                    pFont.Size = 9
                    myColor.RGB = RGB(0, 0, 250)
                ElseIf contatexto = 10 Then
                    pFont.Size = 9
                    myColor.RGB = RGB(0, 0, 250)
                    'ElseIf contatexto = 11 Or contatexto = 12 Or contatexto = 13 Or contatexto = 14 Then
                    'pFont.Size = 3
                    'myColor.RGB = RGB(0, 0, 250)
                Else
                    pFont.Size = 6
                End If


                pFont.Bold = False
                pTxtSym.Font = pFont
                pTxtSym.Color = myColor

                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Plano_variado" Then
            If tipo_opcion = "1" Then
                lista_nmhojas = v_nombre
                lista_cartas = v_codigo
            End If
            Dim posi_y_c As Double
            Dim posi_y_c1 As Double

            If sele_plano1 = "Plano A4 Vertical" Then
                posi_y_c = 27.18
                posi_y_c1 = 27.08

            ElseIf sele_plano1 = "Plano A4 Horizontal" Then
                posi_y_c = 11.8
                posi_y_c1 = 11.5

            ElseIf sele_plano1 = "Plano A3 Vertical" Then
                posi_y_c = 37.65
                posi_y_c1 = 37.55
                'posi_y_c = 40.45
                'posi_y_c1 = 40.35
            ElseIf sele_plano1 = "Plano A3 Horizontal" Then
                posi_y_c = 17.05
                posi_y_c1 = 16.75

            ElseIf sele_plano1 = "Plano A2 Vertical" Then
                posi_y_c = 54.7
                posi_y_c1 = 54.5

            ElseIf sele_plano1 = "Plano A2 Horizontal" Then
                posi_y_c = 24.0
                posi_y_c1 = 23.9

            ElseIf sele_plano1 = "Plano A1 Vertical" Then
                posi_y_c = 74.8
                posi_y_c1 = 74.3
                'posi_y_c = 81.0
                'posi_y_c1 = 80.5

            ElseIf sele_plano1 = "Plano A1 Horizontal" Then
                posi_y_c = 33.95
                posi_y_c1 = 33.65
            ElseIf sele_plano1 = "Plano A0 Vertical" Then
                posi_y_c = 108.4
                posi_y_c1 = 107.4

            ElseIf sele_plano1 = "Plano A0 Horizontal" Then
                posi_y_c = 48.65
                posi_y_c1 = 47.65
            End If

            If sele_plano1 = "Plano A4 Horizontal" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then      'exploracion
                        pTextElement.Text = v_explo1
                        pPoint.X = 24.92
                        pPoint.X = 26.0
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c + 0.6
                        posi_y_c1 = posi_y_c1 + 0.6

                    ElseIf contatexto = 2 Then      'explotacion
                        pTextElement.Text = v_expta1
                        pPoint.X = 24.92
                        pPoint.X = 26.0
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 3 Then      'exploracion - explotacion
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 24.92
                        pPoint.X = 26.0
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c + 0.1
                        posi_y_c1 = posi_y_c1 + 0.1

                    ElseIf contatexto = 4 Then      'cierre
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 37.02
                        pPoint.X = 38.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c + 1.5
                        posi_y_c1 = posi_y_c1 + 1.5

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 22.5
                        pPoint.X = 23.1
                        pPoint.Y = 2.75
                        pPoint.Y = 2.55

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 22.8
                        pPoint.X = 23.4
                        pPoint.Y = 2.35
                        pPoint.Y = 2.15

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 22.8
                        pPoint.X = 23.4
                        pPoint.Y = 1.95
                        pPoint.Y = 1.75

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 22.8
                        pPoint.X = 23.4
                        pPoint.Y = 1.5
                        pPoint.Y = 1.3

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 25.4
                        pPoint.X = 26.0
                        pPoint.Y = 2.35
                        pPoint.Y = 2.15

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 25.4
                        pPoint.X = 26.0
                        pPoint.Y = 1.5
                        pPoint.Y = 1.3

                    ElseIf contatexto = 11 Then     'escala
                        pTextElement.Text = escalaf
                        pPoint.X = 24.1
                        pPoint.X = 24.8
                        pPoint.Y = 11.35
                        pPoint.Y = 13.15

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 26.57
                        pPoint.X = 27.55
                        pPoint.Y = 6.6
                        pPoint.Y = 8.4

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 26.57
                        pPoint.X = 27.55
                        pPoint.Y = 7.3
                        pPoint.Y = 9.1

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 25.4
                        pPoint.X = 26.4
                        pPoint.Y = 6.35
                        pPoint.Y = 7.35

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 4.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 6

                    Else
                        pFont.Size = 5.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A4 Vertical" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 11.35
                        pPoint.X = 12.35
                        pPoint.Y = 27.0
                        pPoint.Y = 26.9

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 11.35
                        pPoint.X = 12.35
                        pPoint.Y = 26.45
                        pPoint.Y = 26.35

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 11.35
                        pPoint.X = 12.35
                        pPoint.Y = 25.95
                        pPoint.Y = 25.85

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 44.4
                        pPoint.X = 45.4
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.41
                        posi_y_c1 = posi_y_c1 - 0.41

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 14.05
                        pPoint.X = 15.05
                        pPoint.Y = 23.27
                        pPoint.Y = 23.17

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 14.4
                        pPoint.X = 15.5
                        pPoint.Y = 22.87
                        pPoint.Y = 22.77

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 14.4
                        pPoint.X = 15.5
                        pPoint.Y = 22.5
                        pPoint.Y = 22.4

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 14.4
                        pPoint.X = 15.5
                        pPoint.Y = 22.0
                        pPoint.Y = 21.9

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 17.5
                        pPoint.X = 18.1
                        pPoint.Y = 22.87
                        pPoint.Y = 22.77

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 17.5
                        pPoint.X = 18.1
                        pPoint.Y = 22.0
                        pPoint.Y = 21.9

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 14.8
                        pPoint.X = 15.4
                        pPoint.Y = 28.15
                        pPoint.Y = 28.05

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 19.03
                        pPoint.X = 19.63
                        pPoint.Y = 25.05
                        pPoint.Y = 26.85

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 19.03
                        pPoint.X = 19.63
                        pPoint.Y = 24.45
                        pPoint.Y = 26.25

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 17.7
                        pPoint.X = 18.7
                        pPoint.Y = 24.15
                        pPoint.Y = 25.15

                    End If

                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor

                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 4.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 6

                    Else
                        pFont.Size = 5.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A3 Horizontal" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 35.5
                        pPoint.X = 36.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 35.5
                        pPoint.X = 36.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 35.5
                        pPoint.X = 36.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1

                        pPoint.X = 48.8
                        pPoint.X = 49.8
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9


                    ElseIf contatexto = 5 Then      'HOJA
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 31.25
                        pPoint.X = 32.25
                        pPoint.Y = 3.6
                        pPoint.Y = 3.35

                    ElseIf contatexto = 6 Then      'CODIGO
                        pTextElement.Text = lista_cartas
                        pPoint.X = 31.75
                        pPoint.X = 32.75
                        pPoint.Y = 3.0
                        pPoint.Y = 2.78

                    ElseIf contatexto = 7 Then      'TAMA�O
                        pTextElement.Text = sele_plano2
                        pPoint.X = 31.75
                        pPoint.X = 32.75
                        pPoint.Y = 2.4
                        pPoint.Y = 2.15

                    ElseIf contatexto = 8 Then      'FECHA
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 31.75
                        pPoint.X = 32.75
                        pPoint.Y = 1.8
                        pPoint.Y = 1.55

                    ElseIf contatexto = 9 Then      'ZONA
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 35.95
                        pPoint.X = 36.55
                        pPoint.Y = 3.0
                        pPoint.Y = 2.78
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 35.95
                        pPoint.X = 36.55
                        pPoint.Y = 1.8
                        pPoint.Y = 1.55

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 35.35
                        pPoint.X = 35.05
                        pPoint.Y = 19.35
                        pPoint.Y = 18.35

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 37.07
                        pPoint.X = 38.67
                        pPoint.Y = 10.85
                        pPoint.Y = 12.65

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 37.07
                        pPoint.X = 38.67
                        pPoint.Y = 9.9
                        pPoint.Y = 11.7

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 35.05
                        pPoint.X = 36.55
                        pPoint.Y = 8.375
                        pPoint.Y = 10.175

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 5.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 7.5

                    Else
                        pFont.Size = 6.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A3 Vertical" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 17.07
                        pPoint.X = 18.07
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.6
                        posi_y_c1 = posi_y_c1 - 0.6

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 17.07
                        pPoint.X = 18.07
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.65
                        posi_y_c1 = posi_y_c1 - 0.65

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 17.07
                        pPoint.X = 18.07
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.65
                        posi_y_c1 = posi_y_c1 - 0.65

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 37.92
                        pPoint.X = 38.92
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9

                    ElseIf contatexto = 5 Then      'HOJA
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 21.8
                        pPoint.X = 22.8
                        pPoint.Y = 32.85
                        pPoint.Y = 32.65

                    ElseIf contatexto = 6 Then      'CODIGO
                        pTextElement.Text = lista_cartas
                        pPoint.X = 22.1
                        pPoint.X = 23.1
                        pPoint.Y = 32.35
                        pPoint.Y = 32.15

                    ElseIf contatexto = 7 Then      'TAMA�O
                        pTextElement.Text = sele_plano2
                        pPoint.X = 22.1
                        pPoint.X = 23.1
                        pPoint.Y = 31.85
                        pPoint.Y = 31.65

                    ElseIf contatexto = 8 Then      'FECHA
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 22.1
                        pPoint.X = 23.1
                        pPoint.Y = 32.35
                        pPoint.Y = 31.15

                    ElseIf contatexto = 9 Then      'ZONA
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 25.2
                        pPoint.X = 26.2
                        pPoint.Y = 32.35
                        pPoint.Y = 32.15
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 25.2
                        pPoint.X = 26.2
                        pPoint.Y = 31.35
                        pPoint.Y = 31.15
                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 22.35
                        pPoint.X = 23.05
                        pPoint.Y = 44.05
                        pPoint.Y = 39.05

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 26.47
                        pPoint.X = 28.07
                        pPoint.Y = 35.8
                        pPoint.Y = 37.6

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 26.47
                        pPoint.X = 28.07
                        pPoint.Y = 34.95
                        pPoint.Y = 36.75

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 25.15
                        pPoint.X = 26.65
                        pPoint.Y = 33.675
                        pPoint.Y = 35.475

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 5.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 7.5
                    Else
                        pFont.Size = 6.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A2 Horizontal" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 51.35
                        pPoint.X = 52.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.15
                        posi_y_c1 = posi_y_c1 - 1.15

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 51.35
                        pPoint.X = 52.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 51.35
                        pPoint.X = 52.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.15
                        posi_y_c1 = posi_y_c1 - 1.15

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 73.25
                        pPoint.X = 74.25
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.4
                        posi_y_c1 = posi_y_c1 - 1.4

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 45.4
                        pPoint.X = 46.4
                        pPoint.Y = 4.9
                        pPoint.Y = 4.5

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 46.3
                        pPoint.X = 47.3
                        pPoint.Y = 4.0
                        pPoint.Y = 3.6

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 46.3
                        pPoint.X = 47.3
                        pPoint.Y = 3.2
                        pPoint.Y = 2.8

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 46.3
                        pPoint.X = 47.3
                        pPoint.Y = 2.3
                        pPoint.Y = 1.9

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 51.3
                        pPoint.X = 52.3
                        pPoint.Y = 4.0
                        pPoint.Y = 3.6

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 51.3
                        pPoint.X = 52.3
                        pPoint.Y = 2.3
                        pPoint.Y = 1.9

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 48.6
                        pPoint.X = 49.6
                        pPoint.Y = 27.05
                        pPoint.Y = 26.05

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 53.87
                        pPoint.X = 55.47
                        pPoint.Y = 16.2
                        pPoint.Y = 18.0

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 53.87
                        pPoint.X = 55.47
                        pPoint.Y = 14.8
                        pPoint.Y = 16.6

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 51.45
                        pPoint.X = 53.0
                        pPoint.Y = 12.6
                        pPoint.Y = 14.4

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 9
                    ElseIf contatexto = 11 Then
                        pFont.Size = 13

                    Else
                        pFont.Size = 10
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A2 Vertical" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 24.7
                        pPoint.X = 25.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 24.7
                        pPoint.X = 25.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 24.7
                        pPoint.X = 25.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 44.7
                        pPoint.X = 45.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 30.3
                        pPoint.X = 31.3
                        pPoint.Y = 48.0
                        pPoint.Y = 46.7

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 46.2
                        pPoint.Y = 45.9

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 45.4
                        pPoint.Y = 45.1

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 44.6
                        pPoint.Y = 44.3

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm

                        pPoint.X = 35.95
                        pPoint.X = 36.95
                        pPoint.Y = 46.2
                        pPoint.Y = 45.9
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 35.95
                        pPoint.X = 36.95
                        pPoint.Y = 44.6
                        pPoint.Y = 44.3
                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 30.9
                        pPoint.X = 31.9
                        pPoint.Y = 56.3
                        pPoint.Y = 55.9

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 38.4
                        pPoint.X = 40.0
                        pPoint.Y = 51.8
                        pPoint.Y = 53.6

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 38.4
                        pPoint.X = 40.0
                        pPoint.Y = 50.6
                        pPoint.Y = 52.4

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 36.2
                        pPoint.X = 37.8
                        pPoint.Y = 48.38
                        pPoint.Y = 50.18

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 9
                    ElseIf contatexto = 11 Then
                        pFont.Size = 13

                    Else
                        pFont.Size = 10
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A1 Horizontal" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 73.05
                        pPoint.X = 74.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.5
                        posi_y_c1 = posi_y_c1 - 1.5

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 73.05
                        pPoint.X = 74.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.5
                        posi_y_c1 = posi_y_c1 - 1.5

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 73.05
                        pPoint.X = 74.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.7
                        posi_y_c1 = posi_y_c1 - 1.7

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 93.05
                        pPoint.X = 94.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.9
                        posi_y_c1 = posi_y_c1 - 1.9

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 64.0
                        pPoint.X = 65.3
                        pPoint.Y = 6.3
                        pPoint.Y = 5.6

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 64.7
                        pPoint.X = 65.9
                        pPoint.Y = 5.1
                        pPoint.Y = 4.4

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 64.7
                        pPoint.X = 65.9
                        pPoint.Y = 3.9
                        pPoint.Y = 3.2

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 64.7
                        pPoint.X = 65.9
                        pPoint.Y = 2.6
                        pPoint.Y = 1.9

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 73.1
                        pPoint.X = 74.1
                        pPoint.Y = 5.1
                        pPoint.Y = 4.4

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 73.1
                        pPoint.X = 74.1
                        pPoint.Y = 2.6
                        pPoint.Y = 1.9

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 70.1
                        pPoint.X = 71.1
                        pPoint.Y = 37.85
                        pPoint.Y = 36.85

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 77.0
                        pPoint.X = 78.6
                        pPoint.Y = 23.7
                        pPoint.Y = 25.5

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 77.0
                        pPoint.X = 78.6
                        pPoint.Y = 21.8
                        pPoint.Y = 23.6

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 73.6
                        pPoint.X = 75.2
                        pPoint.Y = 18.5
                        pPoint.Y = 20.3

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 12.3
                    ElseIf contatexto = 11 Then
                        pFont.Size = 16

                    Else
                        pFont.Size = 15
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A1 Vertical" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 35.8
                        pPoint.X = 36.8
                        pPoint.Y = 76.9
                        pPoint.Y = 76.4

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 35.8
                        pPoint.X = 36.8
                        pPoint.Y = 75.5
                        pPoint.Y = 75.0

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 35.8
                        pPoint.X = 36.8
                        pPoint.Y = 74.0
                        pPoint.Y = 73.5

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 75.8
                        pPoint.X = 76.8
                        pPoint.Y = 72.6
                        pPoint.Y = 72.1

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 42.9
                        pPoint.X = 44.1
                        pPoint.Y = 66.6
                        pPoint.Y = 66.1

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 43.9
                        pPoint.X = 44.9
                        pPoint.Y = 65.5
                        pPoint.Y = 65.0

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2

                        pPoint.X = 43.9
                        pPoint.X = 44.9
                        pPoint.Y = 64.4
                        pPoint.Y = 63.9

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 43.9
                        pPoint.X = 44.9
                        pPoint.Y = 63.2
                        pPoint.Y = 62.7

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zona_dm

                        pPoint.X = 51.5
                        pPoint.X = 52.5
                        pPoint.Y = 65.5
                        pPoint.Y = 65.0
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 51.5
                        pPoint.X = 52.5
                        pPoint.Y = 63.2
                        pPoint.Y = 62.7

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 44.1
                        pPoint.X = 45.1
                        pPoint.Y = 80.9
                        pPoint.Y = 79.9

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 55.1
                        pPoint.X = 56.7
                        pPoint.Y = 74.8
                        pPoint.Y = 76.6

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 55.1
                        pPoint.X = 56.7
                        pPoint.Y = 72.9
                        pPoint.Y = 74.7

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 52.4
                        pPoint.X = 54.0
                        pPoint.Y = 69.9
                        pPoint.Y = 71.7

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 12.3
                    ElseIf contatexto = 11 Then
                        pFont.Size = 16

                    Else
                        pFont.Size = 15
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A0 Horizontal" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 103.75
                        pPoint.X = 104.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 103.75
                        pPoint.X = 104.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 103.75
                        pPoint.X = 104.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 143.75
                        pPoint.X = 144.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 91.7
                        pPoint.X = 92.7
                        pPoint.Y = 15.4
                        pPoint.Y = 9.4

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 92.7
                        pPoint.X = 93.9
                        pPoint.Y = 8.7
                        pPoint.Y = 7.7

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 92.7
                        pPoint.X = 93.9
                        pPoint.Y = 7.1
                        pPoint.Y = 6.1

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 92.7
                        pPoint.X = 93.9
                        pPoint.Y = 5.15
                        pPoint.Y = 4.45

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 103.7
                        pPoint.X = 104.7
                        pPoint.Y = 8.85
                        pPoint.Y = 7.85

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 103.7
                        pPoint.X = 104.7
                        pPoint.Y = 5.15
                        pPoint.Y = 4.45

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 97.25
                        pPoint.X = 98.45
                        pPoint.Y = 53.1
                        pPoint.Y = 52.1

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 109.25
                        pPoint.X = 110.85
                        pPoint.Y = 34.15
                        pPoint.Y = 35.95

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 109.25
                        pPoint.X = 110.85
                        pPoint.Y = 31.6
                        pPoint.Y = 33.4

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 104.75
                        pPoint.X = 106.35
                        pPoint.Y = 27.1
                        pPoint.Y = 28.9

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 17
                    ElseIf contatexto = 11 Then
                        pFont.Size = 22.5
                    Else
                        pFont.Size = 21
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A0 Vertical" Then
                For contatexto = 1 To 14
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 52.4
                        pPoint.X = 52.6
                        pPoint.Y = 108.95
                        pPoint.Y = 107.95

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 52.4
                        pPoint.X = 52.6
                        pPoint.Y = 106.9
                        pPoint.Y = 105.9

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 52.4
                        pPoint.X = 52.6
                        pPoint.Y = 104.9
                        pPoint.Y = 103.9

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = ""  'v_cierre1
                        pPoint.X = 92.3
                        pPoint.X = 92.5
                        pPoint.Y = 98.8
                        pPoint.Y = 97.8

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 61.2
                        pPoint.X = 62.4
                        pPoint.Y = 95.2
                        pPoint.Y = 94.2

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 62.8
                        pPoint.X = 63.8
                        pPoint.Y = 93.6
                        pPoint.Y = 92.6

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 62.8
                        pPoint.X = 63.8
                        pPoint.Y = 92.0
                        pPoint.Y = 91.0
                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 62.8
                        pPoint.X = 63.8
                        pPoint.Y = 90.25
                        pPoint.Y = 89.25

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 72.3
                        pPoint.X = 74.3
                        pPoint.Y = 93.6
                        pPoint.Y = 92.6
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 72.3
                        pPoint.X = 74.3
                        pPoint.Y = 90.25
                        pPoint.Y = 89.25

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 62.9
                        pPoint.X = 63.9
                        pPoint.Y = 114.1
                        pPoint.Y = 113.1
                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 79.0
                        pPoint.X = 80.6
                        pPoint.Y = 109.3
                        pPoint.Y = 108.3

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 79.0
                        pPoint.X = 80.6
                        pPoint.Y = 106.8
                        pPoint.Y = 105.8

                    ElseIf contatexto = 14 Then  'CONTADOR A�O DAC
                        pTextElement.Text = lostr_Join_Codigos_AREA
                        pPoint.X = 74.5
                        pPoint.X = 76.1
                        pPoint.Y = 102.3
                        pPoint.Y = 101.3

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol
                    'fuente del texto
                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto
                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 17
                    ElseIf contatexto = 11 Then
                        pFont.Size = 22.5
                    Else
                        pFont.Size = 21
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            End If
        ElseIf sele_reporte = "Demarca" Then
            For contatexto = 1 To 8
                'Guardar el texto
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    conta_x = v_nombre_dm.Length
                    If conta_x >= 38 Then
                        posi_x = Mid(v_nombre_dm, 1, 38)
                        posi_x1 = Right(v_nombre_dm, (v_nombre_dm.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = v_nombre_dm
                    End If
                    pPoint.X = 17.1
                    'pPoint.Y = 8.1
                    pPoint.Y = 9.1
                    pPoint.X = 21.7
                    'pPoint.Y = 8.9
                    pPoint.Y = 9.9
                ElseIf contatexto = 2 Then
                    pTextElement.Text = v_codigo
                    pPoint.X = 17.9
                    'pPoint.Y = 7.5
                    pPoint.Y = 8.2
                    pPoint.X = 21.7
                    'pPoint.Y = 8.3
                    pPoint.Y = 9.2
                ElseIf contatexto = 3 Then
                    'posi_x = ""
                    'posi_x1 = ""
                    'pTextElement.Text = lista_dist
                    If caso_consulta = "DEMARCACION POLITICA" Then
                        conta_x = lista_dist_dema.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_dist_dema, 1, 38)
                            posi_x1 = Right(lista_dist_dema, (lista_dist_dema.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_dist_dema
                        End If
                    Else 'EVALUACIUON
                        conta_x = lista_dist.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_dist, 1, 38)
                            posi_x1 = Right(lista_dist, (lista_dist.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_dist
                        End If
                    End If
                    pPoint.X = 17.9
                    'pPoint.Y = 6.9
                    pPoint.Y = 7.6
                    pPoint.X = 21.7
                    'pPoint.Y = 7.7
                    pPoint.Y = 8.4

                ElseIf contatexto = 4 Then
                    'pTextElement.Text = lista_prov
                    If caso_consulta = "DEMARCACION POLITICA" Then
                        conta_x = lista_prov_dema.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_prov_dema, 1, 38)
                            posi_x1 = Right(lista_prov_dema, (lista_prov_dema.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_prov_dema
                        End If
                    Else
                        conta_x = lista_prov.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_prov, 1, 38)
                            posi_x1 = Right(lista_prov, (lista_prov.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_prov
                        End If
                    End If

                    pPoint.X = 17.9
                    'pPoint.Y = 6.3
                    pPoint.Y = 6.8
                    pPoint.X = 21.7
                    'pPoint.Y = 7.1
                    pPoint.Y = 7.6
                ElseIf contatexto = 5 Then
                    'pTextElement.Text = lista_depa
                    If caso_consulta = "DEMARCACION POLITICA" Then
                        conta_x = lista_depa_dema.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_depa_dema, 1, 38)
                            posi_x1 = Right(lista_depa_dema, (lista_depa_dema.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_depa_dema
                        End If
                    Else

                        conta_x = lista_depa.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_depa, 1, 38)
                            posi_x1 = Right(lista_depa, (lista_depa.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_depa
                        End If
                    End If

                    pPoint.X = 17.9
                    'pPoint.Y = 5.7
                    pPoint.Y = 5.7
                    pPoint.X = 21.7
                    'pPoint.Y = 6.5
                    pPoint.Y = 6.7
                    ElseIf contatexto = 6 Then
                        pTextElement.Text = fecha
                        pPoint.X = 16.1
                        pPoint.Y = 2.9
                        pPoint.X = 19.8
                        pPoint.Y = 2.1
                    ElseIf contatexto = 7 Then
                        'pTextElement.Text = escala_plano_dema
                        'pPoint.X = 17.3
                        pPoint.X = 18.3
                        pPoint.Y = 2.9
                        'pPoint.X = 21.1
                        pPoint.X = 22.1
                        pPoint.Y = 2.1

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 5.6
                        pPoint.Y = 2.12
                        pPoint.X = 9.6
                        pPoint.Y = 1.32
                    End If

                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv

                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'If contatexto = 6 Then
                    pFont.Size = 7
                    'ElseIf contatexto = 7 Then
                    'pFont.Size = 6
                    'ElseIf contatexto = 8 Then
                    'pFont.Size = 6
                    'Else
                    'pFont.Size = 7
                    'End If
                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    'Propiedades del Simbolo
                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False

                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)
                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto


        ElseIf sele_reporte = "Area Restringida" Then
            For contatexto = 1 To 8
                'Guardar el texto
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    conta_x = v_nombre_dm.Length
                    If conta_x >= 38 Then
                        posi_x = Mid(v_nombre_dm, 1, 38)
                        posi_x1 = Right(v_nombre_dm, (v_nombre_dm.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = v_nombre_dm
                    End If
                    pPoint.X = 17.1
                    'pPoint.Y = 8.1
                    pPoint.Y = 9.1
                    pPoint.X = 21.7
                    'pPoint.Y = 8.9
                    pPoint.Y = 9.9
                ElseIf contatexto = 2 Then
                    pTextElement.Text = v_codigo
                    pPoint.X = 17.9
                    'pPoint.Y = 7.5
                    pPoint.Y = 8.2
                    pPoint.X = 21.7
                    'pPoint.Y = 8.3
                    pPoint.Y = 9.2
                ElseIf contatexto = 3 Then
                    'posi_x = ""
                    'posi_x1 = ""
                    'pTextElement.Text = lista_dist
                    If caso_consulta = "AREA RESTRINGIDA" Then
                        conta_x = lista_dist_dema.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_dist_dema, 1, 38)
                            posi_x1 = Right(lista_dist_dema, (lista_dist_dema.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_dist_dema
                        End If
                    Else 'EVALUACIUON
                        conta_x = lista_dist.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_dist, 1, 38)
                            posi_x1 = Right(lista_dist, (lista_dist.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_dist
                        End If
                    End If
                    pPoint.X = 17.9
                    'pPoint.Y = 6.9
                    pPoint.Y = 7.6
                    pPoint.X = 21.7
                    'pPoint.Y = 7.7
                    pPoint.Y = 8.4

                ElseIf contatexto = 4 Then
                    'pTextElement.Text = lista_prov
                    If caso_consulta = "AREA RESTRINGIDA" Then
                        conta_x = lista_prov_dema.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_prov_dema, 1, 38)
                            posi_x1 = Right(lista_prov_dema, (lista_prov_dema.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_prov_dema
                        End If
                    Else
                        conta_x = lista_prov.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_prov, 1, 38)
                            posi_x1 = Right(lista_prov, (lista_prov.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_prov
                        End If
                    End If

                    pPoint.X = 17.9
                    'pPoint.Y = 6.3
                    pPoint.Y = 6.8
                    pPoint.X = 21.7
                    'pPoint.Y = 7.1
                    pPoint.Y = 7.6
                ElseIf contatexto = 5 Then
                    'pTextElement.Text = lista_depa
                    If caso_consulta = "AREA RESTRINGIDA" Then
                        conta_x = lista_depa_dema.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_depa_dema, 1, 38)
                            posi_x1 = Right(lista_depa_dema, (lista_depa_dema.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_depa_dema
                        End If
                    Else

                        conta_x = lista_depa.Length
                        If conta_x > 38 Then
                            posi_x = Mid(lista_depa, 1, 38)
                            posi_x1 = Right(lista_depa, (lista_depa.Length - 38))
                            pTextElement.Text = posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = lista_depa
                        End If
                    End If

                    pPoint.X = 17.9
                    'pPoint.Y = 5.7
                    pPoint.Y = 5.7
                    pPoint.X = 21.7
                    'pPoint.Y = 6.5
                    pPoint.Y = 6.7
                ElseIf contatexto = 6 Then
                    pTextElement.Text = fecha
                    pPoint.X = 16.1
                    pPoint.Y = 2.9
                    pPoint.X = 19.8
                    pPoint.Y = 2.1
                ElseIf contatexto = 7 Then
                    'pTextElement.Text = escala_plano_dema
                    'pPoint.X = 17.3
                    pPoint.X = 18.3
                    pPoint.Y = 2.9
                    'pPoint.X = 21.1
                    pPoint.X = 22.1
                    pPoint.Y = 2.1

                ElseIf contatexto = 8 Then
                    pTextElement.Text = v_zona_dm
                    pPoint.X = 5.6
                    pPoint.Y = 2.12
                    pPoint.X = 9.6
                    pPoint.Y = 1.32
                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv

                'Simbolo del texto
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'If contatexto = 6 Then
                pFont.Size = 7
                'ElseIf contatexto = 7 Then
                'pFont.Size = 6
                'ElseIf contatexto = 8 Then
                'pFont.Size = 6
                'Else
                'pFont.Size = 7
                'End If
                pFont.Bold = False
                pTxtSym.Font = pFont
                'Propiedades del Simbolo
                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False

                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Plano Venta" Then  'Reporte para plano Venta
            If sele_plano = "Formato A4" Then
                posi_y1 = 4.15
                posi_y = 4.85
                posi_y2 = 4.15
                posi_y3 = 4.85
            ElseIf sele_plano = "Formato A3" Then
                posi_y1 = 4.9
                posi_y = 5.6
                posi_y2 = 4.9
                posi_y3 = 5.6
            ElseIf sele_plano = "Formato A2" Then
                posi_y1 = 3.6
                posi_y = 4.3
                posi_y2 = 3.6
                posi_y3 = 4.3
            ElseIf sele_plano = "Formato A0" Then
                posi_y1 = 3.4
                posi_y = 4.1
                posi_y2 = 4.4
                posi_y3 = 4.1
            End If
            For contatexto = 1 To 15 'contador de textos
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "HOJA : " & valor_codhoja  ' cabecera hoja
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 15.5
                        pPoint.Y = 25.55
                        pPoint.X = 17.5
                        pPoint.Y = 26.55
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 23.5
                        pPoint.Y = 36.4
                        pPoint.X = 25.5
                        pPoint.Y = 38.4
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 35.5
                        pPoint.Y = 51.0
                        pPoint.X = 36.4
                        pPoint.Y = 53.6
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 71.5
                        pPoint.Y = 106.6
                        pPoint.X = 73.0
                        pPoint.Y = 108.4
                    End If

                ElseIf contatexto = 2 Then
                    pTextElement.Text = valor_nmhoja  'nombre hoja
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.2
                        pPoint.Y = posi_y1 - 0.5
                        pPoint.X = 12.9
                        pPoint.Y = posi_y - 0.5
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.7
                        pPoint.Y = posi_y1 - 0.5
                        pPoint.X = 18.7
                        pPoint.Y = posi_y - 0.5
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.5
                        pPoint.Y = posi_y1 + 3.05
                        pPoint.X = 26.6
                        pPoint.Y = posi_y + 3.05
                        posi_y = posi_y - 0.6
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 50.5
                        pPoint.Y = posi_y1 + 10.9
                        pPoint.X = 53.6
                        pPoint.Y = posi_y + 10.9
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 3 Then
                    pTextElement.Text = valor_codhoja  'codigo hoja
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.42
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.42
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.4
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.4
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.8
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.8
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 9.7
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 9.7
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 4 Then

                    If sele_plano = "Formato A4" Then  ' tama�o
                        pTextElement.Text = "A4"
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.33
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.33
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pTextElement.Text = "A3"
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.35
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.35
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pTextElement.Text = "A2"
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.4
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.4
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pTextElement.Text = "A0"
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 8.35
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 8.35
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 5 Then
                    pTextElement.Text = fecha   'fecha
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.29
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.29
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.3
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.3
                        posi_y = posi_y - 0.8
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.05
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.05
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 7.1
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 7.1
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 6 Then
                    ' pTextElement.Text = "PSAD-56"       'datum
                    pTextElement.Text = v_sistema
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 14.5
                        pPoint.Y = posi_y2 - 0.5
                        pPoint.X = 16.2
                        pPoint.Y = posi_y3 - 0.5
                        posi_y3 = posi_y3 - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 22.0
                        pPoint.Y = posi_y2 - 0.5
                        pPoint.X = 23.5
                        pPoint.Y = posi_y3 - 0.5
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 3.1
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 3.1
                        posi_y3 = posi_y3 - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 11.0
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 11.0
                        posi_y3 = posi_y3 - 0.5
                    End If

                ElseIf contatexto = 7 Then
                    pTextElement.Text = v_zona_dm   'zona
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 14.5
                        pPoint.Y = posi_y2 - 0.42
                        pPoint.X = 16.2
                        pPoint.Y = posi_y3 - 0.42
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 22.0
                        pPoint.Y = posi_y2 - 0.4
                        pPoint.X = 23.5
                        pPoint.Y = posi_y3 - 0.4
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 2.7
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 2.7
                        posi_y3 = posi_y3 - 0.53

                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 9.8
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 9.8
                        posi_y3 = posi_y3 - 0.53
                    End If

                ElseIf contatexto = 8 Then
                    pTextElement.Text = valor_zoncat   'zona catastral
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 14.5
                        pPoint.Y = posi_y2 - 0.33
                        pPoint.X = 16.2
                        pPoint.Y = posi_y3 - 0.33
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 22.0
                        pPoint.Y = posi_y2 - 0.35
                        pPoint.X = 23.5
                        pPoint.Y = posi_y3 - 0.35
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 2.3
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 2.3
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 8.4
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 8.4
                        posi_y3 = posi_y3 - 0.53
                    End If

                ElseIf contatexto = 9 Then
                    pTextElement.Text = "GUIDO VALDIVIA PONCE"   'autor
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 13.3
                        pPoint.Y = posi_y2 - 0.25
                        pPoint.X = 15.0
                        pPoint.Y = posi_y3 - 0.25
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 20.5
                        pPoint.Y = posi_y2 - 0.4
                        pPoint.X = 21.9
                        pPoint.Y = posi_y3 - 0.4
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 29.9
                        pPoint.Y = posi_y2 + 2.0
                        pPoint.X = 30.9
                        pPoint.Y = posi_y3 + 2.0
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 61.0
                        pPoint.Y = posi_y2 + 7.1
                        pPoint.X = 62.3
                        pPoint.Y = posi_y3 + 7.1
                    End If

                ElseIf contatexto = 10 Then  'DM Uso MInero
                    If canti_capa_actmin > 0 Then
                        pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 10.4
                        pPoint.Y = posi_y2 + 1.2
                        pPoint.X = 12.3
                        pPoint.Y = posi_y3 + 1.2
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.8
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.8
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 4.9
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 4.9
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 13.1
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 13.1
                    End If

                ElseIf contatexto = 11 Then  'DM Actividad Minera
                    If canti_capa_usomin > 0 Then
                        pTextElement.Text = canti_capa_usomin  'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 10.4
                        pPoint.Y = posi_y2 + 0.67
                        pPoint.X = 12.3
                        pPoint.Y = posi_y3 + 0.67
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.0
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.0
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 3.8
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 3.8
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 10.9
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 10.9
                    End If

                ElseIf contatexto = 12 Then  'Exploracion
                    If v_explo1 > 0 Then
                        pTextElement.Text = v_explo1  'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 5.9
                        pPoint.Y = posi_y2 + 1.12
                        pPoint.X = 8.0
                        pPoint.Y = posi_y3 + 1.12
                        posi_y3 = posi_y3 - 0.45
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 9.75
                        pPoint.Y = posi_y2 + 1.6
                        pPoint.X = 11.15
                        pPoint.Y = posi_y3 + 1.6
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 14.1
                        pPoint.Y = posi_y2 + 4.7
                        pPoint.X = 16.1
                        pPoint.Y = posi_y3 + 4.7
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 30.85
                        pPoint.Y = posi_y2 + 12.7
                        pPoint.X = 32.15
                        pPoint.Y = posi_y3 + 12.7
                    End If

                ElseIf contatexto = 13 Then  'Exploracion y explotacion
                    If v_explo_v_expta1 > 0 Then
                        pTextElement.Text = v_explo_v_expta1  'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 5.9
                        pPoint.Y = posi_y2 + 1.12
                        pPoint.X = 8.0
                        pPoint.Y = posi_y3 + 1.12
                        posi_y3 = posi_y3 - 0.4
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 9.75
                        pPoint.Y = posi_y2 + 1.0
                        pPoint.X = 11.15
                        pPoint.Y = posi_y3 + 1.0
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 14.1
                        pPoint.Y = posi_y2 + 3.9
                        pPoint.X = 16.1
                        pPoint.Y = posi_y3 + 3.9
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 30.85
                        pPoint.Y = posi_y2 + 10.9
                        pPoint.X = 32.15
                        pPoint.Y = posi_y3 + 10.9
                    End If

                ElseIf contatexto = 14 Then  'Explotacion
                    If v_expta1 > 0 Then
                        pTextElement.Text = v_expta1   'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 5.9
                        pPoint.Y = posi_y2 + 1.12
                        pPoint.X = 8.0
                        pPoint.Y = posi_y3 + 1.12
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 9.75
                        pPoint.Y = posi_y2 + 0.4
                        pPoint.X = 11.15
                        pPoint.Y = posi_y3 + 0.4
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 14.1
                        pPoint.Y = posi_y2 + 3.1
                        pPoint.X = 16.1
                        pPoint.Y = posi_y3 + 3.1
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 30.85
                        pPoint.Y = posi_y2 + 9.4
                        pPoint.X = 32.15
                        pPoint.Y = posi_y3 + 9.4
                    End If

                ElseIf contatexto = 15 Then  'CONTADOR A�O DAC
                    pTextElement.Text = lostr_Join_Codigos_AREA   'contador Capa Uso mi
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 9.83
                        pPoint.X = 11.53
                        pPoint.Y = 3.32
                        pPoint.Y = 3.12
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 15.05
                        pPoint.Y = posi_y2 - 0.3
                        pPoint.X = 16.45
                        pPoint.Y = posi_y3 - 0.3
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 21.1
                        pPoint.Y = posi_y2 + 2.2
                        pPoint.X = 23.1
                        pPoint.Y = posi_y3 + 2.2
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 44.25
                        pPoint.Y = posi_y2 + 7.57
                        pPoint.X = 45.55
                        pPoint.Y = posi_y3 + 7.57
                    End If

                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                'Simbolo del texto
                pTxtSym = New TextSymbol
                'fuente del texto

                'Dim pFont As IFontDisp
                pFont = New StdFont
                pFont.Name = "Tahoma"
                If sele_plano = "Formato A4" Then
                    pFont.Size = 4.0
                ElseIf sele_plano = "Formato A3" Then
                    pFont.Size = 6.0
                ElseIf sele_plano = "Formato A2" Then
                    pFont.Size = 8.5
                ElseIf sele_plano = "Formato A0" Then
                    pFont.Size = 14
                End If
                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym.Color = myColor
                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal

                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

            Next contatexto
        End If
    End Sub
  
    Public Sub AgregarTextosLayout_ant220312(ByVal sele_reporte As String, ByVal caso_simula As String)
        'PROGRAMA PARA AGREGAR TEXTOS EN LAYOUT
        '*******************************************
        Dim MyDate As Date
        MyDate = Now
        Dim fecha As String
        fecha = RellenarComodin(MyDate.Year, 2, "0") & "/" & RellenarComodin(MyDate.Month, 2, "0") & "/" & MyDate.Day

        Dim posi_y2 As Double
        Dim posi_y3 As Double

        Dim pPageLayout As IPageLayout
        Dim pActiveView As IActiveView
        Dim pGraphicsContainer As IGraphicsContainer
        Dim pElement As IElement
        Dim pTextElement As ITextElement
        Dim pPoint As IPoint
        Dim pTxtSym As IFormattedTextSymbol
        Dim contatexto As Integer
        Dim myColor As IRgbColor
        Dim pFont As IFontDisp

        pPageLayout = pMxDoc.PageLayout
        pPageLayout.Page.Units = esriUnits.esriCentimeters
        pActiveView = pPageLayout
        pGraphicsContainer = pPageLayout

        Dim posi_x As String
        Dim posi_x1 As String
        Dim conta_x As Long
        'Para obtener fecha

        'Dim fecha_plano As Date
        'fecha_plano = Date
        Dim posi_y As Double
        Dim posi_y1 As Double
        Dim pEnv As IEnvelope
        posi_y = posi_y_priori_CNM
        posi_y1 = posi_y_priori1_CNM
        Dim Nombre_urba As String = ""
        Dim Nombre_rese As String = ""

        'Colocando textos fijos al plano

        If sele_reporte = "Evaluacion" Then
            For contatexto = 1 To 16
                'Guardar el texto
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "Carta : " & v_carta_dm
                    pPoint.X = 8.1
                    pPoint.Y = 18.3
                    pPoint.X = 9.2
                    pPoint.Y = 17.8
                ElseIf contatexto = 2 Then
                    pTextElement.Text = "Fecha : " & fecha
                    pPoint.X = 13.8
                    pPoint.Y = 18.3
                    pPoint.X = 14.8
                    pPoint.Y = 17.8
                ElseIf contatexto = 3 Then
                    pTextElement.Text = "CODIGO DEL DM : " & v_codigo
                    pPoint.X = 14.1
                    pPoint.Y = 16.5
                    pPoint.X = 18.2
                    pPoint.Y = 17.3
                ElseIf contatexto = 4 Then
                    pTextElement.Text = "NOMBRE DEL DM : " & v_nombre_dm
                    pPoint.X = 14.1
                    pPoint.Y = 15.9
                    pPoint.X = 18.2
                    pPoint.Y = 16.7
                ElseIf contatexto = 5 Then
                    pTextElement.Text = "HECTAREA : " & " " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 14.1
                    pPoint.Y = 15.4
                    pPoint.X = 18.2
                    pPoint.Y = 16.1
                ElseIf contatexto = 6 Then
                    If Cuenta_an = 0 Then
                        pTextElement.Text = "DERECHOS PRIORITARIOS :  No Presenta DM Prioritarios"
                    Else
                        pTextElement.Text = "DERECHOS PRIORITARIOS : " & "(" & Cuenta_an & ")"
                    End If
                    pPoint.X = 15.7
                    pPoint.Y = 14.7
                    pPoint.X = 18.2
                    pPoint.Y = 15.5
                ElseIf contatexto = 7 Then
                    If v_tipo_exp = "RD" Then
                        If Cuenta_rd = 0 Then
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad "
                        Else
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad " & "(" & Cuenta_rd & ")"
                        End If
                        pPoint.X = 14.1
                        pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.X = 18.2
                        pPoint.Y = conta_rd_posi_y - 0.2
                    Else
                        pPoint.X = 14.1
                        'pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.Y = 14.7
                        pPoint.X = 18.2
                        'pPoint.Y = conta_rd_posi_y - 0.2
                        pPoint.Y = 15.5

                    End If
                ElseIf contatexto = 8 Then
                    If Cuenta_po = 0 Then
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA : No Presenta "
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES :  No Presenta DM Posteriores"
                        End If
                    Else
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA: " & "(" & Cuenta_rd & ")"
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES : " & "(" & Cuenta_po & ")"
                        End If
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = conta_po_posi_y1 - 0.2
                    pPoint.X = 18.2
                    pPoint.Y = conta_po_posi_y - 0.2

                ElseIf contatexto = 9 Then
                    If Cuenta_si = 0 Then
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  No Presenta DM Simultaneos"
                    Else
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  " & "(" & Cuenta_si & ")"
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = conta_si_posi_y1 - 0.15
                    pPoint.X = 18.2
                    pPoint.Y = conta_si_posi_y - 0.15

                ElseIf contatexto = 10 Then
                    If Cuenta_ex = 0 Then
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  No Presenta DM Extinguidos"
                    Else
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  " & "(" & Cuenta_ex & ")"
                    End If

                    pPoint.X = 14.1
                    pPoint.Y = conta_ex_posi_y1 - 0.15
                    pPoint.X = 18.2
                    pPoint.Y = conta_ex_posi_y - 0.15

                ElseIf contatexto = 11 Then

                    pTextElement.Text = "CATASTRO NO MINERO"
                    pPoint.X = 14.1
                    'pPoint.Y = posi_y1 - 0.4
                    pPoint.Y = posi_y1 - 0.6
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 0.6
                ElseIf contatexto = 12 Then
                    If lista_urba = "" Then
                        pTextElement.Text = "Zonas Urbanas :" & " No se encuentra superpuesto a un Area urbana"
                    Else
                        conta_x = lista_urba.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_urba, 1, 60)
                            posi_x1 = Right(lista_urba, (lista_urba.Length - 60))
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & lista_urba
                        End If
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 1
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1


                ElseIf contatexto = 13 Then
                    If lista_rese = "" Then
                        pTextElement.Text = "Zonas Reservadas :" & " No se encuentra superpuesto a un Area de Reserva"
                    Else
                        conta_x = lista_urba.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_rese, 1, 60)
                            posi_x1 = Right(lista_urba, (lista_rese.Length - 60))
                            pTextElement.Text = "Zonas Reservadas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Reservadas :      " & "  " & lista_rese
                        End If
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 1.4
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1.4
                ElseIf contatexto = 14 Then
                    pTextElement.Text = "L�mites fronterizos (Fuente IGN): Distancia de la l�nea de frontera de " & distancia_fron & " (Km.)"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 1.8
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1.8
                ElseIf contatexto = 15 Then

                    pTextElement.Text = "                                LISTADO DE DERECHOS MINEROS"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.2
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.2

                ElseIf contatexto = 16 Then
                    pTextElement.Text = "N�   NOMBRE                         CODIGO      ZONA   TE      TP   PUBL   INCOR   SUST"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.6
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.6
                    posi_y1_list = posi_y1 - 2.9
                    posi_y2_list = posi_y - 2.9

                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'If ((contatexto = 9) Or (contatexto = 10) Or (contatexto = 11)) Then
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    'pFont.Size = 8
                    pFont.Size = 8
                Else
                    'pFont.Size = 9
                    pFont.Size = 6
                End If

                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    myColor.RGB = RGB(197, 0, 255)
                ElseIf ((contatexto = 1) Or (contatexto = 2) Or (contatexto = 6) Or (contatexto = 7) Or (contatexto = 8) Or (contatexto = 9) Or (contatexto = 10)) Then
                    myColor.RGB = RGB(71, 61, 255)
                ElseIf contatexto = 11 Then
                    myColor.RGB = RGB(230, 0, 0)
                ElseIf ((contatexto = 12) Or (contatexto = 13) Or (contatexto = 14) Or (contatexto = 15)) Then
                    myColor.RGB = RGB(0, 0, 0)
                ElseIf (contatexto = 16) Then
                    myColor.RGB = RGB(71, 61, 255)
                End If
                pTxtSym.Color = myColor

                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                If contatexto = 1 Or contatexto = 2 Or contatexto = 3 Or contatexto = 4 Or contatexto = 5 Or contatexto = 6 Then
                    pGraphicsContainer.AddElement(pTextElement, 1)
                ElseIf contatexto = 7 Then
                    If v_tipo_exp = "RD" Then
                        If v_posi_rd = False Or Cuenta_rd > 0 Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                ElseIf contatexto = 8 Then
                    If v_posi_po = False Or Cuenta_po > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 9 Then
                    If v_posi_si = False Or Cuenta_si > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 10 Then
                    If v_posi_ex = False Or Cuenta_ex > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                Else
                    If contador_hojas < 1 Then
                        If v_posi_pr = False And v_posi_po = False And v_posi_si = False And v_posi_ex = False Or v_posi_rd Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                End If
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Cuadricula" Then
            Dim canti_cuadri As Integer
            canti_cuadri = v_area_eval / 100

            For contatexto = 1 To 8
                'Guardar el texto
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "Carta : " & v_carta_dm
                    pPoint.X = 8.1
                    pPoint.Y = 18.5
                    pPoint.X = 9.2
                    pPoint.Y = 18.0
                ElseIf contatexto = 2 Then
                    pTextElement.Text = "Fecha : " & fecha
                    pPoint.X = 13.8
                    pPoint.Y = 18.5
                    pPoint.X = 14.8
                    pPoint.Y = 18.0
                ElseIf contatexto = 3 Then
                    pTextElement.Text = "CODIGO DEL DM : " & v_codigo
                    pPoint.X = 14.4
                    pPoint.Y = 16.5
                    pPoint.X = 18.5
                    pPoint.Y = 17.3
                ElseIf contatexto = 4 Then
                    pTextElement.Text = "NOMBRE DEL DM : " & v_nombre_dm
                    pPoint.X = 14.4
                    pPoint.Y = 15.9
                    pPoint.X = 18.5
                    pPoint.Y = 16.7
                ElseIf contatexto = 5 Then
                    pTextElement.Text = "HECTAREA : " & " " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 14.4
                    pPoint.Y = 15.4
                    pPoint.X = 18.5
                    pPoint.Y = 16.1
                ElseIf contatexto = 6 Then
                    pTextElement.Text = "* EL PRESENTE PETITORIO HA SIDO FORMULADO POR : " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 17.8
                    pPoint.Y = 11.2
                    pPoint.X = 18.8
                    pPoint.Y = 10.8

                ElseIf contatexto = 7 Then
                    pTextElement.Text = "* ESTA CONFORMADO POR  " & canti_cuadri & " CUADRICULAS DE 100.00 Ha."
                    pPoint.X = 17.8
                    pPoint.Y = 10.6
                    pPoint.X = 18.8
                    pPoint.Y = 10.2
                ElseIf contatexto = 8 Then
                    pTextElement.Text = "* LAS CUADRICULAS SON  " & nombre_datos
                    pPoint.X = 17.8
                    pPoint.Y = 10.0
                    pPoint.X = 18.8
                    pPoint.Y = 9.6
                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTxtSym = New TextSymbol
                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"

                pFont.Size = 6
                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    myColor.RGB = RGB(197, 0, 255)
                ElseIf ((contatexto = 1) Or (contatexto = 2)) Then
                    myColor.RGB = RGB(71, 61, 255)
                ElseIf ((contatexto = 6) Or (contatexto = 7) Or (contatexto = 8)) Then
                    myColor.RGB = RGB(0, 0, 255)

                End If
                pTxtSym.Color = myColor
                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Carta" Then
            Dim posi_y_c As Double
            Dim posi_y_c1 As Double
            posi_y_c = 12.35
            posi_y_c1 = 12.03

            'Guardar el texto
            For contatexto = 1 To 13
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then

                    'pTextElement.Text = "DERECHO MINERO:  " & v_nombre_dm
                    conta_x = v_nombre_dm.Length
                    'If conta_x > 45 Then
                    If conta_x > 60 Then
                        posi_x = Mid(v_nombre_dm, 1, 60)
                        posi_x1 = Right(v_nombre_dm, (v_nombre_dm.Length - 60))
                        pTextElement.Text = "DERECHO MINERO:  " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "DERECHO MINERO:  " & v_nombre_dm
                    End If
                    pPoint.X = 19.1
                    'pPoint.Y = 12.353
                    pPoint.Y = posi_y_c
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                    'pPoint.Y = 12.003

                ElseIf contatexto = 2 Then
                    conta_x = lista_nmhojas.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_nmhojas, 1, 60)
                        posi_x1 = Right(lista_nmhojas, (lista_nmhojas.Length - 60))
                        pTextElement.Text = "Nombre de la Carta : " & "  " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "Nombre de la Carta : " & "  " & lista_nmhojas
                    End If

                    pPoint.X = 19.1
                    'pPoint.Y = 11.984
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 11.634
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 3 Then
                    'pTextElement.Text = "Numero de la Carta :  " & carta_v
                    conta_x = lista_cd_cartas.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_cd_cartas, 1, 60)
                        posi_x1 = Right(lista_cd_cartas, (lista_cd_cartas.Length - 60))
                        pTextElement.Text = "N�mero de la Carta : " & "  " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "N�mero de la Carta : " & "  " & lista_cd_cartas
                    End If
                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 4 Then
                    pTextElement.Text = "Escala                  :" & "    " & "1/100 000"
                    pPoint.X = 19.1
                    'pPoint.Y = 11.341
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 10.991

                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 5 Then
                    'pTextElement.Text = "Zona                    :" & "   " & "ZONA"
                    pTextElement.Text = "Zona                    : " & "   " & v_zona_dm
                    pPoint.X = 19.1
                    'pPoint.Y = 11.02
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 10.68
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 6 Then
                    pTextElement.Text = "UBICACION"
                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 7 Then
                    'pTextElement.Text = "Distrito                :   " & lista_dist
                    conta_x = lista_dist.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_dist, 1, 45)
                        posi_x1 = Right(lista_dist, (lista_dist.Length - 60))
                        pTextElement.Text = "DISTRITOS (S)       : " & "   " & posi_x & vbNewLine & posi_x1
                        posi_y_c = posi_y_c + 0.3
                        posi_y_c1 = posi_y_c1 - 0.3
                    Else
                        pTextElement.Text = "DISTRITOS (S)       : " & "   " & lista_dist
                    End If

                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 8 Then
                    conta_x = lista_prov.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_prov, 1, 60)
                        posi_x1 = Right(lista_prov, (lista_prov.Length - 60))
                        pTextElement.Text = "PROVINCIA (S)       : " & "   " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "PROVINCIA (S)       : " & "   " & lista_prov
                    End If

                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 9 Then
                    conta_x = lista_depa.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_depa, 1, 60)
                        posi_x1 = Right(lista_depa, (lista_depa.Length - 60))
                        pTextElement.Text = "DEPARTAMENTO (S) : " & "   " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "DEPARTAMENTO (S) : " & "   " & lista_depa
                    End If

                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 10 Then
                    pTextElement.Text = "OBSERVACIONES"
                    pPoint.X = 19.1
                    'pPoint.Y = 9.13
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 8.9
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1

                ElseIf contatexto = 11 Then
                    pTextElement.Text = "No existen Observaciones"
                    pPoint.X = 19.1

                    pPoint.X = 19.1
                    'pPoint.Y = 2.1
                    pPoint.Y = posi_y_c - 0.3
                    pPoint.Y = posi_y_c1 - 0.3

                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3
                    posi_y_m = posi_y_c
                    posi_y1_m = posi_y_c1

                    'ElseIf contatexto = 11 Then
                    '    pTextElement.Text = "ELIPSOIDE...............................................INTERNACIONAL"
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.48
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.48

                    'ElseIf contatexto = 12 Then
                    '    pTextElement.Text = "PROYECCION.................................TRANSVERSA DE MERCATOR"
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.289
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.289

                    'ElseIf contatexto = 13 Then
                    '    pTextElement.Text = "DATUM VERTICAL..............................................NIVEL MEDIO DEL MAR"
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.48
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.48

                    'ElseIf contatexto = 14 Then
                    '    pTextElement.Text = "DATUM HORIZONTAL.........DATUM PROVISIONAL SUDAMERICANO DE 1956"
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.289
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.289
                ElseIf contatexto = 12 Then
                    pTextElement.Text = fecha
                    pPoint.X = 16.0
                    pPoint.Y = 2.9
                    pPoint.X = 19.7
                    pPoint.Y = 2.1
                ElseIf contatexto = 13 Then
                    pTextElement.Text = escala_plano_carta
                    'pPoint.X = 17.3
                    pPoint.X = 19.4
                    pPoint.Y = 2.9
                    'pPoint.X = 21.1
                    pPoint.X = 21.9
                    pPoint.Y = 2.1
                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                'Simbolo del texto
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'color del texto
                'Dim pColor As IRgbColor
                myColor = New RgbColor

                'If contatexto = 1 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'ElseIf ((contatexto > 10) And (contatexto < 15)) Then
                'pFont.Size = 5.5
                'ElseIf contatexto = 6 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'ElseIf contatexto = 10 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'Else
                'pFont.Size = 8
                'End If

                If contatexto = 1 Then
                    pFont.Size = 8
                    myColor.RGB = RGB(0, 0, 250)
                ElseIf ((contatexto > 10) And (contatexto < 15)) Then
                    pFont.Size = 5.5
                ElseIf contatexto = 6 Then
                    pFont.Size = 9
                    myColor.RGB = RGB(0, 0, 250)
                ElseIf contatexto = 10 Then
                    pFont.Size = 9
                    myColor.RGB = RGB(0, 0, 250)
                    'ElseIf contatexto = 11 Or contatexto = 12 Or contatexto = 13 Or contatexto = 14 Then
                    'pFont.Size = 3
                    'myColor.RGB = RGB(0, 0, 250)
                Else
                    pFont.Size = 6
                End If


                pFont.Bold = False
                pTxtSym.Font = pFont
                pTxtSym.Color = myColor

                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Plano_variado" Then
            If tipo_opcion = "1" Then
                lista_nmhojas = v_nombre
                lista_cartas = v_codigo
            End If
            Dim posi_y_c As Double
            Dim posi_y_c1 As Double

            If sele_plano1 = "Plano A4 Vertical" Then
                posi_y_c = 27.18
                posi_y_c1 = 27.08

            ElseIf sele_plano1 = "Plano A4 Horizontal" Then
                posi_y_c = 11.8
                posi_y_c1 = 11.5

            ElseIf sele_plano1 = "Plano A3 Vertical" Then
                posi_y_c = 37.65
                posi_y_c1 = 37.55
                'posi_y_c = 40.45
                'posi_y_c1 = 40.35
            ElseIf sele_plano1 = "Plano A3 Horizontal" Then
                posi_y_c = 17.05
                posi_y_c1 = 16.75

            ElseIf sele_plano1 = "Plano A2 Vertical" Then
                posi_y_c = 54.7
                posi_y_c1 = 54.5

            ElseIf sele_plano1 = "Plano A2 Horizontal" Then
                posi_y_c = 24.0
                posi_y_c1 = 23.9

            ElseIf sele_plano1 = "Plano A1 Vertical" Then
                posi_y_c = 74.8
                posi_y_c1 = 74.3
                'posi_y_c = 81.0
                'posi_y_c1 = 80.5

            ElseIf sele_plano1 = "Plano A1 Horizontal" Then
                posi_y_c = 33.95
                posi_y_c1 = 33.65
            ElseIf sele_plano1 = "Plano A0 Vertical" Then
                posi_y_c = 108.4
                posi_y_c1 = 107.4

            ElseIf sele_plano1 = "Plano A0 Horizontal" Then
                posi_y_c = 48.65
                posi_y_c1 = 47.65
            End If
            If sele_plano1 = "Plano A4 Horizontal" Then
                For contatexto = 1 To 13
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then      'exploracion
                        pTextElement.Text = v_explo1
                        pPoint.X = 24.92
                        pPoint.X = 26.0
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c + 0.6
                        posi_y_c1 = posi_y_c1 + 0.6

                    ElseIf contatexto = 2 Then      'explotacion
                        pTextElement.Text = v_expta1

                        pPoint.X = 24.92
                        pPoint.X = 26.0
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1
                    ElseIf contatexto = 3 Then      'exploracion - explotacion
                        pTextElement.Text = v_explo_v_expta1

                        pPoint.X = 24.92
                        pPoint.X = 26.0
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c + 0.1
                        posi_y_c1 = posi_y_c1 + 0.1

                    ElseIf contatexto = 4 Then      'cierre
                        pTextElement.Text = v_cierre1

                        pPoint.X = 37.02
                        pPoint.X = 38.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c + 1.5
                        posi_y_c1 = posi_y_c1 + 1.5


                    ElseIf contatexto = 5 Then      'hoja

                        pTextElement.Text = lista_nmhojas

                        pPoint.X = 22.5
                        pPoint.X = 23.1
                        pPoint.Y = 2.75
                        pPoint.Y = 2.55

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas

                        pPoint.X = 22.8
                        pPoint.X = 23.4
                        pPoint.Y = 2.35
                        pPoint.Y = 2.15

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2

                        pPoint.X = 22.8
                        pPoint.X = 23.4
                        pPoint.Y = 1.95
                        pPoint.Y = 1.75

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 22.8
                        pPoint.X = 23.4
                        pPoint.Y = 1.5
                        pPoint.Y = 1.3

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm

                        pPoint.X = 25.4
                        pPoint.X = 26.0
                        pPoint.Y = 2.35
                        pPoint.Y = 2.15
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 25.4
                        pPoint.X = 26.0
                        pPoint.Y = 1.5
                        pPoint.Y = 1.3

                    ElseIf contatexto = 11 Then     'escala
                        pTextElement.Text = escalaf

                        pPoint.X = 24.1
                        pPoint.X = 24.8
                        pPoint.Y = 11.35
                        pPoint.Y = 13.15

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 26.57
                        pPoint.X = 27.55
                        pPoint.Y = 6.6
                        pPoint.Y = 8.4

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 26.57
                        pPoint.X = 27.55
                        pPoint.Y = 7.3
                        pPoint.Y = 9.1

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 4.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 6

                    Else
                        pFont.Size = 5.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A4 Vertical" Then
                For contatexto = 1 To 13
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 11.35
                        pPoint.X = 12.35
                        pPoint.Y = 27.0
                        pPoint.Y = 26.9

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 11.35
                        pPoint.X = 12.35
                        pPoint.Y = 26.45
                        pPoint.Y = 26.35

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 11.35
                        pPoint.X = 12.35
                        pPoint.Y = 25.95
                        pPoint.Y = 25.85

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 44.4
                        pPoint.X = 45.4
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.41
                        posi_y_c1 = posi_y_c1 - 0.41

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 14.05
                        pPoint.X = 15.05
                        pPoint.Y = 23.27
                        pPoint.Y = 23.17

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 14.4
                        pPoint.X = 15.5
                        pPoint.Y = 22.87
                        pPoint.Y = 22.77

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 14.4
                        pPoint.X = 15.5
                        pPoint.Y = 22.5
                        pPoint.Y = 22.4

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 14.4
                        pPoint.X = 15.5
                        pPoint.Y = 22.0
                        pPoint.Y = 21.9

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 17.5
                        pPoint.X = 18.1
                        pPoint.Y = 22.87
                        pPoint.Y = 22.77

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 17.5
                        pPoint.X = 18.1
                        pPoint.Y = 22.0
                        pPoint.Y = 21.9

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 14.8
                        pPoint.X = 15.4
                        pPoint.Y = 28.15
                        pPoint.Y = 28.05

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 19.03
                        pPoint.X = 19.63
                        pPoint.Y = 25.05
                        pPoint.Y = 26.85

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 19.03
                        pPoint.X = 19.63
                        pPoint.Y = 24.45
                        pPoint.Y = 26.25

                    End If

                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor

                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 4.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 6

                    Else
                        pFont.Size = 5.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A3 Horizontal" Then
                For contatexto = 1 To 13
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 35.5
                        pPoint.X = 36.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 35.5
                        pPoint.X = 36.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 35.5
                        pPoint.X = 36.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1

                        pPoint.X = 48.8
                        pPoint.X = 49.8
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9


                    ElseIf contatexto = 5 Then      'HOJA
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 31.25
                        pPoint.X = 32.25
                        pPoint.Y = 3.6
                        pPoint.Y = 3.35

                    ElseIf contatexto = 6 Then      'CODIGO
                        pTextElement.Text = lista_cartas
                        pPoint.X = 31.75
                        pPoint.X = 32.75
                        pPoint.Y = 3.0
                        pPoint.Y = 2.78

                    ElseIf contatexto = 7 Then      'TAMA�O
                        pTextElement.Text = sele_plano2
                        pPoint.X = 31.75
                        pPoint.X = 32.75
                        pPoint.Y = 2.4
                        pPoint.Y = 2.15

                    ElseIf contatexto = 8 Then      'FECHA
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 31.75
                        pPoint.X = 32.75
                        pPoint.Y = 1.8
                        pPoint.Y = 1.55

                    ElseIf contatexto = 9 Then      'ZONA
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 35.95
                        pPoint.X = 36.55
                        pPoint.Y = 3.0
                        pPoint.Y = 2.78
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 35.95
                        pPoint.X = 36.55
                        pPoint.Y = 1.8
                        pPoint.Y = 1.55

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 35.35
                        pPoint.X = 35.05
                        pPoint.Y = 19.35
                        pPoint.Y = 18.35

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 37.07
                        pPoint.X = 38.67
                        pPoint.Y = 10.85
                        pPoint.Y = 12.65

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 37.07
                        pPoint.X = 38.67
                        pPoint.Y = 9.9
                        pPoint.Y = 11.7

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 5.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 7.5

                    Else
                        pFont.Size = 6.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A3 Vertical" Then
                For contatexto = 1 To 13
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 17.07
                        pPoint.X = 18.07
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.6
                        posi_y_c1 = posi_y_c1 - 0.6

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 17.07
                        pPoint.X = 18.07
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.65
                        posi_y_c1 = posi_y_c1 - 0.65

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 17.07
                        pPoint.X = 18.07
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.65
                        posi_y_c1 = posi_y_c1 - 0.65

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 37.92
                        pPoint.X = 38.92
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9

                    ElseIf contatexto = 5 Then      'HOJA
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 21.8
                        pPoint.X = 22.8
                        pPoint.Y = 32.85
                        pPoint.Y = 32.65

                    ElseIf contatexto = 6 Then      'CODIGO
                        pTextElement.Text = lista_cartas
                        pPoint.X = 22.1
                        pPoint.X = 23.1
                        pPoint.Y = 32.35
                        pPoint.Y = 32.15

                    ElseIf contatexto = 7 Then      'TAMA�O
                        pTextElement.Text = sele_plano2
                        pPoint.X = 22.1
                        pPoint.X = 23.1
                        pPoint.Y = 31.85
                        pPoint.Y = 31.65

                    ElseIf contatexto = 8 Then      'FECHA
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 22.1
                        pPoint.X = 23.1
                        pPoint.Y = 32.35
                        pPoint.Y = 31.15

                    ElseIf contatexto = 9 Then      'ZONA
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 25.2
                        pPoint.X = 26.2
                        pPoint.Y = 32.35
                        pPoint.Y = 32.15
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 25.2
                        pPoint.X = 26.2
                        pPoint.Y = 31.35
                        pPoint.Y = 31.15
                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 22.35
                        pPoint.X = 23.05
                        pPoint.Y = 44.05
                        pPoint.Y = 39.05

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 26.47
                        pPoint.X = 28.07
                        pPoint.Y = 35.8
                        pPoint.Y = 37.6

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 26.47
                        pPoint.X = 28.07
                        pPoint.Y = 34.95
                        pPoint.Y = 36.75

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 5.2
                    ElseIf contatexto = 11 Then
                        pFont.Size = 7.5
                    Else
                        pFont.Size = 6.2
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A2 Horizontal" Then
                For contatexto = 1 To 13
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 51.35
                        pPoint.X = 52.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.15
                        posi_y_c1 = posi_y_c1 - 1.15

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 51.35
                        pPoint.X = 52.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 51.35
                        pPoint.X = 52.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.15
                        posi_y_c1 = posi_y_c1 - 1.15

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 73.25
                        pPoint.X = 74.25
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.4
                        posi_y_c1 = posi_y_c1 - 1.4

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 45.4
                        pPoint.X = 46.4
                        pPoint.Y = 4.9
                        pPoint.Y = 4.5

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 46.3
                        pPoint.X = 47.3
                        pPoint.Y = 4.0
                        pPoint.Y = 3.6

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 46.3
                        pPoint.X = 47.3
                        pPoint.Y = 3.2
                        pPoint.Y = 2.8

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 46.3
                        pPoint.X = 47.3
                        pPoint.Y = 2.3
                        pPoint.Y = 1.9

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 51.3
                        pPoint.X = 52.3
                        pPoint.Y = 4.0
                        pPoint.Y = 3.6

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 51.3
                        pPoint.X = 52.3
                        pPoint.Y = 2.3
                        pPoint.Y = 1.9

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 48.6
                        pPoint.X = 49.6
                        pPoint.Y = 27.05
                        pPoint.Y = 26.05

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 53.87
                        pPoint.X = 55.47
                        pPoint.Y = 16.2
                        pPoint.Y = 18.0

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 53.87
                        pPoint.X = 55.47
                        pPoint.Y = 14.8
                        pPoint.Y = 16.6

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 9
                    ElseIf contatexto = 11 Then
                        pFont.Size = 13

                    Else
                        pFont.Size = 10
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A2 Vertical" Then
                For contatexto = 1 To 13
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 24.7
                        pPoint.X = 25.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 24.7
                        pPoint.X = 25.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 24.7
                        pPoint.X = 25.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 44.7
                        pPoint.X = 45.7
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 30.3
                        pPoint.X = 31.3
                        pPoint.Y = 48.0
                        pPoint.Y = 46.7

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 46.2
                        pPoint.Y = 45.9

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 45.4
                        pPoint.Y = 45.1

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 44.6
                        pPoint.Y = 44.3

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm

                        pPoint.X = 35.95
                        pPoint.X = 36.95
                        pPoint.Y = 46.2
                        pPoint.Y = 45.9
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 35.95
                        pPoint.X = 36.95
                        pPoint.Y = 44.6
                        pPoint.Y = 44.3
                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 30.9
                        pPoint.X = 31.9
                        pPoint.Y = 56.3
                        pPoint.Y = 55.9

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 38.4
                        pPoint.X = 40.0
                        pPoint.Y = 51.8
                        pPoint.Y = 53.6

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 38.4
                        pPoint.X = 40.0
                        pPoint.Y = 50.6
                        pPoint.Y = 52.4

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 9
                    ElseIf contatexto = 11 Then
                        pFont.Size = 13

                    Else
                        pFont.Size = 10
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A1 Horizontal" Then
                For contatexto = 1 To 13
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 73.05
                        pPoint.X = 74.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.5
                        posi_y_c1 = posi_y_c1 - 1.5

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 73.05
                        pPoint.X = 74.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.5
                        posi_y_c1 = posi_y_c1 - 1.5

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 73.05
                        pPoint.X = 74.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.7
                        posi_y_c1 = posi_y_c1 - 1.7

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 93.05
                        pPoint.X = 94.05
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.9
                        posi_y_c1 = posi_y_c1 - 1.9

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 64.0
                        pPoint.X = 65.3
                        pPoint.Y = 6.3
                        pPoint.Y = 5.6

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 64.7
                        pPoint.X = 65.9
                        pPoint.Y = 5.1
                        pPoint.Y = 4.4

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 64.7
                        pPoint.X = 65.9
                        pPoint.Y = 3.9
                        pPoint.Y = 3.2

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 64.7
                        pPoint.X = 65.9
                        pPoint.Y = 2.6
                        pPoint.Y = 1.9

                    ElseIf contatexto = 9 Then      'zona
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 73.1
                        pPoint.X = 74.1
                        pPoint.Y = 5.1
                        pPoint.Y = 4.4

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 73.1
                        pPoint.X = 74.1
                        pPoint.Y = 2.6
                        pPoint.Y = 1.9

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 70.1
                        pPoint.X = 71.1
                        pPoint.Y = 37.85
                        pPoint.Y = 36.85

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 77.0
                        pPoint.X = 78.6
                        pPoint.Y = 23.7
                        pPoint.Y = 25.5

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 77.0
                        pPoint.X = 78.6
                        pPoint.Y = 21.8
                        pPoint.Y = 23.6

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 12.3
                    ElseIf contatexto = 11 Then
                        pFont.Size = 16

                    Else
                        pFont.Size = 15
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A1 Vertical" Then
                For contatexto = 1 To 13
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 35.8
                        pPoint.X = 36.8
                        pPoint.Y = 76.9
                        pPoint.Y = 76.4

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 35.8
                        pPoint.X = 36.8
                        pPoint.Y = 75.5
                        pPoint.Y = 75.0

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 35.8
                        pPoint.X = 36.8
                        pPoint.Y = 74.0
                        pPoint.Y = 73.5

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 75.8
                        pPoint.X = 76.8
                        pPoint.Y = 72.6
                        pPoint.Y = 72.1

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 42.9
                        pPoint.X = 44.1
                        pPoint.Y = 66.6
                        pPoint.Y = 66.1

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 43.9
                        pPoint.X = 44.9
                        pPoint.Y = 65.5
                        pPoint.Y = 65.0

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2

                        pPoint.X = 43.9
                        pPoint.X = 44.9
                        pPoint.Y = 64.4
                        pPoint.Y = 63.9

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 43.9
                        pPoint.X = 44.9
                        pPoint.Y = 63.2
                        pPoint.Y = 62.7

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zona_dm

                        pPoint.X = 51.5
                        pPoint.X = 52.5
                        pPoint.Y = 65.5
                        pPoint.Y = 65.0
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 51.5
                        pPoint.X = 52.5
                        pPoint.Y = 63.2
                        pPoint.Y = 62.7

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 44.1
                        pPoint.X = 45.1
                        pPoint.Y = 80.9
                        pPoint.Y = 79.9

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 55.1
                        pPoint.X = 56.7
                        pPoint.Y = 74.8
                        pPoint.Y = 76.6

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 55.1
                        pPoint.X = 56.7
                        pPoint.Y = 72.9
                        pPoint.Y = 74.7

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 12.3
                    ElseIf contatexto = 11 Then
                        pFont.Size = 16

                    Else
                        pFont.Size = 15
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A0 Horizontal" Then
                For contatexto = 1 To 13
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 103.75
                        pPoint.X = 104.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 103.75
                        pPoint.X = 104.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 103.75
                        pPoint.X = 104.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 143.75
                        pPoint.X = 144.75
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.2
                        posi_y_c1 = posi_y_c1 - 2.2

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 91.7
                        pPoint.X = 92.7
                        pPoint.Y = 15.4
                        pPoint.Y = 9.4

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 92.7
                        pPoint.X = 93.9
                        pPoint.Y = 8.7
                        pPoint.Y = 7.7

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 92.7
                        pPoint.X = 93.9
                        pPoint.Y = 7.1
                        pPoint.Y = 6.1

                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 92.7
                        pPoint.X = 93.9
                        pPoint.Y = 5.15
                        pPoint.Y = 4.45

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 103.7
                        pPoint.X = 104.7
                        pPoint.Y = 8.85
                        pPoint.Y = 7.85

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 103.7
                        pPoint.X = 104.7
                        pPoint.Y = 5.15
                        pPoint.Y = 4.45

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 97.25
                        pPoint.X = 98.45
                        pPoint.Y = 53.1
                        pPoint.Y = 52.1

                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 109.25
                        pPoint.X = 110.85
                        pPoint.Y = 34.15
                        pPoint.Y = 35.95

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 109.25
                        pPoint.X = 110.85
                        pPoint.Y = 31.6
                        pPoint.Y = 33.4

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 17
                    ElseIf contatexto = 11 Then
                        pFont.Size = 22.5
                    Else
                        pFont.Size = 21
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A0 Vertical" Then
                For contatexto = 1 To 13
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 52.4
                        pPoint.X = 52.6
                        pPoint.Y = 108.95
                        pPoint.Y = 107.95

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 52.4
                        pPoint.X = 52.6
                        pPoint.Y = 106.9
                        pPoint.Y = 105.9

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 52.4
                        pPoint.X = 52.6
                        pPoint.Y = 104.9
                        pPoint.Y = 103.9

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 92.3
                        pPoint.X = 92.5
                        pPoint.Y = 98.8
                        pPoint.Y = 97.8

                    ElseIf contatexto = 5 Then      'hoja
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 61.2
                        pPoint.X = 62.4
                        pPoint.Y = 95.2
                        pPoint.Y = 94.2

                    ElseIf contatexto = 6 Then      'codigo
                        pTextElement.Text = lista_cartas
                        pPoint.X = 62.8
                        pPoint.X = 63.8
                        pPoint.Y = 93.6
                        pPoint.Y = 92.6

                    ElseIf contatexto = 7 Then      'tama�o
                        pTextElement.Text = sele_plano2
                        pPoint.X = 62.8
                        pPoint.X = 63.8
                        pPoint.Y = 92.0
                        pPoint.Y = 91.0
                    ElseIf contatexto = 8 Then      'fecha
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 62.8
                        pPoint.X = 63.8
                        pPoint.Y = 90.25
                        pPoint.Y = 89.25

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zona_dm
                        pPoint.X = 72.3
                        pPoint.X = 74.3
                        pPoint.Y = 93.6
                        pPoint.Y = 92.6
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 72.3
                        pPoint.X = 74.3
                        pPoint.Y = 90.25
                        pPoint.Y = 89.25

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 62.9
                        pPoint.X = 63.9
                        pPoint.Y = 114.1
                        pPoint.Y = 113.1
                    ElseIf contatexto = 12 Then  'DM Uso MInero
                        If canti_capa_actmin > 0 Then
                            pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 79.0
                        pPoint.X = 80.6
                        pPoint.Y = 109.3
                        pPoint.Y = 108.3

                    ElseIf contatexto = 13 Then  'Explotacion
                        If v_expta1 > 0 Then
                            pTextElement.Text = v_expta1   'contador Capa Uso mi
                        Else
                            pTextElement.Text = "0"
                        End If
                        pPoint.X = 79.0
                        pPoint.X = 80.6
                        pPoint.Y = 106.8
                        pPoint.Y = 105.8

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol
                    'fuente del texto
                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto
                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 17
                    ElseIf contatexto = 11 Then
                        pFont.Size = 22.5
                    Else
                        pFont.Size = 21
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            End If
        ElseIf sele_reporte = "Demarca" Then
            For contatexto = 1 To 8
                'Guardar el texto
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    conta_x = v_nombre_dm.Length
                    If conta_x >= 38 Then
                        posi_x = Mid(v_nombre_dm, 1, 38)
                        posi_x1 = Right(v_nombre_dm, (v_nombre_dm.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = v_nombre_dm
                    End If
                    pPoint.X = 17.1
                    'pPoint.Y = 8.1
                    pPoint.Y = 9.1
                    pPoint.X = 21.7
                    'pPoint.Y = 8.9
                    pPoint.Y = 9.9
                ElseIf contatexto = 2 Then
                    pTextElement.Text = v_codigo
                    pPoint.X = 17.9
                    'pPoint.Y = 7.5
                    pPoint.Y = 8.2
                    pPoint.X = 21.7
                    'pPoint.Y = 8.3
                    pPoint.Y = 9.2
                ElseIf contatexto = 3 Then
                    'posi_x = ""
                    'posi_x1 = ""
                    'pTextElement.Text = lista_dist
                    conta_x = lista_dist.Length

                    If conta_x > 38 Then
                        posi_x = Mid(lista_dist, 1, 38)
                        posi_x1 = Right(lista_dist, (lista_dist.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = lista_dist
                    End If
                    pPoint.X = 17.9
                    'pPoint.Y = 6.9
                    pPoint.Y = 7.6
                    pPoint.X = 21.7
                    'pPoint.Y = 7.7
                    pPoint.Y = 8.4

                ElseIf contatexto = 4 Then
                    'pTextElement.Text = lista_prov
                    conta_x = lista_prov.Length
                    If conta_x > 38 Then
                        posi_x = Mid(lista_prov, 1, 38)
                        posi_x1 = Right(lista_prov, (lista_prov.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = lista_prov
                    End If
                    pPoint.X = 17.9
                    'pPoint.Y = 6.3
                    pPoint.Y = 6.8
                    pPoint.X = 21.7
                    'pPoint.Y = 7.1
                    pPoint.Y = 7.6
                ElseIf contatexto = 5 Then
                    'pTextElement.Text = lista_depa
                    conta_x = lista_depa.Length
                    If conta_x > 38 Then
                        posi_x = Mid(lista_depa, 1, 38)
                        posi_x1 = Right(lista_depa, (lista_depa.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = lista_depa
                    End If
                    pPoint.X = 17.9
                    'pPoint.Y = 5.7
                    pPoint.Y = 5.7
                    pPoint.X = 21.7
                    'pPoint.Y = 6.5
                    pPoint.Y = 6.7
                ElseIf contatexto = 6 Then
                    pTextElement.Text = fecha
                    pPoint.X = 16.1
                    pPoint.Y = 2.9
                    pPoint.X = 19.8
                    pPoint.Y = 2.1
                ElseIf contatexto = 7 Then
                    pTextElement.Text = escala_plano_dema
                    'pPoint.X = 17.3
                    pPoint.X = 18.3
                    pPoint.Y = 2.9
                    'pPoint.X = 21.1
                    pPoint.X = 22.1
                    pPoint.Y = 2.1

                ElseIf contatexto = 8 Then
                    pTextElement.Text = v_zona_dm
                    pPoint.X = 5.6
                    pPoint.Y = 2.12
                    pPoint.X = 9.6
                    pPoint.Y = 1.32
                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv

                'Simbolo del texto
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'If contatexto = 6 Then
                pFont.Size = 7
                'ElseIf contatexto = 7 Then
                'pFont.Size = 6
                'ElseIf contatexto = 8 Then
                'pFont.Size = 6
                'Else
                'pFont.Size = 7
                'End If
                pFont.Bold = False
                pTxtSym.Font = pFont
                'Propiedades del Simbolo
                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False

                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Plano Venta" Then  'Reporte para plano Venta
            If sele_plano = "Formato A4" Then
                posi_y1 = 4.15
                posi_y = 4.85
                posi_y2 = 4.15
                posi_y3 = 4.85
            ElseIf sele_plano = "Formato A3" Then
                posi_y1 = 4.9
                posi_y = 5.6
                posi_y2 = 4.9
                posi_y3 = 5.6
            ElseIf sele_plano = "Formato A2" Then
                posi_y1 = 3.6
                posi_y = 4.3
                posi_y2 = 3.6
                posi_y3 = 4.3
            ElseIf sele_plano = "Formato A0" Then
                posi_y1 = 3.4
                posi_y = 4.1
                posi_y2 = 4.4
                posi_y3 = 4.1
            End If
            For contatexto = 1 To 15 'contador de textos
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "HOJA : " & valor_codhoja  ' cabecera hoja
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 15.5
                        pPoint.Y = 25.55
                        pPoint.X = 17.5
                        pPoint.Y = 26.55
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 23.5
                        pPoint.Y = 36.4
                        pPoint.X = 25.5
                        pPoint.Y = 38.4

                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 35.5
                        pPoint.Y = 51.0
                        pPoint.X = 36.4
                        pPoint.Y = 53.6

                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 71.5
                        pPoint.Y = 106.6
                        pPoint.X = 73.0
                        pPoint.Y = 108.4
                    End If

                ElseIf contatexto = 2 Then

                    pTextElement.Text = valor_nmhoja  'nombre hoja
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.2
                        pPoint.Y = posi_y1 - 0.5
                        pPoint.X = 12.9
                        pPoint.Y = posi_y - 0.5
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.7
                        pPoint.Y = posi_y1 - 0.5
                        pPoint.X = 18.7
                        pPoint.Y = posi_y - 0.5
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then

                        pPoint.X = 23.5
                        pPoint.Y = posi_y1 + 3.05
                        pPoint.X = 26.6
                        pPoint.Y = posi_y + 3.05
                        posi_y = posi_y - 0.6

                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 50.5
                        pPoint.Y = posi_y1 + 10.9
                        pPoint.X = 53.6
                        pPoint.Y = posi_y + 10.9
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 3 Then
                    pTextElement.Text = valor_codhoja  'codigo hoja
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.42
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.42
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.4
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.4
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.8
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.8
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 9.7
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 9.7
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 4 Then

                    If sele_plano = "Formato A4" Then  ' tama�o
                        pTextElement.Text = "A4"
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.33
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.33
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pTextElement.Text = "A3"
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.35
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.35
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pTextElement.Text = "A2"
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.4
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.4
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pTextElement.Text = "A0"
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 8.35
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 8.35
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 5 Then

                    pTextElement.Text = fecha   'fecha
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.29
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.29
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.3
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.3
                        posi_y = posi_y - 0.8
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.05
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.05
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 7.1
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 7.1
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 6 Then

                    ' pTextElement.Text = "PSAD-56"       'datum
                    pTextElement.Text = v_sistema
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 14.5
                        pPoint.Y = posi_y2 - 0.5
                        pPoint.X = 16.2
                        pPoint.Y = posi_y3 - 0.5
                        posi_y3 = posi_y3 - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 22.0
                        pPoint.Y = posi_y2 - 0.5
                        pPoint.X = 23.5
                        pPoint.Y = posi_y3 - 0.5
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 3.1
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 3.1
                        posi_y3 = posi_y3 - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 11.0
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 11.0
                        posi_y3 = posi_y3 - 0.5
                    End If


                ElseIf contatexto = 7 Then
                    pTextElement.Text = v_zona_dm   'zona
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 14.5
                        pPoint.Y = posi_y2 - 0.42
                        pPoint.X = 16.2
                        pPoint.Y = posi_y3 - 0.42
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 22.0
                        pPoint.Y = posi_y2 - 0.4
                        pPoint.X = 23.5
                        pPoint.Y = posi_y3 - 0.4
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 2.7
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 2.7
                        posi_y3 = posi_y3 - 0.53

                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 9.8
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 9.8
                        posi_y3 = posi_y3 - 0.53
                    End If

                ElseIf contatexto = 8 Then

                    pTextElement.Text = valor_zoncat   'zona catastral
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 14.5
                        pPoint.Y = posi_y2 - 0.33
                        pPoint.X = 16.2
                        pPoint.Y = posi_y3 - 0.33
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 22.0
                        pPoint.Y = posi_y2 - 0.35
                        pPoint.X = 23.5
                        pPoint.Y = posi_y3 - 0.35
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 2.3
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 2.3
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 8.4
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 8.4
                        posi_y3 = posi_y3 - 0.53
                    End If


                ElseIf contatexto = 9 Then
                    pTextElement.Text = "GUIDO VALDIVIA PONCE"   'autor
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 13.3
                        pPoint.Y = posi_y2 - 0.25
                        pPoint.X = 15.0
                        pPoint.Y = posi_y3 - 0.25
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 20.5
                        pPoint.Y = posi_y2 - 0.4
                        pPoint.X = 21.9
                        pPoint.Y = posi_y3 - 0.4
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 29.9
                        pPoint.Y = posi_y2 + 2.0
                        pPoint.X = 30.9
                        pPoint.Y = posi_y3 + 2.0
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 61.0
                        pPoint.Y = posi_y2 + 7.1
                        pPoint.X = 62.3
                        pPoint.Y = posi_y3 + 7.1
                    End If

                ElseIf contatexto = 10 Then  'DM Uso MInero
                    If canti_capa_actmin > 0 Then
                        pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 10.4
                        pPoint.Y = posi_y2 + 1.2
                        pPoint.X = 12.3
                        pPoint.Y = posi_y3 + 1.2
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.8
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.8
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 4.9
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 4.9
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 13.1
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 13.1
                    End If

                ElseIf contatexto = 11 Then  'DM Actividad Minera
                    If canti_capa_usomin > 0 Then
                        pTextElement.Text = canti_capa_usomin  'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 10.4
                        pPoint.Y = posi_y2 + 0.67
                        pPoint.X = 12.3
                        pPoint.Y = posi_y3 + 0.67
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.0
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.0
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 3.8
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 3.8
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 10.9
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 10.9
                    End If
                ElseIf contatexto = 12 Then  'Exploracion
                    If v_explo1 > 0 Then
                        pTextElement.Text = v_explo1  'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 5.9
                        pPoint.Y = posi_y2 + 1.12
                        pPoint.X = 8.0
                        pPoint.Y = posi_y3 + 1.12
                        posi_y3 = posi_y3 - 0.45
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 9.75
                        pPoint.Y = posi_y2 + 1.6
                        pPoint.X = 11.15
                        pPoint.Y = posi_y3 + 1.6
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 14.1
                        pPoint.Y = posi_y2 + 4.7
                        pPoint.X = 16.1
                        pPoint.Y = posi_y3 + 4.7
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 30.85
                        pPoint.Y = posi_y2 + 12.7
                        pPoint.X = 32.15
                        pPoint.Y = posi_y3 + 12.7
                    End If
                ElseIf contatexto = 13 Then  'Exploracion y explotacion
                    If v_explo_v_expta1 > 0 Then
                        pTextElement.Text = v_explo_v_expta1  'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 5.9
                        pPoint.Y = posi_y2 + 1.12
                        pPoint.X = 8.0
                        pPoint.Y = posi_y3 + 1.12
                        posi_y3 = posi_y3 - 0.4
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 9.75
                        pPoint.Y = posi_y2 + 1.0
                        pPoint.X = 11.15
                        pPoint.Y = posi_y3 + 1.0
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 14.1
                        pPoint.Y = posi_y2 + 3.9
                        pPoint.X = 16.1
                        pPoint.Y = posi_y3 + 3.9
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 30.85
                        pPoint.Y = posi_y2 + 10.9
                        pPoint.X = 32.15
                        pPoint.Y = posi_y3 + 10.9
                    End If

                ElseIf contatexto = 14 Then  'Explotacion
                    If v_expta1 > 0 Then
                        pTextElement.Text = v_expta1   'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 5.9
                        pPoint.Y = posi_y2 + 1.12
                        pPoint.X = 8.0
                        pPoint.Y = posi_y3 + 1.12
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 9.75
                        pPoint.Y = posi_y2 + 0.4
                        pPoint.X = 11.15
                        pPoint.Y = posi_y3 + 0.4
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 14.1
                        pPoint.Y = posi_y2 + 3.1
                        pPoint.X = 16.1
                        pPoint.Y = posi_y3 + 3.1
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 30.85
                        pPoint.Y = posi_y2 + 9.4
                        pPoint.X = 32.15
                        pPoint.Y = posi_y3 + 9.4
                    End If

                ElseIf contatexto = 15 Then  'CONTADOR A�O DAC

                    pTextElement.Text = lostr_Join_Codigos_AREA   'contador Capa Uso mi

                    If sele_plano = "Formato A4" Then
                        pPoint.X = 9.8
                        pPoint.Y = posi_y2 + 0.7
                        pPoint.X = 11.9
                        pPoint.Y = posi_y3 + 0.7
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.0
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.0
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 3.8
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 3.8
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 10.9
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 10.9
                    End If

                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                'Simbolo del texto
                pTxtSym = New TextSymbol
                'fuente del texto

                'Dim pFont As IFontDisp
                pFont = New StdFont
                pFont.Name = "Tahoma"
                If sele_plano = "Formato A4" Then
                    pFont.Size = 4.0
                ElseIf sele_plano = "Formato A3" Then
                    pFont.Size = 6.0
                ElseIf sele_plano = "Formato A2" Then
                    pFont.Size = 8.5
                ElseIf sele_plano = "Formato A0" Then
                    pFont.Size = 14
                End If
                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym.Color = myColor
                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal

                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

            Next contatexto
        End If
    End Sub

    Public Sub XXXA_gregarTextosLayout_antiguo_a_150312(ByVal sele_reporte As String, ByVal caso_simula As String)
        'PROGRAMA PARA AGREGAR TEXTOS EN LAYOUT
        '*******************************************
        Dim MyDate As Date
        MyDate = Now
        Dim fecha As String
        fecha = RellenarComodin(MyDate.Year, 2, "0") & "/" & RellenarComodin(MyDate.Month, 2, "0") & "/" & MyDate.Day

        Dim posi_y2 As Double
        Dim posi_y3 As Double

        Dim pPageLayout As IPageLayout
        Dim pActiveView As IActiveView
        Dim pGraphicsContainer As IGraphicsContainer
        Dim pElement As IElement
        Dim pTextElement As ITextElement
        Dim pPoint As IPoint
        Dim pTxtSym As IFormattedTextSymbol
        Dim contatexto As Integer
        Dim myColor As IRgbColor
        Dim pFont As IFontDisp

        pPageLayout = pMxDoc.PageLayout
        pPageLayout.Page.Units = esriUnits.esriCentimeters
        pActiveView = pPageLayout
        pGraphicsContainer = pPageLayout

        Dim posi_x As String
        Dim posi_x1 As String
        Dim conta_x As Long
        'Para obtener fecha

        'Dim fecha_plano As Date
        'fecha_plano = Date
        Dim posi_y As Double
        Dim posi_y1 As Double
        Dim pEnv As IEnvelope
        posi_y = posi_y_priori_CNM
        posi_y1 = posi_y_priori1_CNM
        Dim Nombre_urba As String = ""
        Dim Nombre_rese As String = ""

        'Colocando textos fijos al plano

        If sele_reporte = "Evaluacion" Then
            For contatexto = 1 To 16
                'Guardar el texto
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "Carta : " & v_carta_dm
                    pPoint.X = 8.1
                    pPoint.Y = 18.3
                    pPoint.X = 9.2
                    pPoint.Y = 17.8
                ElseIf contatexto = 2 Then
                    pTextElement.Text = "Fecha : " & fecha
                    pPoint.X = 13.8
                    pPoint.Y = 18.3
                    pPoint.X = 14.8
                    pPoint.Y = 17.8
                ElseIf contatexto = 3 Then
                    pTextElement.Text = "CODIGO DEL DM : " & v_codigo
                    pPoint.X = 14.1
                    pPoint.Y = 16.5
                    pPoint.X = 18.2
                    pPoint.Y = 17.3
                ElseIf contatexto = 4 Then
                    pTextElement.Text = "NOMBRE DEL DM : " & v_nombre_dm
                    pPoint.X = 14.1
                    pPoint.Y = 15.9
                    pPoint.X = 18.2
                    pPoint.Y = 16.7
                ElseIf contatexto = 5 Then
                    pTextElement.Text = "HECTAREA : " & " " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 14.1
                    pPoint.Y = 15.4
                    pPoint.X = 18.2
                    pPoint.Y = 16.1
                ElseIf contatexto = 6 Then
                    If Cuenta_an = 0 Then
                        pTextElement.Text = "DERECHOS PRIORITARIOS :  No Presenta DM Prioritarios"
                    Else
                        pTextElement.Text = "DERECHOS PRIORITARIOS : " & "(" & Cuenta_an & ")"
                    End If
                    pPoint.X = 15.7
                    pPoint.Y = 14.7
                    pPoint.X = 18.2
                    pPoint.Y = 15.5
                ElseIf contatexto = 7 Then
                    If v_tipo_exp = "RD" Then
                        If Cuenta_rd = 0 Then
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad "
                        Else
                            pTextElement.Text = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea " & vbNewLine & "Del derecho extinguido y publicado de libre denunciabilidad " & "(" & Cuenta_rd & ")"
                        End If
                        pPoint.X = 14.1
                        pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.X = 18.2
                        pPoint.Y = conta_rd_posi_y - 0.2
                    Else
                        pPoint.X = 14.1
                        'pPoint.Y = conta_rd_posi_y1 - 0.2
                        pPoint.Y = 14.7
                        pPoint.X = 18.2
                        'pPoint.Y = conta_rd_posi_y - 0.2
                        pPoint.Y = 15.5

                    End If
                ElseIf contatexto = 8 Then
                    If Cuenta_po = 0 Then
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA : No Presenta "
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES :  No Presenta DM Posteriores"
                        End If
                    Else
                        If v_tipo_exp = "RD" Then
                            pTextElement.Text = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA: " & "(" & Cuenta_rd & ")"
                        Else
                            pTextElement.Text = "DERECHOS POSTERIORES : " & "(" & Cuenta_po & ")"
                        End If
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = conta_po_posi_y1 - 0.2
                    pPoint.X = 18.2
                    pPoint.Y = conta_po_posi_y - 0.2

                ElseIf contatexto = 9 Then
                    If Cuenta_si = 0 Then
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  No Presenta DM Simultaneos"
                    Else
                        pTextElement.Text = "DERECHOS SIMULTANEOS :  " & "(" & Cuenta_si & ")"
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = conta_si_posi_y1 - 0.15
                    pPoint.X = 18.2
                    pPoint.Y = conta_si_posi_y - 0.15

                ElseIf contatexto = 10 Then
                    If Cuenta_ex = 0 Then
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  No Presenta DM Extinguidos"
                    Else
                        pTextElement.Text = "DERECHOS EXTINGUIDOS :  " & "(" & Cuenta_ex & ")"
                    End If

                    pPoint.X = 14.1
                    pPoint.Y = conta_ex_posi_y1 - 0.15
                    pPoint.X = 18.2
                    pPoint.Y = conta_ex_posi_y - 0.15

                ElseIf contatexto = 11 Then

                    pTextElement.Text = "CATASTRO NO MINERO"
                    pPoint.X = 14.1
                    'pPoint.Y = posi_y1 - 0.4
                    pPoint.Y = posi_y1 - 0.6
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 0.6
                ElseIf contatexto = 12 Then
                    If lista_urba = "" Then
                        pTextElement.Text = "Zonas Urbanas :" & " No se encuentra superpuesto a un Area urbana"
                    Else
                        conta_x = lista_urba.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_urba, 1, 60)
                            posi_x1 = Right(lista_urba, (lista_urba.Length - 60))
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Urbanas :      " & "  " & lista_urba
                        End If
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 1
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1


                ElseIf contatexto = 13 Then
                    If lista_rese = "" Then
                        pTextElement.Text = "Zonas Reservadas :" & " No se encuentra superpuesto a un Area de Reserva"
                    Else
                        conta_x = lista_urba.Length
                        If conta_x > 60 Then
                            posi_x = Mid(lista_rese, 1, 60)
                            posi_x1 = Right(lista_urba, (lista_rese.Length - 60))
                            pTextElement.Text = "Zonas Reservadas :      " & "  " & posi_x & vbNewLine & posi_x1
                        Else
                            pTextElement.Text = "Zonas Reservadas :      " & "  " & lista_rese
                        End If
                    End If
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 1.4
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1.4
                ElseIf contatexto = 14 Then
                    pTextElement.Text = "L�mites fronterizos (Fuente IGN): Distancia de la l�nea de frontera de " & distancia_fron & " (Km.)"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 1.8
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 1.8
                ElseIf contatexto = 15 Then

                    pTextElement.Text = "                                LISTADO DE DERECHOS MINEROS"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.2
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.2

                ElseIf contatexto = 16 Then
                    pTextElement.Text = "N�   NOMBRE                         CODIGO      ZONA   TE      TP   PUBL   INCOR   SUST"
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 2.6
                    pPoint.X = 18.2
                    pPoint.Y = posi_y - 2.6
                    posi_y1_list = posi_y1 - 2.9
                    posi_y2_list = posi_y - 2.9

                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'If ((contatexto = 9) Or (contatexto = 10) Or (contatexto = 11)) Then
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    'pFont.Size = 8
                    pFont.Size = 8
                Else
                    'pFont.Size = 9
                    pFont.Size = 6
                End If

                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    myColor.RGB = RGB(197, 0, 255)
                ElseIf ((contatexto = 1) Or (contatexto = 2) Or (contatexto = 6) Or (contatexto = 7) Or (contatexto = 8) Or (contatexto = 9) Or (contatexto = 10)) Then
                    myColor.RGB = RGB(71, 61, 255)
                ElseIf contatexto = 11 Then
                    myColor.RGB = RGB(230, 0, 0)
                ElseIf ((contatexto = 12) Or (contatexto = 13) Or (contatexto = 14) Or (contatexto = 15)) Then
                    myColor.RGB = RGB(0, 0, 0)
                ElseIf (contatexto = 16) Then
                    myColor.RGB = RGB(71, 61, 255)
                End If
                pTxtSym.Color = myColor

                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                If contatexto = 1 Or contatexto = 2 Or contatexto = 3 Or contatexto = 4 Or contatexto = 5 Or contatexto = 6 Then
                    pGraphicsContainer.AddElement(pTextElement, 1)
                ElseIf contatexto = 7 Then
                    If v_tipo_exp = "RD" Then
                        If v_posi_rd = False Or Cuenta_rd > 0 Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                ElseIf contatexto = 8 Then
                    If v_posi_po = False Or Cuenta_po > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 9 Then
                    If v_posi_si = False Or Cuenta_si > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                ElseIf contatexto = 10 Then
                    If v_posi_ex = False Or Cuenta_ex > 0 Then
                        pGraphicsContainer.AddElement(pTextElement, 1)
                    End If
                Else
                    If contador_hojas < 1 Then
                        If v_posi_pr = False And v_posi_po = False And v_posi_si = False And v_posi_ex = False Or v_posi_rd Then
                            pGraphicsContainer.AddElement(pTextElement, 1)
                        End If
                    End If

                End If
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Cuadricula" Then
            Dim canti_cuadri As Integer
            canti_cuadri = v_area_eval / 100

            For contatexto = 1 To 8
                'Guardar el texto
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "Carta : " & v_carta_dm
                    pPoint.X = 8.1
                    pPoint.Y = 18.5
                    pPoint.X = 9.2
                    pPoint.Y = 18.0
                ElseIf contatexto = 2 Then
                    pTextElement.Text = "Fecha : " & fecha
                    pPoint.X = 13.8
                    pPoint.Y = 18.5
                    pPoint.X = 14.8
                    pPoint.Y = 18.0
                ElseIf contatexto = 3 Then
                    pTextElement.Text = "CODIGO DEL DM : " & v_codigo
                    pPoint.X = 14.4
                    pPoint.Y = 16.5
                    pPoint.X = 18.5
                    pPoint.Y = 17.3
                ElseIf contatexto = 4 Then
                    pTextElement.Text = "NOMBRE DEL DM : " & v_nombre_dm
                    pPoint.X = 14.4
                    pPoint.Y = 15.9
                    pPoint.X = 18.5
                    pPoint.Y = 16.7
                ElseIf contatexto = 5 Then
                    pTextElement.Text = "HECTAREA : " & " " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 14.4
                    pPoint.Y = 15.4
                    pPoint.X = 18.5
                    pPoint.Y = 16.1
                ElseIf contatexto = 6 Then
                    pTextElement.Text = "* EL PRESENTE PETITORIO HA SIDO FORMULADO POR : " & FormatNumber(v_area_eval, 4) & " Ha."
                    pPoint.X = 17.8
                    pPoint.Y = 11.2
                    pPoint.X = 18.8
                    pPoint.Y = 10.8

                ElseIf contatexto = 7 Then
                    pTextElement.Text = "* ESTA CONFORMADO POR  " & canti_cuadri & " CUADRICULAS DE 100.00 Ha."
                    pPoint.X = 17.8
                    pPoint.Y = 10.6
                    pPoint.X = 18.8
                    pPoint.Y = 10.2
                ElseIf contatexto = 8 Then
                    pTextElement.Text = "* LAS CUADRICULAS SON  " & nombre_datos
                    pPoint.X = 17.8
                    pPoint.Y = 10.0
                    pPoint.X = 18.8
                    pPoint.Y = 9.6
                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTxtSym = New TextSymbol
                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"

                pFont.Size = 6
                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                If ((contatexto = 3) Or (contatexto = 4) Or (contatexto = 5)) Then
                    myColor.RGB = RGB(197, 0, 255)
                ElseIf ((contatexto = 1) Or (contatexto = 2)) Then
                    myColor.RGB = RGB(71, 61, 255)
                ElseIf ((contatexto = 6) Or (contatexto = 7) Or (contatexto = 8)) Then
                    myColor.RGB = RGB(0, 0, 255)

                End If
                pTxtSym.Color = myColor
                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Carta" Then
            Dim posi_y_c As Double
            Dim posi_y_c1 As Double
            posi_y_c = 12.35
            posi_y_c1 = 12.03

            'Guardar el texto
            For contatexto = 1 To 13
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then

                    'pTextElement.Text = "DERECHO MINERO:  " & v_nombre_dm
                    conta_x = v_nombre_dm.Length
                    'If conta_x > 45 Then
                    If conta_x > 60 Then
                        posi_x = Mid(v_nombre_dm, 1, 60)
                        posi_x1 = Right(v_nombre_dm, (v_nombre_dm.Length - 60))
                        pTextElement.Text = "DERECHO MINERO:  " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "DERECHO MINERO:  " & v_nombre_dm
                    End If
                    pPoint.X = 19.1
                    'pPoint.Y = 12.353
                    pPoint.Y = posi_y_c
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                    'pPoint.Y = 12.003

                ElseIf contatexto = 2 Then
                    conta_x = lista_nmhojas.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_nmhojas, 1, 60)
                        posi_x1 = Right(lista_nmhojas, (lista_nmhojas.Length - 60))
                        pTextElement.Text = "Nombre de la Carta : " & "  " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "Nombre de la Carta : " & "  " & lista_nmhojas
                    End If

                    pPoint.X = 19.1
                    'pPoint.Y = 11.984
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 11.634
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 3 Then
                    'pTextElement.Text = "Numero de la Carta :  " & carta_v
                    conta_x = lista_cd_cartas.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_cd_cartas, 1, 60)
                        posi_x1 = Right(lista_cd_cartas, (lista_cd_cartas.Length - 60))
                        pTextElement.Text = "N�mero de la Carta : " & "  " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "N�mero de la Carta : " & "  " & lista_cd_cartas
                    End If
                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 4 Then
                    pTextElement.Text = "Escala                  :" & "    " & "1/100 000"
                    pPoint.X = 19.1
                    'pPoint.Y = 11.341
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 10.991

                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 5 Then
                    'pTextElement.Text = "Zona                    :" & "   " & "ZONA"
                    pTextElement.Text = "Zona                    : " & "   " & v_zona_dm
                    pPoint.X = 19.1
                    'pPoint.Y = 11.02
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 10.68
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 6 Then
                    pTextElement.Text = "UBICACION"
                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 7 Then
                    'pTextElement.Text = "Distrito                :   " & lista_dist
                    conta_x = lista_dist.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_dist, 1, 45)
                        posi_x1 = Right(lista_dist, (lista_dist.Length - 60))
                        pTextElement.Text = "DISTRITOS (S)       : " & "   " & posi_x & vbNewLine & posi_x1
                        posi_y_c = posi_y_c + 0.3
                        posi_y_c1 = posi_y_c1 - 0.3
                    Else
                        pTextElement.Text = "DISTRITOS (S)       : " & "   " & lista_dist
                    End If

                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 8 Then
                    conta_x = lista_prov.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_prov, 1, 60)
                        posi_x1 = Right(lista_prov, (lista_prov.Length - 60))
                        pTextElement.Text = "PROVINCIA (S)       : " & "   " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "PROVINCIA (S)       : " & "   " & lista_prov
                    End If

                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3

                ElseIf contatexto = 9 Then
                    conta_x = lista_depa.Length
                    If conta_x > 60 Then
                        posi_x = Mid(lista_depa, 1, 60)
                        posi_x1 = Right(lista_depa, (lista_depa.Length - 60))
                        pTextElement.Text = "DEPARTAMENTO (S) : " & "   " & posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = "DEPARTAMENTO (S) : " & "   " & lista_depa
                    End If

                    pPoint.X = 19.1
                    pPoint.X = 19.1
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1
                    posi_y_c = posi_y_c - 0.4
                    posi_y_c1 = posi_y_c1 - 0.4

                ElseIf contatexto = 10 Then
                    pTextElement.Text = "OBSERVACIONES"
                    pPoint.X = 19.1
                    'pPoint.Y = 9.13
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 19.1
                    'pPoint.Y = 8.9
                    pPoint.Y = posi_y_c
                    pPoint.Y = posi_y_c1

                ElseIf contatexto = 11 Then
                    pTextElement.Text = "No existen Observaciones"
                    pPoint.X = 19.1

                    pPoint.X = 19.1
                    'pPoint.Y = 2.1
                    pPoint.Y = posi_y_c - 0.3
                    pPoint.Y = posi_y_c1 - 0.3

                    posi_y_c = posi_y_c - 0.3
                    posi_y_c1 = posi_y_c1 - 0.3
                    posi_y_m = posi_y_c
                    posi_y1_m = posi_y_c1

                    'ElseIf contatexto = 11 Then
                    '    pTextElement.Text = "ELIPSOIDE...............................................INTERNACIONAL"
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.48
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.48

                    'ElseIf contatexto = 12 Then
                    '    pTextElement.Text = "PROYECCION.................................TRANSVERSA DE MERCATOR"
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.289
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 1.958
                    '    pPoint.Y = 1.289

                    'ElseIf contatexto = 13 Then
                    '    pTextElement.Text = "DATUM VERTICAL..............................................NIVEL MEDIO DEL MAR"
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.48
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.48

                    'ElseIf contatexto = 14 Then
                    '    pTextElement.Text = "DATUM HORIZONTAL.........DATUM PROVISIONAL SUDAMERICANO DE 1956"
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.289
                    '    'pEnv.LowerLeft = pPoint
                    '    pPoint.X = 10.3
                    '    pPoint.Y = 1.289
                ElseIf contatexto = 12 Then
                    pTextElement.Text = fecha
                    pPoint.X = 16.0
                    pPoint.Y = 2.9
                    pPoint.X = 19.7
                    pPoint.Y = 2.1
                ElseIf contatexto = 13 Then
                    pTextElement.Text = escala_plano_carta
                    'pPoint.X = 17.3
                    pPoint.X = 19.4
                    pPoint.Y = 2.9
                    'pPoint.X = 21.1
                    pPoint.X = 21.9
                    pPoint.Y = 2.1
                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                'Simbolo del texto
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'color del texto
                'Dim pColor As IRgbColor
                myColor = New RgbColor

                'If contatexto = 1 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'ElseIf ((contatexto > 10) And (contatexto < 15)) Then
                'pFont.Size = 5.5
                'ElseIf contatexto = 6 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'ElseIf contatexto = 10 Then
                'pFont.Size = 9
                'myColor.RGB = RGB(0, 0, 250)
                'Else
                'pFont.Size = 8
                'End If

                If contatexto = 1 Then
                    pFont.Size = 8
                    myColor.RGB = RGB(0, 0, 250)
                ElseIf ((contatexto > 10) And (contatexto < 15)) Then
                    pFont.Size = 5.5
                ElseIf contatexto = 6 Then
                    pFont.Size = 9
                    myColor.RGB = RGB(0, 0, 250)
                ElseIf contatexto = 10 Then
                    pFont.Size = 9
                    myColor.RGB = RGB(0, 0, 250)
                    'ElseIf contatexto = 11 Or contatexto = 12 Or contatexto = 13 Or contatexto = 14 Then
                    'pFont.Size = 3
                    'myColor.RGB = RGB(0, 0, 250)
                Else
                    pFont.Size = 6
                End If


                pFont.Bold = False
                pTxtSym.Font = pFont
                pTxtSym.Color = myColor

                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Plano_variado" Then
            If tipo_opcion = "1" Then
                lista_nmhojas = v_nombre
                lista_cartas = v_codigo
            End If
            Dim posi_y_c As Double
            Dim posi_y_c1 As Double

            If sele_plano1 = "Plano A4 Vertical" Then
                posi_y_c = 27.15
                posi_y_c1 = 27.05

            ElseIf sele_plano1 = "Plano A4 Horizontal" Then
                posi_y_c = 11.8
                posi_y_c1 = 11.5

            ElseIf sele_plano1 = "Plano A3 Vertical" Then
                posi_y_c = 40.45
                posi_y_c1 = 40.35
            ElseIf sele_plano1 = "Plano A3 Horizontal" Then
                posi_y_c = 16.5
                posi_y_c1 = 16.2

            ElseIf sele_plano1 = "Plano A2 Vertical" Then
                posi_y_c = 56.9
                posi_y_c1 = 56.9

            ElseIf sele_plano1 = "Plano A2 Horizontal" Then
                posi_y_c = 23.0
                posi_y_c1 = 22.9

            ElseIf sele_plano1 = "Plano A1 Vertical" Then
                posi_y_c = 81.0
                posi_y_c1 = 80.5

            ElseIf sele_plano1 = "Plano A1 Horizontal" Then
                posi_y_c = 32.8
                posi_y_c1 = 32.5
            ElseIf sele_plano1 = "Plano A0 Vertical" Then
                posi_y_c = 114.9
                posi_y_c1 = 113.9

            ElseIf sele_plano1 = "Plano A0 Horizontal" Then
                posi_y_c = 47.0
                posi_y_c1 = 46.0
            End If
            If sele_plano1 = "Plano A4 Horizontal" Then
                For contatexto = 1 To 11
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 27.02
                        pPoint.X = 28.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_expta1

                        pPoint.X = 27.02
                        pPoint.X = 28.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7
                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_explo_v_expta1

                        pPoint.X = 27.02
                        pPoint.X = 28.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1

                        pPoint.X = 27.02
                        pPoint.X = 28.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.7
                        posi_y_c1 = posi_y_c1 - 0.7


                    ElseIf contatexto = 5 Then

                        pTextElement.Text = lista_nmhojas

                        pPoint.X = 23.1
                        pPoint.X = 23.7
                        pPoint.Y = 6.5
                        pPoint.Y = 6.25

                    ElseIf contatexto = 6 Then
                        pTextElement.Text = lista_cartas

                        pPoint.X = 23.3
                        pPoint.X = 23.9
                        pPoint.Y = 6.05
                        pPoint.Y = 5.8

                    ElseIf contatexto = 7 Then
                        pTextElement.Text = sele_plano2

                        pPoint.X = 23.3
                        pPoint.X = 23.9
                        pPoint.Y = 5.6
                        pPoint.Y = 5.4

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 23.3
                        pPoint.X = 23.9
                        pPoint.Y = 5.2
                        pPoint.Y = 5.0

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zonasel

                        pPoint.X = 26.1
                        pPoint.X = 26.7
                        pPoint.Y = 6.05
                        pPoint.Y = 5.8
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 26.1
                        pPoint.X = 26.7
                        pPoint.Y = 5.2
                        pPoint.Y = 5.0

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 25.9
                        pPoint.X = 26.6
                        pPoint.Y = 3.42
                        pPoint.Y = 4.22

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 3.5
                    ElseIf contatexto = 11 Then
                        pFont.Size = 9

                    Else
                        pFont.Size = 5
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A4 Vertical" Then
                For contatexto = 1 To 11
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 17.9
                        pPoint.X = 18.9
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.5
                        posi_y_c1 = posi_y_c1 - 0.5

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 17.9
                        pPoint.X = 18.9
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.5
                        posi_y_c1 = posi_y_c1 - 0.5

                    ElseIf contatexto = 3 Then

                        pTextElement.Text = v_explo_v_expta1

                        pPoint.X = 17.9
                        pPoint.X = 18.9
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.5
                        posi_y_c1 = posi_y_c1 - 0.5

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 17.9
                        pPoint.X = 18.9
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.5
                        posi_y_c1 = posi_y_c1 - 0.5

                    ElseIf contatexto = 5 Then

                        pTextElement.Text = lista_nmhojas

                        pPoint.X = 14.6
                        pPoint.X = 15.6
                        pPoint.Y = 22.8
                        pPoint.Y = 22.7



                    ElseIf contatexto = 6 Then
                        pTextElement.Text = lista_cartas


                        pPoint.X = 14.7
                        pPoint.X = 15.7
                        pPoint.Y = 22.48
                        pPoint.Y = 22.38


                    ElseIf contatexto = 7 Then
                        pTextElement.Text = sele_plano2
                        pPoint.X = 14.6
                        pPoint.X = 15.6
                        pPoint.Y = 22.2
                        pPoint.Y = 22.1

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 14.6
                        pPoint.X = 15.6
                        pPoint.Y = 21.8
                        pPoint.Y = 21.7

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zonasel

                        pPoint.X = 17.5
                        pPoint.X = 18.1
                        pPoint.Y = 22.47
                        pPoint.Y = 22.37

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 17.5
                        pPoint.X = 18.1
                        pPoint.Y = 21.82
                        pPoint.Y = 21.72

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 17.5
                        pPoint.X = 18.1
                        pPoint.Y = 24.5
                        pPoint.Y = 24.4

                    End If

                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor

                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 2.5
                    ElseIf contatexto = 11 Then
                        pFont.Size = 7

                    Else
                        pFont.Size = 6.5
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A3 Horizontal" Then
                For contatexto = 1 To 11
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 38.8
                        pPoint.X = 39.8
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_expta1

                        pPoint.X = 38.8
                        pPoint.X = 39.8
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9


                    ElseIf contatexto = 3 Then

                        pTextElement.Text = v_explo_v_expta1

                        pPoint.X = 38.8
                        pPoint.X = 39.8
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1

                        pPoint.X = 38.8
                        pPoint.X = 39.8
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9


                    ElseIf contatexto = 5 Then

                        pTextElement.Text = lista_nmhojas

                        pPoint.X = 32.1
                        pPoint.X = 33.1
                        pPoint.Y = 8.95
                        pPoint.Y = 8.7

                    ElseIf contatexto = 6 Then
                        pTextElement.Text = lista_cartas
                        pPoint.X = 32.6
                        pPoint.X = 33.6
                        pPoint.Y = 8.35
                        pPoint.Y = 8.13

                    ElseIf contatexto = 7 Then
                        pTextElement.Text = sele_plano2

                        pPoint.X = 32.6
                        pPoint.X = 33.6
                        pPoint.Y = 7.75
                        pPoint.Y = 7.5

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 32.6
                        pPoint.X = 33.6
                        pPoint.Y = 7.15
                        pPoint.Y = 6.9

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zonasel

                        pPoint.X = 37.1
                        pPoint.X = 37.7
                        pPoint.Y = 8.35
                        pPoint.Y = 8.13
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 37.1
                        pPoint.X = 37.7
                        pPoint.Y = 7.15
                        pPoint.Y = 6.9

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 36.9
                        pPoint.X = 37.6
                        pPoint.Y = 6.9
                        pPoint.Y = 5.9

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 6
                    ElseIf contatexto = 11 Then
                        pFont.Size = 9

                    Else
                        pFont.Size = 8
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto

            ElseIf sele_plano1 = "Plano A3 Vertical" Then
                For contatexto = 1 To 11
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 26.5
                        pPoint.X = 27.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.8
                        posi_y_c1 = posi_y_c1 - 0.8

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 26.5
                        pPoint.X = 27.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.8
                        posi_y_c1 = posi_y_c1 - 0.8

                    ElseIf contatexto = 3 Then

                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 26.5
                        pPoint.X = 27.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.8
                        posi_y_c1 = posi_y_c1 - 0.8

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1

                        pPoint.X = 26.5
                        pPoint.X = 27.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 0.9
                        posi_y_c1 = posi_y_c1 - 0.9

                    ElseIf contatexto = 5 Then

                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 21.5
                        pPoint.X = 22.5
                        pPoint.Y = 33.8
                        pPoint.Y = 33.6

                    ElseIf contatexto = 6 Then
                        pTextElement.Text = lista_cartas
                        pPoint.X = 21.9
                        pPoint.X = 22.9
                        pPoint.Y = 33.3
                        pPoint.Y = 33.1

                    ElseIf contatexto = 7 Then
                        pTextElement.Text = sele_plano2

                        pPoint.X = 21.9
                        pPoint.X = 22.9
                        pPoint.Y = 32.8
                        pPoint.Y = 32.6

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 21.9
                        pPoint.X = 22.9
                        pPoint.Y = 32.3
                        pPoint.Y = 32.1

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zonasel

                        pPoint.X = 24.9
                        pPoint.X = 25.9
                        pPoint.Y = 33.3
                        pPoint.Y = 33.1
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 24.9
                        pPoint.X = 25.9
                        pPoint.Y = 32.3
                        pPoint.Y = 32.1
                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 36.9
                        pPoint.X = 37.6
                        pPoint.Y = 6.9
                        pPoint.Y = 5.9

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 4
                    ElseIf contatexto = 11 Then
                        pFont.Size = 9

                    Else
                        pFont.Size = 8
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A2 Horizontal" Then
                For contatexto = 1 To 11
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 55.25
                        pPoint.X = 56.25
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.2
                        posi_y_c1 = posi_y_c1 - 1.2

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_expta1

                        pPoint.X = 55.25
                        pPoint.X = 56.25
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.4
                        posi_y_c1 = posi_y_c1 - 1.4

                    ElseIf contatexto = 3 Then

                        pTextElement.Text = v_explo_v_expta1

                        pPoint.X = 55.25
                        pPoint.X = 56.25
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.3
                        posi_y_c1 = posi_y_c1 - 1.3

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1

                        pPoint.X = 55.25
                        pPoint.X = 56.25
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.4
                        posi_y_c1 = posi_y_c1 - 1.4


                    ElseIf contatexto = 5 Then
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 46.4
                        pPoint.X = 47.4
                        pPoint.Y = 12.7
                        pPoint.Y = 12.3

                    ElseIf contatexto = 6 Then
                        pTextElement.Text = lista_cartas
                        pPoint.X = 46.5
                        pPoint.X = 47.5
                        pPoint.Y = 11.8
                        pPoint.Y = 11.4

                    ElseIf contatexto = 7 Then
                        pTextElement.Text = sele_plano2

                        pPoint.X = 46.5
                        pPoint.X = 47.5
                        pPoint.Y = 11.0
                        pPoint.Y = 10.6

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 46.5
                        pPoint.X = 47.5
                        pPoint.Y = 10.1
                        pPoint.Y = 9.7


                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zonasel

                        pPoint.X = 53.5
                        pPoint.X = 54.5
                        pPoint.Y = 11.8
                        pPoint.Y = 11.4


                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 53.5
                        pPoint.X = 54.5
                        pPoint.Y = 10.1
                        pPoint.Y = 9.7

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 52.5
                        pPoint.X = 53.5
                        pPoint.Y = 9.4
                        pPoint.Y = 8.4

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 6
                    ElseIf contatexto = 11 Then
                        pFont.Size = 12

                    Else
                        pFont.Size = 16
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A2 Vertical" Then
                For contatexto = 1 To 11
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 39.1
                        pPoint.X = 39.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 39.1
                        pPoint.X = 39.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 3 Then
                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 39.1
                        pPoint.X = 39.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 39.1
                        pPoint.X = 39.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.1
                        posi_y_c1 = posi_y_c1 - 1.1

                    ElseIf contatexto = 5 Then
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 47.8
                        pPoint.Y = 47.5

                    ElseIf contatexto = 6 Then
                        pTextElement.Text = lista_cartas
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 47.1
                        pPoint.Y = 46.8

                    ElseIf contatexto = 7 Then
                        pTextElement.Text = sele_plano2
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 46.4
                        pPoint.Y = 46.1

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = DateTime.Today
                        pPoint.X = 31.1
                        pPoint.X = 32.1
                        pPoint.Y = 45.7
                        pPoint.Y = 45.4

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zonasel

                        pPoint.X = 36.9
                        pPoint.X = 37.9
                        pPoint.Y = 47.1
                        pPoint.Y = 46.8
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"
                        pPoint.X = 36.6
                        pPoint.X = 37.6
                        pPoint.Y = 47.7
                        pPoint.Y = 45.4
                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf
                        pPoint.X = 35.9
                        pPoint.X = 36.9
                        pPoint.Y = 51.6
                        pPoint.Y = 51.2

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 6
                    ElseIf contatexto = 11 Then
                        pFont.Size = 13

                    Else
                        pFont.Size = 11
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A1 Horizontal" Then
                For contatexto = 1 To 11
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 78.5
                        pPoint.X = 79.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.8
                        posi_y_c1 = posi_y_c1 - 1.8

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_expta1

                        pPoint.X = 78.5
                        pPoint.X = 79.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.8
                        posi_y_c1 = posi_y_c1 - 1.8

                    ElseIf contatexto = 3 Then

                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 78.5
                        pPoint.X = 79.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.9
                        posi_y_c1 = posi_y_c1 - 1.9

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1
                        pPoint.X = 78.5
                        pPoint.X = 79.5
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.9
                        posi_y_c1 = posi_y_c1 - 1.9

                    ElseIf contatexto = 5 Then
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 66.0
                        pPoint.X = 67.3
                        pPoint.Y = 18.2
                        pPoint.Y = 17.5

                    ElseIf contatexto = 6 Then
                        pTextElement.Text = lista_cartas
                        pPoint.X = 66.3
                        pPoint.X = 67.5
                        pPoint.Y = 16.9
                        pPoint.Y = 16.2

                    ElseIf contatexto = 7 Then
                        pTextElement.Text = sele_plano2

                        pPoint.X = 66.3
                        pPoint.X = 67.5
                        pPoint.Y = 15.7
                        pPoint.Y = 15.0

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 66.3
                        pPoint.X = 67.5
                        pPoint.Y = 14.5
                        pPoint.Y = 13.8


                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zonasel

                        pPoint.X = 75.5
                        pPoint.X = 76.5
                        pPoint.Y = 16.9
                        pPoint.Y = 16.2

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 75.1
                        pPoint.X = 76.1
                        pPoint.Y = 14.5
                        pPoint.Y = 13.8

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 73.4
                        pPoint.X = 74.4
                        pPoint.Y = 12.7
                        pPoint.Y = 11.7

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 11
                    ElseIf contatexto = 11 Then
                        pFont.Size = 20

                    Else
                        pFont.Size = 22
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A1 Vertical" Then
                For contatexto = 1 To 11
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 54.1
                        pPoint.X = 55.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.4
                        posi_y_c1 = posi_y_c1 - 1.4

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 54.1
                        pPoint.X = 55.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.5
                        posi_y_c1 = posi_y_c1 - 1.5

                    ElseIf contatexto = 3 Then

                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 54.1
                        pPoint.X = 55.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.5
                        posi_y_c1 = posi_y_c1 - 1.5

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1

                        pPoint.X = 54.1
                        pPoint.X = 55.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 1.4
                        posi_y_c1 = posi_y_c1 - 1.4

                    ElseIf contatexto = 5 Then

                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 44.8
                        pPoint.X = 46.1
                        pPoint.Y = 67.8
                        pPoint.Y = 67.3

                    ElseIf contatexto = 6 Then
                        pTextElement.Text = lista_cartas
                        pPoint.X = 45.1
                        pPoint.X = 46.1
                        pPoint.Y = 66.8
                        pPoint.Y = 66.3

                    ElseIf contatexto = 7 Then
                        pTextElement.Text = sele_plano2

                        pPoint.X = 45.1
                        pPoint.X = 46.1
                        pPoint.Y = 65.8
                        pPoint.Y = 65.3

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 45.1
                        pPoint.X = 46.1
                        pPoint.Y = 64.8
                        pPoint.Y = 64.3

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zonasel

                        pPoint.X = 51.9
                        pPoint.X = 52.9
                        pPoint.Y = 66.8
                        pPoint.Y = 66.3
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 50.5
                        pPoint.X = 51.5
                        pPoint.Y = 64.8
                        pPoint.Y = 64.3

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 51.2
                        pPoint.X = 52.2
                        pPoint.Y = 73.6
                        pPoint.Y = 72.6

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 8
                    ElseIf contatexto = 11 Then
                        pFont.Size = 15

                    Else
                        pFont.Size = 16
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A0 Horizontal" Then
                For contatexto = 1 To 11
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 111.35
                        pPoint.X = 112.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.6
                        posi_y_c1 = posi_y_c1 - 2.6


                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_expta1

                        pPoint.X = 111.35
                        pPoint.X = 112.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.6
                        posi_y_c1 = posi_y_c1 - 2.6

                    ElseIf contatexto = 3 Then

                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 111.35
                        pPoint.X = 112.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.6
                        posi_y_c1 = posi_y_c1 - 2.6

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1

                        pPoint.X = 111.35
                        pPoint.X = 112.35
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.6
                        posi_y_c1 = posi_y_c1 - 2.6


                    ElseIf contatexto = 5 Then
                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 93.8
                        pPoint.X = 94.8
                        pPoint.Y = 25.55
                        pPoint.Y = 24.55

                    ElseIf contatexto = 6 Then
                        pTextElement.Text = lista_cartas
                        pPoint.X = 94.3
                        pPoint.X = 95.5
                        pPoint.Y = 23.85
                        pPoint.Y = 22.85

                    ElseIf contatexto = 7 Then
                        pTextElement.Text = sele_plano2

                        pPoint.X = 94.3
                        pPoint.X = 95.5
                        pPoint.Y = 22.1
                        pPoint.Y = 21.1

                    ElseIf contatexto = 8 Then
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 94.3
                        pPoint.X = 95.5
                        pPoint.Y = 20.3
                        pPoint.Y = 19.6


                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zonasel

                        pPoint.X = 105.5
                        pPoint.X = 106.5
                        pPoint.Y = 23.85
                        pPoint.Y = 22.85

                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 106.5
                        pPoint.X = 107.5
                        pPoint.Y = 20.3
                        pPoint.Y = 19.6

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 105.2
                        pPoint.X = 106.4
                        pPoint.Y = 17.7
                        pPoint.Y = 16.7

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 15
                    ElseIf contatexto = 11 Then
                        pFont.Size = 24

                    Else
                        pFont.Size = 32
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            ElseIf sele_plano1 = "Plano A0 Vertical" Then
                For contatexto = 1 To 11
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    If contatexto = 1 Then
                        pTextElement.Text = v_explo1
                        pPoint.X = 77.1
                        pPoint.X = 78.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.3
                        posi_y_c1 = posi_y_c1 - 2.3

                    ElseIf contatexto = 2 Then
                        pTextElement.Text = v_expta1
                        pPoint.X = 77.1
                        pPoint.X = 78.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.3
                        posi_y_c1 = posi_y_c1 - 2.3

                    ElseIf contatexto = 3 Then

                        pTextElement.Text = v_explo_v_expta1
                        pPoint.X = 77.1
                        pPoint.X = 78.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.05
                        posi_y_c1 = posi_y_c1 - 2.05

                    ElseIf contatexto = 4 Then
                        pTextElement.Text = v_cierre1

                        pPoint.X = 77.1
                        pPoint.X = 78.1
                        pPoint.Y = posi_y_c
                        pPoint.Y = posi_y_c1
                        posi_y_c = posi_y_c - 2.3
                        posi_y_c1 = posi_y_c1 - 2.3

                    ElseIf contatexto = 5 Then

                        pTextElement.Text = lista_nmhojas
                        pPoint.X = 62.6
                        pPoint.X = 63.8
                        pPoint.Y = 95.9
                        pPoint.Y = 94.9

                    ElseIf contatexto = 6 Then
                        pTextElement.Text = lista_cartas
                        pPoint.X = 64.8
                        pPoint.X = 65.8
                        pPoint.Y = 94.6
                        pPoint.Y = 93.6

                    ElseIf contatexto = 7 Then
                        pTextElement.Text = sele_plano2

                        pPoint.X = 64.8
                        pPoint.X = 65.8
                        pPoint.Y = 93.2
                        pPoint.Y = 92.2
                    ElseIf contatexto = 8 Then
                        pTextElement.Text = DateTime.Today

                        pPoint.X = 64.8
                        pPoint.X = 65.8
                        pPoint.Y = 91.65
                        pPoint.Y = 90.65

                    ElseIf contatexto = 9 Then
                        pTextElement.Text = v_zonasel

                        pPoint.X = 72.8
                        pPoint.X = 74.8
                        pPoint.Y = 94.6
                        pPoint.Y = 93.6
                    ElseIf contatexto = 10 Then
                        pTextElement.Text = "Guido Valdivia"

                        pPoint.X = 72.8
                        pPoint.X = 73.8
                        pPoint.Y = 91.65
                        pPoint.Y = 90.65

                    ElseIf contatexto = 11 Then
                        pTextElement.Text = escalaf

                        pPoint.X = 72.9
                        pPoint.X = 73.9
                        pPoint.Y = 103.6
                        pPoint.Y = 102.6

                    End If
                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    'Simbolo del texto
                    pTxtSym = New TextSymbol

                    'fuente del texto

                    pFont = New StdFont
                    pFont.Name = "Tahoma"
                    'color del texto

                    myColor = New RgbColor
                    If contatexto = 5 Or contatexto = 6 Or contatexto = 7 Or contatexto = 8 Or contatexto = 9 Or contatexto = 10 Then
                        pFont.Size = 13
                    ElseIf contatexto = 11 Then
                        pFont.Size = 24

                    Else
                        pFont.Size = 32
                    End If

                    myColor.RGB = RGB(0, 0, 0)

                    pFont.Bold = False
                    pTxtSym.Font = pFont
                    pTxtSym.Color = myColor

                    'Propiedades del Simbolo

                    pTxtSym.Angle = 0
                    pTxtSym.RightToLeft = False
                    pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                    'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                    pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                    pTxtSym.CharacterSpacing = 20
                    pTxtSym.Case = esriTextCase.esriTCNormal
                    pTextElement.Symbol = pTxtSym
                    pGraphicsContainer.AddElement(pTextElement, 1)

                    'Refrescar solo los graficos del PageLayout
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
                Next contatexto
            End If
        ElseIf sele_reporte = "Demarca" Then
            For contatexto = 1 To 8
                'Guardar el texto
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    conta_x = v_nombre_dm.Length
                    If conta_x >= 38 Then
                        posi_x = Mid(v_nombre_dm, 1, 38)
                        posi_x1 = Right(v_nombre_dm, (v_nombre_dm.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = v_nombre_dm
                    End If
                    pPoint.X = 17.1
                    'pPoint.Y = 8.1
                    pPoint.Y = 9.1
                    pPoint.X = 21.7
                    'pPoint.Y = 8.9
                    pPoint.Y = 9.9
                ElseIf contatexto = 2 Then
                    pTextElement.Text = v_codigo
                    pPoint.X = 17.9
                    'pPoint.Y = 7.5
                    pPoint.Y = 8.2
                    pPoint.X = 21.7
                    'pPoint.Y = 8.3
                    pPoint.Y = 9.2
                ElseIf contatexto = 3 Then
                    'posi_x = ""
                    'posi_x1 = ""
                    'pTextElement.Text = lista_dist
                    conta_x = lista_dist.Length

                    If conta_x > 38 Then
                        posi_x = Mid(lista_dist, 1, 38)
                        posi_x1 = Right(lista_dist, (lista_dist.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = lista_dist
                    End If
                    pPoint.X = 17.9
                    'pPoint.Y = 6.9
                    pPoint.Y = 7.6
                    pPoint.X = 21.7
                    'pPoint.Y = 7.7
                    pPoint.Y = 8.4

                ElseIf contatexto = 4 Then
                    'pTextElement.Text = lista_prov
                    conta_x = lista_prov.Length
                    If conta_x > 38 Then
                        posi_x = Mid(lista_prov, 1, 38)
                        posi_x1 = Right(lista_prov, (lista_prov.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = lista_prov
                    End If
                    pPoint.X = 17.9
                    'pPoint.Y = 6.3
                    pPoint.Y = 6.8
                    pPoint.X = 21.7
                    'pPoint.Y = 7.1
                    pPoint.Y = 7.6
                ElseIf contatexto = 5 Then
                    'pTextElement.Text = lista_depa
                    conta_x = lista_depa.Length
                    If conta_x > 38 Then
                        posi_x = Mid(lista_depa, 1, 38)
                        posi_x1 = Right(lista_depa, (lista_depa.Length - 38))
                        pTextElement.Text = posi_x & vbNewLine & posi_x1
                    Else
                        pTextElement.Text = lista_depa
                    End If
                    pPoint.X = 17.9
                    'pPoint.Y = 5.7
                    pPoint.Y = 5.7
                    pPoint.X = 21.7
                    'pPoint.Y = 6.5
                    pPoint.Y = 6.7
                ElseIf contatexto = 6 Then
                    pTextElement.Text = fecha
                    pPoint.X = 16.1
                    pPoint.Y = 2.9
                    pPoint.X = 19.8
                    pPoint.Y = 2.1
                ElseIf contatexto = 7 Then
                    pTextElement.Text = escala_plano_dema
                    'pPoint.X = 17.3
                    pPoint.X = 18.3
                    pPoint.Y = 2.9
                    'pPoint.X = 21.1
                    pPoint.X = 22.1
                    pPoint.Y = 2.1

                ElseIf contatexto = 8 Then
                    pTextElement.Text = v_zona_dm
                    pPoint.X = 5.6
                    pPoint.Y = 2.12
                    pPoint.X = 9.6
                    pPoint.Y = 1.32
                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv

                'Simbolo del texto
                pTxtSym = New TextSymbol

                'fuente del texto

                pFont = New StdFont
                pFont.Name = "Tahoma"
                'If contatexto = 6 Then
                pFont.Size = 7
                'ElseIf contatexto = 7 Then
                'pFont.Size = 6
                'ElseIf contatexto = 8 Then
                'pFont.Size = 6
                'Else
                'pFont.Size = 7
                'End If
                pFont.Bold = False
                pTxtSym.Font = pFont
                'Propiedades del Simbolo
                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False

                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                'pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)
                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
        ElseIf sele_reporte = "Plano Venta" Then  'Reporte para plano Venta
            If sele_plano = "Formato A4" Then
                posi_y1 = 4.15
                posi_y = 4.85
                posi_y2 = 4.15
                posi_y3 = 4.85
            ElseIf sele_plano = "Formato A3" Then
                posi_y1 = 4.9
                posi_y = 5.6
                posi_y2 = 4.9
                posi_y3 = 5.6
            ElseIf sele_plano = "Formato A2" Then
                posi_y1 = 3.6
                posi_y = 4.3
                posi_y2 = 3.6
                posi_y3 = 4.3
            ElseIf sele_plano = "Formato A0" Then
                posi_y1 = 3.4
                posi_y = 4.1
                posi_y2 = 4.4
                posi_y3 = 4.1
            End If
            For contatexto = 1 To 15 'contador de textos
                pTextElement = New TextElement
                'Punto de ubicaci�n del texto
                pEnv = New Envelope
                pPoint = New Point
                If contatexto = 1 Then
                    pTextElement.Text = "HOJA : " & valor_codhoja  ' cabecera hoja
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 13.5
                        pPoint.Y = 25.4
                        pPoint.X = 15.5
                        pPoint.Y = 26.4
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 21.5
                        pPoint.Y = 36.5
                        pPoint.X = 23.5
                        pPoint.Y = 38.5

                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 33.5
                        pPoint.Y = 51.5
                        pPoint.X = 34.4
                        pPoint.Y = 54.1

                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 71.5
                        pPoint.Y = 106.6
                        pPoint.X = 73.0
                        pPoint.Y = 108.4
                    End If

                ElseIf contatexto = 2 Then

                    pTextElement.Text = valor_nmhoja  'nombre hoja
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.5
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.5
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 18.0
                        pPoint.Y = posi_y1 - 0.5
                        pPoint.X = 19.0
                        pPoint.Y = posi_y - 0.5
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then

                        pPoint.X = 23.5
                        pPoint.Y = posi_y1 + 3.05
                        pPoint.X = 26.6
                        pPoint.Y = posi_y + 3.05
                        posi_y = posi_y - 0.6

                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 50.5
                        pPoint.Y = posi_y1 + 10.9
                        pPoint.X = 53.6
                        pPoint.Y = posi_y + 10.9
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 3 Then
                    pTextElement.Text = valor_codhoja  'codigo hoja
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.42
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.42
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.4
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.4
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.8
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.8
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 9.7
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 9.7
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 4 Then

                    If sele_plano = "Formato A4" Then  ' tama�o
                        pTextElement.Text = "A4"
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.33
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.33
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pTextElement.Text = "A3"
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.35
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.35
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pTextElement.Text = "A2"
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.4
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.4
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pTextElement.Text = "A0"
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 8.35
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 8.35
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 5 Then

                    pTextElement.Text = fecha   'fecha
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 8.8
                        pPoint.Y = posi_y1 - 0.29
                        pPoint.X = 13.5
                        pPoint.Y = posi_y - 0.29
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 17.5
                        pPoint.Y = posi_y1 - 0.3
                        pPoint.X = 19.5
                        pPoint.Y = posi_y - 0.3
                        posi_y = posi_y - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 24.5
                        pPoint.Y = posi_y1 + 2.05
                        pPoint.X = 27.6
                        pPoint.Y = posi_y + 2.05
                        posi_y = posi_y - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 52.8
                        pPoint.Y = posi_y1 + 7.1
                        pPoint.X = 55.9
                        pPoint.Y = posi_y + 7.1
                        posi_y = posi_y - 0.5
                    End If

                ElseIf contatexto = 6 Then

                    'pTextElement.Text = "PSAD-56"       'datum
                    pTextElement.Text = v_sistema
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 14.5
                        pPoint.Y = posi_y2 - 0.5
                        pPoint.X = 16.2
                        pPoint.Y = posi_y3 - 0.5
                        posi_y3 = posi_y3 - 0.5
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 22.0
                        pPoint.Y = posi_y2 - 0.5
                        pPoint.X = 23.5
                        pPoint.Y = posi_y3 - 0.5
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 3.1
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 3.1
                        posi_y3 = posi_y3 - 0.5
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 11.0
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 11.0
                        posi_y3 = posi_y3 - 0.5
                    End If


                ElseIf contatexto = 7 Then
                    pTextElement.Text = v_zona_dm   'zona
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 14.5
                        pPoint.Y = posi_y2 - 0.42
                        pPoint.X = 16.2
                        pPoint.Y = posi_y3 - 0.42
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 22.0
                        pPoint.Y = posi_y2 - 0.4
                        pPoint.X = 23.5
                        pPoint.Y = posi_y3 - 0.4
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 2.7
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 2.7
                        posi_y3 = posi_y3 - 0.53

                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 9.8
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 9.8
                        posi_y3 = posi_y3 - 0.53
                    End If

                ElseIf contatexto = 8 Then

                    pTextElement.Text = valor_zoncat   'zona catastral
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 14.5
                        pPoint.Y = posi_y2 - 0.33
                        pPoint.X = 16.2
                        pPoint.Y = posi_y3 - 0.33
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 22.0
                        pPoint.Y = posi_y2 - 0.35
                        pPoint.X = 23.5
                        pPoint.Y = posi_y3 - 0.35
                        posi_y3 = posi_y3 - 0.7
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 32.0
                        pPoint.Y = posi_y2 + 2.3
                        pPoint.X = 33.0
                        pPoint.Y = posi_y3 + 2.3
                        posi_y3 = posi_y3 - 0.53
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 64.5
                        pPoint.Y = posi_y2 + 8.4
                        pPoint.X = 65.5
                        pPoint.Y = posi_y3 + 8.4
                        posi_y3 = posi_y3 - 0.53
                    End If


                ElseIf contatexto = 9 Then
                    pTextElement.Text = "GUIDO VALDIVIA PONCE"   'autor
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 13.3
                        pPoint.Y = posi_y2 - 0.25
                        pPoint.X = 15.0
                        pPoint.Y = posi_y3 - 0.25
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 20.5
                        pPoint.Y = posi_y2 - 0.3
                        pPoint.X = 21.9
                        pPoint.Y = posi_y3 - 0.3
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 29.9
                        pPoint.Y = posi_y2 + 2.0
                        pPoint.X = 30.9
                        pPoint.Y = posi_y3 + 2.0
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 61.0
                        pPoint.Y = posi_y2 + 7.1
                        pPoint.X = 62.3
                        pPoint.Y = posi_y3 + 7.1
                    End If

                ElseIf contatexto = 10 Then  'DM Uso MInero
                    If canti_capa_actmin > 0 Then
                        pTextElement.Text = canti_capa_actmin   'contador capa_actmin
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 10.4
                        pPoint.Y = posi_y2 + 1.2
                        pPoint.X = 12.3
                        pPoint.Y = posi_y3 + 1.2
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.8
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.8
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 4.9
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 4.9
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 13.1
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 13.1
                    End If

                ElseIf contatexto = 11 Then  'DM Actividad Minera
                    If canti_capa_usomin > 0 Then
                        pTextElement.Text = canti_capa_usomin  'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 10.4
                        pPoint.Y = posi_y2 + 0.67
                        pPoint.X = 12.3
                        pPoint.Y = posi_y3 + 0.67
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.0
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.0
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 3.8
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 3.8
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 10.9
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 10.9
                    End If
                ElseIf contatexto = 12 Then  'Exploracion
                    If v_explo1 > 0 Then
                        pTextElement.Text = v_explo1  'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 5.8
                        pPoint.Y = posi_y2 + 1.02
                        pPoint.X = 7.9
                        pPoint.Y = posi_y3 + 1.02
                        posi_y3 = posi_y3 - 0.8
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.0
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.0
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 3.8
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 3.8
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 10.9
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 10.9
                    End If
                ElseIf contatexto = 13 Then  'Exploracion y explotacion
                    If v_explo_v_expta1 > 0 Then
                        pTextElement.Text = v_explo_v_expta1  'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 5.8
                        pPoint.Y = posi_y2 + 1.02
                        pPoint.X = 7.9
                        pPoint.Y = posi_y3 + 1.02
                        posi_y3 = posi_y3 - 0.8
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.0
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.0
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 3.8
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 3.8
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 10.9
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 10.9
                    End If




                ElseIf contatexto = 14 Then  'Explotacion
                    If v_expta1 > 0 Then
                        pTextElement.Text = v_expta1   'contador Capa Uso mi
                    Else
                        pTextElement.Text = "0"
                    End If
                    If sele_plano = "Formato A4" Then
                        pPoint.X = 5.8
                        pPoint.Y = posi_y2 + 1.02
                        pPoint.X = 7.9
                        pPoint.Y = posi_y3 + 1.02
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.0
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.0
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 3.8
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 3.8
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 10.9
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 10.9
                    End If

                ElseIf contatexto = 15 Then  'CONTADOR A�O

                    pTextElement.Text = lostr_Join_Codigos_AREA   'contador Capa Uso mi

                    If sele_plano = "Formato A4" Then
                        pPoint.X = 6.8
                        pPoint.Y = posi_y2 + 1.02
                        pPoint.X = 8.9
                        pPoint.Y = posi_y3 + 1.02
                    ElseIf sele_plano = "Formato A3" Then
                        pPoint.X = 16.25
                        pPoint.Y = posi_y2 + 1.0
                        pPoint.X = 17.65
                        pPoint.Y = posi_y3 + 1.0
                    ElseIf sele_plano = "Formato A2" Then
                        pPoint.X = 23.1
                        pPoint.Y = posi_y2 + 3.8
                        pPoint.X = 25.1
                        pPoint.Y = posi_y3 + 3.8
                    ElseIf sele_plano = "Formato A0" Then
                        pPoint.X = 49.0
                        pPoint.Y = posi_y2 + 10.9
                        pPoint.X = 50.3
                        pPoint.Y = posi_y3 + 10.9
                    End If

                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                'Simbolo del texto
                pTxtSym = New TextSymbol
                'fuente del texto

                'Dim pFont As IFontDisp
                pFont = New StdFont
                pFont.Name = "Tahoma"
                If sele_plano = "Formato A4" Then
                    pFont.Size = 4.2
                ElseIf sele_plano = "Formato A3" Then
                    pFont.Size = 5.5
                ElseIf sele_plano = "Formato A2" Then
                    pFont.Size = 8
                ElseIf sele_plano = "Formato A0" Then
                    pFont.Size = 14
                End If
                pFont.Bold = False
                pTxtSym.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym.Color = myColor
                'Propiedades del Simbolo

                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal

                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

            Next contatexto
        End If
    End Sub

    Public Sub LEERESULTADOSEVAL(ByVal tipo_plano As String)
        Dim pFeatureCursor As IFeatureCursor
        Dim pFeatLayer As IFeatureLayer = Nothing
        Dim fclas_tema As IFeatureClass
        Dim pMxApp As IMxApplication
        Dim cadenatexto2 As String
        Dim cadenatexto3 As String
        Dim cadenatexto As String


        conta_libredenu = conta_libredenu + 1
        'pMxApp = pApp
        pMxApp = p_App
        'pMxDoc = pApp.Document

        Dim c1 As String = ""
        Dim c2 As String = ""
        Dim c3 As String = ""
        Dim c4 As String = ""
        Dim c5 As String = ""
        Dim c6 As String = ""
        If tipo_plano = "Plano_Evaluacion" Then
            If procesoautmatico_eval = "" Then  'cuando no es proceso automatico pasa aqui


                lodtbReporte.Columns.Add("CONTADOR", Type.GetType("System.String"))
                lodtbReporte.Columns.Add("CONCESION", Type.GetType("System.String"))
                lodtbReporte.Columns.Add("TIPO_EX", Type.GetType("System.String"))
                lodtbReporte.Columns.Add("CODIGOU", Type.GetType("System.String"))
                lodtbReporte.Columns.Add("ESTADO", Type.GetType("System.String"))
                lodtbReporte.Columns.Add("EVAL", Type.GetType("System.String"))
            End If
        ElseIf tipo_plano = "Plano_libredenu" Then
            If conta_libredenu = 1 Then
                lodtbReporte.Columns.Add("CONTADOR", Type.GetType("System.String"))
                lodtbReporte.Columns.Add("CONCESION", Type.GetType("System.String"))
                lodtbReporte.Columns.Add("TIPO_EX", Type.GetType("System.String"))
                lodtbReporte.Columns.Add("CODIGOU", Type.GetType("System.String"))
                lodtbReporte.Columns.Add("ESTADO", Type.GetType("System.String"))
                lodtbReporte.Columns.Add("EVAL", Type.GetType("System.String"))
            End If
        End If

        Dim pFeatSelection As IFeatureSelection
        Dim consulta As IQueryFilter
        Dim capa_sele As ISelectionSet
        pMap = pMxDoc.FocusMap

        Dim pElement As IElement
        Dim pTextElement As ITextElement

        Dim afound As Boolean = False
        For A As Integer = 0 To pMap.LayerCount - 1
            If pMap.Layer(A).Name = "Catastro" Then
                pFeatLayer = pMap.Layer(A)
                pFeatureLayer = pFeatLayer
                afound = True
                Exit For
            End If
        Next A
        If Not afound Then
            MsgBox("No se encuentra el Layer")
            Exit Sub
        End If

        'Buscando los campos del tema

        fclas_tema = pFeatLayer.FeatureClass
        pFields = fclas_tema.Fields

        c1 = pFields.FindField("EVAL")
        c2 = pFields.FindField("TIPO_EX")
        c3 = pFields.FindField("ESTADO")
        c4 = pFields.FindField("CONCESION")
        c5 = pFields.FindField("CODIGOU")
        c6 = pFields.FindField("CONTADOR")
        Dim v_conta As String
        pFeatureCursor = pFeatLayer.Search(Nothing, False)
        'Relacionando al layout
        Dim pPageLayout As IPageLayout
        Dim pActiveView As IActiveView
        Dim pGraphicsContainer As IGraphicsContainer
        Dim myColor As IRgbColor
        'Define layout
        pPageLayout = pMxDoc.PageLayout
        pPageLayout.Page.Units = esriUnits.esriCentimeters
        pActiveView = pPageLayout
        pGraphicsContainer = pPageLayout

        'Para obtener fecha

        'Dim fecha_plano As Date
        'fecha_plano = Date

        Dim pEnv As IEnvelope
        Dim pFont As IFontDisp
        pFeature = pFeatureCursor.NextFeature
        Dim posi_y As Double
        Dim posi_y1 As Double
        Dim posi_x As Double
        Dim posi_x1 As Double
        Dim posi_y_priori As Double
        Dim posi_y_priori1 As Double
        Dim posi_x_priori As Double
        Dim posi_x_priori1 As Double
        Dim conta_an As Long
        Dim cuenta_ev As Long
        Dim sele_criterio As String = ""

        'fuente del texto

        pFont = New StdFont
        'pFont.Name = "Tahoma"
        'pFont.Size = 5

        pFont.Name = "Courier New"
        'pFont.Size = 5       

        pFont.Size = 6.0     'se cambio 2015
        pFont.Bold = False
        pTxtSym1 = New TextSymbol
        pTxtSym1.Font = pFont
        myColor = New RgbColor
        myColor.RGB = RGB(0, 0, 0)
        pTxtSym1.Color = myColor
        'Propiedades del Simbolo
        pTxtSym1.Angle = 0
        pTxtSym1.RightToLeft = False
        pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
        pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
        pTxtSym1.CharacterSpacing = 20
        pTxtSym1.Case = esriTextCase.esriTCNormal
        'Barriendo los 4 criterios AN, PO, SI, Ex

        For cuenta_ev = 1 To 5
            If cuenta_ev = "1" Then
                sele_criterio = "PR"
                'Define posicion "Y" por cada criterio
                posi_y = 16.2
                posi_y1 = 15.6
                posi_x = 14.1
                posi_x1 = 18.2
                posi_y_priori = posi_y
                posi_y_priori1 = posi_y1
                posi_x_priori = posi_x
                posi_x_priori1 = posi_x1

            ElseIf cuenta_ev = "2" Then
                'Define posicion "Y" por cada criterio
                sele_criterio = "AR"
                posi_y = posi_y_priori
                posi_y1 = posi_y_priori1
                posi_x = posi_x_priori
                posi_x1 = posi_x_priori1

            ElseIf cuenta_ev = "3" Then
                'Define posicion "Y" por cada criterio
                sele_criterio = "PO"
                posi_y = posi_y_priori
                posi_y1 = posi_y_priori1
                posi_x = posi_x_priori
                posi_x1 = posi_x_priori1
            ElseIf cuenta_ev = "4" Then
                'Define posicion "Y" por cada criterio
                sele_criterio = "SI"
                posi_y = posi_y_priori
                posi_y1 = posi_y_priori1
                posi_x = posi_x_priori
                posi_x1 = posi_x_priori1
            ElseIf cuenta_ev = "5" Then
                'Define posicion "Y" por cada criterio
                sele_criterio = "EX"
                posi_y = posi_y_priori
                posi_y1 = posi_y_priori1
                posi_x = posi_x_priori
                posi_x1 = posi_x_priori1
            End If

            'Haciendo la consulta segun criterio
            '-------------------------------------

            consulta = New QueryFilter
            pFeatSelection = pFeatLayer
            consulta.WhereClause = "EVAL = '" & sele_criterio & "'"
            'consulta.WhereClause = "EVAL = 'AN'"
            pMxDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, Nothing, Nothing)
            pFeatSelection.SelectFeatures(consulta, esriSelectionResultEnum.esriSelectionResultNew, False)
            pMxDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, Nothing, Nothing)

            pFeatSelection = pFeatLayer
            capa_sele = pFeatSelection.SelectionSet
            capa_sele.Search(Nothing, True, pFeatureCursor)
            fclas_tema = pFeatLayer.FeatureClass
            If capa_sele.Count > 0 Then  ' Si existe
                'Definendo posicion de la ubicacion del texto en layout              

                'posi_y = posi_y_priori - 0.3
                'posi_y1 = posi_y_priori1 - 0.3
                'posi_x = posi_x_priori
                'posi_x1 = posi_x_priori1
                If sele_criterio = "PR" Then
                    Cuenta_an = capa_sele.Count
                    posi_y = posi_y_priori - 0.3
                    posi_y1 = posi_y_priori1 - 0.3
                    posi_x = posi_x_priori
                    posi_x1 = posi_x_priori1
                ElseIf sele_criterio = "AR" Then
                    Cuenta_rd = capa_sele.Count
                    conta_rd_posi_y = posi_y_priori - 0.3   ' Para colocar como fijo DM Posteriores
                    conta_rd_posi_y1 = posi_y_priori1 - 0.3 ' Para colocar como fijo DM Posteriores
                ElseIf sele_criterio = "PO" Then
                    Cuenta_po = capa_sele.Count
                    conta_po_posi_y = posi_y_priori - 0.3   ' Para colocar como fijo DM Posteriores
                    conta_po_posi_y1 = posi_y_priori1 - 0.3 ' Para colocar como fijo DM Posteriores
                ElseIf sele_criterio = "SI" Then
                    Cuenta_si = capa_sele.Count
                    conta_si_posi_y = posi_y_priori - 0.3   ' Para colocar como fijo DM Simultaneos
                    conta_si_posi_y1 = posi_y_priori1 - 0.3 ' Para colocar como fijo DM Simultaneos
                ElseIf sele_criterio = "EX" Then
                    Cuenta_ex = capa_sele.Count
                    conta_ex_posi_y = posi_y_priori - 0.2   ' Para colocar como fijo DM Extinguidos
                    conta_ex_posi_y1 = posi_y_priori1 - 0.2 ' Para colocar como fijo DM Extinguidos
                Else
                    Cuenta_an = capa_sele.Count
                End If

                conta_an = 0
                pFeature = pFeatureCursor.NextFeature

                'Obteniendo los valores de los campos
                Dim contatexto As Integer
                Do Until pFeature Is Nothing
                    conta_an = conta_an + 1
                    v_eval_x = pFeature.Value(c1)
                    v_tipo_exp_x = pFeature.Value(c2)
                    v_estado_x = pFeature.Value(c3)
                    nombre_dm_x = pFeature.Value(c4)
                    v_codigo_x = pFeature.Value(c5)
                    v_conta = pFeature.Value(c6)
                    If v_conta = "99" Then
                        '  MsgBox("1", MsgBoxStyle.Critical, "x")
                    End If
                    If tipo_plano = "Plano_Evaluacion" Or tipo_plano = "Plano_libredenu" Then
                        '*****************
                        If procesoautmatico_eval = "" Then  'cuando no es proceso automatico pasa aqui
                            Dim dRow As DataRow
                            dRow = lodtbReporte.NewRow
                            dRow.Item("CONTADOR") = v_conta
                            dRow.Item("CONCESION") = nombre_dm_x
                            dRow.Item("TIPO_EX") = v_tipo_exp_x
                            dRow.Item("CODIGOU") = v_codigo_x
                            dRow.Item("ESTADO") = v_estado_x
                            dRow.Item("EVAL") = v_eval_x
                            lodtbReporte.Rows.Add(dRow)
                        End If

                    End If
                    '*****************
                    If conta_an = 1 Then  ' Para 1er punto
                        posi_y1 = posi_y1 - 0.8
                        posi_y = posi_y - 0.8

                    Else  'Para n puntos
                        posi_y1 = posi_y1
                        posi_y = posi_y
                    End If
                    If ((posi_y < 1.2) Or (posi_y1 < 1.2)) Then
                        conta_hoja_sup = conta_hoja_sup + 1
                        If sele_criterio = "PR" Then
                            v_posi_pr = True
                            v_posi_rd = True
                            v_posi_po = True
                            v_posi_si = True
                            v_posi_ex = True
                            Cuenta_po = 0
                            Cuenta_si = 0
                            Cuenta_ex = 0
                            Cuenta_rd = 0
                        ElseIf sele_criterio = "AR" Then
                            v_posi_rd = True
                            v_posi_po = True
                            v_posi_si = True
                            v_posi_ex = True
                            Cuenta_si = 0
                            Cuenta_ex = 0
                            Cuenta_rd = 0
                        ElseIf sele_criterio = "PO" Then
                            v_posi_po = True
                            v_posi_si = True
                            v_posi_ex = True
                            Cuenta_si = 0
                            Cuenta_ex = 0
                            Cuenta_po = 0

                        ElseIf sele_criterio = "SI" Then
                            v_posi_si = True
                            v_posi_ex = True
                            Cuenta_si = 0
                            Cuenta_ex = 0
                        ElseIf sele_criterio = "EX" Then
                            v_posi_ex = True
                            Cuenta_ex = 0
                        End If

                        pFeatSelection.Clear()
                        '  Exit Sub

                        If conta_hoja_sup > 50 Then
                            Exit Sub
                        Else
                        End If
                    End If

                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point


                    If Len(v_codigo_x) = 13 Then
                        cadenatexto3 = v_codigo_x & "   "
                    ElseIf Len(v_codigo_x) = 12 Then
                        cadenatexto3 = v_codigo_x & "    "
                    ElseIf Len(v_codigo_x) = 11 Then
                        cadenatexto3 = v_codigo_x & "     "
                    ElseIf Len(v_codigo_x) = 10 Then
                        cadenatexto3 = v_codigo_x & "      "
                    ElseIf Len(v_codigo_x) = 9 Then
                        cadenatexto3 = v_codigo_x & "       "
                    ElseIf Len(v_codigo_x) = 8 Then
                        cadenatexto3 = v_codigo_x & "        "
                    End If


                    If Len(v_conta).ToString = 1 Then
                        cadenatexto2 = (nombre_dm_x.PadRight(25, " "))



                        'cadenatexto = v_campo1 & "    " & cadenatexto2 & "   " & cadenatexto3 & v_campo4 & "   " & v_campo5 & "     " & v_campo6 & "     " & v_campo7 & "      " & cadenatexto4 & "   " & v_campo9

                        cadenatexto = v_conta & "    " & cadenatexto2 & "   " & cadenatexto3 & v_tipo_exp_x & "     " & v_eval_x & "       " & v_estado_x

                    ElseIf Len(v_conta).ToString = 2 Then
                        cadenatexto2 = (nombre_dm_x.PadRight(25, " "))
                        cadenatexto = v_conta & "   " & cadenatexto2 & "   " & cadenatexto3 & v_tipo_exp_x & "     " & v_eval_x & "       " & v_estado_x
                    ElseIf Len(v_conta).ToString = 3 Then
                        cadenatexto2 = (nombre_dm_x.PadRight(25, " "))
                        cadenatexto = v_conta & "  " & cadenatexto2 & "   " & cadenatexto3 & v_tipo_exp_x & "     " & v_eval_x & "       " & v_estado_x
                    End If

                    pTextElement.Text = cadenatexto
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 0.3
                    pPoint.X = 18.2
                    pPoint.Y = posi_y


                    'pPoint.X = 17.2
                    'pPoint.Y = posi_y1 - 0.3
                    'pPoint.X = 18.2
                    'pPoint.Y = posi_y

                    'pTextElement.Text = cadenatexto
                    'pPoint.X = 17.2
                    'pPoint.Y = posi_y1 - 0.6
                    'pPoint.X = 18.2
                    'pPoint.Y = posi_y





                    'Define elemento texto de texto nombre de DM
                    'For contatexto = 1 To 6
                    '    pTextElement = New TextElement
                    '    pEnv = New Envelope
                    '    pPoint = New Point
                    '    If contatexto = 1 Then
                    '        pTextElement.Text = v_conta
                    '        pPoint.X = 14.1
                    '        pPoint.Y = posi_y1 - 0.3
                    '        pPoint.X = 18.2
                    '        pPoint.Y = posi_y

                    '    ElseIf contatexto = 2 Then
                    '        pTextElement.Text = nombre_dm_x
                    '        pPoint.X = 20.8
                    '        pPoint.Y = posi_y1 - 0.3
                    '        pPoint.X = 19.2
                    '        pPoint.Y = posi_y
                    '    ElseIf contatexto = 3 Then
                    '        pTextElement.Text = v_tipo_exp_x
                    '        pPoint.X = 21.1
                    '        pPoint.Y = posi_y1 - 0.3
                    '        pPoint.X = 23.7
                    '        pPoint.Y = posi_y
                    '    ElseIf contatexto = 4 Then
                    '        pTextElement.Text = v_codigo_x
                    '        pPoint.X = 22.1
                    '        pPoint.Y = posi_y1 - 0.3
                    '        pPoint.X = 24.7
                    '        pPoint.Y = posi_y
                    '    ElseIf contatexto = 5 Then
                    '        pTextElement.Text = v_eval_x
                    '        pPoint.X = 26.7
                    '        pPoint.Y = posi_y1 - 0.3
                    '        pPoint.X = 27.1
                    '        pPoint.Y = posi_y
                    '    ElseIf contatexto = 6 Then

                    '        pTextElement.Text = v_estado_x
                    '        pPoint.X = 27.2
                    '        pPoint.Y = posi_y1 - 0.3
                    '        pPoint.X = 27.9
                    '        pPoint.Y = posi_y
                    '    End If

                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    pTextElement.Symbol = pTxtSym1
                    pGraphicsContainer.AddElement(pTextElement, 1)
                    ' Next contatexto
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

                    posi_y1 = posi_y1 - 0.35
                    posi_y = posi_y - 0.35
                    pFeature = pFeatureCursor.NextFeature


                Loop

                posi_y1 = posi_y1 + 0.2
                posi_y = posi_y + 0.2

                pFeatSelection.Clear()  'Limpia el tema
                posi_y_priori = posi_y + 0.1   'Posicion ultima
                posi_y_priori1 = posi_y1 + 0.1 'Posicion ultima

            Else  ' Si en caso es cero se define posicion

                'posi_y = posi_y_priori - 0.6
                'posi_y1 = posi_y_priori1 - 0.6
                posi_y = posi_y_priori - 0.4  'posicion entre item generales
                posi_y1 = posi_y_priori1 - 0.4
                If sele_criterio = "PR" Then
                    'posi_y_priori = posi_y + 0.6
                    'posi_y_priori1 = posi_y1 + 0.6
                    posi_y_priori = posi_y + 0.6
                    posi_y_priori1 = posi_y1 + 0.6
                    conta_po_posi_y = posi_y_priori
                    conta_po_posi_y = posi_y_priori1
                    posi_y_priori = posi_y
                    posi_y_priori1 = posi_y1
                ElseIf sele_criterio = "PO" Then
                    posi_y_priori = posi_y + 0.6
                    posi_y_priori1 = posi_y1 + 0.6
                    conta_po_posi_y = posi_y_priori
                    conta_po_posi_y = posi_y_priori1
                    posi_y_priori = posi_y
                    posi_y_priori1 = posi_y1

                ElseIf sele_criterio = "AR" Then
                    If v_tipo_exp = "RD" Then
                        posi_y_priori = posi_y + 0.6
                        posi_y_priori1 = posi_y1 + 0.6
                        conta_rd_posi_y = posi_y_priori
                        conta_rd_posi_y = posi_y_priori1
                        posi_y_priori = posi_y
                        posi_y_priori1 = posi_y1
                    End If


                ElseIf sele_criterio = "SI" Then
                    posi_y_priori = posi_y + 0.6
                    posi_y_priori1 = posi_y1 + 0.6
                    conta_si_posi_y = posi_y_priori
                    conta_si_posi_y = posi_y_priori1
                    posi_y_priori = posi_y
                    posi_y_priori1 = posi_y1
                ElseIf sele_criterio = "EX" Then
                    posi_y_priori = posi_y + 0.6
                    posi_y_priori1 = posi_y1 + 0.6
                    conta_ex_posi_y = posi_y_priori
                    conta_ex_posi_y = posi_y_priori1
                    posi_y_priori = posi_y
                    posi_y_priori1 = posi_y1
                End If

            End If
        Next cuenta_ev  'Termino barrido
        posi_y_priori1_CNM = posi_y_priori1
        posi_y_priori_CNM = posi_y_priori
        'MsgBox(lodtbReporte)
    End Sub

    Public Sub LEERESULTADOSEVAL_SIM(ByVal tipo_plano As String)
        Dim pFeatureCursor As IFeatureCursor
        Dim pFeatLayer As IFeatureLayer = Nothing
        Dim fclas_tema As IFeatureClass
        Dim pMxApp As IMxApplication
        Dim cadenatexto2 As String
        Dim cadenatexto3 As String
        Dim cadenatexto As String
        Dim cadenatexto_coor As String
        conta_libredenu = conta_libredenu + 1
        pMxApp = p_App
        Dim c1 As String = ""
        Dim c2 As String = ""
        Dim c3 As String = ""
        Dim c4 As String = ""
        Dim c5 As String = ""
        Dim c6 As String = ""
        Dim pFeatSelection As IFeatureSelection
        Dim consulta As IQueryFilter
        Dim capa_sele As ISelectionSet
        pMap = pMxDoc.FocusMap
        Dim pElement As IElement
        Dim pTextElement As ITextElement
        Dim afound As Boolean = False
        For A As Integer = 0 To pMap.LayerCount - 1
            If pMap.Layer(A).Name = "Catastro_sim" Then
                pFeatLayer = pMap.Layer(A)
                pFeatureLayer = pFeatLayer
                afound = True
                Exit For
            End If
        Next A
        If Not afound Then
            MsgBox("No se encuentra el Layer")
            Exit Sub
        End If
        'Buscando los campos del tema
        fclas_tema = pFeatLayer.FeatureClass
        pFields = fclas_tema.Fields
        c1 = pFields.FindField("CODIGOU")
        c2 = pFields.FindField("CONCESION")
        c3 = pFields.FindField("TIT_CONCES")
        c4 = pFields.FindField("HECTAGIS")
        c5 = pFields.FindField("TIPO_EX")
        Dim v_conta As String
        pFeatureCursor = pFeatLayer.Search(Nothing, False)
        'Relacionando al layout
        Dim pPageLayout As IPageLayout
        Dim pActiveView As IActiveView
        Dim pGraphicsContainer As IGraphicsContainer
        Dim myColor As IRgbColor
        'Define layout
        pPageLayout = pMxDoc.PageLayout
        pPageLayout.Page.Units = esriUnits.esriCentimeters
        pActiveView = pPageLayout
        pGraphicsContainer = pPageLayout
        'Para obtener fecha
        Dim MyDate As Date
        MyDate = Now
        Dim fecha As String
        'fecha = RellenarComodin(MyDate.Year, 2, "0") & "/" & RellenarComodin(MyDate.Month, 2, "0") & "/" & MyDate.Day
        fecha = MyDate.Day & "/" & RellenarComodin(MyDate.Month, 2, "0") & "/" & RellenarComodin(MyDate.Year, 2, "0")
        Dim pEnv As IEnvelope
        Dim pFont As IFontDisp
        Dim posi_y As Double
        Dim posi_y1 As Double
        Dim posi_x As Double
        Dim posi_x1 As Double
        Dim posi_y_priori As Double
        Dim posi_y_priori1 As Double
        Dim posi_x_priori As Double
        Dim posi_x_priori1 As Double
        Dim conta_an As Long
        Dim cuenta_ev As Long
        Dim sele_criterio As String = ""
        pFeature = pFeatureCursor.NextFeature
        'fuente del texto
        pFont = New StdFont
        'pFont.Name = "Tahoma"
        'pFont.Size = 5
        pFont.Name = "Courier New"
        'pFont.Size = 5       
        pFont.Size = 6.1     ' 5.5    'se cambio 2015
        pFont.Bold = False
        pTxtSym1 = New TextSymbol
        pTxtSym1.Font = pFont
        myColor = New RgbColor
        myColor.RGB = RGB(0, 0, 0)
        pTxtSym1.Color = myColor
        'Propiedades del Simbolo
        pTxtSym1.Angle = 0
        pTxtSym1.RightToLeft = False
        pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
        pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
        pTxtSym1.CharacterSpacing = 20
        pTxtSym1.Case = esriTextCase.esriTCNormal

        posi_y = 8.2   '9.7     9.4
        posi_y1 = 7.6    '9.1    8.8
        posi_x = 14.1
        posi_x1 = 18.2
        fclas_tema = pFeatLayer.FeatureClass
        pFeatureCursor = fclas_tema.Search(Nothing, False)
        pFeature = pFeatureCursor.NextFeature

        'Obteniendo los valores de los campos
        Dim contatexto As Integer
        Dim v_titconces_x As String     ' se agrego 
        Do Until pFeature Is Nothing
            conta_an = conta_an + 1
            v_codigo_x = pFeature.Value(c1)
            nombre_dm_x = pFeature.Value(c2)
            v_titconces_x = pFeature.Value(c3)
            v_hecta_x = pFeature.Value(c4)
            v_tipo_exp_x = pFeature.Value(c5)
            ' Para 1er punto
            posi_y1 = posi_y1 - 0.2   '0.4
            posi_y = posi_y - 0.3   '0.4  0.8   <---------------------

            For contador As Integer = 0 To 4
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                pFont = New StdFont
                pFont.Name = "Courier New"
                pFont.Size = 6.1  '6.25     'se cambio 2015
                pFont.Bold = False
                pTxtSym1 = New TextSymbol
                pTxtSym1.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym1.Color = myColor
                pTxtSym1.Angle = 0
                pTxtSym1.RightToLeft = False
                pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym1.CharacterSpacing = 20
                pTxtSym1.Case = esriTextCase.esriTCNormal
                If contador = 0 Then
                    pTextElement.Text = v_codigo_x
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 0.7
                    pPoint.X = 18.2
                    pPoint.Y = posi_y
                ElseIf contador = 1 Then
                    pTextElement.Text = nombre_dm_x
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 0.7
                    pPoint.X = 19.7
                    pPoint.Y = posi_y
                ElseIf contador = 2 Then
                    pTextElement.Text = v_titconces_x
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 0.7
                    pPoint.X = 22.5
                    pPoint.Y = posi_y
                ElseIf contador = 3 Then
                    pTextElement.Text = v_hecta_x
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 0.7
                    pPoint.X = 27.65
                    pPoint.Y = posi_y
                ElseIf contador = 4 Then
                    pTextElement.Text = v_tipo_exp_x
                    pPoint.X = 14.1
                    pPoint.Y = posi_y1 - 0.7
                    pPoint.X = 28.2
                    pPoint.Y = posi_y
                End If
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTextElement.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement, 1)
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            Next contador
            posi_y1 = posi_y1 - 0.2
            posi_y = posi_y - 0.2
            pFeature = pFeatureCursor.NextFeature
        Loop

        'Do Until pFeature Is Nothing
        '    conta_an = conta_an + 1
        '    v_codigo_x = pFeature.Value(c1)
        '    nombre_dm_x = pFeature.Value(c2)
        '    v_titconces_x = pFeature.Value(c3)
        '    v_hecta_x = pFeature.Value(c4)
        '    v_tipo_exp_x = pFeature.Value(c5)

        '    ' Para 1er punto
        '    posi_y1 = posi_y1 - 0.2   '0.4
        '    posi_y = posi_y - 0.3   '0.4  0.8   <---------------------

        '    pTextElement = New TextElement
        '    pEnv = New Envelope
        '    pPoint = New Point

        '    If Len(v_codigo_x) = 13 Then
        '        cadenatexto3 = v_codigo_x & " "
        '    ElseIf Len(v_codigo_x) = 12 Then
        '        cadenatexto3 = v_codigo_x & " "
        '    ElseIf Len(v_codigo_x) = 11 Then
        '        cadenatexto3 = v_codigo_x & " "
        '    ElseIf Len(v_codigo_x) = 10 Then
        '        cadenatexto3 = v_codigo_x & " "
        '    ElseIf Len(v_codigo_x) = 9 Then
        '        cadenatexto3 = v_codigo_x & " "
        '    ElseIf Len(v_codigo_x) = 8 Then
        '        cadenatexto3 = v_codigo_x & " "
        '    End If
        '    cadenatexto = cadenatexto3 & nombre_dm_x & "   " & v_titconces_x & "   " & v_hecta_x & "   " & v_tipo_exp_x
        '    pTextElement.Text = cadenatexto

        '    pPoint.X = 14.1
        '    pPoint.Y = posi_y1 - 0.7
        '    pPoint.X = 18.2
        '    pPoint.Y = posi_y

        '    pEnv.UpperRight = pPoint
        '    pElement = pTextElement
        '    pElement.Geometry = pEnv
        '    pTextElement.Symbol = pTxtSym1
        '    pGraphicsContainer.AddElement(pTextElement, 1)
        '    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

        '    posi_y1 = posi_y1 - 0.2
        '    posi_y = posi_y - 0.2
        '    pFeature = pFeatureCursor.NextFeature
        'Loop
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim v_coorsim As DataTable
        Dim v_numver, v_corest, v_cornor As String
        Dim x_i, y_i, x_k, y_k, xxi, xxk, yyi, yyk, zx, zy As Integer

        posi_y = 15.2
        posi_y1 = 14.6
        posi_x = 14.1
        posi_x1 = 18.2

        dm_sim = v_codigo       '  OJO SE AGREGO PARA PROBAR

        v_coorsim = cls_oracle.FT_coorsim(dm_sim)

        posi_y1 = posi_y1 - 0.2   '0.4
        posi_y = posi_y - 0.3   '0.4  0.8

        posi_y = 15.9   'se agrego
        '--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        Try
            Dim reg_final As Integer = dt_coordenada.Rows.Count - 1
            Dim vertice(0 To reg_final, 0 To 3) As String
            Dim num_ver_error As Integer = 0
            For i As Integer = 0 To reg_final
                vertice(i, 0) = Convert.ToString(dt_coordenada.Rows(i).Item("num"))
                vertice(i, 1) = Convert.ToString(dt_coordenada.Rows(i).Item("x"))
                vertice(i, 2) = Convert.ToString(dt_coordenada.Rows(i).Item("y"))
                vertice(i, 3) = Convert.ToString(0)
            Next i

            For P As Integer = 0 To reg_final
                If P = 0 Then
                    If (Convert.ToSingle(vertice(P, 1)) / Convert.ToSingle(vertice(reg_final, 1)) = 1) And (Convert.ToSingle(vertice(P, 1)) / Convert.ToSingle(vertice(P + 1, 1)) = 1) Then
                        vertice(P, 3) = "1"
                        num_ver_error = num_ver_error + 1
                    End If
                    If (Convert.ToSingle(vertice(P, 2)) / Convert.ToSingle(vertice(reg_final, 2)) = 1) And (Convert.ToSingle(vertice(P, 2)) / Convert.ToSingle(vertice(P + 1, 2)) = 1) Then
                        vertice(P, 3) = "1"
                        num_ver_error = num_ver_error + 1
                    End If
                ElseIf P = reg_final Then
                    If (Convert.ToSingle(vertice(P, 1)) / Convert.ToSingle(vertice(P - 1, 1)) = 1) And (Convert.ToSingle(vertice(P, 1)) / Convert.ToSingle(vertice(0, 1)) = 1) Then
                        vertice(P, 3) = "1"
                        num_ver_error = num_ver_error + 1
                    End If
                    If (Convert.ToSingle(vertice(P, 2)) / Convert.ToSingle(vertice(P - 1, 2)) = 1) And (Convert.ToSingle(vertice(P, 2)) / Convert.ToSingle(vertice(0, 2)) = 1) Then
                        vertice(P, 3) = "1"
                        num_ver_error = num_ver_error + 1
                    End If
                Else
                    If (Convert.ToSingle(vertice(P, 1)) / Convert.ToSingle(vertice(P - 1, 1)) = 1) And (Convert.ToSingle(vertice(P, 1)) / Convert.ToSingle(vertice(P + 1, 1)) = 1) Then
                        vertice(P, 3) = "1"
                        num_ver_error = num_ver_error + 1
                    End If
                    If (Convert.ToSingle(vertice(P, 2)) / Convert.ToSingle(vertice(P - 1, 2)) = 1) And (Convert.ToSingle(vertice(P, 2)) / Convert.ToSingle(vertice(P + 1, 2)) = 1) Then
                        vertice(P, 3) = "1"
                        num_ver_error = num_ver_error + 1
                    End If
                End If
            Next P

            Dim verticev(0 To reg_final - num_ver_error, 0 To 3) As String
            num_ver = 0
            For q As Integer = 0 To reg_final
                If vertice(q, 3) <> "1" Then
                    verticev(num_ver, 0) = Convert.ToString(num_ver) 'vertice(num_ver, 0)
                    verticev(num_ver, 1) = vertice(q, 1)
                    verticev(num_ver, 2) = vertice(q, 2)
                    verticev(num_ver, 3) = vertice(q, 3)
                    num_ver = num_ver + 1
                Else
                    num_ver = q
                End If
            Next q

            'For contador As Integer = 0 To dt_coordenada.Rows.Count - 1
            For contador As Integer = 0 To num_ver - 1
                'For contador As Integer = 0 To v_coorsim.Rows.Count - 1
                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point

                pFont = New StdFont
                pFont.Name = "Courier New"
                pFont.Size = 8.0  '6.25     'se cambio 2015
                pFont.Bold = False
                pTxtSym1 = New TextSymbol
                pTxtSym1.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym1.Color = myColor
                pTxtSym1.Angle = 0
                pTxtSym1.RightToLeft = False
                pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym1.CharacterSpacing = 20
                pTxtSym1.Case = esriTextCase.esriTCNormal

                'v_numver = v_coorsim.Rows(contador).Item("CD_NUMVER")
                'v_corest = v_coorsim.Rows(contador).Item("CD_COREST")
                'v_cornor = v_coorsim.Rows(contador).Item("CD_CORNOR")
                'v_numver = dt_coordenada.Rows(contador).Item("num")
                'v_corest = dt_coordenada.Rows(contador).Item("x")
                'v_cornor = dt_coordenada.Rows(contador).Item("y")

                v_numver = verticev(contador, 0)
                v_corest = verticev(contador, 1)
                v_cornor = verticev(contador, 2)

                'cadenatexto_coor = v_numver & "            " & v_cornor & "         " & v_corest
                cadenatexto_coor = (v_numver + 1) & "     " & v_cornor & ".00" & "     " & v_corest & ".00"
                pTextElement.Text = cadenatexto_coor

                pPoint.X = 14.0   '14.1
                pPoint.Y = posi_y1 - 0.7    ' 0.4
                pPoint.X = 20.2   '21.0   '18.2   ---- unico cambio
                pPoint.Y = posi_y
                posi_y = posi_y - 0.4    '0.2        Este es el que controla el espaciamiento entre lineas

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTextElement.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement, 1)
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            Next contador
        Catch ex As Exception

        End Try

        Dim v_hojasim As DataTable
        Dim v_cd_hoja, v_nm_hoja, tipo As String

        tipo = "3"
        If op_sim = 1 Then
            v_hojasim = cls_oracle.FT_hojasim(v_codigo, tipo)
        Else
            v_hojasim = cls_oracle.FT_hojasim(dm_sim, tipo)
        End If

        For contador As Integer = 0 To v_hojasim.Rows.Count - 1
            v_cd_hoja = v_hojasim.Rows(contador).Item("CD_HOJA")
            v_nm_hoja = v_hojasim.Rows(contador).Item("NM_HOJA")
        Next contador

        For contatexto = 1 To 6
            pTextElement = New TextElement
            pEnv = New Envelope
            pPoint = New Point
            If contatexto = 1 Then
                pFont = New StdFont
                pFont.Name = "Courier New"
                pFont.Size = 9.0  '6.25     'se cambio 2015
                pFont.Bold = False
                pTxtSym1 = New TextSymbol
                pTxtSym1.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym1.Color = myColor
                pTxtSym1.Angle = 0
                pTxtSym1.RightToLeft = False
                pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym1.CharacterSpacing = 20
                pTxtSym1.Case = esriTextCase.esriTCNormal

                'pTextElement.Text = "�rea UTM: " & hectagis_min & " Ha."
                pTextElement.Text = "�rea UTM: " & v_areasim & ".00" & " Ha."
                posi_y = posi_y - 0.2    '  espacio entre el �ltimo vertice y el hectagis

                pPoint.X = 12.2   '14.1
                pPoint.Y = posi_y1 - 0.7    ' 0.4
                pPoint.X = 21.2  '22.1   '18.2   ---- unico cambio
                pPoint.Y = posi_y
                posi_y = posi_y - 0.4
            ElseIf contatexto = 2 Then
                pFont = New StdFont
                pFont.Name = "Courier New"
                pFont.Size = 6.8   '6.25  'se cambio 2015
                pFont.Bold = False
                pTxtSym1 = New TextSymbol
                pTxtSym1.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym1.Color = myColor
                pTxtSym1.Angle = 0
                pTxtSym1.RightToLeft = False
                pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym1.CharacterSpacing = 20
                pTxtSym1.Case = esriTextCase.esriTCNormal
                pTextElement.Text = "Fecha: " & fecha
                pPoint.X = 11.4
                pPoint.Y = 18.6
                pPoint.X = 15.0   '15.1
                pPoint.Y = 17.8
            ElseIf contatexto = 3 Then
                pFont = New StdFont
                pFont.Name = "Arial Black"
                pFont.Size = 10.75     'se cambio 2015
                pFont.Bold = False
                pTxtSym1 = New TextSymbol
                pTxtSym1.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym1.Color = myColor
                pTxtSym1.Angle = 0
                pTxtSym1.RightToLeft = False
                pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym1.CharacterSpacing = 20
                pTxtSym1.Case = esriTextCase.esriTCNormal

                pTextElement.Text = "�REA " & sgrupo_sim
                pPoint.X = 18.5
                pPoint.Y = 18.1
                pPoint.X = 22.3
                pPoint.Y = 17.3
            ElseIf contatexto = 4 Then
                pFont = New StdFont
                pFont.Name = "Courier New"
                pFont.Size = 6.8   '6.25     'se cambio 2015
                pFont.Bold = False
                pTxtSym1 = New TextSymbol
                pTxtSym1.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym1.Color = myColor
                pTxtSym1.Angle = 0
                pTxtSym1.RightToLeft = False
                pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym1.CharacterSpacing = 20
                pTxtSym1.Case = esriTextCase.esriTCNormal

                pTextElement.Text = v_cd_hoja
                pPoint.X = 5.6
                pPoint.Y = 19.1
                pPoint.X = 9.3
                pPoint.Y = 18.3
            ElseIf contatexto = 5 Then
                pFont = New StdFont
                pFont.Name = "Courier New"
                pFont.Size = 6.8    '6.25     'se cambio 2015
                pFont.Bold = False
                pTxtSym1 = New TextSymbol
                pTxtSym1.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym1.Color = myColor
                pTxtSym1.Angle = 0
                pTxtSym1.RightToLeft = False
                pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym1.CharacterSpacing = 20
                pTxtSym1.Case = esriTextCase.esriTCNormal

                pTextElement.Text = v_nm_hoja
                pPoint.X = 5.4
                pPoint.Y = 18.6
                pPoint.X = 9.1
                pPoint.Y = 17.8
            ElseIf contatexto = 6 Then
                pFont = New StdFont
                pFont.Name = "Courier New"
                pFont.Size = 8
                pFont.Bold = True
                pTxtSym1 = New TextSymbol
                pTxtSym1.Font = pFont
                myColor = New RgbColor
                myColor.RGB = RGB(0, 0, 0)
                pTxtSym1.Color = myColor
                pTxtSym1.Angle = 0
                pTxtSym1.RightToLeft = False
                pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym1.CharacterSpacing = 20
                pTxtSym1.Case = esriTextCase.esriTCNormal

                pTextElement.Text = v_sistema
                pPoint.X = 5.3
                pPoint.Y = 2.23
                pPoint.X = 9.0
                pPoint.Y = 1.43
            End If
            pEnv.UpperRight = pPoint
            pElement = pTextElement
            pElement.Geometry = pEnv
            pTextElement.Symbol = pTxtSym1
            pGraphicsContainer.AddElement(pTextElement, 1)
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        Next contatexto

        ' pFeatSelection.Clear()  'Limpia el tema
        posi_y_priori = posi_y + 0.1   'Posicion ultima
        posi_y_priori1 = posi_y1 + 0.1 'Posicion ultima

    End Sub

    Public Function Leer_Resultados_Eval_Reporte() As DataTable
        Dim dRow As DataRow
        Dim pFeatureCursor As IFeatureCursor
        Dim pFeatLayer As IFeatureLayer = Nothing
        Dim fclas_tema As IFeatureClass
        Dim pMxApp As IMxApplication
        pMxApp = p_App
        Dim c1 As String = ""
        Dim c2 As String = ""
        Dim c3 As String = ""
        Dim c4 As String = ""
        Dim c5 As String = ""
        Dim c6 As String = ""
        lodtbReporte = New DataTable
        lodtbReporte.Columns.Add("CONTADOR", Type.GetType("System.String"))
        lodtbReporte.Columns.Add("CONCESION", Type.GetType("System.String"))
        lodtbReporte.Columns.Add("TIPO_EX", Type.GetType("System.String"))
        lodtbReporte.Columns.Add("CODIGOU", Type.GetType("System.String"))
        lodtbReporte.Columns.Add("ESTADO", Type.GetType("System.String"))
        lodtbReporte.Columns.Add("EVAL", Type.GetType("System.String"))
        Dim pFeatSelection As IFeatureSelection
        Dim consulta As IQueryFilter
        Dim capa_sele As ISelectionSet
        pMap = pMxDoc.FocusMap
        Dim afound As Boolean = False
        For A As Integer = 0 To pMap.LayerCount - 1
            If pMap.Layer(A).Name = "Catastro" Then
                pFeatLayer = pMap.Layer(A)
                pFeatureLayer = pFeatLayer
                afound = True
                Exit For
            End If
        Next A
        If Not afound Then
            MsgBox("No se encuentra el Layer")
            Exit Function
        End If
        'Buscando los campos del tema
        fclas_tema = pFeatLayer.FeatureClass
        pFields = fclas_tema.Fields
        c1 = pFields.FindField("EVAL")
        c2 = pFields.FindField("TIPO_EX")
        c3 = pFields.FindField("ESTADO")
        c4 = pFields.FindField("CONCESION")
        c5 = pFields.FindField("CODIGOU")
        c6 = pFields.FindField("CONTADOR")
        pFeatureCursor = pFeatLayer.Search(Nothing, False)
        pFeature = pFeatureCursor.NextFeature
        Dim cuenta_ev As Long
        Dim sele_criterio As String = ""
        For cuenta_ev = 1 To 5
            Select Case cuenta_ev
                Case 1
                    sele_criterio = "PR"
                Case 2
                    sele_criterio = "AR"
                Case 3
                    sele_criterio = "PO"
                Case 4
                    sele_criterio = "SI"
                Case 5
                    sele_criterio = "AR"
            End Select
            consulta = New QueryFilter
            pFeatSelection = pFeatLayer
            consulta.WhereClause = "EVAL = '" & sele_criterio & "'"
            pFeatSelection.SelectFeatures(consulta, esriSelectionResultEnum.esriSelectionResultNew, False)
            capa_sele = pFeatSelection.SelectionSet
            Select Case sele_criterio
                Case "PR"
                    Cuenta_an = capa_sele.Count
                Case "PO"
                    Cuenta_po = capa_sele.Count
                Case "SI"
                    Cuenta_si = capa_sele.Count
                Case "EX"
                    Cuenta_ex = capa_sele.Count
                Case "AR"
                    Cuenta_rd = capa_sele.Count
            End Select
            capa_sele.Search(Nothing, True, pFeatureCursor)
            fclas_tema = pFeatLayer.FeatureClass
            pFeature = pFeatureCursor.NextFeature
            'Obteniendo los valores de los campos
            Do Until pFeature Is Nothing
                dRow = lodtbReporte.NewRow
                dRow.Item("CONTADOR") = pFeature.Value(c6)
                dRow.Item("CONCESION") = pFeature.Value(c4)
                dRow.Item("TIPO_EX") = pFeature.Value(c2)
                dRow.Item("CODIGOU") = pFeature.Value(c5)
                dRow.Item("ESTADO") = pFeature.Value(c3)
                Select Case pFeature.Value(c1) 'xxxxxx
                    Case "PR"
                        'If Cuenta_an = 0 Then
                        'dRow.Item("EVAL") = "DERECHOS PRIORITARIOS :  No Presenta DM Prioritarios"
                        'Else
                        dRow.Item("EVAL") = "DERECHOS PRIORITARIOS : " & "(" & Cuenta_an & ")"
                        'End If
                    Case "AR"
                        'If Cuenta_rd = 0 Then
                        'dRow.Item("EVAL") = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea del derecho extinguido y publicado de libre denunciabilidad "
                        'Else
                        dRow.Item("EVAL") = "Petitorio formulado al amparo del Art.12 de la Ley 26615, sobre el �rea del derecho extinguido y publicado de libre denunciabilidad " & "(" & Cuenta_rd & ")"
                        'End If
                    Case "PO"
                        'If Cuenta_po = 0 Then
                        'If v_tipo_exp = "RD" Then
                        'dRow.Item("EVAL") = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA : No Presenta "
                        'Else
                        'dRow.Item("EVAL") = "DERECHOS POSTERIORES :  No Presenta DM Posteriores"
                        'End If
                        'Else
                        If v_tipo_exp = "RD" Then
                            dRow.Item("EVAL") = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA: " & "(" & Cuenta_rd & ")"
                        Else
                            dRow.Item("EVAL") = "DERECHOS POSTERIORES : " & "(" & Cuenta_po & ")"
                        End If
                        'End If
                    Case "SI"
                        'If Cuenta_si = 0 Then
                        'dRow.Item("EVAL") = "DERECHOS SIMULTANEOS :  No Presenta DM Simultaneos"
                        'Else
                        dRow.Item("EVAL") = "DERECHOS SIMULTANEOS :  " & "(" & Cuenta_si & ")"
                        'End If
                    Case "EX"
                        'If Cuenta_ex = 0 Then
                        'dRow.Item("EVAL") = "DERECHOS EXTINGUIDOS :  No Presenta DM Extinguidos"
                        'Else
                        dRow.Item("EVAL") = "DERECHOS EXTINGUIDOS :  " & "(" & Cuenta_ex & ")"
                        'End If
                End Select
                lodtbReporte.Rows.Add(dRow)
                pFeatSelection.Clear()
                pFeature = pFeatureCursor.NextFeature
            Loop
        Next cuenta_ev
        'Dim dRow As DataRow
        If Cuenta_an = 0 Then
            dRow = lodtbReporte.NewRow
            dRow.Item("EVAL") = "DERECHOS PRIORITARIOS :  No Presenta DM Prioritarios"
        End If
        If Cuenta_po = 0 Then
            If v_tipo_exp = "RD" Then
                dRow = lodtbReporte.NewRow
                dRow.Item("EVAL") = "DERECHOS QUE DEBEN RESPETAR EL AREA PETICIONADA : No Presenta "
                lodtbReporte.Rows.Add(dRow)
            Else
                dRow = lodtbReporte.NewRow
                dRow.Item("EVAL") = "DERECHOS POSTERIORES :  No Presenta DM Posteriores"
                lodtbReporte.Rows.Add(dRow)
            End If
        End If
        If Cuenta_si = 0 Then
            dRow = lodtbReporte.NewRow
            dRow.Item("EVAL") = "DERECHOS SIMULTANEOS :  No Presenta DM Simultaneos"
            lodtbReporte.Rows.Add(dRow)
        End If
        If Cuenta_ex = 0 Then
            dRow = lodtbReporte.NewRow
            dRow.Item("EVAL") = "DERECHOS EXTINGUIDOS :  No Presenta DM Extinguidos"
            lodtbReporte.Rows.Add(dRow)
        End If
        'lodtbReporte.Rows.Add(dRow)
        Return lodtbReporte
    End Function

    Public Sub GENERALISTADODM_PLANOEV1(ByVal p_App As IApplication)
        If v_posi_pr = False And v_posi_po = False And v_posi_si = False And v_posi_ex = False Then
            Dim pFeatureCursor_1 As IFeatureCursor
            Dim pfeature_1 As IFeature

            Dim pFeatLayer As IFeatureLayer = Nothing
            Dim fclas_tema As IFeatureClass
            Dim pMxApp As IMxApplication
            Dim pdisplay As IScreenDisplay

            ' Dim pFeature_1 As IFeature

            pMxApp = p_App
            Dim c1 As String
            Dim c2 As String
            Dim c3 As String
            Dim c4 As String
            Dim c5 As String
            Dim c6 As String
            Dim c7 As String
            Dim c8 As String
            Dim c9 As String
            pMap = pMxDoc.FocusMap

            Dim afound As Boolean = False
            For A As Integer = 0 To pMap.LayerCount - 1
                If pMap.Layer(A).Name = "Catastro" Then
                    pFeatLayer = pMap.Layer(A)
                    pFeatureLayer = pFeatLayer
                    afound = True
                    Exit For
                End If
            Next A
            If Not afound Then
                MsgBox("No se encuentra la capa Catastro en la vista para generar listado en plano", MsgBoxStyle.Critical, "Observacion")
                Exit Sub
            End If

            ' pFeatLayer = pFeatureLayer
            'fclas_tema = pFeatLayer.FeatureClass
            fclas_tema = pFeatureLayer.FeatureClass
            pFields = fclas_tema.Fields
            c1 = pFields.FindField("CONTADOR")
            c2 = pFields.FindField("CONCESION")
            c3 = pFields.FindField("CODIGOU")
            c4 = pFields.FindField("ZONA")
            c5 = pFields.FindField("TIPO_EX")
            c6 = pFields.FindField("ESTADO")
            c7 = pFields.FindField("DE_PUBL")
            c8 = pFields.FindField("DE_IDEN")
            c9 = pFields.FindField("NATURALEZA")
            Dim v_campo1 As String
            Dim v_campo2 As String
            Dim v_campo3 As String
            Dim v_campo4 As String
            Dim v_campo5 As String
            Dim v_campo6 As String
            Dim v_campo7 As String
            Dim v_campo8 As String
            Dim v_campo9 As String

            'pFeatureCursor = pFeatureLayer.Search(Nothing, False)

            'Relacionando al layout
            Dim pPageLayout As IPageLayout
            Dim pActiveView As IActiveView
            Dim pGraphicsContainer1 As IGraphicsContainer
            'Dim pElement As IElement
            Dim pElement1 As IElement
            Dim pElement2 As IElement
            Dim pElement3 As IElement
            Dim pElement4 As IElement
            Dim pElement5 As IElement
            Dim pElement6 As IElement
            Dim pElement7 As IElement
            Dim pElement8 As IElement
            Dim pElement9 As IElement

            Dim pTextElement1 As ITextElement
            Dim pTextElement2 As ITextElement
            Dim pTextElement3 As ITextElement
            Dim pTextElement4 As ITextElement
            Dim pTextElement5 As ITextElement
            Dim pTextElement6 As ITextElement
            Dim pTextElement7 As ITextElement
            Dim pTextElement8 As ITextElement
            Dim pTextElement9 As ITextElement

            'Dim contatexto As Integer
            Dim myColor As IRgbColor

            'Define layout

            pPageLayout = pMxDoc.PageLayout
            pdisplay = pMxApp.Display

            ' pPageLayout.Page.Units = esriUnits.esriCentimeters
            ' pActiveView = pPageLayout
            pGraphicsContainer = pPageLayout
            pGraphicsContainer.Reset()
            'Para obtener fecha
            Dim pEnv As IEnvelope
            Dim pFont As IFontDisp
            'pFeature = pFeatureCursor.NextFeature
            Dim posi_y As Double
            Dim posi_y1 As Double
            posi_y1 = posi_y1_list
            posi_y = posi_y2_list
            '  fclas_tema = pFeatLayer.FeatureClass
            pTxtSym1 = New TextSymbol
            pFont = New StdFont
            pFont.Name = "Tahoma"

            pFont.Size = 4.5
            'pFont.Size = 6.5
            pFont.Bold = False
            pTxtSym1.Font = pFont
            myColor = New RgbColor
            myColor.RGB = RGB(0, 0, 0)
            pTxtSym1.Color = myColor

            pTxtSym1.Angle = 0
            pTxtSym1.RightToLeft = False
            pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
            pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
            pTxtSym1.CharacterSpacing = 20
            pTxtSym1.Case = esriTextCase.esriTCNormal
            Dim conta1 As Integer = 0
            Dim pQueryFilter As IQueryFilter

            'Dim pfeature_2 As IFeature
            'Dim pFeatureCursor_2 As IFeatureCursor
            ' pFeatureCursor_2 = pFeatureLayer.Search(Nothing, False)
            'pfeature_2 = pFeatureCursor_2.NextFeature


            'Do Until pfeature_2 Is Nothing
            'conta1 = conta1 + 1

            'v_campo3 = Trim(pfeature_2.Value(c3))

            'pQueryFilter = New QueryFilter
            'pQueryFilter.WhereClause = "CODIGOU = '" & v_campo3 & "'"
            'pFeatureCursor_1 = pFeatureLayer.Search(pQueryFilter, True)
            pFeatureCursor_1 = pFeatureLayer.Search(Nothing, False)
            pfeature_1 = pFeatureCursor_1.NextFeature
            ' Dim conta1 As Integer = 0

            Do Until pfeature_1 Is Nothing

                conta1 = conta1 + 1
                '    MsgBox(conta1)
                v_campo1 = pfeature_1.Value(c1)
                v_campo2 = Trim(pfeature_1.Value(c2))
                v_campo3 = Trim(pfeature_1.Value(c3))


                If v_campo3 = v_codigo Then  'Si es DM evaluado                
                    myColor = New RgbColor
                    myColor.RGB = RGB(169, 0, 230)
                    pTxtSym1.Color = myColor
                Else
                    myColor = New RgbColor
                    myColor.RGB = RGB(0, 0, 0)
                    pTxtSym1.Color = myColor
                End If

                v_campo4 = Trim(pfeature_1.Value(c4))
                v_campo5 = Trim(pfeature_1.Value(c5))
                v_campo6 = Trim(pfeature_1.Value(c6))
                v_campo7 = Trim(pfeature_1.Value(c7))
                If v_campo7 = "" Then
                    v_campo7 = "NP"
                End If
                v_campo8 = Trim(pfeature_1.Value(c8))
                If v_campo8 = "" Then
                    v_campo8 = "NI"
                End If
                v_campo9 = Trim(pfeature_1.Value(c9))
                If ((posi_y < 1.2) Or (posi_y1 < 1.2)) Then
                    Exit Sub
                End If

                'Dim contatexto As Integer
                ' For contatexto = 1 To 9
                pTextElement1 = New TextElement
                pTextElement2 = New TextElement
                pTextElement3 = New TextElement
                pTextElement4 = New TextElement
                pTextElement5 = New TextElement
                pTextElement6 = New TextElement
                pTextElement7 = New TextElement
                pTextElement8 = New TextElement
                pTextElement9 = New TextElement
                pElement1 = pTextElement1
                pElement2 = pTextElement2
                pElement3 = pTextElement3
                pElement4 = pTextElement4
                pElement5 = pTextElement5
                pElement6 = pTextElement6
                pElement7 = pTextElement7
                pElement8 = pTextElement8
                pElement9 = pTextElement9

                pEnv = New Envelope
                pPoint = New Point
                ' Select Case contatexto
                'Guardar el texto
                '    Case 1
                pTextElement1.Text = v_campo1
                pPoint.X = 17.2
                pPoint.Y = posi_y1 - 0.6
                pPoint.X = 18.2
                pPoint.Y = posi_y

                pEnv.UpperRight = pPoint
                'pElement = pTextElement1
                pElement1.Geometry = pEnv
                pTextElement1.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement1, 1)
                '   posi_y1 = posi_y1 - 0.3
                '   posi_y = posi_y - 0.3
                '   Case 2

                pEnv = New Envelope
                pPoint = New Point
                pTextElement2.Text = v_campo2
                pPoint.X = 20.5
                pPoint.Y = posi_y1 - 0.6
                pPoint.X = 18.7
                pPoint.Y = posi_y

                pEnv.UpperRight = pPoint
                pElement2 = pTextElement2
                pElement2.Geometry = pEnv
                pTextElement2.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement2, 1)
                ' posi_y1 = posi_y1 - 0.3
                'posi_y = posi_y - 0.3
                '  Case 3

                pEnv = New Envelope
                pPoint = New Point

                pTextElement3.Text = v_campo3
                pPoint.X = 22.9
                pPoint.Y = posi_y1 - 0.6
                pPoint.X = 21.9
                pPoint.Y = posi_y


                pEnv.UpperRight = pPoint
                pElement3 = pTextElement3
                pElement3.Geometry = pEnv
                pTextElement3.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement3, 1)
                'posi_y1 = posi_y1 - 0.3
                ' posi_y = posi_y - 0.3

                ' Case 4

                pEnv = New Envelope
                pPoint = New Point
                pTextElement4.Text = v_campo4
                pPoint.X = 24.3
                pPoint.Y = posi_y1 - 0.6
                pPoint.X = 23.5
                pPoint.Y = posi_y

                pEnv.UpperRight = pPoint
                pElement4 = pTextElement4
                pElement4.Geometry = pEnv
                pTextElement4.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement4, 1)
                ' posi_y1 = posi_y1 - 0.3
                ' posi_y = posi_y - 0.3


                ''Case 5

                pEnv = New Envelope
                pPoint = New Point

                pTextElement5.Text = v_campo5
                pPoint.X = 25.0
                pPoint.Y = posi_y1 - 0.6
                pPoint.X = 24.2
                pPoint.Y = posi_y

                pEnv.UpperRight = pPoint
                pElement5 = pTextElement5
                pElement5.Geometry = pEnv
                pTextElement5.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement5, 1)
                '   posi_y1 = posi_y1 - 0.3
                '  posi_y = posi_y - 0.3

                'Case 6

                pEnv = New Envelope
                pPoint = New Point

                pTextElement6.Text = v_campo6
                pPoint.X = 25.8
                pPoint.Y = posi_y1 - 0.6
                pPoint.X = 25.0
                pPoint.Y = posi_y

                pEnv.UpperRight = pPoint
                pElement6 = pTextElement6
                pElement6.Geometry = pEnv
                pTextElement6.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement6, 1)
                '  posi_y1 = posi_y1 - 0.3
                '  posi_y = posi_y - 0.3

                'Case 7

                pEnv = New Envelope
                pPoint = New Point

                pTextElement7.Text = v_campo7
                pPoint.X = 26.6
                pPoint.Y = posi_y1 - 0.6
                pPoint.X = 25.8
                pPoint.Y = posi_y

                pEnv.UpperRight = pPoint
                pElement7 = pTextElement7
                pElement7.Geometry = pEnv
                pTextElement7.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement7, 1)
                '   posi_y1 = posi_y1 - 0.3
                ' posi_y = posi_y - 0.3

                'Case 8

                pEnv = New Envelope
                pPoint = New Point

                pTextElement8.Text = v_campo8
                pPoint.X = 27.4
                pPoint.Y = posi_y1 - 0.6
                pPoint.X = 26.6
                pPoint.Y = posi_y


                pEnv.UpperRight = pPoint
                pElement8 = pTextElement8
                pElement8.Geometry = pEnv
                pTextElement8.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement8, 1)
                '  posi_y1 = posi_y1 - 0.3
                '  posi_y = posi_y - 0.3

                'Case 9

                pEnv = New Envelope
                pPoint = New Point

                pTextElement9.Text = v_campo9
                pPoint.X = 28.2
                pPoint.Y = posi_y1 - 0.6
                pPoint.X = 27.4
                pPoint.Y = posi_y


                'End If
                'End Select

                pEnv.UpperRight = pPoint
                pElement9 = pTextElement9
                pElement9.Geometry = pEnv
                pTextElement9.Symbol = pTxtSym1
                pGraphicsContainer.AddElement(pTextElement9, 1)
                posi_y1 = posi_y1 - 0.3
                posi_y = posi_y - 0.3

                'Next contatexto

                'pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
                'posi_y1 = posi_y1 - 0.3
                'posi_y = posi_y - 0.3
                'pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
                pMxDoc.ActiveView.Refresh()

                'End If
                pfeature_1 = pFeatureCursor_1.NextFeature
                ' Loop

                'pfeature_2 = pFeatureCursor_2.NextFeature

            Loop
            '  pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        End If
    End Sub
    Public Sub GENERALISTADODM_PLANOEV_ant()
        If v_posi_pr = False And v_posi_po = False And v_posi_si = False And v_posi_ex = False Then
            Dim pFeatureCursor_1 As IFeatureCursor
            Dim pfeature_1 As IFeature

            Dim pFeatLayer As IFeatureLayer = Nothing
            Dim fclas_tema As IFeatureClass
            Dim pMxApp As IMxApplication
            ' Dim pFeature_1 As IFeature

            pMxApp = p_App
            Dim c1 As String
            Dim c2 As String
            Dim c3 As String
            Dim c4 As String
            Dim c5 As String
            Dim c6 As String
            Dim c7 As String
            Dim c8 As String
            Dim c9 As String
            pMap = pMxDoc.FocusMap

            Dim afound As Boolean = False
            For A As Integer = 0 To pMap.LayerCount - 1
                If pMap.Layer(A).Name = "Catastro" Then
                    pFeatLayer = pMap.Layer(A)
                    'pFeatureLayer = pFeatLayer
                    afound = True
                    Exit For
                End If
            Next A
            If Not afound Then
                MsgBox("No se encuentra la capa Catastro en la vista para generar listado en plano", MsgBoxStyle.Critical, "Observacion")
                Exit Sub
            End If

            ' pFeatLayer = pFeatureLayer
            fclas_tema = pFeatLayer.FeatureClass
            'fclas_tema = pFeatureLayer.FeatureClass
            pFields = fclas_tema.Fields
            c1 = pFields.FindField("CONTADOR")
            c2 = pFields.FindField("CONCESION")
            c3 = pFields.FindField("CODIGOU")
            c4 = pFields.FindField("ZONA")
            c5 = pFields.FindField("TIPO_EX")
            c6 = pFields.FindField("ESTADO")
            c7 = pFields.FindField("DE_PUBL")
            c8 = pFields.FindField("DE_IDEN")
            c9 = pFields.FindField("NATURALEZA")
            Dim v_campo1 As String
            Dim v_campo2 As String
            Dim v_campo3 As String
            Dim v_campo4 As String
            Dim v_campo5 As String
            Dim v_campo6 As String
            Dim v_campo7 As String
            Dim v_campo8 As String
            Dim v_campo9 As String
            pFeatureCursor_1 = pFeatLayer.Search(Nothing, False)
            'pFeatureCursor = pFeatureLayer.Search(Nothing, False)

            'Relacionando al layout
            Dim pPageLayout As IPageLayout
            Dim pActiveView As IActiveView
            Dim pGraphicsContainer As IGraphicsContainer
            Dim pElement As IElement
            Dim pTextElement As ITextElement

            'Dim contatexto As Integer
            Dim myColor As IRgbColor

            'Define layout

            pPageLayout = pMxDoc.PageLayout
            pPageLayout.Page.Units = esriUnits.esriCentimeters
            pActiveView = pPageLayout
            pGraphicsContainer = pPageLayout

            'Para obtener fecha
            Dim pEnv As IEnvelope
            Dim pFont As IFontDisp
            'pFeature = pFeatureCursor.NextFeature
            Dim posi_y As Double
            Dim posi_y1 As Double
            posi_y1 = posi_y1_list
            posi_y = posi_y2_list
            '  fclas_tema = pFeatLayer.FeatureClass
            pTxtSym1 = New TextSymbol
            pFont = New StdFont
            pFont.Name = "Tahoma"

            pFont.Size = 4.5
            'pFont.Size = 6.5
            pFont.Bold = False
            pTxtSym1.Font = pFont
            myColor = New RgbColor
            myColor.RGB = RGB(0, 0, 0)
            pTxtSym1.Color = myColor

            pTxtSym1.Angle = 0
            pTxtSym1.RightToLeft = False
            pTxtSym1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
            pTxtSym1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull

            pTxtSym1.CharacterSpacing = 20
            pTxtSym1.Case = esriTextCase.esriTCNormal
            pfeature_1 = pFeatureCursor_1.NextFeature

            Dim conta1 As Integer = 0

            Do Until pfeature_1 Is Nothing
                conta1 = conta1 + 1
                'MsgBox(conta1)
                v_campo1 = pfeature_1.Value(c1)
                v_campo2 = Trim(pfeature_1.Value(c2))
                v_campo3 = Trim(pfeature_1.Value(c3))
                If v_campo3 = v_codigo Then  'Si es DM evaluado                
                    myColor = New RgbColor
                    myColor.RGB = RGB(169, 0, 230)
                    pTxtSym1.Color = myColor
                Else
                    myColor = New RgbColor
                    myColor.RGB = RGB(0, 0, 0)
                    pTxtSym1.Color = myColor
                End If

                v_campo4 = Trim(pfeature_1.Value(c4))
                v_campo5 = Trim(pfeature_1.Value(c5))
                v_campo6 = Trim(pfeature_1.Value(c6))
                v_campo7 = Trim(pfeature_1.Value(c7))
                If v_campo7 = "" Then
                    v_campo7 = "NP"
                End If
                v_campo8 = Trim(pfeature_1.Value(c8))
                If v_campo8 = "" Then
                    v_campo8 = "NI"
                End If
                v_campo9 = Trim(pfeature_1.Value(c9))
                If ((posi_y < 1.2) Or (posi_y1 < 1.2)) Then
                    Exit Sub
                End If

                Dim contatexto As Integer
                For contatexto = 1 To 9
                    pTextElement = New TextElement
                    pEnv = New Envelope
                    pPoint = New Point
                    Select Case contatexto
                        'Guardar el texto
                        Case 1
                            pTextElement.Text = v_campo1
                            pPoint.X = 17.2
                            pPoint.Y = posi_y1 - 0.6
                            pPoint.X = 18.2
                            pPoint.Y = posi_y
                        Case 2
                            pTextElement.Text = v_campo2
                            pPoint.X = 20.5
                            pPoint.Y = posi_y1 - 0.6
                            pPoint.X = 18.7
                            pPoint.Y = posi_y
                        Case 3
                            pTextElement.Text = v_campo3
                            pPoint.X = 22.9
                            pPoint.Y = posi_y1 - 0.6
                            pPoint.X = 21.9
                            pPoint.Y = posi_y
                        Case 4
                            pTextElement.Text = v_campo4
                            pPoint.X = 24.3
                            pPoint.Y = posi_y1 - 0.6
                            pPoint.X = 23.5
                            pPoint.Y = posi_y

                        Case 5
                            pTextElement.Text = v_campo5
                            pPoint.X = 25.0
                            pPoint.Y = posi_y1 - 0.6
                            pPoint.X = 24.2
                            pPoint.Y = posi_y

                        Case 6
                            pTextElement.Text = v_campo6
                            pPoint.X = 25.8
                            pPoint.Y = posi_y1 - 0.6
                            pPoint.X = 25.0
                            pPoint.Y = posi_y

                        Case 7
                            pTextElement.Text = v_campo7
                            pPoint.X = 26.6
                            pPoint.Y = posi_y1 - 0.6
                            pPoint.X = 25.8
                            pPoint.Y = posi_y

                        Case 8
                            pTextElement.Text = v_campo8
                            pPoint.X = 27.4
                            pPoint.Y = posi_y1 - 0.6
                            pPoint.X = 26.6
                            pPoint.Y = posi_y

                        Case 9
                            pTextElement.Text = v_campo9
                            pPoint.X = 28.2
                            pPoint.Y = posi_y1 - 0.6
                            pPoint.X = 27.4
                            pPoint.Y = posi_y
                            'End If
                    End Select

                    pEnv.UpperRight = pPoint
                    pElement = pTextElement
                    pElement.Geometry = pEnv
                    pTextElement.Symbol = pTxtSym1
                    pGraphicsContainer.AddElement(pTextElement, 1)

                Next contatexto

                'pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
                posi_y1 = posi_y1 - 0.3
                posi_y = posi_y - 0.3
                pfeature_1 = pFeatureCursor_1.NextFeature

            Loop
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        End If
    End Sub


    Public Sub GENERALISTADODM_PLANOEV(ByVal p_app As IApplication)
        If v_posi_pr = False And v_posi_po = False And v_posi_si = False And v_posi_ex = False Then
            Dim pFeatureCursor_1 As IFeatureCursor
            Dim pfeature_1 As IFeature

            Dim pFeatLayer As IFeatureLayer = Nothing
            Dim fclas_tema As IFeatureClass
            Dim pMxApp As IMxApplication
            ' Dim pFeature_1 As IFeature

            pMxApp = p_App
            Dim c1 As String
            Dim c2 As String
            Dim c3 As String
            Dim c4 As String
            Dim c5 As String
            Dim c6 As String
            Dim c7 As String
            Dim c8 As String
            Dim c9 As String
            pMap = pMxDoc.FocusMap

            Dim afound As Boolean = False
            For A As Integer = 0 To pMap.LayerCount - 1
                If pMap.Layer(A).Name = "Catastro" Then
                    pFeatLayer = pMap.Layer(A)
                    'pFeatureLayer = pFeatLayer
                    afound = True
                    Exit For
                End If
            Next A
            If Not afound Then
                MsgBox("No se encuentra la capa Catastro en la vista para generar listado en plano", MsgBoxStyle.Critical, "Observacion")
                Exit Sub
            End If

            ' pFeatLayer = pFeatureLayer
            fclas_tema = pFeatLayer.FeatureClass
            'fclas_tema = pFeatureLayer.FeatureClass
            pFields = fclas_tema.Fields
            c1 = pFields.FindField("CONTADOR")
            c2 = pFields.FindField("CONCESION")
            c3 = pFields.FindField("CODIGOU")
            c4 = pFields.FindField("ZONA")
            c5 = pFields.FindField("TIPO_EX")
            c6 = pFields.FindField("ESTADO")
            c7 = pFields.FindField("DE_PUBL")
            c8 = pFields.FindField("DE_IDEN")
            c9 = pFields.FindField("NATURALEZA")
            Dim v_campo1 As String
            Dim v_campo2 As String
            Dim v_campo3 As String
            Dim v_campo4 As String
            Dim v_campo5 As String
            Dim v_campo6 As String
            Dim v_campo7 As String
            Dim v_campo8 As String
            Dim v_campo9 As String
            pFeatureCursor_1 = pFeatLayer.Search(Nothing, False)
            'pFeatureCursor = pFeatureLayer.Search(Nothing, False)

            'Relacionando al layout
            Dim pPageLayout As IPageLayout
            Dim pActiveView As IActiveView
            Dim pdisplay As IScreenDisplay

            ' Dim pGraphicsContainer As IGraphicsContainer
            Dim pElement As IElement
            Dim pTextElement As ITextElement
            Dim pGraphicsContainer1 As IGraphicsContainer
            ' Dim pTextSymbol_1 As ITextSymbol
            Dim pTextSymbol_1 As IFormattedTextSymbol

            'Dim contatexto As Integer
            Dim myColor As IRgbColor


            'Define layout

            pPageLayout = pMxDoc.PageLayout
            pPageLayout.Page.Units = esriUnits.esriCentimeters
            'pActiveView = pPageLayout
            ' pdisplay = pMxApp.Display

            pGraphicsContainer1 = pPageLayout
            pGraphicsContainer1.Reset()

            'Para obtener fecha
            Dim pEnv As IEnvelope
            Dim pFont As IFontDisp
            'pFeature = pFeatureCursor.NextFeature
            Dim posi_y As Double
            Dim posi_y1 As Double
            posi_y1 = posi_y1_list
            posi_y = posi_y2_list

            'posi_y = FormatNumber(posi_y2_list, 2)
            'posi_y1 = FormatNumber(posi_y2_list, 2)

            '  fclas_tema = pFeatLayer.FeatureClass
            pTextSymbol_1 = New TextSymbol
            pFont = New StdFont
            pFont.Name = "Courier New"

            'pFont.Size = 4.5
            pFont.Size = 6.0      'cambio 2015
            pFont.Bold = False
            pTxtSym1.Font = pFont
            myColor = New RgbColor
            myColor.RGB = RGB(0, 0, 0)
            ' pTxtSym1.Color = myColor
            pTextSymbol_1.Color = myColor

            pTextSymbol_1.Angle = 0
            pTextSymbol_1.RightToLeft = False
            ' pTextSymbol_1.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline

            pTextSymbol_1.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull

            pTextSymbol_1.CharacterSpacing = 20


            '  pTextSymbol_1.CharacterSpacing = 20
            '  pTextSymbol_1.c
            pTxtSym1.Case = esriTextCase.esriTCNormal

            Dim cadenatexto As String
            Dim cadenatexto_1 As String
            Dim cadenatexto_t As String
            Dim cadenatexto2 As String
            Dim cadenatexto3 As String
            Dim cadenatexto4 As String
            Dim cuenta_cad4 As Integer

            Dim conta1 As Integer = 0
            pfeature_1 = pFeatureCursor_1.NextFeature


            Do Until pfeature_1 Is Nothing
                conta1 = conta1 + 1
                'MsgBox(conta1)
                v_campo1 = pfeature_1.Value(c1)
                v_campo2 = Trim(pfeature_1.Value(c2))
                v_campo3 = Trim(pfeature_1.Value(c3))

                'If v_campo3 = "010591408A" Then
                'MsgBox(v_campo3)
                'End If

                If v_campo3 = v_codigo Then  'Si es DM evaluado                
                    myColor = New RgbColor
                    myColor.RGB = RGB(169, 0, 230)
                    pTxtSym1.Color = myColor
                Else
                    myColor = New RgbColor
                    myColor.RGB = RGB(0, 0, 0)
                    pTxtSym1.Color = myColor
                End If

                v_campo4 = Trim(pfeature_1.Value(c4))
                v_campo5 = Trim(pfeature_1.Value(c5))
                v_campo6 = Trim(pfeature_1.Value(c6))
                v_campo7 = Trim(pfeature_1.Value(c7))
                If v_campo7 = "" Then
                    v_campo7 = "NP"
                End If
                v_campo8 = Trim(pfeature_1.Value(c8))
                If v_campo8 = "" Then
                    v_campo8 = "NI"
                End If
                v_campo9 = Trim(pfeature_1.Value(c9))
                If ((posi_y < 1.2) Or (posi_y1 < 1.2)) Then
                    Exit Sub
                End If

                If Len(v_campo3) = 13 Then
                    cadenatexto3 = v_campo3 & "   "
                    'cadenatexto3 = (v_campo3.PadRight(3, " "))
                ElseIf Len(v_campo3) = 12 Then
                    cadenatexto3 = v_campo3 & "    "
                    'cadenatexto3 = (v_campo3.PadRight(4, " "))
                ElseIf Len(v_campo3) = 11 Then
                    'cadenatexto3 = v_campo3 & "     "
                    cadenatexto3 = v_campo3 & " "
                    'cadenatexto3 = (v_campo3.PadRight(5, " "))
                ElseIf Len(v_campo3) = 10 Then
                    'cadenatexto3 = v_campo3 & "      "

                    cadenatexto3 = v_campo3 & "  "
                    'cadenatexto3 = (v_campo3.PadRight(6, " "))
                ElseIf Len(v_campo3) = 9 Then
                    'cadenatexto3 = v_campo3 & "       "
                    cadenatexto3 = v_campo3 & "   "
                    'cadenatexto3 = (v_campo3.PadRight(7, " "))
                ElseIf Len(v_campo3) = 8 Then
                    cadenatexto3 = v_campo3 & "        "
                    'cadenatexto3 = (v_campo3.PadRight(8, " "))
                End If
                'para controlar ancho del 1er caracter...







                If Len(v_campo1) = 1 Then
                    cadenatexto2 = (v_campo2.PadRight(25, " "))

                    'If Len(v_campo3) = 13 Then
                    '    cadenatexto3 = v_campo3 & "   "
                    '    'cadenatexto3 = (v_campo3.PadRight(3, " "))
                    'ElseIf Len(v_campo3) = 12 Then
                    '    cadenatexto3 = v_campo3 & "    "
                    '    'cadenatexto3 = (v_campo3.PadRight(4, " "))
                    'ElseIf Len(v_campo3) = 11 Then
                    '    cadenatexto3 = v_campo3 & "     "
                    '    'cadenatexto3 = (v_campo3.PadRight(5, " "))
                    'ElseIf Len(v_campo3) = 10 Then
                    '    cadenatexto3 = v_campo3 & "      "
                    '    'cadenatexto3 = (v_campo3.PadRight(6, " "))
                    'ElseIf Len(v_campo3) = 9 Then
                    '    cadenatexto3 = v_campo3 & "       "
                    '    'cadenatexto3 = (v_campo3.PadRight(7, " "))
                    'ElseIf Len(v_campo3) = 8 Then
                    '    cadenatexto3 = v_campo3 & "        "
                    '    'cadenatexto3 = (v_campo3.PadRight(8, " "))
                    'End If

                    If Len(v_campo8) = 2 Then
                        cadenatexto4 = v_campo8 & "  "

                    ElseIf Len(v_campo8) = 1 Then
                        'cadenatexto4 = v_campo8 & "    "
                        cadenatexto4 = v_campo8 & "   "
                    End If

                    'cadenatexto = v_campo1 & "    " & cadenatexto2 & "   " & cadenatexto3 & v_campo4 & "   " & v_campo5 & "     " & v_campo6 & "     " & v_campo7 & "      " & cadenatexto4 & "   " & v_campo9
                    cadenatexto = v_campo1 & "    " & cadenatexto2 & " " & cadenatexto3 & "  " & v_campo5 & "   " & v_campo6 & "   " & cadenatexto4 & "  " & v_campo9






                ElseIf Len(v_campo1) = 2 Then
                    cadenatexto2 = (v_campo2.PadRight(25, " "))
                    If Len(v_campo8) = 2 Then
                        cadenatexto4 = v_campo8 & "  "
                    ElseIf Len(v_campo8) = 1 Then
                        cadenatexto4 = v_campo8 & "   "
                    End If
                    'cadenatexto = v_campo1 & "   " & cadenatexto2 & "   " & cadenatexto3 & v_campo4 & "   " & v_campo5 & "     " & v_campo6 & "     " & v_campo7 & "      " & cadenatexto4 & "   " & v_campo9
                    cadenatexto = v_campo1 & "   " & cadenatexto2 & " " & cadenatexto3 & "  " & v_campo5 & "   " & v_campo6 & "   " & cadenatexto4 & "  " & v_campo9

                ElseIf Len(v_campo1) = 3 Then
                    cadenatexto2 = (v_campo2.PadRight(25, " "))
                    If Len(v_campo8) = 2 Then
                        cadenatexto4 = v_campo8 & "   "
                    ElseIf Len(v_campo8) = 1 Then
                        cadenatexto4 = v_campo8 & "    "
                    End If
                    cadenatexto = v_campo1 & "  " & cadenatexto2 & "   " & cadenatexto3 & v_campo4 & "   " & v_campo5 & "     " & v_campo6 & "     " & v_campo7 & "      " & cadenatexto4 & "   " & v_campo9
                End If

                'Dim contatexto As Integer
                'For contatexto = 1 To 100  'antiguamente, se tuvo de quitar para agilizar plano

                pTextElement = New TextElement
                pEnv = New Envelope
                pPoint = New Point
                'pTextElement.Text = contatexto
                pTextElement.Text = cadenatexto
                pPoint.X = 17.2
                pPoint.Y = posi_y1 - 0.6
                pPoint.X = 18.2
                pPoint.Y = posi_y

                'Select Case contatexto
                '    'Guardar el texto
                '    Case 1
                '        pTextElement.Text = v_campo1
                '        'pTextElement.Symbol = pTextSymbol_1
                '        pPoint.X = 17.2
                '        pPoint.Y = posi_y1 - 0.6
                '        pPoint.X = 18.2
                '        pPoint.Y = posi_y
                '    Case 2
                '        pTextElement.Text = v_campo2
                '        'pTextElement.Symbol = pTextSymbol_1
                '        pPoint.X = 20.5
                '        pPoint.Y = posi_y1 - 0.6
                '        pPoint.X = 18.7
                '        pPoint.Y = posi_y
                '    Case 3
                '        pTextElement.Text = v_campo3
                '        'pTextElement.Symbol = pTextSymbol_1
                '        pPoint.X = 22.9
                '        pPoint.Y = posi_y1 - 0.6
                '        pPoint.X = 21.9
                '        pPoint.Y = posi_y
                '    Case 4
                '        pTextElement.Text = v_campo4
                '        'pTextElement.Symbol = pTextSymbol_1
                '        pPoint.X = 24.3
                '        pPoint.Y = posi_y1 - 0.6
                '        pPoint.X = 23.5
                '        pPoint.Y = posi_y

                '    Case 5
                '        pTextElement.Text = v_campo5
                '        'pTextElement.Symbol = pTextSymbol_1
                '        pPoint.X = 25.0
                '        pPoint.Y = posi_y1 - 0.6
                '        pPoint.X = 24.2
                '        pPoint.Y = posi_y

                '    Case 6
                '        pTextElement.Text = v_campo6
                '        'pTextElement.Symbol = pTextSymbol_1
                '        pPoint.X = 25.8
                '        pPoint.Y = posi_y1 - 0.6
                '        pPoint.X = 25.0
                '        pPoint.Y = posi_y

                '    Case 7
                '        pTextElement.Text = v_campo7
                '        ' pTextElement.Symbol = pTextSymbol_1
                '        pPoint.X = 26.6
                '        pPoint.Y = posi_y1 - 0.6
                '        pPoint.X = 25.8
                '        pPoint.Y = posi_y

                '    Case 8
                '        pTextElement.Text = v_campo8
                '        ' pTextElement.Symbol = pTextSymbol_1
                '        pPoint.X = 27.4
                '        pPoint.Y = posi_y1 - 0.6
                '        pPoint.X = 26.6
                '        pPoint.Y = posi_y

                '    Case 9
                '        pTextElement.Text = v_campo9
                '        ' pTextElement.Symbol = pTextSymbol_1
                '        pPoint.X = 28.2
                '        pPoint.Y = posi_y1 - 0.6
                '        pPoint.X = 27.4
                '        pPoint.Y = posi_y
                '        'End If
                'End Select
                'Aqwi termino el comentado antiguo de lineas arriba del programa
                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                pTextElement.Symbol = pTxtSym1
                'pGraphicsContainer.AddElement(pTextElement, 1)
                pGraphicsContainer1.AddElement(pElement, 1)

                pMxDoc.ActiveView.Refresh()
                posi_y1 = posi_y1 - 0.3
                posi_y = posi_y - 0.3

                ' Next contatexto

                'pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
                'posi_y1 = posi_y1 - 0.3
                ' posi_y = posi_y - 0.3
                ' Exit Sub

                pfeature_1 = pFeatureCursor_1.NextFeature

            Loop
            '   pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        End If
    End Sub
    Public Sub Genera_ObservacionesCarta_original()
        Dim cls_Oracle As New cls_Oracle
        Dim pPageLayout As IPageLayout
        Dim pActiveView As IActiveView
        Dim pGraphicsContainer As IGraphicsContainer
        Dim pElement As IElement
        Dim pTextElement As ITextElement
        Dim pPoint As IPoint
        Dim pTxtSym As IFormattedTextSymbol
        Dim contatexto As Integer
        pPageLayout = pMxDoc.PageLayout
        pPageLayout.Page.Units = esriUnits.esriCentimeters
        pActiveView = pPageLayout
        pGraphicsContainer = pPageLayout
        Dim conta_ob As Integer
        Dim lostr_descri As String = ""
        For conta_ob = 1 To 20
            If conta_ob = 1 Then
                If v_checkbox1 <> "" Then
                    colecciones_obs.Add("GT")
                End If
            ElseIf conta_ob = 2 Then
                If v_checkbox2 <> "" Then
                    colecciones_obs.Add("GP")
                End If
            ElseIf conta_ob = 3 Then
                If v_checkbox3 <> "" Then
                    colecciones_obs.Add("MT")
                End If
            ElseIf conta_ob = 4 Then
                If v_checkbox4 <> "" Then
                    colecciones_obs.Add("MP")
                End If
            ElseIf conta_ob = 5 Then
                If v_checkbox5 <> "" Then
                    colecciones_obs.Add("CS")
                End If
            ElseIf conta_ob = 6 Then
                If v_checkbox6 <> "" Then
                    colecciones_obs.Add("CF")
                End If
            ElseIf conta_ob = 7 Then
                If v_checkbox7 <> "" Then
                    colecciones_obs.Add("BT")
                End If
            ElseIf conta_ob = 8 Then
                If v_checkbox8 <> "" Then
                    colecciones_obs.Add("BP")
                End If
            ElseIf conta_ob = 9 Then
                If v_checkbox9 <> "" Then
                    colecciones_obs.Add("RT")
                End If
            ElseIf conta_ob = 10 Then
                If v_checkbox10 <> "" Then
                    colecciones_obs.Add("RP")
                End If
            ElseIf conta_ob = 11 Then
                If v_checkbox11 <> "" Then
                    colecciones_obs.Add("RH")
                End If

            ElseIf conta_ob = 12 Then
                If v_checkbox12 <> "" Then
                    colecciones_obs.Add("CL")
                End If
            ElseIf conta_ob = 13 Then
                If v_checkbox13 <> "" Then
                    colecciones_obs.Add("LG")
                End If
            ElseIf conta_ob = 14 Then
                If v_checkbox14 <> "" Then
                    colecciones_obs.Add("RS")
                End If
            ElseIf conta_ob = 15 Then
                If v_checkbox15 <> "" Then
                    colecciones_obs.Add("FE")
                End If
            ElseIf conta_ob = 16 Then
                If v_checkbox16 <> "" Then
                    colecciones_obs.Add("TE")
                End If
            ElseIf conta_ob = 17 Then
                If v_checkbox17 <> "" Then
                    colecciones_obs.Add("RQ")
                End If
            ElseIf conta_ob = 18 Then
                If v_checkbox18 <> "" Then
                    colecciones_obs.Add("TL")
                End If
            ElseIf conta_ob = 19 Then
                If v_checkbox19 <> "" Then
                    colecciones_obs.Add("FR")
                End If
            ElseIf conta_ob = 20 Then
                If v_checkbox20 <> "" Then
                    colecciones_obs.Add("AU")
                End If
            End If
        Next conta_ob

        '*****************************************
        Dim lostrEstado_Observacion As String
        Dim lostrMensaje As String = ""
        Dim lostrEstado As String = ""
        'lostrEstado_Observacion = cls_Oracle.FT_Ver_Estado_Doc(IIf(txhSolicitante = "I", "SO", ""), txhNumero)
        lostrEstado_Observacion = cls_Oracle.FT_Estado_Observacion(v_codigo)
        Select Case lostrEstado_Observacion
            Case "0"
                lostrMensaje = " � Desea Grabar la Informaci�n ? "
                lostrEstado = "INSERTA"
            Case "1"
                lostrMensaje = " � Informaci�n Existes, desesa Actualizar ? "
                lostrEstado = "UPDATE"
        End Select
        If MsgBox(lostrMensaje, MsgBoxStyle.YesNo, "BDGeocatmin") = vbNo Then
            Exit Sub
        End If

        Dim lostrObervacion As String = ""
        For i As Integer = 1 To colecciones_obs.Count
            If colecciones_obs.Item(i) = "GT" Then
                'lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & "Area Urbana de: " & v_checkbox20 & " || "
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox1 & " || "
            ElseIf colecciones_obs.Item(i) = "GP" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox2 & " || "
            ElseIf colecciones_obs.Item(i) = "MT" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox3 & " || "
            ElseIf colecciones_obs.Item(i) = "MP" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox4 & " || "
            ElseIf colecciones_obs.Item(i) = "CS" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox5 & " || "
            ElseIf colecciones_obs.Item(i) = "CF" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox6 & " || "
            ElseIf colecciones_obs.Item(i) = "BT" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox7 & " || "
            ElseIf colecciones_obs.Item(i) = "BP" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox8 & " || "
            ElseIf colecciones_obs.Item(i) = "RT" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox9 & " || "
            ElseIf colecciones_obs.Item(i) = "RP" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox10 & " || "
            ElseIf colecciones_obs.Item(i) = "RH" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox11 & " || "
                'ElseIf colecciones_obs.Item(i) = "HD" Then
                '    lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox11 & " || "
                '    lostr_descri = v_checkbox11

            ElseIf colecciones_obs.Item(i) = "CL" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox12 & " || "

            ElseIf colecciones_obs.Item(i) = "LG" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox13 & " || "
                ' lostr_descri = v_checkbox13
            ElseIf colecciones_obs.Item(i) = "RS" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox14 & " || "
            ElseIf colecciones_obs.Item(i) = "FE" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox15 & " || "
            ElseIf colecciones_obs.Item(i) = "TE" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox16 & " || "
            ElseIf colecciones_obs.Item(i) = "RQ" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox17 & " || "
            ElseIf colecciones_obs.Item(i) = "TL" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox18 & " || "
            ElseIf colecciones_obs.Item(i) = "FR" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox19 & " || "
            ElseIf colecciones_obs.Item(i) = "AU" Then
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox20 & " || "
                'lostr_descri = v_checkbox20
            Else
                lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & " || "
            End If
        Next i

        Dim lostrRetorno As String = cls_Oracle.FT_Man_Observacion_CartaDM(v_codigo, v_codigo, lostrObervacion, _
            gstrCodigo_Usuario, lostr_descri, gstrCodigo_Usuario, lostrEstado)




        If Val(lostrRetorno) > 0 Then
            MsgBox("La operaci�n se realiz� exitosamente. ", MsgBoxStyle.Information, "[BDGeocatmin]")
        End If


        '*****************************************
        'Borra textbox del layout en item observaciones si encuentra

        Dim pContainer As IGraphicsContainer
        Dim pElement1 As IElement
        Dim pTextElement1 As ITextElement
        pContainer = pPageLayout
        pContainer.Reset()
        pElement1 = pContainer.Next
        While Not pElement1 Is Nothing
            If TypeOf pElement1 Is ITextElement Then
                pTextElement1 = pElement1
                If pTextElement1.Text = "Reservorio" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Zona Agricola Parcial" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Zona Agricola Total" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Dominio Maritimo Parcial" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Dominio Maritimo Total" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Carretera Asfaltada" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Carretera Afirmada" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Zona de Bosque Parcial" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Zona de Bosque Total" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Con Recubrimiento Parcial" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Sin Recubrimiento Total" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "L�nea de Alta Tensi�n El�ctrica" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Rio(s)" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Canal" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Laguna(s)" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "L�nea Ferrea" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "L�nea Frontera" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Zona de Traslape" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "Restos Arqueologicos" Then
                    pContainer.DeleteElement(pTextElement1)
                ElseIf pTextElement1.Text = "No existen Observaciones" Then
                    pContainer.DeleteElement(pTextElement1)
                End If
            End If
            pElement1 = pContainer.Next
        End While
        pMxDoc.UpdateContents()
        pMxDoc.ActiveView.Refresh()
        'Termino la parte de borrar textos
        Dim POSI_y1 As Double
        Dim POSI_y2 As Double
        Dim POSI_1x As Double
        Dim POSI_2x As Double
        Dim POSI_x1 As Double
        Dim POSI_x2 As Double
        POSI_y2 = posi_y_m
        POSI_y1 = posi_y1_m
        posi_y_m = POSI_y2
        posi_y1_m = POSI_y1
        POSI_x1 = 13.2
        POSI_x2 = 19.2
        For contatexto = 1 To 20
            pTextElement = New TextElement
            Dim pEnv As IEnvelope
            pEnv = New Envelope
            pPoint = New Point
            If POSI_y1 < 6.6 Or POSI_y2 < 6.6 Then
                POSI_x1 = POSI_x1 + 3.2
                POSI_x2 = POSI_x2 + 3.2
                POSI_y2 = posi_y_m
                POSI_y1 = posi_y1_m
            End If

            'End If
            If contatexto = 1 Then
                If v_checkbox1 <> "" Then
                    pTextElement.Text = v_checkbox1
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.1
                    'pPoint.Y = 8.5
                    pPoint.X = 21.7
                    'pPoint.Y = 9.3
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If

            ElseIf contatexto = 2 Then
                If v_checkbox2 <> "" Then
                    pTextElement.Text = v_checkbox2
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 3 Then
                If v_checkbox3 <> "" Then
                    pTextElement.Text = v_checkbox3
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If

            ElseIf contatexto = 4 Then
                If v_checkbox4 <> "" Then
                    pTextElement.Text = v_checkbox4
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 5 Then
                If v_checkbox5 <> "" Then
                    pTextElement.Text = v_checkbox5
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 6 Then
                If v_checkbox6 <> "" Then
                    pTextElement.Text = v_checkbox6
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If

            ElseIf contatexto = 7 Then
                If v_checkbox7 <> "" Then
                    pTextElement.Text = v_checkbox7
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If

            ElseIf contatexto = 8 Then
                If v_checkbox8 <> "" Then
                    pTextElement.Text = v_checkbox8
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3

                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If

            ElseIf contatexto = 9 Then
                If v_checkbox9 <> "" Then
                    pTextElement.Text = v_checkbox9
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If

            ElseIf contatexto = 10 Then
                If v_checkbox10 <> "" Then
                    pTextElement.Text = v_checkbox10
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 11 Then
                If v_checkbox11 <> "" Then
                    pTextElement.Text = "Rio de " & v_checkbox11
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    'pEnv.LowerLeft = pPoint
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If

            ElseIf contatexto = 12 Then
                If v_checkbox12 <> "" Then
                    pTextElement.Text = v_checkbox12
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 13 Then
                If v_checkbox13 <> "" Then
                    pTextElement.Text = "Laguna de " & v_checkbox13
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 14 Then
                If v_checkbox14 <> "" Then
                    pTextElement.Text = v_checkbox14
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 15 Then
                If v_checkbox15 <> "" Then
                    pTextElement.Text = v_checkbox15
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 16 Then
                If v_checkbox16 <> "" Then
                    pTextElement.Text = v_checkbox16
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 17 Then
                If v_checkbox17 <> "" Then
                    pTextElement.Text = v_checkbox17
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 18 Then
                If v_checkbox18 <> "" Then
                    pTextElement.Text = v_checkbox18
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 19 Then
                If v_checkbox19 <> "" Then
                    pTextElement.Text = v_checkbox19
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            ElseIf contatexto = 20 Then
                If v_checkbox20 <> "" Then
                    '  MsgBox(v_checkbox20)
                    pTextElement.Text = "Area Urbana de " & v_checkbox20
                    pPoint.X = POSI_x1
                    pPoint.Y = POSI_y2 - 0.3
                    pPoint.X = POSI_x2
                    pPoint.Y = POSI_y1 - 0.3
                    POSI_y1 = POSI_y1 - 0.3
                    POSI_y2 = POSI_y2 - 0.3
                Else
                    pTextElement.Text = ""
                    pPoint.X = 17.9
                    pPoint.Y = POSI_y2 + 0.6
                    pPoint.X = 21.7
                    pPoint.Y = POSI_y1 + 0.6
                    POSI_1x = POSI_y1
                    POSI_2x = POSI_y2
                End If
            End If

            pEnv.UpperRight = pPoint
            pElement = pTextElement
            pElement.Geometry = pEnv
            'Simbolo del texto

            pTxtSym = New TextSymbol
            'fuente del texto

            Dim pFont As IFontDisp
            pFont = New StdFont
            pFont.Name = "Tahoma"
            pFont.Size = 6
            pFont.Bold = False
            pTxtSym.Font = pFont
            'Propiedades del Simbolo


            pTxtSym.Angle = 0
            pTxtSym.RightToLeft = False
            pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
            pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
            pTxtSym.CharacterSpacing = 20
            pTxtSym.Case = esriTextCase.esriTCNormal
            pTextElement.Symbol = pTxtSym
            pGraphicsContainer.AddElement(pTextElement, 1)

            'Refrescar solo los graficos del PageLayout
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        Next contatexto
    End Sub

    Public Sub Genera_ObservacionesCarta()
        Try
            Dim cls_Oracle As New cls_Oracle
            Dim pPageLayout As IPageLayout
            Dim pActiveView As IActiveView
            Dim pGraphicsContainer As IGraphicsContainer
            Dim pElement As IElement
            Dim pTextElement As ITextElement
            Dim pPoint As IPoint
            Dim pTxtSym As IFormattedTextSymbol

            Dim contatexto As Integer
            pPageLayout = pMxDoc.PageLayout
            pPageLayout.Page.Units = esriUnits.esriCentimeters
            pActiveView = pPageLayout
            pGraphicsContainer = pPageLayout
            Dim conta_ob As Integer

            For conta_ob = 1 To 20
                If conta_ob = 1 Then
                    If v_checkbox1 <> "" Then
                        colecciones_obs.Add("GT")
                    End If
                ElseIf conta_ob = 2 Then
                    If v_checkbox2 <> "" Then
                        colecciones_obs.Add("GP")
                    End If
                ElseIf conta_ob = 3 Then
                    If v_checkbox3 <> "" Then
                        colecciones_obs.Add("MT")
                    End If
                ElseIf conta_ob = 4 Then
                    If v_checkbox4 <> "" Then
                        colecciones_obs.Add("MP")
                    End If
                ElseIf conta_ob = 5 Then
                    If v_checkbox5 <> "" Then
                        colecciones_obs.Add("CS")
                    End If
                ElseIf conta_ob = 6 Then
                    If v_checkbox6 <> "" Then
                        colecciones_obs.Add("CF")
                    End If
                ElseIf conta_ob = 7 Then
                    If v_checkbox7 <> "" Then
                        colecciones_obs.Add("BT")
                    End If
                ElseIf conta_ob = 8 Then
                    If v_checkbox8 <> "" Then
                        colecciones_obs.Add("BP")
                    End If
                ElseIf conta_ob = 9 Then
                    If v_checkbox9 <> "" Then
                        colecciones_obs.Add("RT")
                    End If
                ElseIf conta_ob = 10 Then
                    If v_checkbox10 <> "" Then
                        colecciones_obs.Add("RP")
                    End If
                ElseIf conta_ob = 11 Then
                    If v_checkbox11 <> "" Then
                        '  colecciones_obs.Add("HD")
                        colecciones_obs.Add("RH")
                    End If

                ElseIf conta_ob = 12 Then
                    If v_checkbox12 <> "" Then
                        colecciones_obs.Add("CL")
                    End If
                ElseIf conta_ob = 13 Then
                    If v_checkbox13 <> "" Then
                        colecciones_obs.Add("LG")
                    End If
                ElseIf conta_ob = 14 Then
                    If v_checkbox14 <> "" Then
                        colecciones_obs.Add("RS")
                    End If
                ElseIf conta_ob = 15 Then
                    If v_checkbox15 <> "" Then
                        colecciones_obs.Add("FE")
                    End If
                ElseIf conta_ob = 16 Then
                    If v_checkbox16 <> "" Then
                        colecciones_obs.Add("TE")
                    End If
                ElseIf conta_ob = 17 Then
                    If v_checkbox17 <> "" Then
                        colecciones_obs.Add("RQ")
                    End If
                ElseIf conta_ob = 18 Then
                    If v_checkbox18 <> "" Then
                        colecciones_obs.Add("TL")
                    End If
                ElseIf conta_ob = 19 Then
                    If v_checkbox19 <> "" Then
                        colecciones_obs.Add("FR")
                    End If
                ElseIf conta_ob = 20 Then
                    If v_checkbox20 <> "" Then
                        ' colecciones_obs.Add("PU")
                        colecciones_obs.Add("AU")
                    End If
                End If
            Next conta_ob



            '*****************************************
            'Adicionando de colecciones observaciones
            Dim v_formart As String = "1"

            For i As Integer = 1 To colecciones_obs_update.Count
                colecciones_obs.Add(colecciones_obs_update.Item(i))
            Next


            'termino

            Dim lostrEstado_Observacion As String
            Dim lostrMensaje As String = ""
            Dim lostrEstado As String = ""
            Dim lostr_obseva As String = ""
            Dim lostr_descri As String = ""
            Dim lostrOpcion1 As String = ""

            'lostrEstado_Observacion = cls_Oracle.FT_Ver_Estado_Doc(IIf(txhSolicitante = "I", "SO", ""), txhNumero)
            lostrEstado_Observacion = cls_Oracle.FT_Estado_Observacion(v_codigo)


            Select Case lostrEstado_Observacion
                Case "0"
                    lostrMensaje = " � Desea Grabar la Informaci�n ? "
                    lostrEstado = "INSERTA"
                    lostr_obseva = "OB"
                    If MsgBox(lostrMensaje, MsgBoxStyle.YesNo, "BDGeocatmin") = vbNo Then
                        Exit Sub
                    End If
                Case "1"
                    lostrMensaje = " � Informaci�n Existe, desea Actualizar ? "
                    'lostrOpcion1 = MsgBox("� Ya existen Datos de Observacion Guardados anteriormente para el Informe T�cnico, Haga Click en .. ?" & vbNewLine & "SI:  Para Generar Historico de la informaci�n y Reemplazar los datos existentes" & vbNewLine & "NO:  Para Guardar informacion que reemplazara los datos existentes sin Generar Informaci�n Historica", MsgBoxStyle.YesNoCancel, "Evaluacion de DM...")
                    lostrEstado = "UPDATE"
                    lostr_obseva = "OB"

                    'Select Case lostrOpcion1
                    'Case "2" 'Cancel
                    '   Exit Sub  'No hace nada y sale del formulario evaluador
                    'Case "6" 'Si, Generar Hist�rico y Graba la Informaci�n reemplazandola.
                    '   cls_Oracle.FT_GENERA_HISTORICO(v_codigo, v_formart)

                    ' cls_Oracle.FT_SG_D_EVALGIS("INS", v_codigo, v_formart, gstrCodigo_Usuario)

                    'Case "7" 'No Genera Histotico pero si Graba la informacion reemplanzandola

                    'End Select
                    If MsgBox(lostrMensaje, MsgBoxStyle.YesNo, "BDGeocatmin") = vbNo Then
                        Exit Sub
                    End If

            End Select


            'If MsgBox(lostrMensaje, MsgBoxStyle.YesNo, "BDGeocatmin") = vbNo Then
            '    Exit Sub
            'End If

            Dim lostrObervacion As String = ""


            'Primero borrar lo que el usuario deselecciono de las observaciones de carta}

            Dim valor_obs_b As String
            'recorre lista de los indicadores a eliminar

            For k As Integer = 1 To colecciones_obs_upd_borrar.Count
                valor_obs_b = colecciones_obs_upd_borrar.Item(k)
                cls_Oracle.FT_Man_Observacion_CartaDM(v_codigo, v_codigo, valor_obs_b, _
                 lostr_obseva, lostr_descri, gstrCodigo_Usuario, "DELETE")
            Next k


            Dim lostrRetorno As String

            For i As Integer = 1 To colecciones_obs.Count
                lostr_descri = ""
                If colecciones_obs.Item(i) = "GT" Then

                    lostrObervacion = "GT"
                ElseIf colecciones_obs.Item(i) = "GP" Then
                    lostrObervacion = "GP"
                ElseIf colecciones_obs.Item(i) = "MT" Then
                    lostrObervacion = "MT"
                ElseIf colecciones_obs.Item(i) = "MP" Then
                    lostrObervacion = "MP"
                ElseIf colecciones_obs.Item(i) = "CS" Then
                    lostrObervacion = "CS"
                ElseIf colecciones_obs.Item(i) = "CF" Then
                    lostrObervacion = "CF"
                ElseIf colecciones_obs.Item(i) = "BT" Then
                    lostrObervacion = "BT"
                ElseIf colecciones_obs.Item(i) = "BP" Then
                    lostrObervacion = "BP"
                ElseIf colecciones_obs.Item(i) = "RT" Then
                    lostrObervacion = "RT"
                ElseIf colecciones_obs.Item(i) = "RP" Then
                    lostrObervacion = "RP"
                    'ElseIf colecciones_obs.Item(i) = "HD" Then
                    'lostrObervacion = "HD"
                    '    lostr_descri = v_checkbox11
                ElseIf colecciones_obs.Item(i) = "RH" Then
                    lostrObervacion = "RH"
                    ' lostr_descri = v_checkbox11
                    'lostr_descri = lostrObervacion & ":" & v_checkbox11
                    lostr_descri = v_checkbox11
                ElseIf colecciones_obs.Item(i) = "CL" Then
                    lostrObervacion = "CL"
                ElseIf colecciones_obs.Item(i) = "LG" Then
                    lostrObervacion = "LG"
                    'lostr_descri = lostrObervacion & ":" & v_checkbox13
                    lostr_descri = v_checkbox13
                ElseIf colecciones_obs.Item(i) = "RS" Then
                    lostrObervacion = "RS"
                ElseIf colecciones_obs.Item(i) = "FE" Then
                    lostrObervacion = "FE"
                ElseIf colecciones_obs.Item(i) = "TE" Then
                    lostrObervacion = "TE"
                ElseIf colecciones_obs.Item(i) = "RQ" Then
                    lostrObervacion = "RQ"
                ElseIf colecciones_obs.Item(i) = "TL" Then
                    lostrObervacion = "TL"
                ElseIf colecciones_obs.Item(i) = "FR" Then
                    lostrObervacion = "FR"
                    'ElseIf colecciones_obs.Item(i) = "PU" Then
                    '   lostrObervacion = "PU"
                ElseIf colecciones_obs.Item(i) = "AU" Then
                    lostrObervacion = "AU"
                    'lostr_descri = lostrObervacion & ":" & v_checkbox20
                    lostr_descri = v_checkbox20
                    'ElseIf colecciones_obs.Item(i) = "PU" Then
                    ' lostrObervacion = "PU"
                    ' lostr_descri = v_checkbox20
                End If

                If v_observa_carta = "" Then  ' Esto hace cuando no hace click en Validar ninguna(caso de modificacion)
                    If lostrEstado = "UPDATE" Then
                        cls_Oracle.FT_Man_Observacion_CartaDM(v_codigo, v_codigo, lostrObervacion, _
                       lostr_obseva, lostr_descri, gstrCodigo_Usuario, "DELETE")
                    End If
                End If
                Dim lostrRetorno_cuenta As Integer

                'If lostrEstado = "INSERTA" Then
                '    Dim lostrRetorno_ins As String = ""
                '    lostrRetorno_cuenta = cls_Oracle.FT_SG_CUENTA_REG_IN_EVALGIS("SG_D_EVALGIS", v_codigo, v_formart, "", "")
                '    If lostrRetorno_cuenta = 0 Then
                '        lostrRetorno_ins = cls_Oracle.FT_SG_D_EVALGIS("INS", v_codigo, v_formart, gstrCodigo_Usuario)
                '        If lostrRetorno_ins = "1" Then  'ya inserto en tabla sg_d_evalgis

                lostrRetorno = cls_Oracle.FT_Man_Observacion_CartaDM(v_codigo, v_codigo, lostrObervacion, _
                    gstrCodigo_Usuario, lostr_descri, gstrCodigo_Usuario, lostrEstado)  'tabla sg_d_carac_evalgis

                '  End If
                '    Else
                'lostrRetorno = cls_Oracle.FT_Man_Observacion_CartaDM_desa(v_codigo, v_formart, v_codigo, lostrObervacion, _
                'lostr_obseva, lostr_descri, gstrCodigo_Usuario, lostrEstado)
                '    End If

                'Else  'update
                'lostrRetorno = cls_Oracle.FT_Man_Observacion_CartaDM_desa(v_codigo, v_formart, v_codigo, lostrObervacion, _
                'lostr_obseva, lostr_descri, gstrCodigo_Usuario, lostrEstado)
                'End If

            Next i
            If Val(lostrRetorno) > 0 Then
                MsgBox("La operaci�n se realiz� exitosamente. ", MsgBoxStyle.Information, "[BDGeocatmin]")
            End If
            ' Next i


            '*****************************************
            'Borra textbox del layout en item observaciones si encuentra

            Dim pContainer As IGraphicsContainer
            Dim pElement1 As IElement
            Dim pTextElement1 As ITextElement
            pContainer = pPageLayout
            pContainer.Reset()
            pElement1 = pContainer.Next
            While Not pElement1 Is Nothing
                If TypeOf pElement1 Is ITextElement Then
                    pTextElement1 = pElement1
                    If pTextElement1.Text = "Reservorio" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Zona Agricola Parcial" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Zona Agricola Total" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Dominio Maritimo Parcial" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Dominio Maritimo Total" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Carretera Asfaltada" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Carretera Afirmada" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Zona de Bosque Parcial" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Zona de Bosque Total" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Con Recubrimiento Parcial" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Sin Recubrimiento Total" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "L�nea de Alta Tensi�n El�ctrica" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Rio(s)" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Canal" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Laguna(s)" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "L�nea Ferrea" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "L�nea Frontera" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Zona de Traslape" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Restos Arqueologicos" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "No existen Observaciones" Then
                        pContainer.DeleteElement(pTextElement1)
                    End If
                End If
                pElement1 = pContainer.Next
            End While
            pMxDoc.UpdateContents()
            pMxDoc.ActiveView.Refresh()
            'Termino la parte de borrar textos
            Dim POSI_y1 As Double
            Dim POSI_y2 As Double
            Dim POSI_1x As Double
            Dim POSI_2x As Double
            Dim POSI_x1 As Double
            Dim POSI_x2 As Double
            POSI_y2 = posi_y_m
            POSI_y1 = posi_y1_m
            posi_y_m = POSI_y2
            posi_y1_m = POSI_y1
            POSI_x1 = 13.2
            POSI_x2 = 19.2
            For contatexto = 1 To 20
                pTextElement = New TextElement
                Dim pEnv As IEnvelope
                pEnv = New Envelope
                pPoint = New Point
                If POSI_y1 < 6.6 Or POSI_y2 < 6.6 Then
                    POSI_x1 = POSI_x1 + 3.2
                    POSI_x2 = POSI_x2 + 3.2
                    POSI_y2 = posi_y_m
                    POSI_y1 = posi_y1_m
                End If

                'End If
                If contatexto = 1 Then
                    If v_checkbox1 <> "" Then
                        pTextElement.Text = v_checkbox1
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.1
                        'pPoint.Y = 8.5
                        pPoint.X = 21.7
                        'pPoint.Y = 9.3
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 2 Then
                    If v_checkbox2 <> "" Then
                        pTextElement.Text = v_checkbox2
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 3 Then
                    If v_checkbox3 <> "" Then
                        pTextElement.Text = v_checkbox3
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 4 Then
                    If v_checkbox4 <> "" Then
                        pTextElement.Text = v_checkbox4
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 5 Then
                    If v_checkbox5 <> "" Then
                        pTextElement.Text = v_checkbox5
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 6 Then
                    If v_checkbox6 <> "" Then
                        pTextElement.Text = v_checkbox6
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 7 Then
                    If v_checkbox7 <> "" Then
                        pTextElement.Text = v_checkbox7
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 8 Then
                    If v_checkbox8 <> "" Then
                        pTextElement.Text = v_checkbox8
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3

                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 9 Then
                    If v_checkbox9 <> "" Then
                        pTextElement.Text = v_checkbox9
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 10 Then
                    If v_checkbox10 <> "" Then
                        pTextElement.Text = v_checkbox10
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 11 Then
                    If v_checkbox11 <> "" Then
                        pTextElement.Text = "Rio de " & v_checkbox11
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        'pEnv.LowerLeft = pPoint
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 12 Then
                    If v_checkbox12 <> "" Then
                        pTextElement.Text = v_checkbox12
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 13 Then
                    If v_checkbox13 <> "" Then
                        pTextElement.Text = "Laguna de " & v_checkbox13
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 14 Then
                    If v_checkbox14 <> "" Then
                        pTextElement.Text = v_checkbox14
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 15 Then
                    If v_checkbox15 <> "" Then
                        pTextElement.Text = v_checkbox15
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 16 Then
                    If v_checkbox16 <> "" Then
                        pTextElement.Text = v_checkbox16
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 17 Then
                    If v_checkbox17 <> "" Then
                        pTextElement.Text = v_checkbox17
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 18 Then
                    If v_checkbox18 <> "" Then
                        pTextElement.Text = v_checkbox18
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 19 Then
                    If v_checkbox19 <> "" Then
                        pTextElement.Text = v_checkbox19
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 20 Then
                    If v_checkbox20 <> "" Then
                        '  MsgBox(v_checkbox20)
                        pTextElement.Text = "Posible �rea Urbana/Exp. Urbana de " & v_checkbox20
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                'Simbolo del texto

                pTxtSym = New TextSymbol
                'fuente del texto

                Dim pFont As IFontDisp
                pFont = New StdFont
                pFont.Name = "Tahoma"
                pFont.Size = 6
                pFont.Bold = False
                pTxtSym.Font = pFont
                'Propiedades del Simbolo


                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
            colecciones_obs_update.Clear()
            colecciones_obs_upd_borrar.Clear()
        Catch ex As Exception
            MsgBox("Existe un Indicador que ya esta ingresado, verificar y volver a ejecutar Observaciones en Carta IGM...", MsgBoxStyle.Critical, "GIS")

        End Try

    End Sub

    Public Sub Genera_ObservacionesCarta_desarrollo()
        Try
            Dim cls_Oracle As New cls_Oracle
            Dim pPageLayout As IPageLayout
            Dim pActiveView As IActiveView
            Dim pGraphicsContainer As IGraphicsContainer
            Dim pElement As IElement
            Dim pTextElement As ITextElement
            Dim pPoint As IPoint
            Dim pTxtSym As IFormattedTextSymbol

            Dim contatexto As Integer
            pPageLayout = pMxDoc.PageLayout
            pPageLayout.Page.Units = esriUnits.esriCentimeters
            pActiveView = pPageLayout
            pGraphicsContainer = pPageLayout
            Dim conta_ob As Integer

            For conta_ob = 1 To 20
                If conta_ob = 1 Then
                    If v_checkbox1 <> "" Then
                        colecciones_obs.Add("GT")
                    End If
                ElseIf conta_ob = 2 Then
                    If v_checkbox2 <> "" Then
                        colecciones_obs.Add("GP")
                    End If
                ElseIf conta_ob = 3 Then
                    If v_checkbox3 <> "" Then
                        colecciones_obs.Add("MT")
                    End If
                ElseIf conta_ob = 4 Then
                    If v_checkbox4 <> "" Then
                        colecciones_obs.Add("MP")
                    End If
                ElseIf conta_ob = 5 Then
                    If v_checkbox5 <> "" Then
                        colecciones_obs.Add("CS")
                    End If
                ElseIf conta_ob = 6 Then
                    If v_checkbox6 <> "" Then
                        colecciones_obs.Add("CF")
                    End If
                ElseIf conta_ob = 7 Then
                    If v_checkbox7 <> "" Then
                        colecciones_obs.Add("BT")
                    End If
                ElseIf conta_ob = 8 Then
                    If v_checkbox8 <> "" Then
                        colecciones_obs.Add("BP")
                    End If
                ElseIf conta_ob = 9 Then
                    If v_checkbox9 <> "" Then
                        colecciones_obs.Add("RT")
                    End If
                ElseIf conta_ob = 10 Then
                    If v_checkbox10 <> "" Then
                        colecciones_obs.Add("RP")
                    End If
                ElseIf conta_ob = 11 Then
                    If v_checkbox11 <> "" Then
                        colecciones_obs.Add("HD")
                    End If

                ElseIf conta_ob = 12 Then
                    If v_checkbox12 <> "" Then
                        colecciones_obs.Add("CL")
                    End If
                ElseIf conta_ob = 13 Then
                    If v_checkbox13 <> "" Then
                        colecciones_obs.Add("LG")
                    End If
                ElseIf conta_ob = 14 Then
                    If v_checkbox14 <> "" Then
                        colecciones_obs.Add("RS")
                    End If
                ElseIf conta_ob = 15 Then
                    If v_checkbox15 <> "" Then
                        colecciones_obs.Add("FE")
                    End If
                ElseIf conta_ob = 16 Then
                    If v_checkbox16 <> "" Then
                        colecciones_obs.Add("TE")
                    End If
                ElseIf conta_ob = 17 Then
                    If v_checkbox17 <> "" Then
                        colecciones_obs.Add("RQ")
                    End If
                ElseIf conta_ob = 18 Then
                    If v_checkbox18 <> "" Then
                        colecciones_obs.Add("TL")
                    End If
                ElseIf conta_ob = 19 Then
                    If v_checkbox19 <> "" Then
                        colecciones_obs.Add("FR")
                    End If
                ElseIf conta_ob = 20 Then
                    If v_checkbox20 <> "" Then
                        colecciones_obs.Add("PU")
                    End If
                End If
            Next conta_ob



            '*****************************************
            'Adicionando de colecciones observaciones
            Dim v_formart As String = "1"

            For i As Integer = 1 To colecciones_obs_update.Count
                colecciones_obs.Add(colecciones_obs_update.Item(i))
            Next


            'termino

            Dim lostrEstado_Observacion As String
            Dim lostrMensaje As String = ""
            Dim lostrEstado As String = ""
            Dim lostr_obseva As String = ""
            Dim lostr_descri As String = ""
            Dim lostrOpcion1 As String = ""

            'lostrEstado_Observacion = cls_Oracle.FT_Ver_Estado_Doc(IIf(txhSolicitante = "I", "SO", ""), txhNumero)
            lostrEstado_Observacion = cls_Oracle.FT_Estado_Observacion(v_codigo)
            Select Case lostrEstado_Observacion
                Case "0"
                    lostrMensaje = " � Desea Grabar la Informaci�n ? "
                    lostrEstado = "INSERTA"
                    lostr_obseva = "OB"
                    If MsgBox(lostrMensaje, MsgBoxStyle.YesNo, "BDGeocatmin") = vbNo Then
                        Exit Sub
                    End If
                Case "1"
                    'lostrMensaje = " � Informaci�n Existes, desesa Actualizar ? "
                    lostrOpcion1 = MsgBox("� Ya existen Datos de Observacion Guardados anteriormente para el Informe T�cnico, Haga Click en .. ?" & vbNewLine & "SI:  Para Generar Historico de la informaci�n y Reemplazar los datos existentes" & vbNewLine & "NO:  Para Guardar informacion que reemplazara los datos existentes sin Generar Informaci�n Historica", MsgBoxStyle.YesNoCancel, "Evaluacion de DM...")
                    lostrEstado = "UPDATE"
                    lostr_obseva = "OB"

                    Select Case lostrOpcion1
                        Case "2" 'Cancel
                            Exit Sub  'No hace nada y sale del formulario evaluador
                        Case "6" 'Si, Generar Hist�rico y Graba la Informaci�n reemplazandola.
                            cls_Oracle.FT_GENERA_HISTORICO(v_codigo, v_formart)

                            cls_Oracle.FT_SG_D_EVALGIS("INS", v_codigo, v_formart, gstrCodigo_Usuario)

                        Case "7" 'No Genera Histotico pero si Graba la informacion reemplanzandola

                    End Select


            End Select


            'If MsgBox(lostrMensaje, MsgBoxStyle.YesNo, "BDGeocatmin") = vbNo Then
            '    Exit Sub
            'End If

            Dim lostrObervacion As String = ""
            'For i As Integer = 1 To colecciones_obs.Count
            '    If colecciones_obs.Item(i) = "GT" Then
            '        'lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & "Area Urbana de: " & v_checkbox20 & " || "
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox1 & " || "
            '    ElseIf colecciones_obs.Item(i) = "GP" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox2 & " || "
            '    ElseIf colecciones_obs.Item(i) = "MT" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox3 & " || "
            '    ElseIf colecciones_obs.Item(i) = "MP" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox4 & " || "
            '    ElseIf colecciones_obs.Item(i) = "CS" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox5 & " || "
            '    ElseIf colecciones_obs.Item(i) = "CF" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox6 & " || "
            '    ElseIf colecciones_obs.Item(i) = "BT" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox7 & " || "
            '    ElseIf colecciones_obs.Item(i) = "BP" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox8 & " || "
            '    ElseIf colecciones_obs.Item(i) = "RT" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox9 & " || "
            '    ElseIf colecciones_obs.Item(i) = "RP" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox10 & " || "
            '    ElseIf colecciones_obs.Item(i) = "RH" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox11 & " || "
            '    ElseIf colecciones_obs.Item(i) = "CL" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox12 & " || "
            '    ElseIf colecciones_obs.Item(i) = "LG" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox13 & " || "
            '    ElseIf colecciones_obs.Item(i) = "RS" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox14 & " || "
            '    ElseIf colecciones_obs.Item(i) = "FE" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox15 & " || "
            '    ElseIf colecciones_obs.Item(i) = "TE" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox16 & " || "
            '    ElseIf colecciones_obs.Item(i) = "RQ" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox17 & " || "
            '    ElseIf colecciones_obs.Item(i) = "TL" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox18 & " || "
            '    ElseIf colecciones_obs.Item(i) = "FR" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox19 & " || "
            '    ElseIf colecciones_obs.Item(i) = "AU" Then
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox20 & " || "
            '    Else
            '        lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & " || "
            '    End If
            'Next i

            'Dim lostrRetorno As String = cls_Oracle.FT_Man_Observacion_CartaDM(v_codigo, v_formart, v_codigo, lostrObervacion, _
            'gstrCodigo_Usuario, gstrCodigo_Usuario, lostrEstado)



            'Primero borrar lo que el usuario deselecciono de las observaciones de carta}

            Dim valor_obs_b As String
            'recorre lista de los indicadores a eliminar

            For k As Integer = 1 To colecciones_obs_upd_borrar.Count
                valor_obs_b = colecciones_obs_upd_borrar.Item(k)
                cls_Oracle.FT_Man_Observacion_CartaDM_desa(v_codigo, v_formart, v_codigo, valor_obs_b, _
                 lostr_obseva, lostr_descri, gstrCodigo_Usuario, "DELETE")
            Next k


            Dim lostrRetorno As String

            For i As Integer = 1 To colecciones_obs.Count
                lostr_descri = ""
                If colecciones_obs.Item(i) = "GT" Then

                    lostrObervacion = "GT"
                ElseIf colecciones_obs.Item(i) = "GP" Then
                    lostrObervacion = "GP"
                ElseIf colecciones_obs.Item(i) = "MT" Then
                    lostrObervacion = "MT"
                ElseIf colecciones_obs.Item(i) = "MP" Then
                    lostrObervacion = "MP"
                ElseIf colecciones_obs.Item(i) = "CS" Then
                    lostrObervacion = "CS"
                ElseIf colecciones_obs.Item(i) = "CF" Then
                    lostrObervacion = "CF"
                ElseIf colecciones_obs.Item(i) = "BT" Then
                    lostrObervacion = "BT"
                ElseIf colecciones_obs.Item(i) = "BP" Then
                    lostrObervacion = "BP"
                ElseIf colecciones_obs.Item(i) = "RT" Then
                    lostrObervacion = "RT"
                ElseIf colecciones_obs.Item(i) = "RP" Then
                    lostrObervacion = "RP"
                ElseIf colecciones_obs.Item(i) = "HD" Then
                    lostrObervacion = "HD"
                    lostr_descri = v_checkbox11
                ElseIf colecciones_obs.Item(i) = "CL" Then
                    lostrObervacion = "CL"
                ElseIf colecciones_obs.Item(i) = "LG" Then
                    lostrObervacion = "LG"
                    lostr_descri = v_checkbox13
                ElseIf colecciones_obs.Item(i) = "RS" Then
                    lostrObervacion = "RS"
                ElseIf colecciones_obs.Item(i) = "FE" Then
                    lostrObervacion = "FE"
                ElseIf colecciones_obs.Item(i) = "TE" Then
                    lostrObervacion = "TE"
                ElseIf colecciones_obs.Item(i) = "RQ" Then
                    lostrObervacion = "RQ"
                ElseIf colecciones_obs.Item(i) = "TL" Then
                    lostrObervacion = "TL"
                ElseIf colecciones_obs.Item(i) = "FR" Then
                    lostrObervacion = "FR"
                ElseIf colecciones_obs.Item(i) = "PU" Then
                    lostrObervacion = "PU"
                    lostr_descri = v_checkbox20
                End If

                If v_observa_carta = "" Then  ' Esto hace cuando no hace click en Validar ninguna(caso de modificacion)
                    If lostrEstado = "UPDATE" Then
                        cls_Oracle.FT_Man_Observacion_CartaDM_desa(v_codigo, v_formart, v_codigo, lostrObervacion, _
                       lostr_obseva, lostr_descri, gstrCodigo_Usuario, "DELETE")
                    End If
                End If
                Dim lostrRetorno_cuenta As Integer

                If lostrEstado = "INSERTA" Then
                    Dim lostrRetorno_ins As String = ""
                    lostrRetorno_cuenta = cls_Oracle.FT_SG_CUENTA_REG_IN_EVALGIS("SG_D_EVALGIS", v_codigo, v_formart, "", "")
                    If lostrRetorno_cuenta = 0 Then
                        lostrRetorno_ins = cls_Oracle.FT_SG_D_EVALGIS("INS", v_codigo, v_formart, gstrCodigo_Usuario)
                        If lostrRetorno_ins = "1" Then  'ya inserto en tabla sg_d_evalgis

                            lostrRetorno = cls_Oracle.FT_Man_Observacion_CartaDM_desa(v_codigo, v_formart, v_codigo, lostrObervacion, _
                                lostr_obseva, lostr_descri, gstrCodigo_Usuario, lostrEstado)  'tabla sg_d_carac_evalgis

                        End If
                    Else
                        lostrRetorno = cls_Oracle.FT_Man_Observacion_CartaDM_desa(v_codigo, v_formart, v_codigo, lostrObervacion, _
                        lostr_obseva, lostr_descri, gstrCodigo_Usuario, lostrEstado)
                    End If

                Else  'update
                    lostrRetorno = cls_Oracle.FT_Man_Observacion_CartaDM_desa(v_codigo, v_formart, v_codigo, lostrObervacion, _
                    lostr_obseva, lostr_descri, gstrCodigo_Usuario, lostrEstado)
                End If

            Next i
            If Val(lostrRetorno) > 0 Then
                MsgBox("La operaci�n se realiz� exitosamente. ", MsgBoxStyle.Information, "[BDGeocatmin]")
            End If
            ' Next i


            '*****************************************
            'Borra textbox del layout en item observaciones si encuentra

            Dim pContainer As IGraphicsContainer
            Dim pElement1 As IElement
            Dim pTextElement1 As ITextElement
            pContainer = pPageLayout
            pContainer.Reset()
            pElement1 = pContainer.Next
            While Not pElement1 Is Nothing
                If TypeOf pElement1 Is ITextElement Then
                    pTextElement1 = pElement1
                    If pTextElement1.Text = "Reservorio" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Zona Agricola Parcial" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Zona Agricola Total" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Dominio Maritimo Parcial" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Dominio Maritimo Total" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Carretera Asfaltada" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Carretera Afirmada" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Zona de Bosque Parcial" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Zona de Bosque Total" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Con Recubrimiento Parcial" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Sin Recubrimiento Total" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "L�nea de Alta Tensi�n El�ctrica" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Rio(s)" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Canal" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Laguna(s)" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "L�nea Ferrea" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "L�nea Frontera" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Zona de Traslape" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "Restos Arqueologicos" Then
                        pContainer.DeleteElement(pTextElement1)
                    ElseIf pTextElement1.Text = "No existen Observaciones" Then
                        pContainer.DeleteElement(pTextElement1)
                    End If
                End If
                pElement1 = pContainer.Next
            End While
            pMxDoc.UpdateContents()
            pMxDoc.ActiveView.Refresh()
            'Termino la parte de borrar textos
            Dim POSI_y1 As Double
            Dim POSI_y2 As Double
            Dim POSI_1x As Double
            Dim POSI_2x As Double
            Dim POSI_x1 As Double
            Dim POSI_x2 As Double
            POSI_y2 = posi_y_m
            POSI_y1 = posi_y1_m
            posi_y_m = POSI_y2
            posi_y1_m = POSI_y1
            POSI_x1 = 13.2
            POSI_x2 = 19.2
            For contatexto = 1 To 20
                pTextElement = New TextElement
                Dim pEnv As IEnvelope
                pEnv = New Envelope
                pPoint = New Point
                If POSI_y1 < 6.6 Or POSI_y2 < 6.6 Then
                    POSI_x1 = POSI_x1 + 3.2
                    POSI_x2 = POSI_x2 + 3.2
                    POSI_y2 = posi_y_m
                    POSI_y1 = posi_y1_m
                End If

                'End If
                If contatexto = 1 Then
                    If v_checkbox1 <> "" Then
                        pTextElement.Text = v_checkbox1
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.1
                        'pPoint.Y = 8.5
                        pPoint.X = 21.7
                        'pPoint.Y = 9.3
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 2 Then
                    If v_checkbox2 <> "" Then
                        pTextElement.Text = v_checkbox2
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 3 Then
                    If v_checkbox3 <> "" Then
                        pTextElement.Text = v_checkbox3
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 4 Then
                    If v_checkbox4 <> "" Then
                        pTextElement.Text = v_checkbox4
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 5 Then
                    If v_checkbox5 <> "" Then
                        pTextElement.Text = v_checkbox5
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 6 Then
                    If v_checkbox6 <> "" Then
                        pTextElement.Text = v_checkbox6
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 7 Then
                    If v_checkbox7 <> "" Then
                        pTextElement.Text = v_checkbox7
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 8 Then
                    If v_checkbox8 <> "" Then
                        pTextElement.Text = v_checkbox8
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3

                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 9 Then
                    If v_checkbox9 <> "" Then
                        pTextElement.Text = v_checkbox9
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 10 Then
                    If v_checkbox10 <> "" Then
                        pTextElement.Text = v_checkbox10
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 11 Then
                    If v_checkbox11 <> "" Then
                        pTextElement.Text = "Rio de " & v_checkbox11
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        'pEnv.LowerLeft = pPoint
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If

                ElseIf contatexto = 12 Then
                    If v_checkbox12 <> "" Then
                        pTextElement.Text = v_checkbox12
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 13 Then
                    If v_checkbox13 <> "" Then
                        pTextElement.Text = "Laguna de " & v_checkbox13
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 14 Then
                    If v_checkbox14 <> "" Then
                        pTextElement.Text = v_checkbox14
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 15 Then
                    If v_checkbox15 <> "" Then
                        pTextElement.Text = v_checkbox15
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 16 Then
                    If v_checkbox16 <> "" Then
                        pTextElement.Text = v_checkbox16
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 17 Then
                    If v_checkbox17 <> "" Then
                        pTextElement.Text = v_checkbox17
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 18 Then
                    If v_checkbox18 <> "" Then
                        pTextElement.Text = v_checkbox18
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 19 Then
                    If v_checkbox19 <> "" Then
                        pTextElement.Text = v_checkbox19
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                ElseIf contatexto = 20 Then
                    If v_checkbox20 <> "" Then
                        '  MsgBox(v_checkbox20)
                        pTextElement.Text = "Area Urbana de " & v_checkbox20
                        pPoint.X = POSI_x1
                        pPoint.Y = POSI_y2 - 0.3
                        pPoint.X = POSI_x2
                        pPoint.Y = POSI_y1 - 0.3
                        POSI_y1 = POSI_y1 - 0.3
                        POSI_y2 = POSI_y2 - 0.3
                    Else
                        pTextElement.Text = ""
                        pPoint.X = 17.9
                        pPoint.Y = POSI_y2 + 0.6
                        pPoint.X = 21.7
                        pPoint.Y = POSI_y1 + 0.6
                        POSI_1x = POSI_y1
                        POSI_2x = POSI_y2
                    End If
                End If

                pEnv.UpperRight = pPoint
                pElement = pTextElement
                pElement.Geometry = pEnv
                'Simbolo del texto

                pTxtSym = New TextSymbol
                'fuente del texto

                Dim pFont As IFontDisp
                pFont = New StdFont
                pFont.Name = "Tahoma"
                pFont.Size = 6
                pFont.Bold = False
                pTxtSym.Font = pFont
                'Propiedades del Simbolo


                pTxtSym.Angle = 0
                pTxtSym.RightToLeft = False
                pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
                pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
                pTxtSym.CharacterSpacing = 20
                pTxtSym.Case = esriTextCase.esriTCNormal
                pTextElement.Symbol = pTxtSym
                pGraphicsContainer.AddElement(pTextElement, 1)

                'Refrescar solo los graficos del PageLayout
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Next contatexto
            colecciones_obs_update.Clear()
            colecciones_obs_upd_borrar.Clear()
        Catch ex As Exception
            MsgBox("Existe un Indicador que ya esta ingresado, verificar y volver a ejecutar Observaciones en Carta IGM...", MsgBoxStyle.Critical, "GIS")

        End Try

    End Sub



    'Public Sub Genera_ObservacionesCarta1()
    '    Dim cls_Oracle As New cls_Oracle
    '    Dim pPageLayout As IPageLayout
    '    Dim pActiveView As IActiveView
    '    Dim pGraphicsContainer As IGraphicsContainer
    '    Dim pElement As IElement
    '    Dim pTextElement As ITextElement
    '    Dim pPoint As IPoint
    '    Dim pTxtSym As IFormattedTextSymbol
    '    Dim contatexto As Integer
    '    pPageLayout = pMxDoc.PageLayout
    '    pPageLayout.Page.Units = esriUnits.esriCentimeters
    '    pActiveView = pPageLayout
    '    pGraphicsContainer = pPageLayout
    '    Dim v_format As String

    '    Dim conta_ob As Integer

    '    For conta_ob = 1 To 20
    '        If conta_ob = 1 Then
    '            If v_checkbox1 <> "" Then
    '                colecciones_obs.Add("GT")
    '            End If
    '        ElseIf conta_ob = 2 Then
    '            If v_checkbox2 <> "" Then
    '                colecciones_obs.Add("GP")
    '            End If
    '        ElseIf conta_ob = 3 Then
    '            If v_checkbox3 <> "" Then
    '                colecciones_obs.Add("MT")
    '            End If
    '        ElseIf conta_ob = 4 Then
    '            If v_checkbox4 <> "" Then
    '                colecciones_obs.Add("MP")
    '            End If
    '        ElseIf conta_ob = 5 Then
    '            If v_checkbox5 <> "" Then
    '                colecciones_obs.Add("CS")
    '            End If
    '        ElseIf conta_ob = 6 Then
    '            If v_checkbox6 <> "" Then
    '                colecciones_obs.Add("CF")
    '            End If
    '        ElseIf conta_ob = 7 Then
    '            If v_checkbox7 <> "" Then
    '                colecciones_obs.Add("BT")
    '            End If
    '        ElseIf conta_ob = 8 Then
    '            If v_checkbox8 <> "" Then
    '                colecciones_obs.Add("BP")
    '            End If
    '        ElseIf conta_ob = 9 Then
    '            If v_checkbox9 <> "" Then
    '                colecciones_obs.Add("RT")
    '            End If
    '        ElseIf conta_ob = 10 Then
    '            If v_checkbox10 <> "" Then
    '                colecciones_obs.Add("RP")
    '            End If
    '        ElseIf conta_ob = 11 Then
    '            If v_checkbox11 <> "" Then
    '                colecciones_obs.Add("RH")
    '            End If

    '        ElseIf conta_ob = 12 Then
    '            If v_checkbox12 <> "" Then
    '                colecciones_obs.Add("CL")
    '            End If
    '        ElseIf conta_ob = 13 Then
    '            If v_checkbox13 <> "" Then
    '                colecciones_obs.Add("LG")
    '            End If
    '        ElseIf conta_ob = 14 Then
    '            If v_checkbox14 <> "" Then
    '                colecciones_obs.Add("RS")
    '            End If
    '        ElseIf conta_ob = 15 Then
    '            If v_checkbox15 <> "" Then
    '                colecciones_obs.Add("FE")
    '            End If
    '        ElseIf conta_ob = 16 Then
    '            If v_checkbox16 <> "" Then
    '                colecciones_obs.Add("TE")
    '            End If
    '        ElseIf conta_ob = 17 Then
    '            If v_checkbox17 <> "" Then
    '                colecciones_obs.Add("RQ")
    '            End If
    '        ElseIf conta_ob = 18 Then
    '            If v_checkbox18 <> "" Then
    '                colecciones_obs.Add("TL")
    '            End If
    '        ElseIf conta_ob = 19 Then
    '            If v_checkbox19 <> "" Then
    '                colecciones_obs.Add("FR")
    '            End If
    '        ElseIf conta_ob = 20 Then
    '            If v_checkbox20 <> "" Then
    '                colecciones_obs.Add("AU")
    '            End If
    '        End If
    '    Next conta_ob

    '    '*****************************************
    '    Dim lostrEstado_Observacion As String
    '    Dim lostrMensaje As String = ""
    '    Dim lostrEstado As String = ""
    '    'lostrEstado_Observacion = cls_Oracle.FT_Ver_Estado_Doc(IIf(txhSolicitante = "I", "SO", ""), txhNumero)
    '    lostrEstado_Observacion = cls_Oracle.FT_Estado_Observacion(v_codigo)
    '    Select Case lostrEstado_Observacion
    '        Case "0"
    '            lostrMensaje = " � Desea Grabar la Informaci�n ? "
    '            lostrEstado = "INSERTA"
    '        Case "1"
    '            lostrMensaje = " � Informaci�n Existes, desesa Actualizar ? "
    '            lostrEstado = "UPDATE"
    '    End Select
    '    If MsgBox(lostrMensaje, MsgBoxStyle.YesNo, "BDGeocatmin") = vbNo Then
    '        Exit Sub
    '    End If

    '    Dim lostrObervacion As String = ""
    '    For i As Integer = 1 To colecciones_obs.Count
    '        If colecciones_obs.Item(i) = "AU" Then
    '            'lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & "Area Urbana de: " & v_checkbox20 & " || "
    '            lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox20 & " || "
    '        ElseIf colecciones_obs.Item(i) = "RH" Then
    '            lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox11 & " || "
    '        ElseIf colecciones_obs.Item(i) = "LG" Then
    '            lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & "-" & v_checkbox13 & " || "
    '        Else
    '            lostrObervacion = lostrObervacion & i + 1 & " | " & colecciones_obs.Item(i) & " || "
    '        End If
    '    Next i

    '    Dim lostrRetorno As String = cls_Oracle.FT_Man_Observacion_CartaDM(v_codigo, v_format, v_codigo, lostrObervacion, _
    '         gstrCodigo_Usuario, gstrCodigo_Usuario, lostrEstado)

    '    'Dim lostrRetorno As String = cls_Oracle.FT_Man_Observacion_CartaDM(v_codigo, v_codigo, lostrObervacion, _
    '    '    gstrCodigo_Usuario, gstrCodigo_Usuario, lostrEstado)

    '    If Val(lostrRetorno) > 0 Then
    '        MsgBox("La operaci�n se realiz� exitosamente. ", MsgBoxStyle.Information, "[BDGeocatmin]")
    '    End If


    '    '*****************************************
    '    'Borra textbox del layout en item observaciones si encuentra

    '    Dim pContainer As IGraphicsContainer
    '    Dim pElement1 As IElement
    '    Dim pTextElement1 As ITextElement
    '    pContainer = pPageLayout
    '    pContainer.Reset()
    '    pElement1 = pContainer.Next
    '    While Not pElement1 Is Nothing
    '        If TypeOf pElement1 Is ITextElement Then
    '            pTextElement1 = pElement1
    '            If pTextElement1.Text = "Reservorio" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Zona Agricola Parcial" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Zona Agricola Total" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Dominio Maritimo Parcial" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Dominio Maritimo Total" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Carretera Asfaltada" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Carretera Afirmada" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Zona de Bosque Parcial" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Zona de Bosque Total" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Con Recubrimiento Parcial" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Sin Recubrimiento Total" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "L�nea de Alta Tensi�n El�ctrica" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Rio(s)" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Canal" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Laguna(s)" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "L�nea Ferrea" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "L�nea Frontera" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Zona de Traslape" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "Restos Arqueologicos" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            ElseIf pTextElement1.Text = "No existen Observaciones" Then
    '                pContainer.DeleteElement(pTextElement1)
    '            End If
    '        End If
    '        pElement1 = pContainer.Next
    '    End While
    '    pMxDoc.UpdateContents()
    '    pMxDoc.ActiveView.Refresh()
    '    'Termino la parte de borrar textos
    '    Dim POSI_y1 As Double
    '    Dim POSI_y2 As Double
    '    Dim POSI_1x As Double
    '    Dim POSI_2x As Double
    '    Dim POSI_x1 As Double
    '    Dim POSI_x2 As Double
    '    POSI_y2 = posi_y_m
    '    POSI_y1 = posi_y1_m
    '    posi_y_m = POSI_y2
    '    posi_y1_m = POSI_y1
    '    POSI_x1 = 13.2
    '    POSI_x2 = 19.2
    '    For contatexto = 1 To 20
    '        pTextElement = New TextElement
    '        Dim pEnv As IEnvelope
    '        pEnv = New Envelope
    '        pPoint = New Point
    '        If POSI_y1 < 6.6 Or POSI_y2 < 6.6 Then
    '            POSI_x1 = POSI_x1 + 3.2
    '            POSI_x2 = POSI_x2 + 3.2
    '            POSI_y2 = posi_y_m
    '            POSI_y1 = posi_y1_m
    '        End If

    '        'End If
    '        If contatexto = 1 Then
    '            If v_checkbox1 <> "" Then
    '                pTextElement.Text = v_checkbox1
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.1
    '                'pPoint.Y = 8.5
    '                pPoint.X = 21.7
    '                'pPoint.Y = 9.3
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If

    '        ElseIf contatexto = 2 Then
    '            If v_checkbox2 <> "" Then
    '                pTextElement.Text = v_checkbox2
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 3 Then
    '            If v_checkbox3 <> "" Then
    '                pTextElement.Text = v_checkbox3
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If

    '        ElseIf contatexto = 4 Then
    '            If v_checkbox4 <> "" Then
    '                pTextElement.Text = v_checkbox4
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 5 Then
    '            If v_checkbox5 <> "" Then
    '                pTextElement.Text = v_checkbox5
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 6 Then
    '            If v_checkbox6 <> "" Then
    '                pTextElement.Text = v_checkbox6
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If

    '        ElseIf contatexto = 7 Then
    '            If v_checkbox7 <> "" Then
    '                pTextElement.Text = v_checkbox7
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If

    '        ElseIf contatexto = 8 Then
    '            If v_checkbox8 <> "" Then
    '                pTextElement.Text = v_checkbox8
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3

    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If

    '        ElseIf contatexto = 9 Then
    '            If v_checkbox9 <> "" Then
    '                pTextElement.Text = v_checkbox9
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If

    '        ElseIf contatexto = 10 Then
    '            If v_checkbox10 <> "" Then
    '                pTextElement.Text = v_checkbox10
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 11 Then
    '            If v_checkbox11 <> "" Then
    '                pTextElement.Text = "Rio de " & v_checkbox11
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                'pEnv.LowerLeft = pPoint
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If

    '        ElseIf contatexto = 12 Then
    '            If v_checkbox12 <> "" Then
    '                pTextElement.Text = v_checkbox12
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 13 Then
    '            If v_checkbox13 <> "" Then
    '                pTextElement.Text = "Laguna de " & v_checkbox13
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 14 Then
    '            If v_checkbox14 <> "" Then
    '                pTextElement.Text = v_checkbox14
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 15 Then
    '            If v_checkbox15 <> "" Then
    '                pTextElement.Text = v_checkbox15
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 16 Then
    '            If v_checkbox16 <> "" Then
    '                pTextElement.Text = v_checkbox16
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 17 Then
    '            If v_checkbox17 <> "" Then
    '                pTextElement.Text = v_checkbox17
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 18 Then
    '            If v_checkbox18 <> "" Then
    '                pTextElement.Text = v_checkbox18
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 19 Then
    '            If v_checkbox19 <> "" Then
    '                pTextElement.Text = v_checkbox19
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        ElseIf contatexto = 20 Then
    '            If v_checkbox20 <> "" Then
    '                '  MsgBox(v_checkbox20)
    '                pTextElement.Text = "Area Urbana de " & v_checkbox20
    '                pPoint.X = POSI_x1
    '                pPoint.Y = POSI_y2 - 0.3
    '                pPoint.X = POSI_x2
    '                pPoint.Y = POSI_y1 - 0.3
    '                POSI_y1 = POSI_y1 - 0.3
    '                POSI_y2 = POSI_y2 - 0.3
    '            Else
    '                pTextElement.Text = ""
    '                pPoint.X = 17.9
    '                pPoint.Y = POSI_y2 + 0.6
    '                pPoint.X = 21.7
    '                pPoint.Y = POSI_y1 + 0.6
    '                POSI_1x = POSI_y1
    '                POSI_2x = POSI_y2
    '            End If
    '        End If

    '        pEnv.UpperRight = pPoint
    '        pElement = pTextElement
    '        pElement.Geometry = pEnv
    '        'Simbolo del texto

    '        pTxtSym = New TextSymbol
    '        'fuente del texto

    '        Dim pFont As IFontDisp
    '        pFont = New StdFont
    '        pFont.Name = "Tahoma"
    '        'pFont.Size = 6
    '        pFont.Size = 9
    '        pFont.Bold = False
    '        pTxtSym.Font = pFont
    '        'Propiedades del Simbolo


    '        pTxtSym.Angle = 0
    '        pTxtSym.RightToLeft = False
    '        pTxtSym.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline
    '        pTxtSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull
    '        pTxtSym.CharacterSpacing = 20
    '        pTxtSym.Case = esriTextCase.esriTCNormal
    '        pTextElement.Symbol = pTxtSym
    '        pGraphicsContainer.AddElement(pTextElement, 1)

    '        'Refrescar solo los graficos del PageLayout
    '        pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    '    Next contatexto
    'End Sub

    Public Sub agrega_logofoliacion_plano(ByVal p_app As IApplication, ByVal caso_consulta As String)
        'Agrega Logo foliacion a planos
        Dim pPictElement As IPictureElement
        pPictElement = New JpgPictureElement
        ' pMxDoc = p_app.Document
        Dim XMIN As Double
        Dim XMAX As Double
        Dim YMIN As Double
        Dim YMAX As Double
        Dim pEnv As IEnvelope
        Dim vistaactiva As IActiveView
        Dim pElement As IElement
        If caso_consulta = "CARTA IGN" Then
            'glo_pathGeoCatMin()
            'If Dir("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_H.JPG", vbArchive) <> "" Then
            If Dir(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_h.jpg", vbArchive) <> "" Then
                'pPictElement.ImportPictureFromFile("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_H.JPG")
                pPictElement.ImportPictureFromFile(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_h.jpg")
                'XMIN = 25.9
                'XMAX = 27.8
                'YMIN = 6.42
                'YMAX = 7.42

                'XMIN = 24.9
                'XMAX = 27.8
                'YMIN = 6.42
                'YMAX = 8.42

                XMIN = 22.9
                XMAX = 27.8
                YMIN = 6.42
                YMAX = 8.92

            End If

        ElseIf caso_consulta = "DEMARCACION POLITICA" Or caso_consulta = "MAPA GEOLOGICO" Then
            'If Dir("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG", vbArchive) <> "" Then
            If Dir(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_v.jpg", vbArchive) <> "" Then
                'pPictElement.ImportPictureFromFile("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG")
                pPictElement.ImportPictureFromFile(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_v.jpg")
                'XMIN = 2.1
                'XMAX = 4.1
                'YMIN = 2.82
                'YMAX = 5.32

                'XMIN = 2.1
                'XMAX = 4.9
                'YMIN = 2.82
                'YMAX = 6.2

                XMIN = 2.2
                XMAX = 5.5
                YMIN = 2.32
                YMAX = 7.2
            End If

        ElseIf caso_consulta = "CATASTRO MINERO" Then
            'If Dir("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG", vbArchive) <> "" Then
            If Dir(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_v.jpg", vbArchive) <> "" Then
                'pPictElement.ImportPictureFromFile("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG")
                pPictElement.ImportPictureFromFile(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_v.jpg")
                'XMIN = 2.1
                'XMAX = 4.1
                'YMIN = 2.82
                'YMAX = 5.32

                'XMIN = 2.1
                'XMAX = 4.9
                'YMIN = 2.82
                'YMAX = 6.2

                XMIN = 1.7
                XMAX = 5.0
                YMIN = 2.52
                YMAX = 7.4

                ''ElseIf caso_consulta = "DEMARCACION POLITICA" Or caso_consulta = "CATASTRO MINERO" Then
                'If Dir("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG", vbArchive) <> "" Then
                ''If Dir(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_v.jpg", vbArchive) <> "" Then
                'pPictElement.ImportPictureFromFile("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG")
                'pPictElement.ImportPictureFromFile(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_v.jpg")
                'XMIN = 2.1
                'XMAX = 4.1
                'YMIN = 2.82
                'YMAX = 5.32

                'XMIN = 2.1
                'XMAX = 4.9
                'YMIN = 2.82
                'YMAX = 6.2

                '' XMIN = 1.7
                ''XMAX = 5.0
                '' YMIN = 2.52
                '' YMAX = 7.4
            End If
            ElseIf caso_consulta = "Plano Area superpuesta" Then
                If Dir(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_v.jpg", vbArchive) <> "" Then

                    pPictElement.ImportPictureFromFile(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_v.jpg")


                XMIN = 2.5
                    XMAX = 4.8
                    YMIN = 2.52
                YMAX = 7.1

                End If

                'ElseIf caso_consulta = "CATASTRO MINERO" Then
                '    If Dir("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG", vbArchive) <> "" Then
                '        pPictElement.ImportPictureFromFile("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG")
                '        XMIN = 2.1
                '        XMAX = 4.1
                '        YMIN = 2.82
                '        YMAX = 5.32
                '    End If

            End If

            pElement = pPictElement
            vistaactiva = pMxDoc.PageLayout
            pEnv = vistaactiva.Extent

            pEnv.PutCoords(XMIN, YMIN, XMAX, YMAX)
            pElement.Geometry = pEnv
            Dim pGC As IGraphicsContainer
            pGC = vistaactiva
            pGC.AddElement(pElement, 0)
    End Sub


    Public Sub AddLegend(ByVal posX As System.Double, ByVal posY As System.Double, ByVal legW As System.Double)
        'no funciona, hay que acondicionar tomado de ayuda esri parte leyenda
        Dim pageLayout As ESRI.ArcGIS.Carto.IPageLayout
        Dim map As ESRI.ArcGIS.Carto.IMap

        If pageLayout Is Nothing OrElse map Is Nothing Then

            Return

        End If

        Dim graphicsContainer As ESRI.ArcGIS.Carto.IGraphicsContainer = TryCast(pageLayout, ESRI.ArcGIS.Carto.IGraphicsContainer) ' Dynamic Cast
        Dim mapFrame As ESRI.ArcGIS.Carto.IMapFrame = TryCast(graphicsContainer.FindFrame(map), ESRI.ArcGIS.Carto.IMapFrame) ' Dynamic Cast
        Dim uid As ESRI.ArcGIS.esriSystem.IUID = New ESRI.ArcGIS.esriSystem.UIDClass
        uid.Value = "esriCarto.Legend"
        Dim mapSurroundFrame As ESRI.ArcGIS.Carto.IMapSurroundFrame = mapFrame.CreateSurroundFrame(CType(uid, ESRI.ArcGIS.esriSystem.UID), Nothing) ' Explicit Cast

        'Get aspect ratio
        Dim querySize As ESRI.ArcGIS.Carto.IQuerySize = TryCast(mapSurroundFrame.MapSurround, ESRI.ArcGIS.Carto.IQuerySize) ' Dynamic Cast
        Dim w As System.Double = 0
        Dim h As System.Double = 0
        querySize.QuerySize(w, h)
        Dim aspectRatio As System.Double = w / h

        Dim envelope As ESRI.ArcGIS.Geometry.IEnvelope = New ESRI.ArcGIS.Geometry.EnvelopeClass
        envelope.PutCoords(posX, posY, (posX * legW), (posY * legW / aspectRatio))
        Dim element As ESRI.ArcGIS.Carto.IElement = TryCast(mapSurroundFrame, ESRI.ArcGIS.Carto.IElement)
        element.Geometry = envelope
        graphicsContainer.AddElement(element, 0)

    End Sub

    Public Sub Genera_Leyenda_capa(ByVal objeto As String, ByVal p_App As IApplication)
        Dim pMxDoc As IMxDocument
        Dim pActiveView As IActiveView
        Dim pEnv As IEnvelope
        Dim pID As New UID
        Dim pMapSurround As IMapSurround
        'Dim pMarkerNorthArrow As IMarkerNorthArrow
        'Dim pCharacterMarkerSymbol As ICharacterMarkerSymbol

        pMxDoc = p_App.Document
        pActiveView = pMxDoc.PageLayout
        pEnv = New Envelope

        If objeto = "Leyenda" Then
            pEnv.PutCoords(18.7, 5.5, 19, 9)
            'pEnv.PutCoords(2.7, 5.5, 3, 8.5)
            pID.Value = "esriCarto.Legend"
            pMapSurround = Create_Surround(pID, pEnv, "Leyenda", pMxDoc.FocusMap, pMxDoc.PageLayout)
            'CustomAreaPatch(p_App)
            'CustomAreaPatch_PRUEBA(p_App)  'final
            CustomAreaPatch_new(p_App)
            'CustomAreaPatch_xxx(p_App)  'bien
            'RefreshLegend(p_App)

        End If


        pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        'pGCS.UnselectAllElements()


        'Este paso generala la leyenda en grafico, tiene que estar seleccionado

        Dim pGCS As IGraphicsContainerSelect
        pGCS = pMxDoc.PageLayout

        If pGCS.ElementSelectionCount = 0 Then
            Exit Sub  'no hay objeto
        Else
            If pGCS.ElementSelectionCount > 1 Then

            End If
        End If

        If Not TypeOf pGCS.SelectedElement(0) Is IGraphicsComposite Then
            Debug.Print("not a graphicscomposite")
            Exit Sub
        End If

        Dim pGComp As IGraphicsComposite
        pGComp = pGCS.SelectedElement(0)
        Dim pEnumElem As IEnumElement
        Dim pAV As IActiveView
        pAV = pMxDoc.PageLayout
        Dim pUnk As IUnknown
        pEnumElem = pGComp.Graphics(pAV.ScreenDisplay, pUnk)

        Debug.Print(pUnk Is Nothing) ' prints true, what is this?

        Dim pNewGElement As IGroupElement
        pNewGElement = New GroupElement

        Dim pClone As IClone
        pClone = pEnumElem.Next
        Do While Not pClone Is Nothing
            pNewGElement.AddElement(pClone.Clone)
            pClone = pEnumElem.Next
        Loop
        Dim pGC As IGraphicsContainer
        pGC = pGCS
        pGC.DeleteElement(pGComp)
        pGC.AddElement(pNewGElement, 0)

        pAV.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pNewGElement, Nothing)


    End Sub

    Public Function Create_Surround(ByVal pID As UID, ByVal pEnv As IEnvelope, ByVal strName As String, _
    ByVal pMap As IMap, ByVal pPageLayout As IPageLayout) As IMapSurround

        Dim pGraphicsContainer As IGraphicsContainer
        Dim pActiveView As IActiveView
        Dim pMapSurroundFrame As IMapSurroundFrame
        'Dim pMapSurround As IMapSurround
        Dim pMapFrame As IMapFrame
        Dim pElement As IElement

        'MapSurrounds are held in a MapSurroundFrame
        'MapSurroundFrames are related to MapFrames
        'MapFrames hold Maps
        pGraphicsContainer = pPageLayout
        pMapFrame = pGraphicsContainer.FindFrame(pMap)
        pMapSurroundFrame = pMapFrame.CreateSurroundFrame(pID, Nothing)
        pMapSurroundFrame.MapSurround.Name = strName

        'Set the geometry of the MapSurroundFrame to give it a location
        'Activate it and add it to the PageLayout's graphics container

        pElement = pMapSurroundFrame
        pActiveView = pPageLayout
        pElement.Geometry = pEnv
        pElement.Activate(pActiveView.ScreenDisplay)

        'Allow the legend frame size to be altered after the legend has been
        'added to the GraphicsContainer 

        Dim PTrack As ITrackCancel
        PTrack = New CancelTracker
        pElement.Draw(pActiveView.ScreenDisplay, PTrack)

        pGraphicsContainer.AddElement(pElement, 0)
        'Re-apply the change to the Legend MapSurroundFrame Geometry 
        pElement.Geometry = pEnv
        Create_Surround = pMapSurroundFrame.MapSurround
    End Function

    Public Sub CustomAreaPatch_new(ByVal p_App As IApplication)
        Dim pMxDoc As IMxDocument
        Dim pGCS As IGraphicsContainerSelect
        Dim pLegendSurround As IMapSurroundFrame
        Dim pLegend As ILegend
        Dim pLegendItem As ILegendItem
        Dim pPatch As IPatch
        Dim pElement As IElement
        Dim pLegendFormat As ILegendFormat
        Dim pLegendClassFormat As ILegendClassFormat
        Dim pPoint As IPoint
        pPoint = New Point

        Dim pGeoFeatureLayer As IGeoFeatureLayer 'xx
        Dim pLegendInfo As ILegendInfo  'xx
        Dim pFeatureRenderer As IFeatureRenderer
        Dim pUniqueValueRenderer As IUniqueValueRenderer
        Dim pLegendGroup As ILegendGroup
        Dim pBorder As IBorder
        Dim pSymbolBorder As ISymbolBorder

        '2201
        'pSymbolBorder = New SymbolBorder
        ''Dim pLegendClass As ILegendClass
        'pLineSymbol = pSLSymbol
        'pLineSymbol.Width = 0
        'pSymbolBorder.LineSymbol = pLineSymbol
        'pBorder = pSymbolBorder

        'pMapFrame = pElement
        'pFrameElement = pMapFrame
        'pFrameElement.Border = pBorder
        '2201

        'Dim pHorizontalLegendItem As IHorizontalLegendItem
        Dim pFontDisp As IFontDisp
        pFontDisp = New StdFont
        Dim pTextSymbol As ITextSymbol
        pTextSymbol = New TextSymbol

        pMxDoc = p_App.Document
        pGCS = pMxDoc.PageLayout

        pLegendSurround = pGCS.SelectedElement(0)
        pLegend = pLegendSurround.MapSurround

        pElement = pGCS.SelectedElement(0)
        pPatch = New AreaPatch
        pPatch.Geometry = pElement.Geometry
        pGC = pPageLayout

        pLegendFormat = pLegend.Format
        ' pLegend.Title = "                           Leyenda"
        pLegend.Title = "                          "
        '        pLegend.Map.ClipBorder.Ga

        pLegendFormat.DefaultAreaPatch = pPatch
        pLegendFormat.DefaultPatchHeight = 7 '14
        pLegendFormat.DefaultPatchWidth = 12 '28
        pLegendFormat.DefaultAreaPatch = pPatch
        pLegendFormat.HorizontalPatchGap = 5  'espacio entre patch y label
        pLegendFormat.TitlePosition = esriRectanglePosition.esriLeftSide
        pLegend.Format.TextGap = 8

        pLegend.Format = pLegendFormat
        pLegend.Refresh()
        'With pFontDisp
        '    .Name = "Tahoma"
        '    .Size = 10
        '    .Bold = False
        'End With

        'pTextSymbol.Font = pFontDisp
        'pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
        'pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter

        pGeoFeatureLayer = pFeatureLayer
        pLegendInfo = pGeoFeatureLayer
        pUniqueValueRenderer = New UniqueValueRenderer
        pFeatureRenderer = pUniqueValueRenderer

        pLegendGroup = pLegendInfo.LegendGroup(0)
        pLegendClassFormat = New LegendClassFormat

        Dim RowCounter As Integer
        For r As Integer = 0 To pLegend.ItemCount - 1
            pLegendItem = pLegend.Item(r)
            'pGeoFeatureLayer = pLegendItem.Layer
            pLegendInfo = pGeoFeatureLayer.Renderer

            For s As Integer = 0 To pLegendInfo.LegendGroupCount - 1
                pLegendGroup = pLegendInfo.LegendGroup(s)
                RowCounter = RowCounter + pLegendGroup.ClassCount
                pLegendGroup.Editable = True
                pLegendGroup.Heading = pGeoFeatureLayer.Name  'xx
                'If pGeoFeatureLayer.Name = "Proyectos_y_Operaciones" Then
                '    pGeoFeatureLayer.Visible = False  'new
                'End If
            Next s
        Next r

        pLegendItem = New HorizontalLegendItem

        pLegendItem.Layer = pFeatureLayer  'xxx
        pLegendItem.Columns = 1
        pLegendItem.ShowDescriptions = True
        pLegendItem.ShowHeading = True  'xx
        pLegendItem.ShowLabels = True
        pLegendItem.ShowLayerName = True
        pLegendItem.LegendClassFormat = pLegendClassFormat
        'pLegendItem.LegendClassFormat.PatchHeight = 5  'alto de items de la leyenda
        'pLegendItem.LegendClassFormat.PatchWidth = 10   'ancho de items de la leyenda

        'pPoint.X = 17.75
        'pPoint.Y = 6
        'pPoint.X = 18.75
        'pPoint.Y = 5

        'Define el boundary del cuerpo de la leyenda

        pPoint.X = 0.7
        pPoint.Y = 2.8
        pPoint.X = 1.7
        pPoint.Y = 1.8

        pElement.Geometry = pPoint
        pLegend.Refresh()
        pMxDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    End Sub

    Public Sub agrega_logofoliacion_plano_ANT(ByVal p_app As IApplication, ByVal caso_consulta As String)
        'Agrega Logo foliacion a planos
        Dim pPictElement As IPictureElement
        pPictElement = New JpgPictureElement
        pMxDoc = p_app.Document
        Dim XMIN As Double
        Dim XMAX As Double
        Dim YMIN As Double
        Dim YMAX As Double
        Dim pEnv As IEnvelope
        Dim vistaactiva As IActiveView
        Dim pElement As IElement
        If caso_consulta = "CARTA IGN" Then
            'glo_pathGeoCatMin()
            'If Dir("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_H.JPG", vbArchive) <> "" Then
            If Dir(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_h.jpg", vbArchive) <> "" Then
                'pPictElement.ImportPictureFromFile("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_H.JPG")
                pPictElement.ImportPictureFromFile(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_h.jpg")
                'XMIN = 25.9
                'XMAX = 27.8
                'YMIN = 6.42
                'YMAX = 7.42

                XMIN = 24.9
                XMAX = 27.8
                YMIN = 6.42
                YMAX = 8.42
            End If

        ElseIf caso_consulta = "DEMARCACION POLITICA" Or caso_consulta = "CATASTRO MINERO" Then
            'If Dir("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG", vbArchive) <> "" Then
            If Dir(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_v.jpg", vbArchive) <> "" Then
                'pPictElement.ImportPictureFromFile("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG")
                pPictElement.ImportPictureFromFile(glo_pathServidor & "\datos\leyenda\logo_foliacion\logo_foliacion_v.jpg")
                'XMIN = 2.1
                'XMAX = 4.1
                'YMIN = 2.82
                'YMAX = 5.32

                XMIN = 2.1
                XMAX = 4.9
                YMIN = 2.82
                YMAX = 6.2

            End If

            'ElseIf caso_consulta = "CATASTRO MINERO" Then
            '    If Dir("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG", vbArchive) <> "" Then
            '        pPictElement.ImportPictureFromFile("U:\DATOS\LEYENDA\LOGO_FOLIACION\LOGO_FOLIACION_V.JPG")
            '        XMIN = 2.1
            '        XMAX = 4.1
            '        YMIN = 2.82
            '        YMAX = 5.32
            '    End If
        End If

        pElement = pPictElement
        vistaactiva = pMxDoc.PageLayout
        pEnv = vistaactiva.Extent

        pEnv.PutCoords(XMIN, YMIN, XMAX, YMAX)
        pElement.Geometry = pEnv
        Dim pGC As IGraphicsContainer
        pGC = vistaactiva
        pGC.AddElement(pElement, 0)
    End Sub

    Public Sub generaplanoventa(ByVal p_app As IApplication)
        Dim formulario2 As New Frm_formatoplanos
        Dim pRubberBand As IRubberBand
        pMap = pMxDoc.FocusMap
        Dim pActiveView As IActiveView
        pActiveView = pMap
        pRubberBand = New RubberEnvelope
        pActiveView.Extent = pRubberBand.TrackNew(pActiveView.ScreenDisplay, Nothing)
        pActiveView.Refresh()
        If pMap.LayerCount = 0 Then
            MsgBox("NO EXISTE TEMAS EN LA VISTA PARA GENERAR PLANO", vbCritical, "OBSERVACION...")
            Exit Sub
        End If
        formulario2.Show()
    End Sub
    Public Sub generaplano_Ate_publico(ByVal p_app As IApplication)
        Dim cls_planos As New Cls_planos
        Dim cls_Prueba As New cls_Prueba
        Dim cls_catastro As New cls_DM_1
        Dim escala_mu As Double
        caso_consulta = "CATASTRO MINERO"
        If pMap.Name <> "CATASTRO MINERO" Then
            cls_planos.buscaadataframe(caso_consulta, False)
            If valida = False Then
                pMap.Name = "CATASTRO MINERO"
                pMxDoc.UpdateContents()
            End If
            cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
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
        Dim generaplano As Integer
        Dim generaplano_situ As Integer
        Dim lo_Carta As String = ""
        generaplano = MsgBox("Seleccione : YES si desea generar Plano Catastral, seleccion No para generar otro plano", MsgBoxStyle.YesNoCancel, "Generaci�n de Planos...")

        If generaplano = 6 Then
            seleccion_plano_si = "Plano Catastral Publico"

            lo_Carta = cls_Prueba.Get_Unique_Values_FC("Catastro", "CARTA", p_app, "")
            If lo_Carta <> "" Then
                lo_Carta = Left(lo_Carta, 4)
            Else
                MsgBox("No existe Carta del Area de Interes para Generar Plano", MsgBoxStyle.Critical, "BDGEOCATMIN")
                Exit Sub
            End If
        ElseIf generaplano = 7 Then
            generaplano_situ = MsgBox("Seleccione : YES si desea generar de Situacion de DM", MsgBoxStyle.YesNoCancel, "Generaci�n de Planos...")
            If generaplano_situ = 6 Then
                seleccion_plano_si = "Plano Catastral Situacion DM"

                lo_Carta = cls_Prueba.Get_Unique_Values_FC("Situacion_DM", "CARTA", p_app, "")
                If lo_Carta <> "" Then
                    lo_Carta = Left(lo_Carta, 4)
                Else
                    MsgBox("No existe Carta del Area de Interes para Generar Plano", MsgBoxStyle.Critical, "BDGEOCATMIN")
                    Exit Sub
                End If
            Else
                seleccion_plano_si = ""
                MsgBox("No ha seleccionado ningun plano a escoger, por lo tanto se cancelara dicha opci�n", MsgBoxStyle.Critical, " SIGCATMIN")
                Exit Sub

            End If
        Else
            seleccion_plano_si = ""
            MsgBox("No ha seleccionado ningun plano a escoger, por lo tanto se cancelara dicha opci�n", MsgBoxStyle.Critical, " SIGCATMIN")
            Exit Sub
        End If

        'If lo_Carta <> "" Then
        '    lo_Carta = Left(lo_Carta, 4)
        'Else
        '    MsgBox("No existe Carta del Area de Interes para Generar Plano", MsgBoxStyle.Critical, "BDGEOCATMIN")
        '    Exit Sub
        'End If


        valor_codhoja = lo_Carta
        cls_planos.Obtiene_Datos_Hoja(valor_codhoja)
        'caso_consulta = "Plano Venta"
        pGC = pMxDoc.PageLayout
        pMapFrame1 = pGC.FindFrame(pMxDoc.FocusMap)
        pMapFrame1.MapScale = escalaf
        ''pMapFrame1.ExtentType = ESRI.ArcGIS.Carto.esriExtentTypeEnum.esriExtentScale
        pMxDoc.UpdateContents()
        If tipo_plano = "Plano para Atenci�n P�blico" Then
            caso_consulta = "Plano Venta"
            cls_planos.Generaplanoreporte("Plano Venta")
        ElseIf tipo_plano = "Planos Diversos" Then
            cls_planos.Generaplanoreporte("Plano_variado")
        End If

        cls_planos.creacionmedidasgrillas("CATASTRO MINERO")
        If tipo_plano = "Plano para Atenci�n P�blico" Then
            cls_planos.AgregarTextosLayout("Plano Venta", "")
            cls_planos.CambiaADataView(p_app)
            caso_consulta = "CATASTRO MINERO"
            cls_eval.ActivaDataframe_Opcion(caso_consulta, p_app)
            caso_consulta = "Plano Venta"
            cls_planos.asigna_escaladaplanolayout("Plano Venta", p_app)
            cls_planos.CambiaALayout(p_app)
            pMapFrame1.ExtentType = ESRI.ArcGIS.Carto.esriExtentTypeEnum.esriExtentDefault
            Dim pagina As IPageLayout
            pagina = pMxDoc.PageLayout
            pagina.ZoomToWidth()
            pagina.ZoomToWhole()
            'pMxDoc.UpdateContents()
        ElseIf tipo_plano = "Planos Diversos" Then

            cls_planos.AgregarTextosLayout("Plano_variado", "")
            existe_area = True

            If existe_area = True Then
                'Termino la parte de generacion de rectangulo area de interes
                cls_planos.CambiaADataView(p_app)
                'Agrega Mapa_ubicaci�n
                'cls_eval.Eliminadataframe()  'elimina dataframe

                cls_eval.adicionadataframe("MAPA_UBICACION")
                pMxDoc.UpdateContents()
                pMxDoc.ActiveView.Refresh()

                Dim capadepa As String

                If v_sistema = "PSAD-56" Then
                    capadepa = "Departamentos"
                Else
                    capadepa = "Departamentos_wgs"
                End If

                'cls_eval.AddLayerFromFile1(m_application, "Departamentos")
                cls_eval.AddLayerFromFile1(m_application, capadepa)

                pLayer = pMap.Layer(0)

                pLayer.Name = "Departamento"
                pMxDoc.ActiveView.Refresh()
                Dim lo_Filtro_Dpto As String = ""
                lo_Filtro_Dpto = cls_eval.f_Intercepta_temas("Departamento", v_dato1, v_dato2, v_dato3, v_dato4, m_application)
                Dim nm_depa As String = ""
                Dim nm_depa1 As String = ""
                If colecciones_depa.Count = 0 Then
                    lista_depa = ""
                End If
                For contador As Integer = 1 To colecciones_depa.Count
                    nm_depa = colecciones_depa.Item(contador)
                    If contador = 1 Then
                        lista_depa = nm_depa
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

                colecciones_depa.Clear()
                cls_eval.consultacapaDM("Departamento", "Departamento", "Departamento")
                pFeatureSelection = pLayer

                cls_catastro.Add_ShapeFile1("F_" & fecha_archi2, m_application, "Rectangulo")
                cls_catastro.Zoom_to_Layer("Rectangulo")



                ' pFeatureSelection.Clear()
                ' pdocument = m_application.Document
                pMap = pMxDoc.FocusMap
                escala_mu = pMap.MapScale
                '  cls_catastro.Shade_Poligono("Rectangulo", m_application)
                cls_catastro.Color_Poligono_Simple(m_application, "Rectangulo")



                escala_mu = escala_mu * 15
                pMap.MapScale = escala_mu
                pMxDoc.ActiveView.Refresh()

                'Hace zoom a lo seleccionado
                Dim pCmdItem As ICommandItem
                Dim pUID As New UID
                pUID.Value = "esriArcMapUI.ZoomToSelectedCommand"
                pCmdItem = p_app.Document.CommandBars.Find(pUID)
                pCmdItem.Execute()

                cls_eval.activadataframe("CATASTRO MINERO")
                pMxDoc.ActiveView.Refresh()


                pFeatureSelection.Clear()

                ''cls_planos.CambiaADataView(p_app)
                caso_consulta = "CATASTRO MINERO"
                cls_eval.ActivaDataframe_Opcion(caso_consulta, p_app)
                'caso_consulta = "Plano Venta"
                'cls_planos.asigna_escaladaplanolayout("Plano Venta", p_app)
                cls_planos.CambiaALayout(p_app)
                pMapFrame1.ExtentType = ESRI.ArcGIS.Carto.esriExtentTypeEnum.esriExtentDefault
                Dim pagina As IPageLayout
                pagina = pMxDoc.PageLayout
                pagina.ZoomToWidth()
                pagina.ZoomToWhole()
            Else
                'cls_catastro.Borra_Todo_Feature("", m_application)
                'cls_catastro.Limpiar_Texto_Pantalla(m_application)

                Exit Sub
            End If
        End If

    End Sub


    Public Sub Obtiene_Datos_Hoja(ByVal valor_codhoja As String)
        Dim pTable As ITable
        Dim pWorkspaceFactory1 As IWorkspaceFactory
        Dim pRow As IRow
        pWorkspaceFactory1 = New ShapefileWorkspaceFactory
        Dim pFWS As IFeatureWorkspace
        'pFWS = pWorkspaceFactory1.OpenFromFile("U:\DATOS\dbf", 0)
        pFWS = pWorkspaceFactory1.OpenFromFile(glo_pathServidor & "\datos\dbf", 0)
        pTable = pFWS.OpenTable("Hoja")
        Dim pCursor As ICursor
        'Dim pselection As ITableSelection
        'Dim sel As ISelectionSet

        'pselection = pTable

        Dim pfields3 As Fields
        pfields3 = pTable.Fields
        Dim pQueryFilter As IQueryFilter
        pfields3 = pTable.Fields
        pQueryFilter = New QueryFilter
        pQueryFilter.WhereClause = "CD_HOJA = '" & valor_codhoja & "'"
        pCursor = pTable.Search(pQueryFilter, True)
        pRow = pCursor.NextRow
        Do Until pRow Is Nothing
            'valor_codhoja = pRow.Value(pfields3.FindField("CD_HOJA"))
            'colecciones_hoj.Add(valor_codhoja)
            valor_nmhoja = pRow.Value(pfields3.FindField("NM_HOJA"))
            valor_zoncat = pRow.Value(pfields3.FindField("ZONCAT"))
            pRow = pCursor.NextRow
        Loop
    End Sub


    Public Sub genera_planocuadriculas(ByVal p_app As IApplication)
        'Dim cls_planos As New Cls_planos
        If ((tipo_seleccion = "OP_11") Or (tipo_seleccion = "OP_12")) Then
            buscaadataframe("CATASTRO MINERO", False)
            If valida = False Then
                Exit Sub
            End If
            s_tipo_plano = "Plano_cuadriculas"

            'Dim cls_planos As New Cls_planos
            Dim cls_catastro As New cls_DM_1
            asignaescaladataframe("CATASTRO MINERO")
            mueveposiciondataframe("CATASTRO MINERO", p_app)
            prendecapas()
            cls_catastro.Zoom_to_Layer("Catastro")
            asigna_escaladaplanolayout("CATASTRO MINERO", p_app)
            escalaf = escala_plano_eval
            Generaplanoreporte("Plano Cuadricula")
            creacionmedidasgrillas("CATASTRO MINERO")
            ' LEERESULTADOSEVAL()
            AgregarTextosLayout("Cuadricula", "Cuadricula")
            'GENERALISTADODM_PLANOEV()
            'agrega_logofoliacion_plano(p_app, "CATASTRO MINERO")
            caso_consulta = "CATASTRO MINERO"
            cls_eval.ActivaDataframe_Opcion(caso_consulta, m_application)
            asigna_escaladaplanolayout("CATASTRO MINERO", p_app)
            nombre_datos = ""
            CambiaALayout(p_app)
            s_tipo_plano = ""
            pMapFrame1.ExtentType = esriExtentTypeEnum.esriExtentDefault
            Dim pagina As IPageLayout
            pagina = pMxDoc.PageLayout
            pagina.ZoomToWhole()
        Else
            MsgBox("No genero la opci�n de Evaluaci�n de DM", MsgBoxStyle.Critical, "BDGEOCATMIN")
        End If


    End Sub

End Class
