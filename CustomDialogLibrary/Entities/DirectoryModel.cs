using CustomDialogLibrary.Interfaces;

namespace CustomDialogLibrary.Entities;

public sealed class DirectoryModel(DirectoryInfo directory) : FileEntityModel(directory), ILoadable;