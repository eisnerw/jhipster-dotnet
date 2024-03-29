import { ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpHeaders, HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { of } from "rxjs";

import { JobService } from "../service/job.service";

import { JobComponent } from "./job.component";

describe("Component Tests", () => {
  describe("Job Management Component", () => {
    let comp: JobComponent;
    let fixture: ComponentFixture<JobComponent>;
    let service: JobService;

    beforeEach(() => {
      TestBed.configureTestingModule({
        imports: [HttpClientTestingModule],
        declarations: [JobComponent],
      })
        .overrideTemplate(JobComponent, "")
        .compileComponents();

      fixture = TestBed.createComponent(JobComponent);
      comp = fixture.componentInstance;
      service = TestBed.inject(JobService);

      const headers = new HttpHeaders().append("link", "link;link");
      spyOn(service, "query").and.returnValue(
        of(
          new HttpResponse({
            body: [{ id: 123 }],
            headers,
          })
        )
      );
    });

    it("Should call load all on init", () => {
      // WHEN
      comp.ngOnInit();

      // THEN
      expect(service.query).toHaveBeenCalled();
      expect(comp.jobs?.[0]).toEqual(jasmine.objectContaining({ id: 123 }));
    });
  });
});
