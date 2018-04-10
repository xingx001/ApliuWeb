<html>
<body>
<?php
$con = mysql_connect("221.231.6.226:3306","a0825111004","323440");
if (!$con)
  {
  die('Could not connect: ' . mysql_error());
  }

mysql_select_db("a0825111004", $con);

$result = mysql_query("SELECT * FROM message");

$tableone='<div class="job_right"><table border="1" height="200px" width="100%" rules="cols" bordercolor="#CCCCCC" cellpadding="0" cellspacing="0"><tr><td align="center" rowspan="2" width="100px">';
$tabletwo='</td><td align="left">';
$tablethree='</td></tr> <tr height="20px"><td align="right">';
$tablefour='</td></tr></table></div>';
#$tablebody=$tableone . "A" . $tabletwo . "B" . $tablethree . "C" . $tablefour;*/
$tablebody="";
while($row = mysql_fetch_array($result))
{
	$tablebody =$tablebody . $tableone . $row['username'] . $tabletwo . $row['message'] . $tablethree . $row['date'] . $tablefour;
}
#$tablebody = htmlspecialchars($tablebody, ENT_QUOTES);
# &lt;a href=&#039;test&#039;&gt;Test&lt;/a&gt;
# 如果需要展现<br>，那么浏览器解析HTML的时候会自动将他变为换行
# 但是通过htmlspecialchars就可以让< 变为 &#039;
echo $tablebody;
mysql_close($con);
?>
</body>
</html>