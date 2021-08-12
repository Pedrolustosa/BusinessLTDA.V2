import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-department',
  templateUrl: './department.component.html',
  styleUrls: ['./department.component.css']
})
export class DepartmentComponent implements OnInit {

  constructor(private http: HttpClient) { }
  departments: any = []

  modalTitle = "";
  DepartmentId = 0;
  DepartmentName = "";

  ngOnInit(): void {
    this.refreshList();
  }

  refreshList(): void {
    this.http.get<any>(environment.API_URL + 'departments')
      .subscribe(data => {
        this.departments = data;
      })
  }

  addClick(): void{
    this.modalTitle="Novo Departamento";
    this.DepartmentId = 0;
    this.DepartmentName = "";
  }

  editClick(dep: any){
    this.modalTitle="Editar Departamento";
    this.DepartmentId = dep.DepartmentId;
    this.DepartmentName = dep.DepartmentName;
  }

  createClick(){
    var val = {
      DepartmentName: this.DepartmentName
    };
    this.http.post(environment.API_URL+'departments', val)
    .subscribe(res => {
      alert(res.toString());
      this.refreshList();
    });
  }

  updateClick(){
    var val = {
      DepartmentId: this.DepartmentId,
      DepartmentName: this.DepartmentName
    };
    this.http.put(environment.API_URL+'departments', val)
    .subscribe(res => {
      alert(res.toString());
      this.refreshList();
    });
  }

  deleteClick(id: any){
    if(confirm('Você deseja mesmo excluir ?'))
    this.http.delete(environment.API_URL+'departments/'+id)
    .subscribe(res => {
      alert(res.toString());
      this.refreshList();
    });
  }

}
