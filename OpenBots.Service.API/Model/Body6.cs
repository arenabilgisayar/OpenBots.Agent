/* 
 * OpenBots Server API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = OpenBots.Service.API.Client.SwaggerDateConverter;

namespace OpenBots.Service.API.Model
{
    /// <summary>
    /// Body6
    /// </summary>
    [DataContract]
        public partial class Body6 :  IEquatable<Body6>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Body6" /> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="name">name (required).</param>
        /// <param name="version">version.</param>
        /// <param name="versionId">versionId.</param>
        /// <param name="createdOn">createdOn.</param>
        /// <param name="createdBy">createdBy.</param>
        /// <param name="status">status.</param>
        /// <param name="_file">_file.</param>
        /// <param name="fileId">fileId.</param>
        /// <param name="organizationId">organizationId.</param>
        public Body6(Guid? id = default(Guid?), string name = default(string), int? version = default(int?), Guid? versionId = default(Guid?), DateTime? createdOn = default(DateTime?), string createdBy = default(string), string status = default(string), byte[] _file = default(byte[]), Guid? fileId = default(Guid?), Guid? organizationId = default(Guid?))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new InvalidDataException("name is a required property for Body6 and cannot be null");
            }
            else
            {
                this.Name = name;
            }
            this.Id = id;
            this.Version = version;
            this.VersionId = versionId;
            this.CreatedOn = createdOn;
            this.CreatedBy = createdBy;
            this.Status = status;
            this.File = _file;
            this.FileId = fileId;
            this.OrganizationId = organizationId;
        }
        
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="Id", EmitDefaultValue=false)]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="Name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Version
        /// </summary>
        [DataMember(Name="Version", EmitDefaultValue=false)]
        public int? Version { get; set; }

        /// <summary>
        /// Gets or Sets VersionId
        /// </summary>
        [DataMember(Name="VersionId", EmitDefaultValue=false)]
        public Guid? VersionId { get; set; }

        /// <summary>
        /// Gets or Sets CreatedOn
        /// </summary>
        [DataMember(Name="CreatedOn", EmitDefaultValue=false)]
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="CreatedBy", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name="Status", EmitDefaultValue=false)]
        public string Status { get; set; }

        /// <summary>
        /// Gets or Sets File
        /// </summary>
        [DataMember(Name="File", EmitDefaultValue=false)]
        public byte[] File { get; set; }

        /// <summary>
        /// Gets or Sets FileId
        /// </summary>
        [DataMember(Name="FileId", EmitDefaultValue=false)]
        public Guid? FileId { get; set; }

        /// <summary>
        /// Gets or Sets OrganizationId
        /// </summary>
        [DataMember(Name="OrganizationId", EmitDefaultValue=false)]
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Body6 {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("  VersionId: ").Append(VersionId).Append("\n");
            sb.Append("  CreatedOn: ").Append(CreatedOn).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  File: ").Append(File).Append("\n");
            sb.Append("  FileId: ").Append(FileId).Append("\n");
            sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as Body6);
        }

        /// <summary>
        /// Returns true if Body6 instances are equal
        /// </summary>
        /// <param name="input">Instance of Body6 to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Body6 input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.Version == input.Version ||
                    (this.Version != null &&
                    this.Version.Equals(input.Version))
                ) && 
                (
                    this.VersionId == input.VersionId ||
                    (this.VersionId != null &&
                    this.VersionId.Equals(input.VersionId))
                ) && 
                (
                    this.CreatedOn == input.CreatedOn ||
                    (this.CreatedOn != null &&
                    this.CreatedOn.Equals(input.CreatedOn))
                ) && 
                (
                    this.CreatedBy == input.CreatedBy ||
                    (this.CreatedBy != null &&
                    this.CreatedBy.Equals(input.CreatedBy))
                ) && 
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
                ) && 
                (
                    this.File == input.File ||
                    (this.File != null &&
                    this.File.Equals(input.File))
                ) && 
                (
                    this.FileId == input.FileId ||
                    (this.FileId != null &&
                    this.FileId.Equals(input.FileId))
                ) && 
                (
                    this.OrganizationId == input.OrganizationId ||
                    (this.OrganizationId != null &&
                    this.OrganizationId.Equals(input.OrganizationId))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Id != null)
                    hashCode = hashCode * 59 + this.Id.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Version != null)
                    hashCode = hashCode * 59 + this.Version.GetHashCode();
                if (this.VersionId != null)
                    hashCode = hashCode * 59 + this.VersionId.GetHashCode();
                if (this.CreatedOn != null)
                    hashCode = hashCode * 59 + this.CreatedOn.GetHashCode();
                if (this.CreatedBy != null)
                    hashCode = hashCode * 59 + this.CreatedBy.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                if (this.File != null)
                    hashCode = hashCode * 59 + this.File.GetHashCode();
                if (this.FileId != null)
                    hashCode = hashCode * 59 + this.FileId.GetHashCode();
                if (this.OrganizationId != null)
                    hashCode = hashCode * 59 + this.OrganizationId.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
