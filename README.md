private Task<HttpResponseMessage> WriteToExcelFolder()
{
	HttpResponseMessage response = null;
	string fileName = @"DM Reporting " + DateTime.Now.ToString("MMddyyyyhhmm");
	List<Student> students = GetStudents();

	using (XLWorkbook wb = new XLWorkbook())
	{
		var wsheet = wb.Worksheets.Add("Student sheet 1");
		var wsheet2 = wb.Worksheets.Add("Student sheet 2");
		var currentRow = 1;
		var currentFirstColumn = 0;
		var currentSecondColumn = string.Empty;

		// define header
		wsheet.Cell(currentRow, 1).Value = "id";
		DecorateHeader(wsheet, currentRow, 1);
		DecorateHeader(wsheet, currentRow, 2);
		DecorateHeader(wsheet, currentRow, 3);
		DecorateHeader(wsheet, currentRow, 4);

		wsheet.Cell(currentRow, 2).Value = "Name";
		wsheet.Cell(currentRow, 3).Value = "first Name";
		wsheet.Cell(currentRow, 4).Value = "Last Name";

		wsheet2.Cell(currentRow, 1).Value = "id 2";
		wsheet2.Cell(currentRow, 2).Value = "Name 2";
		wsheet2.Cell(currentRow, 3).Value = "first Name 2";
		wsheet2.Cell(currentRow, 4).Value = "Last Name 2";

		//body

		foreach (var student in students)
		{
			currentRow++;

			if (currentFirstColumn != student.Id)
				wsheet.Cell(currentRow, 1).Value = student.Id;
			else
				wsheet.Cell(currentRow, 1).Value = string.Empty;

			if (currentFirstColumn != student.Id)
				wsheet.Cell(currentRow, 2).Value = student.Name;
			else
				wsheet.Cell(currentRow, 2).Value = string.Empty;

			wsheet.Cell(currentRow, 3).Value = student.FirstName;
			wsheet.Cell(currentRow, 4).Value = student.LastName;

			wsheet2.Cell(currentRow, 1).Value = student.Id;
			wsheet2.Cell(currentRow, 2).Value = student.Name;
			wsheet2.Cell(currentRow, 3).Value = student.FirstName;
			wsheet2.Cell(currentRow, 4).Value = student.LastName;
			currentFirstColumn = student.Id;
			currentSecondColumn = student.Name;
		}

		// save to folder
		wb.SaveAs(GetPathFolder(fileName));
	}

	return Task.FromResult(response);
}

private void DecorateHeader(IXLWorksheet wsheet, int currentRow, int currentColumn)
{
	wsheet.Cell(currentRow, currentColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
	wsheet.Cell(currentRow, currentColumn).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
	wsheet.Cell(currentRow, currentColumn).Style.Font.Bold = true;
	wsheet.Cell(currentRow, currentColumn).Style.Fill.SetBackgroundColor(XLColor.Red);
}

private string GetPathFolder(string file)
{
	var folderPath = @"G:\\DMReporting";

	var fileName = file + ".xlsx";

	var pathFolder = Path.Combine(
				folderPath,
				fileName);
	if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
	return pathFolder;
}

private List<Student> GetStudents()
{
	List<Student> students = new List<Student>();
	for (var i = 0; i < 10; i++)
	{
		if (i < 3)
			students.Add(new Student()
			{
				Id = 100,
				Name = "Johan 100",
				FirstName = "Cruyff " + i + i,
				LastName = "Teo " + i
			});
		else if (i > 2 && i < 7)
			students.Add(new Student()
			{
				Id = 200,
				Name = "Johan 200",
				FirstName = "Cruyff " + i + i,
				LastName = "Teo " + i
			});
		else
			students.Add(new Student()
			{
				Id = 300,
				Name = "Johan 300",
				FirstName = "Cruyff " + i + i,
				LastName = "Teo " + i
			});
	}
	return students;
}