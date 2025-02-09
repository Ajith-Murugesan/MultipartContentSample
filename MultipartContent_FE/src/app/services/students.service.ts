import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class StudentsService {

  private apiUrl = 'https://localhost:44381/api/Students/exportWithDto'; // Replace with your API endpoint

  constructor(private http: HttpClient) {}

  fetchStudentsWithExcel() {
    console.log('inside service call')
    return this.http.get(this.apiUrl, {
      responseType: 'blob', // Expect binary response (Excel + JSON)
    });
  }

  parseMultipartResponse(response: Blob): Promise<{ students: any[]; excelBlob: Blob | null }> {
    console.log('inside multiparse')
    return new Promise((resolve, reject) => {
      const reader = new FileReader();

      reader.onload = () => {
        const text = reader.result as string;
        const boundary = this.getBoundary(response.type);
        console.log('response  : ',response)
        console.log('boundary  : ',boundary)
        if (!boundary) {
          reject('Invalid boundary in response.');
          return;
        }

        const parts = text.split(`--${boundary}`).filter((part) => part.trim().length > 0 && part !== '--');

        // Parse JSON
        const jsonPart = parts.find((part) => part.includes('application/json'));
        const jsonData = jsonPart
          ? JSON.parse(jsonPart.split('\r\n\r\n')[1].trim())
          : [];
          console.log('jsonData  : ',jsonData)
        // Extract Excel data
        const excelPart = parts.find((part) =>
          part.includes('application/vnd.openxmlformats-officedocument.spreadsheetml.sheet')
        );
        const excelBlob = excelPart
          ? new Blob([excelPart.split('\r\n\r\n')[1].trim()], {
              type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            })
          : null;

        resolve({ students: jsonData, excelBlob });
      };

      reader.onerror = (err) => reject(err);

      reader.readAsText(response);
    });
  }

  private getBoundary(contentType: string): string | null {
    const match = contentType.match(/boundary=(.*)/);
    return 'boundary123';
  }

  downloadExcel(blob: Blob, filename: string): void {
    saveAs(blob, filename);
  }
}
