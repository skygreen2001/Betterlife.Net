初始化数据库准备
**********************在PHP:betterlife里****************************
betterlife地址:https://github.com/skygreen2001/betterlife
1.之前是在MysqlWorkbench里进行数据库原型的设计，然后导入Mysql数据库
  然后生成从Mysql到遵循Sqlserver数据库表设计规范的创建数据库脚本
		http://127.0.0.1/betterlife/tools/tools/db/sqlserver/db_sqlserver_convert_prepare.php
2.在BetterlifeNet里运行生成sql脚本后，生成了该Init_Db\Mysql\BetterlifeNet.sql

**********************在Net:Betterlife.Net里*************************
Betterlife.Net地址:https://github.com/skygreen2001/betterlife.net
1.先确保整个解决方案正常编译完成.
2.运行Betterlife.Net\Common\Tools\工程,在弹出窗口点击选择按钮:显示数据库信息.
  工具箱-〉显示数据库信息-〉{移植数据库脚本[Mysql-〉SQLServer]}
  可生成所有生成BetterlifeNet数据库的脚本[基于T-SQL]；
3.选择数据库类型:Mysql,数据库名称选择:BetterlifeNet,点击选择按钮:
  移植数据库脚本[从Mysql->Sqlserver],生成创建Sqlserver数据库脚本
4.利用SSMS创建数据库BetterlifeNet,运行上一步生成的sql脚本