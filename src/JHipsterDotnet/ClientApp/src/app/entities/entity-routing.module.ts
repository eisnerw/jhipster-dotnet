import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: "region",
        data: { pageTitle: "Regions" },
        loadChildren: () =>
          import("./region/region.module").then((m) => m.RegionModule),
      },
      {
        path: "country",
        data: { pageTitle: "Countries" },
        loadChildren: () =>
          import("./country/country.module").then((m) => m.CountryModule),
      },
      {
        path: "location",
        data: { pageTitle: "Locations" },
        loadChildren: () =>
          import("./location/location.module").then((m) => m.LocationModule),
      },
      {
        path: "department",
        data: { pageTitle: "Departments" },
        loadChildren: () =>
          import("./department/department.module").then(
            (m) => m.DepartmentModule
          ),
      },
      {
        path: "task",
        data: { pageTitle: "Tasks" },
        loadChildren: () =>
          import("./task/task.module").then((m) => m.TaskModule),
      },
      {
        path: "employee",
        data: { pageTitle: "Employees" },
        loadChildren: () =>
          import("./employee/employee.module").then((m) => m.EmployeeModule),
      },
      {
        path: "job",
        data: { pageTitle: "Jobs" },
        loadChildren: () => import("./job/job.module").then((m) => m.JobModule),
      },
      {
        path: "job-history",
        data: { pageTitle: "JobHistories" },
        loadChildren: () =>
          import("./job-history/job-history.module").then(
            (m) => m.JobHistoryModule
          ),
      },
      /* jhipster-needle-add-entity-route - JHipster will add entity modules routes here */
    ]),
  ],
})
export class EntityRoutingModule {}
