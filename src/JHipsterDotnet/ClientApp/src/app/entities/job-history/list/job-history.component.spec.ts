import { ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpHeaders, HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { of } from "rxjs";

import { JobHistoryService } from "../service/job-history.service";

import { JobHistoryComponent } from "./job-history.component";

describe("Component Tests", () => {
  describe("JobHistory Management Component", () => {
    let comp: JobHistoryComponent;
    let fixture: ComponentFixture<JobHistoryComponent>;
    let service: JobHistoryService;

    beforeEach(() => {
      TestBed.configureTestingModule({
        imports: [HttpClientTestingModule],
        declarations: [JobHistoryComponent],
      })
        .overrideTemplate(JobHistoryComponent, "")
        .compileComponents();

      fixture = TestBed.createComponent(JobHistoryComponent);
      comp = fixture.componentInstance;
      service = TestBed.inject(JobHistoryService);

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
      expect(comp.jobHistories?.[0]).toEqual(
        jasmine.objectContaining({ id: 123 })
      );
    });
  });
});
