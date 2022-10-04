Imports PORTAL_Clases
Imports System.IO
Imports System.Collections
Imports ESRI.ArcGIS.Framework
Imports System.Drawing
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.DataSourcesOleDB
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Public Class Frm_formatoplanos
    Public p_App As IApplication
    Private Sub Frm_formatoplanos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If s_tipo_plano = "Plano_areasuperpuesta" Or s_tipo_plano = "Plano Acumulacion" Or s_tipo_plano = "Plano DMxArea_Restringida" Then


            ' Me.lstformatoplanos.Items.Add("Plano Area superpuesta N�" & conta_hoja_sup)
            ' MsgBox("1")
        Else
            If tipo_plano = "" Then
                Me.lstformatoplanos.Items.Add("Plano para Atenci�n P�blico")
                Me.lstformatoplanos.Items.Add("Planos Diversos")
            End If

            If tipo_plano = "Planos Diversos" Then
                Me.btnGraficar.Enabled = False
                Me.lstformatoplanos.Items.Add("Plano A4 Horizontal")
                Me.lstformatoplanos.Items.Add("Plano A4 Vertical")
                Me.lstformatoplanos.Items.Add("Plano A3 Horizontal")
                Me.lstformatoplanos.Items.Add("Plano A3 Vertical")
                Me.lstformatoplanos.Items.Add("Plano A2 Horizontal")
                Me.lstformatoplanos.Items.Add("Plano A2 Vertical")
                Me.lstformatoplanos.Items.Add("Plano A1 Horizontal")
                Me.lstformatoplanos.Items.Add("Plano A1 Vertical")
                Me.lstformatoplanos.Items.Add("Plano A0 Horizontal")
                Me.lstformatoplanos.Items.Add("Plano A0 Vertical")
            ElseIf tipo_plano = "Plano para Atenci�n P�blico" Then
                Me.btnGraficar.Enabled = False
                Me.lstformatoplanos.Items.Add("Formato A4")
                Me.lstformatoplanos.Items.Add("Formato A3")
                Me.lstformatoplanos.Items.Add("Formato A2")
                Me.lstformatoplanos.Items.Add("Formato A0")
            End If
        End If
    End Sub

    Private Sub btnGraficar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraficar.Click
        ' 010226410
        'Dim listaescala As New Frm_listadoescalaplanos
        'Dim sele_plano1 As String
        Dim cls_planos As New Cls_planos
        Dim pForm1 As New Frm_listadoescalaplanos
        pForm1.p_app = p_App
        pForm1.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
        If s_tipo_plano = "Plano_areasuperpuesta" Then  'para plano areas superpuestas

            If colecciones_planos.Count = 0 Then
                pForm1.Close()
            Else
                sele_plano_sup = Me.lstformatoplanos.Text
                corta_nplano = Mid(sele_plano_sup, InStr(sele_plano_sup, "�") + 1)
                'conta_hoja_sup = 1
                conta_hoja_sup = corta_nplano
                conta_reg = 0

                cls_planos.Generaplanoreporte("Plano Area superpuesta")

                'Ingresando escala del Plano
                Dim mensaje_escala As String

                mensaje_escala = InputBox("Ingrese escala del Plano...", "Plano de Areas Superpuestas", "100000")
                If mensaje_escala = "" Then
                    mensaje_escala = "100000" 'escala por defecto
                    escalaf = mensaje_escala
                Else
                    escalaf = mensaje_escala
                End If

                cls_planos.creacionmedidasgrillas("CATASTRO MINERO")

                cls_planos.agrega_logofoliacion_plano(m_application, "Plano Area superpuesta")
                cls_planos.INSERTAAREASUPERPUESTA_PLANO()
            End If
            Me.Close()
            v_calculAreaint = True

        ElseIf s_tipo_plano = "Plano DMxArea_Restringida" Then
            'Solo para planos de Dm vs areas restringidas

            If colecciones_planos.Count = 0 Then
                pForm1.Close()
            Else
                sele_plano_sup = Me.lstformatoplanos.Text
                corta_nplano = Mid(sele_plano_sup, InStr(sele_plano_sup, "�") + 1)
                'conta_hoja_sup = 1
                conta_hoja_sup = corta_nplano
                conta_reg = 0

                cls_planos.Generaplanoreporte("Plano Area superpuesta")

                'Ingresando escala del Plano
                Dim mensaje_escala As String
                If s_tipo_plano = "Plano DMxArea_Restringida" Then
                    mensaje_escala = "100000" 'escala por defecto
                    escalaf = mensaje_escala
                Else
                    mensaje_escala = InputBox("Ingrese escala del Plano...", "Plano de Areas Superpuestas", "100000")
                    If mensaje_escala = "" Then
                        mensaje_escala = "100000" 'escala por defecto
                        escalaf = mensaje_escala
                    Else
                        escalaf = mensaje_escala
                    End If
                End If


                cls_planos.creacionmedidasgrillas("CATASTRO MINERO")

                cls_planos.agrega_logofoliacion_plano(m_application, "Plano Area superpuesta")
                cls_planos.INSERTAAREASUPERPUESTA_PLANO()
            End If
            Me.Close()
            v_calculAreaint = True

        ElseIf s_tipo_plano = "Plano Acumulacion" Then
            If colecciones_planos.Count = 0 Then
                pForm1.Close()
            Else
                sele_plano_sup = Me.lstformatoplanos.Text
                corta_nplano = Mid(sele_plano_sup, InStr(sele_plano_sup, "�") + 1)
                'conta_hoja_sup = 1
                conta_hoja_sup = corta_nplano
                conta_reg = 0

                cls_planos.Generaplanoreporte("Plano Acumulacion")
                escalaf = 100000
            End If

            cls_planos.creacionmedidasgrillas("CATASTRO MINERO")
            cls_planos.agrega_logofoliacion_plano(m_application, "Plano Acumulacion")

            cls_planos.INSERTAAREASUPERPUESTA_PLANO()

            Me.Close()


        Else
            sele_plano1 = Me.lstformatoplanos.Text
            Me.Close()

            sele_plano2 = Mid(sele_plano1, 7, 2)

            If tipo_sel_escala <> "SI" Then

                tipo_plano = Me.lstformatoplanos.Text
                pForm1.Close()
                Me.Close()
                Dim pForm As New Frm_formatoplanos
                If tipo_plano = "Plano para Atenci�n P�blico" Then
                    tipo_sel_escala = "SI"
                    pForm.p_App = p_App
                    pForm.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                    pForm.ShowDialog()
                ElseIf tipo_plano = "Planos Diversos" Then
                    tipo_sel_escala = "SI"
                    pForm.p_App = p_App
                    pForm.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                    pForm.ShowDialog()
                End If
            End If


            'End If
            'Me.Close()
            If tipo_sel_escala = "SI" Then
                sele_plano1 = Me.lstformatoplanos.Text
                Me.Close()
                'pForm1.p_app = p_App
                'pForm1.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                sele_plano2 = Mid(sele_plano1, 7, 2)
                'If sele_plano1 = "Formato A4" Then
                If sele_plano1 = "Plano A4 Horizontal" Or sele_plano1 = "Plano A4 Vertical" Or sele_plano1 = "Formato A4" Then

                    pForm1.lstescala.Items.Add("2x2 Km - (1/14000)")
                    pForm1.lstescala.Items.Add("3x3 Km - (1/20000)")
                    pForm1.lstescala.Items.Add("4x4 Km - (1/25000)")
                    pForm1.lstescala.Items.Add("6x6 Km - (1/40000)")
                    pForm1.lstescala.Items.Add("8x8 Km - (1/50000)")
                    pForm1.lstescala.Items.Add("10x10 Km - (1/60000)")
                    pForm1.lstescala.Items.Add("12x12 Km - (1/75000)")
                    pForm1.lstescala.Items.Add("16x16 Km - (1/100000)")
                ElseIf sele_plano1 = "Plano A3 Horizontal" Or sele_plano1 = "Plano A3 Vertical" Or sele_plano1 = "Formato A3" Then
                    'ElseIf sele_plano1 = "Formato A3" Then
                    pForm1.lstescala.Items.Add("32x32 Km - (1/100000)")
                    pForm1.lstescala.Items.Add("16x16 Km - (1/50000)")
                    'ElseIf sele_plano1 = "Formato A2" Then
                ElseIf sele_plano1 = "Plano A2 Horizontal" Or sele_plano1 = "Plano A2 Vertical" Or sele_plano1 = "Formato A2" Then
                    pForm1.lstescala.Items.Add("32x32 Km - (1/100000)")
                    pForm1.lstescala.Items.Add("16x16 Km - (1/50000)")
                ElseIf sele_plano1 = "Plano A1 Horizontal" Or sele_plano1 = "Plano A1 Vertical" Then
                    pForm1.lstescala.Items.Add("32x32 Km - (1/100000)")
                    pForm1.lstescala.Items.Add("16x16 Km - (1/50000)")
                ElseIf sele_plano1 = "Plano A0 Horizontal" Or sele_plano1 = "Plano A0 Vertical" Or sele_plano1 = "Formato A0" Then
                    pForm1.lstescala.Items.Add("6x6 Km - (1/10000)")
                    pForm1.lstescala.Items.Add("10x10 Km - (1/20000)")
                    pForm1.lstescala.Items.Add("12x12 Km - (1/25000)")
                    pForm1.lstescala.Items.Add("25x25 Km - (1/50000)")
                    pForm1.lstescala.Items.Add("40x40 Km - (1/75000)")
                    pForm1.lstescala.Items.Add("56x56 Km - (1/100000)")
                End If
                sele_plano = sele_plano1
                pForm1.ShowDialog()
            End If
        End If
    End Sub

    Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

    Private Sub lstformatoplanos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstformatoplanos.SelectedIndexChanged
        If Me.lstformatoplanos.TopIndex <> -1 Then
            Me.btnGraficar.Enabled = True
        End If
    End Sub

    Private Sub btnGraficar_DockChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGraficar.DockChanged

    End Sub
End Class