Imports System.Runtime.InteropServices
Imports System.Drawing
Imports ESRI.ArcGIS.ADF.BaseClasses
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.ArcMapUI

<ComClass(btn_GeneraPlanoCuadricula.ClassId, btn_GeneraPlanoCuadricula.InterfaceId, btn_GeneraPlanoCuadricula.EventsId), _
 ProgId("GEOCATMIN.btn_GeneraPlanoCuadricula")> _
Public NotInheritable Class btn_GeneraPlanoCuadricula
    Inherits BaseCommand

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "b2bdb623-401a-44f8-aadb-02f1f6decc81"
    Public Const InterfaceId As String = "b0d3af3d-03d8-4bf4-8e76-f7e1c3ef7ba9"
    Public Const EventsId As String = "906d9cb3-9fe7-42f8-b513-f9dfdff0cff5"
#End Region

#Region "COM Registration Function(s)"
    <ComRegisterFunction(), ComVisibleAttribute(False)> _
    Public Shared Sub RegisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryRegistration(registerType)

        'Add any COM registration code after the ArcGISCategoryRegistration() call

    End Sub

    <ComUnregisterFunction(), ComVisibleAttribute(False)> _
    Public Shared Sub UnregisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryUnregistration(registerType)

        'Add any COM unregistration code after the ArcGISCategoryUnregistration() call

    End Sub

#Region "ArcGIS Component Category Registrar generated code"
    Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommands.Register(regKey)

    End Sub
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommands.Unregister(regKey)

    End Sub

#End Region
#End Region


    Private m_application As IApplication

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()

        ' TODO: Define values for the public properties
        MyBase.m_category = "Plano Cuadriculas"  'localizable text 
        MyBase.m_caption = "Plano Cuadriculas"   'localizable text 
        MyBase.m_message = "Plano Cuadriculas"   'localizable text 
        MyBase.m_toolTip = "Plano Cuadriculas" 'localizable text 
        MyBase.m_name = "Plano Cuadriculas"  'unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

        Try
            'TODO: change bitmap name if necessary
            Dim bitmapResourceName As String = Me.GetType().Name + ".bmp"
            MyBase.m_bitmap = New Bitmap(Me.GetType(), bitmapResourceName)
        Catch ex As Exception
            System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap")
        End Try


    End Sub


    Public Overrides Sub OnCreate(ByVal hook As Object)
        If Not hook Is Nothing Then
            m_application = CType(hook, IApplication)

            'Disable if it is not ArcMap
            If TypeOf hook Is IMxApplication Then
                MyBase.m_enabled = True
            Else
                MyBase.m_enabled = False
            End If
        End If

        ' TODO:  Add other initialization code
    End Sub

    Public Overrides Sub OnClick()
        'TODO: Add btn_GeneraPlanoCuadricula.OnClick implementation
        sele_opcion_cuadri = False

        Dim cls_planos As New Cls_planos
        caso_consulta = "CATASTRO MINERO"
        Dim pForm As New frm_Generar_Malla_100 '  frm_Generar_Circulo
        pForm.m_Application = m_application
        pForm.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
        pForm.Show()
        SetWindowLong(pForm.Handle.ToInt32(), GWL_HWNDPARENT, m_application.hWnd)

    End Sub
End Class



