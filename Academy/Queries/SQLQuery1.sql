--USE VPD_311_Import;
--GO

--SELECT
--		  direction_name,
--		  COUNT(DISTINCT group_id) AS N'���������� �����',
--		  COUNT(DISTINCT stud_id) AS N'���������� ���������'
--FROM      Groups, Directions, Students
--WHERE     direction = direction_id
--AND		 [group]    = group_id
--GROUP BY  direction_name;

USE VPD_311_Import;
GO

SELECT
		  direction_name,
		  COUNT(DISTINCT group_id) AS N'���������� �����',
		  COUNT(DISTINCT stud_id) AS N'���������� ���������'
FROM      Students
JOIN      Groups     ON ([group] = group_id)
RIGHT JOIN      Directions ON (direction = direction_id)
WHERE     direction = direction_id

GROUP BY  direction_name;
;
 --INNER JOIN(Defoult);
 --OUTER JOIN
 --LEFT JOIN; ����� ��������� �������� (���������) �������, �.�. ��, ������� ����� ������ �� ������ �������.
 --RIGHT JOIN; ������ ��������� ������������ (�����������) �������, �.�. ��, ������� ������������� ������.
 --FULL JOIN
 --