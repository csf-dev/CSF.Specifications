# CSF.Data
These types assist in operating with data-sources.  Highlights are:

## IQuery
This is an interface which is designed to be used within applications that need access to an object-based data-source, such as an ORM.
Essentially it is an interface for a generic repository service, which exposes a data-source via Linq.
Firstly, this permits abstracting away from that data-source's own interfaces.
Secondly, it solves a specific problem present in the **NHibernate** ORM, in that the `ISession.Query<T>()` method is an extension method, which is problematic to mock in unit tests.

## InMemoryQuery
This implementation of `IQuery` holds data transiently in-memory.
It may be used as a data-source where an in-memory repository is appropriate.
It may also be used as a test fake, for substituting a real (database-backed) query in unit tests.

## InMemoryDataReader
This type is intended to be used as a test fake, when you wish to mock `IDataReader`.
It exposes a data-set which is passed into the constructor.

## Open source license
All source files within this project are released as open source software,
under the terms of [the MIT license].

[the MIT license]: http://opensource.org/licenses/MIT

This software is distributed in the hope that it will be useful, but please
remember that:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.