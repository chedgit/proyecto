# -*- coding: utf-8 -*-
# ---------------------------------------------------------------------------
# Script generado por César Egocheaga
# Versión Python: 2.7
# Genera una copia del feature class generado del servicio wfs de SERFOR
# y procede a realizar modificaciones y actualizaciones a la copia
# como es eliminar campos, agregar campos, actualizar los valores de los campos en
# función a criterios definidos por el usuario
# ---------------------------------------------------------------------------

import arcpy
arcpy.env.overwriteOutput = True

#  Variables
capa_origen = "Database Connections\\data_cat.sde\\DATA_CAT.DS_SERVICIOS_WFS_GCS_WGS84\\DATA_CAT.SERFOR_0802_CONCESIONES"
# capa_origen = "Database Connections\\data_cat.sde\\DATA_CAT.DS_SERVICIOS_WFS_GCS_WGS84\\DATA_CAT.SERFOR_0802_CONCESIONES_VISOR"
capa_copy = "Database Connections\\data_cat.sde\\DATA_CAT.DS_SERVICIOS_WFS_GCS_WGS84\\DATA_CAT.SERFOR_0802_CONCESIONES_MOD"
capa_are = "Database Connections\\data_cat.sde\\DATA_CAT.DS_SERVICIOS_WFS_GCS_WGS84\\DATA_CAT.SERFOR_0802_CONCESIONES_ARE"

# def delete_layer(capa):
#     # Elimina el feature class
#     arcpy.Delete_management(capa, "FeatureClass")
#
# def copy_layer(capa_in, capa_out):
#     # Process: Copy Features
#     arcpy.CopyFeatures_management(capa_in, capa_out, "", "0", "0", "0")
#
# def delete_field():
#     # Process: DeleteField
#     arcpy.DeleteField_management(capa_copy, ["ORIGEN", "ESPECIE", "CONCUR", "PROTOC", "SUPAPR", "PERIME", "VOLAPR", "FINALI", "SITUAC", "ESTMOD", "SUPSUS", "NROTUR", "PAGODA", "TIPAGO"])
#
# def add_field():
#     # Variables
#     capa_copy_2 = capa_copy
#     capa_copy_3 = capa_copy_2
#     capa_copy_4 = capa_copy_3
#     capa_copy_5 = capa_copy_4
#     capa_copy_6 = capa_copy_5
#     capa_copy_7 = capa_copy_6
#
#     # Process: Add Field
#     arcpy.AddField_management(capa_copy, "TP_CONCE", "TEXT", "", "", "50", "", "NULLABLE", "NON_REQUIRED", "")
#     arcpy.AddField_management(capa_copy_2, "ESTCON_DES", "TEXT", "", "", "30", "", "NULLABLE", "NON_REQUIRED", "")
#     arcpy.AddField_management(capa_copy_3, "ESTOSI_DES", "TEXT", "", "", "30", "", "NULLABLE", "NON_REQUIRED", "")
#     arcpy.AddField_management(capa_copy_4, "CD_CONCE", "TEXT", "", "", "70", "", "NULLABLE", "NON_REQUIRED", "")
#     arcpy.AddField_management(capa_copy_5, "TP_INICIAL", "TEXT", "", "", "8", "", "NULLABLE", "NON_REQUIRED", "")
#     arcpy.AddField_management(capa_copy_6, "CD_CONCE1", "TEXT", "", "", "10", "", "NULLABLE", "NON_REQUIRED", "")

def delete_row(capa):
    # Elimina todos los registros del feature class
    arcpy.DeleteFeatures_management(capa)

def append_layer(capa_in, capa_out):
    # Process: Append
    arcpy.Append_management([capa_in], capa_out, "NO_TEST", "", "")

def upd_field(capa):
    with arcpy.da.UpdateCursor(capa, ['TIPCON', 'TP_CONCE', 'ESTCON', 'ESTCON_DES', 'ESTOSI', 'ESTOSI_DES', 'CD_CONCE', 'OBJECTID', 'NROPAR']) as cursor:

        for row in cursor:
            # row[6] = str(row[7]) + "_" + str(row[1])
            # row[6] = str(row[1])[0:2]  ERROR
            # cursor.updateRow(row)

            if row[0] == 80201:
                row[1] = "CONSERVACIÓN"
                cursor.updateRow(row)
            elif row[0] == 80202:
                row[1] = "ECOTURISMO"
                cursor.updateRow(row)
            elif row[0] == 80203:
                row[1] = "PRODUCTOS FORESTALES DIFERENTES A LA MADERA"
                cursor.updateRow(row)
            elif row[0] == 80204:
                row[1] = "FINES MADERABLES"
                cursor.updateRow(row)
            elif row[0] == 80205:
                row[1] = "PLANTACIONES FORESTALES"
                cursor.updateRow(row)
            elif row[0] == 80206:
                row[1] = "FORESTACION Y/O REFORESTACIÓN"
                cursor.updateRow(row)

            if row[0] == 80201:
                row[8] = "C"
                cursor.updateRow(row)
            elif row[0] == 80202:
                row[8] = "E"
                cursor.updateRow(row)
            elif row[0] == 80203:
                row[8] = "PFDM"
                cursor.updateRow(row)
            elif row[0] == 80204:
                row[8] = "FM"
                cursor.updateRow(row)
            elif row[0] == 80205:
                row[8] = "PF"
                cursor.updateRow(row)
            elif row[0] == 80206:
                row[8] = "FR"
                cursor.updateRow(row)

            if row[2] == 1:
                row[3] = "VIGENTE"
                cursor.updateRow(row)
            elif row[2] == 2:
                row[3] = "NO VIGENTE"
                cursor.updateRow(row)
            elif row[2] == 3:
                row[3] = "SUSPENDIDO"
                cursor.updateRow(row)
            elif row[2] == 4:
                row[3] = "EXTINGUIDO"
                cursor.updateRow(row)

            if row[4] == 0:
                row[5] = "NO APLICA"
                cursor.updateRow(row)
            elif row[4] == 1:
                row[5] = "CADUCADO"
                cursor.updateRow(row)
            elif row[4] == 2:
                row[5] = "MEDIDA CAUTELAR"
                cursor.updateRow(row)
            elif row[4] == 3:
                row[5] = "MEDIDA PROVISORIA"
                cursor.updateRow(row)

            row[6] = str(row[7]) + "_" + str(row[8])
            cursor.updateRow(row)

if __name__ == '__main__':
    delete_row(capa_copy)
    delete_row(capa_are)
    append_layer(capa_origen, capa_copy)
    upd_field(capa_copy)
    append_layer(capa_copy, capa_are)
    print("Proceso 1: Capa de Concesiones Modificado en DATA_CAT")



