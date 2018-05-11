--produce var statements
--hacked from https://stackoverflow.com/questions/8739203/oracle-query-to-fetch-column-names
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
    atc.table_name = obj.object_name
ORDER BY 
    obj.object_name, 
    atc.column_name
    ) obj2
    ) obj3 WHERE object_name LIKE '%\_SALE';
	