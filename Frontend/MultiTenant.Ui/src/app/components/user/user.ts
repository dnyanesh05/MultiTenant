import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user.html',
  styleUrls: ['./user.css']
  //template: `
    //<h2>User Management</h2>
    //<p><strong>Tenant ID:</strong> {{ tenantId }}</p>

    //<!-- Add User -->
    //<form (ngSubmit)="addUser()" #addForm="ngForm">
    //  <input [(ngModel)]="newUser.name" name="name" required placeholder="Full Name" />
    //  <input [(ngModel)]="newUser.username" name="username" required placeholder="Username" />
    //  <input [(ngModel)]="newUser.email" name="email" required placeholder="Email" />
    //  <button type="submit" [disabled]="addForm.invalid">Add</button>
    //</form>

    //<br />
    //<h3>Users List</h3>
    //<table border="1" cellpadding="5">
    //  <thead>
    //    <tr>
    //      <th>Name</th>
    //      <th>Username</th>
    //      <th>Email</th>
    //      <th>Actions</th>
    //    </tr>
    //  </thead>
    //  <tbody>
    //    <tr *ngFor="let user of users">
    //      <ng-container *ngIf="editingId !== user.id; else editRow">
    //        <td>{{ user.name }}</td>
    //        <td>{{ user.username }}</td>
    //        <td>{{ user.email }}</td>
    //        <td>
    //          <button (click)="startEdit(user)">Edit</button>
    //          <button (click)="deleteUser(user.id)">Delete</button>
    //        </td>
    //      </ng-container>

    //      <ng-template #editRow>
    //        <td><input [(ngModel)]="editUser.name" name="editName" /></td>
    //        <td><input [(ngModel)]="editUser.username" name="editUsername" /></td>
    //        <td><input [(ngModel)]="editUser.email" name="editEmail" /></td>
    //        <td>
    //          <button (click)="saveEdit(user.id)">Save</button>
    //          <button (click)="cancelEdit()">Cancel</button>
    //        </td>
    //      </ng-template>
    //    </tr>
    //  </tbody>
    //</table>
  //`
})
export class UserComponent implements OnInit {
  users: any[] = [];
  tenantId: string | null = null;

  newUser = { name: '', username: '', email: '' };
  editUser = { name: '', username: '', email: '' };
  editingId: number | null = null;

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }

  ngOnInit(): void {
    const token = localStorage.getItem('jwt');    
    if (token) {
      const decoded = this.jwtHelper.decodeToken(token);
      this.tenantId = decoded?.tenantId || null;
      console.log('JWT in localStorage:', localStorage.getItem('jwt'));
      this.fetchUsers();
    }
  }

  fetchUsers(): void {
    const token = localStorage.getItem('jwt');
    const headers = {
      Authorization: `Bearer ${token}`
    };
    this.http.get<any[]>(`${environment.apiUrl}/user/getall`, { headers }).subscribe({
      next: (res) => (this.users = res),
      error: (err) => console.error('Error fetching users:', err)
    });
  }

  addUser(): void {
    const token = localStorage.getItem('jwt');
    const headers = {
      Authorization: `Bearer ${token}`
    };
    this.http.post(`${environment.apiUrl}/user/add`, this.newUser, { headers }).subscribe({
      next: () => {
        this.newUser = { name: '', username: '', email: '' };
        this.fetchUsers();
      },
      error: (err) => console.error('Error adding user:', err)
    });
  }

  deleteUser(id: number): void {
    const token = localStorage.getItem('jwt');
    const headers = {
      Authorization: `Bearer ${token}`
    };
    this.http.delete(`${environment.apiUrl}/user/delete/${id}`, { headers }).subscribe({
      next: () => this.fetchUsers(),
      error: (err) => console.error('Error deleting user:', err)
    });
  }

  startEdit(user: any): void {
    this.editingId = user.id;
    this.editUser = { ...user };
  }

  cancelEdit(): void {
    this.editingId = null;
    this.editUser = { name: '', username: '', email: '' };
  }

  saveEdit(id: number): void {
    const token = localStorage.getItem('jwt');
    const headers = {
      Authorization: `Bearer ${token}`
    };
    this.http.put(`${environment.apiUrl}/user/edit`, this.editUser, { headers }).subscribe({
      next: () => {
        this.cancelEdit();
        this.fetchUsers();
      },
      error: (err) => console.error('Error updating user:', err)
    });
  }
}


