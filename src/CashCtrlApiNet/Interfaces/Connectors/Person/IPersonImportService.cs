/*
MIT License

Copyright (c) 2022 Philip Näf <philip.naef@amanda-technology.ch>
Copyright (c) 2022 Manuel Gysin <manuel.gysin@amanda-technology.ch>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Runtime.InteropServices;
using CashCtrlApiNet.Abstractions.Models.Api;
using CashCtrlApiNet.Abstractions.Models.Person.Import;

namespace CashCtrlApiNet.Interfaces.Connectors.Person;

/// <summary>
/// CashCtrl person import service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/import">API Doc - Person/Import</a>
/// </summary>
public interface IPersonImportService
{
    /// <summary>
    /// Create a new person import. Requires a file ID of an uploaded vCard (.vcf) file.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/import/create.json">API Doc - Person/Import/Create</a>
    /// </summary>
    /// <param name="importCreate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(PersonImportCreate importCreate, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Define the mapping for a person import.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/import/mapping.json">API Doc - Person/Import/Mapping</a>
    /// </summary>
    /// <param name="importMapping"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Mapping(PersonImportMapping importMapping, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Get available mapping fields for a person import.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/import/mapping_combo.json">API Doc - Person/Import/Mapping fields</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult> GetMappingFields([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Get a preview of a person import.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/import/preview.json">API Doc - Person/Import/Preview</a>
    /// </summary>
    /// <param name="importPreview"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Preview(PersonImportPreview importPreview, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Execute a person import.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/person/import/execute.json">API Doc - Person/Import/Execute</a>
    /// </summary>
    /// <param name="importExecute"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Execute(PersonImportExecute importExecute, [Optional] CancellationToken cancellationToken);
}
