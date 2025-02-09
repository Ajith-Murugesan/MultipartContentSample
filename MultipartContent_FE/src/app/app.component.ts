import { Component } from '@angular/core';
import { StudentsService } from './services/students.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  students: any[] = [];
name:string ='sol'
  constructor(private studentsService: StudentsService) {}
  hii(){
console.log('hiiii')
  }
  fetchAndDownload(): void {
    console.log('inside')
    this.studentsService.fetchStudentsWithExcel().subscribe(
      async (response) => {
        try {
          const { students, excelBlob } = await this.studentsService.parseMultipartResponse(response);
console.log('students  : ',students)
console.log('excelBlob  : ',excelBlob)
          // Update the student list
          this.students = students;

          // Download the Excel file
          if (excelBlob) {
            this.studentsService.downloadExcel(excelBlob, 'Students.xlsx');
          }
        } catch (error) {
          console.error('Error processing response:', error);
        }
      },
      (error) => {
        console.error('API call error:', error);
      }
    );
  }
}
