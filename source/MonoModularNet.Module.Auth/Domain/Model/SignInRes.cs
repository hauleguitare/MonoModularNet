﻿namespace MonoModularNet.Module.Auth.Domain.Model;

public record SignInRes
{
    public string Id { get; set; }
    public string UserName { get; set; }
}