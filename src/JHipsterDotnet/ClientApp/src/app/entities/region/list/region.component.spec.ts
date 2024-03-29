import { ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpHeaders, HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { of } from "rxjs";

import { RegionService } from "../service/region.service";

import { RegionComponent } from "./region.component";

describe("Component Tests", () => {
  describe("Region Management Component", () => {
    let comp: RegionComponent;
    let fixture: ComponentFixture<RegionComponent>;
    let service: RegionService;

    beforeEach(() => {
      TestBed.configureTestingModule({
        imports: [HttpClientTestingModule],
        declarations: [RegionComponent],
      })
        .overrideTemplate(RegionComponent, "")
        .compileComponents();

      fixture = TestBed.createComponent(RegionComponent);
      comp = fixture.componentInstance;
      service = TestBed.inject(RegionService);

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
      expect(comp.regions?.[0]).toEqual(jasmine.objectContaining({ id: 123 }));
    });
  });
});
