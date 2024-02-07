using System.IO;
using CustomDialog.Models.Interfaces;

namespace CustomDialog.Models.Entities;

public sealed class DirectoryModel(DirectoryInfo directory) : FileEntityModel(directory), ILoadable;