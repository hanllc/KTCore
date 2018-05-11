--produce var statements
--remove typically unneeded fields
--remove some extra tables
set pagesize 10000;
SELECT concat(a,b) FROM 
(SELECT CONCAT('variable "', column_name) a, concat('" source "', concat(object_name,'"')) b, object_name from (SELECT 
    obj.object_name, 
    atc.column_name, 
    atc.data_type, 
    atc.data_length 
FROM 
    all_tab_columns atc,
    (SELECT 
        * 
     FROM 
         all_objects
     WHERE 
        --object_name like 'JOE_JE%'
        --AND 
		owner = 'JOE'
        AND object_type in ('TABLE')   
    ) obj
WHERE 
    atc.table_name = obj.object_name AND
	  column_name NOT IN ('WEN', 'WHO', 'WHOCALC', 'WENCALC', 'TRANS_ID', 'SEQ', 'UPD_STATUS', 'IASW_ID', 'DEACTIVAT', 'CUR') --PARID SALEKEY TAXYR JUR
    AND
    object_name NOT IN ('MKSUBDAT_SALE', 'MKSUBVAL_SALE', 'SUBJCOMP_SALE') 
ORDER BY 
    obj.object_name, 
    atc.column_name
    ) obj2
    ) obj3 WHERE object_name LIKE '%\_SALE' ESCAPE '\';
	