初始化数据库准备
1.
之前是在MysqlWorkbench里进行数据库原型的设计，然后导入Mysql数据库
然后生成从Mysql到遵循Sqlserver数据库表设计规范的创建数据库脚本
	http://localhost/betterlife/tools/tools/db/db_sqlserver_convert_prepare.php
在Mysql中运行该数据库脚本后，生成了该Init_Db\Mysql\BetterlifeNet.sql

2.然后运行Betterlife.Net中的Tools项目中的
  工具箱-〉显示数据库信息-〉{移植数据库脚本[Mysql-〉SQLServer]}
  可生成所有生成BetterlifeNet数据库的脚本[基于T-SQL]；
  需要注意要将脚本中的外键引用：[Sender]和[Receiver]修改成[User]即可全部正常运行
  生成了该Init_Db\Sqlserver\BetterlifeNet.bak数据库备份文件