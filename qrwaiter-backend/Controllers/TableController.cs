using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Extensions.UnitOfWork;
using qrwaiter_backend.Mapping.DTOs;
using qrwaiter_backend.Services.Interfaces;
using Table = qrwaiter_backend.Data.Models.Table;

namespace qrwaiter_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TableController : Controller
    {
        private readonly ITableService _tableService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TableController(
                               ITableService tableService,
                               IUnitOfWork unitOfWork,
                               IMapper mapper)
        {
            _tableService = tableService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPut]
        public async Task<ActionResult<TableDTO>> Create([FromBody] AddTableDTO tableDTO)
        {
            var table = new Table()
            {
                Id = Guid.NewGuid(),
                Name = tableDTO.Name,
                Number = tableDTO.Number,
                IdQrCode = Guid.NewGuid(),
                IdResaurant = tableDTO.IdResaurant,
                IsDeleted = false
            };
            var qrCode = new QrCode();
            qrCode.Id = table.IdQrCode;
            qrCode.IdTable = table.Id;
            qrCode.Title = "Позвать официанта";

            table.QrCode = qrCode;
            table = await _unitOfWork.TableRepository.Insert(table);
            _unitOfWork.SaveChanges();
            return Ok(_mapper.Map<Table, TableDTO>(table));
        }
        [HttpGet("restaurant/{id}")]
        public async Task<ActionResult<List<TableDTO>>> GetTablesByRestaurantId([FromRoute] Guid id)
        {
            var tables = await _unitOfWork.TableRepository.GetAll();




            return Ok(_mapper.Map<List<Table>, List<TableDTO>>(
                tables.Where(t => t.IdResaurant == id && t.IsDeleted == false).OrderBy(t => t.Number).ToList()
                ));
        }

        [HttpPost]
        public async Task<ActionResult<TableDTO>> Update([FromBody] TableDTO tableDTO)
        {
            var table = await _unitOfWork.TableRepository.GetById(tableDTO.Id);
            _mapper.Map<TableDTO, Table>(tableDTO, table);
            table = await _unitOfWork.TableRepository.Update(table);
            _unitOfWork.SaveChanges();
            return Ok(_mapper.Map<Table, TableDTO>(table));
        }

        // GET: TableController/Edit/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TableDTO>> Delete([FromRoute] Guid id, [FromQuery] bool softDelete = true)
        {
            if (softDelete)
            {
                var table = await _unitOfWork.TableRepository.GetById(id);
                table.IsDeleted = true;
                await _unitOfWork.TableRepository.Update(table);
                _unitOfWork.SaveChanges();
                return Ok(_mapper.Map<Table, TableDTO>(table));
            }
            else
            {
                var table = await _unitOfWork.TableRepository.GetById(id);
                _unitOfWork.TableRepository.DeleteById(id);
                _unitOfWork.SaveChanges();
                return Ok(_mapper.Map<Table, TableDTO>(table));
            }
        }


    }
}
