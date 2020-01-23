/* Get Text for website tabs */
SELECT DISTINCT substr(name, 1, 3) || ' ' || substr(year, 3, 4) FROM Semester INNER JOIN SemesterName ON SemesterName.semesterNameID = Semester.nameFK ORDER BY year DESC, nameFK DESC;

/* Get class schedule */
SELECT subject || catalog, title, credit, User.last,
CASE days & 1 WHEN 0 THEN '' ELSE 'U' END ||
CASE days & 2 WHEN 0 THEN '' ELSE 'M' END ||
CASE days & 4 WHEN 0 THEN '' ELSE 'T' END ||
CASE days & 8 WHEN 0 THEN '' ELSE 'W' END ||
CASE days & 16 WHEN 0 THEN '' ELSE 'R' END ||
CASE days & 32 WHEN 0 THEN '' ELSE 'F' END ||
CASE days & 64 WHEN 0 THEN '' ELSE 'S ' END ||
CASE WHEN (strftime('%H', startTime) - 12) = -12 THEN '12' || strftime('%M', startTime) 
WHEN (strftime('%H', startTime) - 12) = 0 THEN '12' || strftime('%M', startTime)
WHEN (strftime('%H', startTime) - 12) < 0 THEN cast(strftime('%H', startTime) as integer) || strftime('%M', startTime)
ELSE (cast(strftime('%H', startTime) as integer) - 12) || strftime('%M', startTime) END || '-' ||
CASE WHEN (strftime('%H', startTime) - 12) = -12 THEN '12' || strftime('%M', startTime) ||'AM'
WHEN (strftime('%H', startTime) - 12) = 0 THEN '12' || strftime('%M', startTime) ||'PM'
WHEN (strftime('%H', startTime) - 12) < 0 THEN cast(strftime('%H', startTime) as integer) || strftime('%M', startTime) ||'AM'
ELSE (cast(strftime('%H', startTime) as integer) - 12) || strftime('%M', startTime) ||'PM' END AS times,
r1.building || ' ' || r1.wing || r1.roomNumber || r1.subRoom ||
CASE r2.building = 0 WHEN 0 THEN  "/" || r2.building || ' ' || r2.wing || r2.roomNumber || r2.subRoom ELSE '' END AS room,
Semester.year, Semester.nameFK, SemesterName.name
FROM Schedule
JOIN Course ON courseFK = Course.courseID 
JOIN User ON instructor1FK = User.userID
JOIN Room AS r1 ON room1FK = r1.roomID
JOIN Room AS r2 ON room2FK = r2.roomID
JOIN Semester ON semesterID = Semester.semesterID
JOIN SemesterName ON Semester.nameFK = SemesterName.semesterNameID
ORDER BY year, nameFK;