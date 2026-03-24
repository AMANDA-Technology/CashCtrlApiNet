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
using CashCtrlApiNet.Abstractions.Models.Base;
using CashCtrlApiNet.Abstractions.Models.Common.SequenceNumber;

namespace CashCtrlApiNet.Interfaces.Connectors.Common;

/// <summary>
/// CashCtrl common sequence number service endpoint. <a href="https://app.cashctrl.com/static/help/en/api/index.html#/sequencenumber">API Doc - Common/Sequence number</a>
/// </summary>
public interface ISequenceNumberService
{
    /// <summary>
    /// Read sequence number. Returns a single sequence number by ID.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/sequencenumber/read.json">API Doc - Common/Read sequence number</a>
    /// </summary>
    /// <param name="sequenceNumber">The entry containing the ID of the sequence number.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SequenceNumber>>> Get(Entry sequenceNumber, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// List sequence numbers. Returns a list of sequence numbers.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/sequencenumber/list.json">API Doc - Common/List sequence numbers</a>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<ListResponse<SequenceNumberListed>>> GetList([Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Create sequence number. Creates a new sequence number. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/sequencenumber/create.json">API Doc - Common/Create sequence number</a>
    /// </summary>
    /// <param name="sequenceNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Create(SequenceNumberCreate sequenceNumber, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Update sequence number. Updates an existing sequence number. Returns either a success or multiple error messages (for each issue).
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/sequencenumber/update.json">API Doc - Common/Update sequence number</a>
    /// </summary>
    /// <param name="sequenceNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Update(SequenceNumberUpdate sequenceNumber, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Delete sequence numbers. Deletes one or multiple sequence numbers. Returns either a success or error message.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/sequencenumber/delete.json">API Doc - Common/Delete sequence numbers</a>
    /// </summary>
    /// <param name="sequenceNumbers"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<NoContentResponse>> Delete(Entries sequenceNumbers, [Optional] CancellationToken cancellationToken);

    /// <summary>
    /// Get generated number. Returns the next generated number for the specified sequence number.
    /// <a href="https://app.cashctrl.com/static/help/en/api/index.html#/sequencenumber/get">API Doc - Common/Get generated number</a>
    /// </summary>
    /// <param name="sequenceNumber">The entry containing the ID of the sequence number.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ApiResult<SingleResponse<SequenceNumber>>> GetGeneratedNumber(Entry sequenceNumber, [Optional] CancellationToken cancellationToken);
}
