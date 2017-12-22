
Spike motivated by DSL articles 
https://www.codeproject.com/Articles/39031/Making-DSLs-in-F

https://github.com/dungpa/dsls-in-action-fsharp/blob/master/DSLCheatsheet.md
http://dungpa.github.io/fsharp-cheatsheet/

http://imps.mcmaster.ca/pouyalarjani/fscodegen.pdf
https://blog.inf.ed.ac.uk/aplcourse/2009/02/heterogeneous-metaprogramming-fsharp/

https://tomasp.net/blog/fsharp-generic-numeric.aspx        

wow - agent based CQRS
http://apprize.info/programming/f_1/9.html

http://apprize.info/programming/f_1/index.html
http://funscript.info/

http://www.fssnip.net/qL/title/Dict-utilities-to-use-Dictionary-in-more-Fy-way

Nice example code
https://github.com/stuartjdavies/FSharp.Cloud.AWS/blob/master/FSharp.Cloud.AWS/DynamoDB.fs

RUST ORM
http://diesel.rs/guides/getting-started/

SCALA DSL
http://debasishg.blogspot.com/2008/04/external-dsls-made-easy-with-scala.html
https://github.com/deanwampler/SeductionsOfScalaTutorial/blob/master/tutorial-exercises/ex11-external-dsl.scala
https://hedleyproctor.com/2014/11/writing-a-dsl-with-scala-parser-combinators/
https://www.infoq.com/articles/External-DSL-Vaughn-Vernon

NICE ARTICLE
http://www.havelund.com/Publications/dsl-scala-2015.pdf


variable "var1.1" source "pardat_sale"

use var1 var2 var3 from table1
use varx from table2
exlude var3 var5 var6 from table2
use all from table3

over table7 name main_baths is main_bath_count card is 1

over table4 name total_rooms is sum of sfla

over table5 name total_stories is max of stories 

over this name sales_by_month_year as count unique salekey by month and year

name lot_to_improved as total_rooms / total_stories
