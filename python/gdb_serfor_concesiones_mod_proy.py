# -*- coding: utf-8 -*-
# ---------------------------------------------------------------------------
# Script generado por César Egocheaga
# Versión Python: 2.7
# El feature class modificado de Concesiones de SERFOR, es proyectado en DATA_CAT y en U:
# ---------------------------------------------------------------------------

import arcpy
arcpy.env.overwriteOutput = True

#  Variables
# capa_origen = "Database Connections\\data_cat.sde\\DATA_CAT.DS_SERVICIOS_WFS_GCS_WGS84\\DATA_CAT.SERFOR_0802_CONCESIONES"
capa_copy = "Database Connections\\data_cat.sde\\DATA_CAT.DS_SERVICIOS_WFS_GCS_WGS84\\DATA_CAT.SERFOR_0802_CONCESIONES_MOD"

concesiones_shp_w17 = "U:/DATOS/SHAPE/WGS_84/SERVICIOSWFS/cata_forestal17.shp"
concesiones_shp_w18 = "U:/DATOS/SHAPE/WGS_84/SERVICIOSWFS/cata_forestal18.shp"
concesiones_shp_w19 = "U:/DATOS/SHAPE/WGS_84/SERVICIOSWFS/cata_forestal19.shp"
concesiones_shp_p17 = "U:/DATOS/SHAPE/PSAD_56/SERVICIOSWFS/cata_forestal17.shp"
concesiones_shp_p18 = "U:/DATOS/SHAPE/PSAD_56/SERVICIOSWFS/cata_forestal18.shp"
concesiones_shp_p19 = "U:/DATOS/SHAPE/PSAD_56/SERVICIOSWFS/cata_forestal19.shp"

concesiones_gdb_w17 = "Database Connections\\data_cat.sde\\DATA_CAT.DS_CATASTRO_MINERO_WGS84_17_T\\DATA_CAT.SERFOR_TMP_CONCESIONES_W17"
concesiones_gdb_w18 = "Database Connections\\data_cat.sde\\DATA_CAT.DS_CATASTRO_MINERO_WGS84_18_T\\DATA_CAT.SERFOR_TMP_CONCESIONES_W18"
concesiones_gdb_w19 = "Database Connections\\data_cat.sde\\DATA_CAT.DS_CATASTRO_MINERO_WGS84_19_T\\DATA_CAT.SERFOR_TMP_CONCESIONES_W19"
concesiones_gdb_p17 = "Database Connections\\data_cat.sde\\DATA_CAT.DS_CATASTRO_MINERO_PSAD56_17_T\\DATA_CAT.SERFOR_TMP_CONCESIONES_P17"
concesiones_gdb_p18 = "Database Connections\\data_cat.sde\\DATA_CAT.DS_CATASTRO_MINERO_PSAD56_18_T\\DATA_CAT.SERFOR_TMP_CONCESIONES_P18"
concesiones_gdb_p19 = "Database Connections\\data_cat.sde\\DATA_CAT.DS_CATASTRO_MINERO_PSAD56_19_T\\DATA_CAT.SERFOR_TMP_CONCESIONES_P19"

concesiones_gdb_prod_w17 = "Database Connections\\data_cat.sde\\DATA_GIS.DS_CATASTRO_MINERO_WGS84_17\\DATA_GIS.GPO_SERFOR_CONCESIONES_W17"
concesiones_gdb_prod_w18 = "Database Connections\\data_cat.sde\\DATA_GIS.DS_CATASTRO_MINERO_WGS84_18\\DATA_GIS.GPO_SERFOR_CONCESIONES_W18"
concesiones_gdb_prod_w19 = "Database Connections\\data_cat.sde\\DATA_GIS.DS_CATASTRO_MINERO_WGS84_19\\DATA_GIS.GPO_SERFOR_CONCESIONES_W19"
concesiones_gdb_prod_p17 = "Database Connections\\data_cat.sde\\DATA_GIS.DS_CATASTRO_MINERO_PSAD56_17\\DATA_GIS.GPO_SERFOR_CONCESIONES_P17"
concesiones_gdb_prod_p18 = "Database Connections\\data_cat.sde\\DATA_GIS.DS_CATASTRO_MINERO_PSAD56_18\\DATA_GIS.GPO_SERFOR_CONCESIONES_P18"
concesiones_gdb_prod_p19 = "Database Connections\\data_cat.sde\\DATA_GIS.DS_CATASTRO_MINERO_PSAD56_19\\DATA_GIS.GPO_SERFOR_CONCESIONES_P19"

epsg_w17 = '32717'
epsg_w18 = '32718'
epsg_w19 = '32719'
epsg_p17 = '24877'
epsg_p18 = '24878'
epsg_p19 = '24879'

def proy_layer(capa_in, capa_out, epsg):
    arcpy.Project_management(capa_in, capa_out, epsg)

def delete_row(capa):
    # Elimina todos los registros del feature class
    arcpy.DeleteFeatures_management(capa)

def append_layer(capa_in, capa_out):
    # Process: Append
    arcpy.Append_management([capa_in], capa_out, "NO_TEST", "", "")

if __name__ == '__main__':
    proy_layer(capa_copy, concesiones_shp_w17, epsg_w17)
    proy_layer(capa_copy, concesiones_shp_w18, epsg_w18)
    proy_layer(capa_copy, concesiones_shp_w19, epsg_w19)
    proy_layer(capa_copy, concesiones_shp_p17, epsg_p17)
    proy_layer(capa_copy, concesiones_shp_p18, epsg_p18)
    proy_layer(capa_copy, concesiones_shp_p19, epsg_p19)
    proy_layer(capa_copy, concesiones_gdb_w17, epsg_w17)
    proy_layer(capa_copy, concesiones_gdb_w18, epsg_w18)
    proy_layer(capa_copy, concesiones_gdb_w19, epsg_w19)
    proy_layer(capa_copy, concesiones_gdb_p17, epsg_p17)
    proy_layer(capa_copy, concesiones_gdb_p18, epsg_p18)
    proy_layer(capa_copy, concesiones_gdb_p19, epsg_p19)
    print("Proceso 2: Capa de Concesiones proyectados en DATA_CAT y U:")
    delete_row(concesiones_gdb_prod_p17)
    delete_row(concesiones_gdb_prod_p18)
    delete_row(concesiones_gdb_prod_p19)
    delete_row(concesiones_gdb_prod_w17)
    delete_row(concesiones_gdb_prod_w18)
    delete_row(concesiones_gdb_prod_w19)
    append_layer(concesiones_gdb_p17, concesiones_gdb_prod_p17)
    append_layer(concesiones_gdb_p18, concesiones_gdb_prod_p18)
    append_layer(concesiones_gdb_p19, concesiones_gdb_prod_p19)
    append_layer(concesiones_gdb_w17, concesiones_gdb_prod_w17)
    append_layer(concesiones_gdb_w18, concesiones_gdb_prod_w18)
    append_layer(concesiones_gdb_w19, concesiones_gdb_prod_w19)
    print("Proceso 2: Actualización de capa de Concesiones en DATA_GIS")
    print("Proceso completo")




